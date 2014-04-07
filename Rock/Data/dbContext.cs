﻿// <copyright>
// Copyright 2013 by the Spark Development Network
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
//
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Reflection;
using System.Web;
using Rock.Model;
using Rock.Utility;
using Rock.Workflow;

namespace Rock.Data
{
    /// <summary>
    /// Entity Framework Context
    /// </summary>
    public abstract class DbContext : System.Data.Entity.DbContext
    {
        /// <summary>
        /// Gets any error messages that occurred during a SaveChanges
        /// </summary>
        /// <value>
        /// The save error messages.
        /// </value>
        public virtual List<string> SaveErrorMessages { get; private set; }

        /// <summary>
        /// Saves all changes made in this context to the underlying database.
        /// </summary>
        /// <returns>
        /// The number of objects written to the underlying database.
        /// </returns>
        public override int SaveChanges()
        {
            return SaveChanges( false );
        }

        /// <summary>
        /// Saves all changes made in this context to the underlying database.  The
        /// default pre and post processing can also optionally be disabled.  This 
        /// would disable audit records being created, workflows being triggered, and
        /// any PreSaveChanges() methods being called for changed entities.  
        /// </summary>
        /// <param name="disablePrePostProcessing">if set to <c>true</c> disables 
        /// the Pre and Post processing from being run. This should only be disabled
        /// when updating a large number of records at a time (e.g. importing records).</param>
        /// <returns></returns>
        public int SaveChanges(bool disablePrePostProcessing)
        {
            // Pre and Post processing has been disabled, just call the base 
            // SaveChanges() method and return
            if ( disablePrePostProcessing )
            {
                return base.SaveChanges();
            }

            int result = 0;

            SaveErrorMessages = new List<string>();

            // Try to get the current person alias and id
            PersonAlias personAlias = null;
            if ( HttpContext.Current != null && HttpContext.Current.Items.Contains( "CurrentPerson" ) )
            {
                var currentPerson = HttpContext.Current.Items["CurrentPerson"] as Person;
                if ( currentPerson != null && currentPerson.PrimaryAlias != null )
                {
                    personAlias = currentPerson.PrimaryAlias;
                }
            }

            // Evaluate the current context for items that have changes
            var updatedItems = RockPreSave( this, personAlias );

            // If update was not cancelled by triggered workflow 
            if ( updatedItems != null )
            {
                // Save the context changes
                result = base.SaveChanges();

                // If any items changed process audit and triggers
                if ( updatedItems.Any() )
                {
                    RockPostSave( updatedItems, personAlias );
                }
            }

            return result;
        }

        /// <summary>
        /// Updates the Created/Modified data for any model being created or modified
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="personAlias">The person alias.</param>
        /// <returns></returns>
        protected virtual List<ContextItem> RockPreSave( DbContext dbContext, PersonAlias personAlias )
        {
            int? personAliasId = null;
            if (personAlias != null)
            {
                personAliasId = personAlias.Id;
            }

            var updatedItems = new List<ContextItem>();
            foreach ( var entry in dbContext.ChangeTracker.Entries()
                .Where( c => 
                    c.Entity is IEntity &&
                    (c.State == EntityState.Added || c.State == EntityState.Modified || c.State == EntityState.Deleted) ) )
            {
                // Cast entry as IEntity
                var entity = entry.Entity as IEntity;

                // Get the context item to track audits
                var contextItem = new ContextItem( entity, entry.State );

                // If entity was added or modifed, update the Created/Modified fields
                if ( entry.State == EntityState.Added || entry.State == EntityState.Modified )
                {
                    if ( !TriggerWorkflows( entity, WorkflowTriggerType.PreSave, personAlias ) )
                    {
                        return null;
                    }

                    if ( entry.Entity is IModel )
                    {
                        var model = entry.Entity as IModel;

                        model.PreSaveChanges( this, entry.State );

                        // Update Created/Modified person and times
                        if ( entry.State == EntityState.Added )
                        {
                            if ( !model.CreatedDateTime.HasValue )
                            {
                                model.CreatedDateTime = RockDateTime.Now;
                            }
                            if ( !model.CreatedByPersonAliasId.HasValue )
                            {
                                model.CreatedByPersonAliasId = personAliasId;
                            }

                            model.ModifiedDateTime = RockDateTime.Now;
                            model.ModifiedByPersonAliasId = personAliasId;
                        }
                        else if ( entry.State == EntityState.Modified )
                        {
                            model.ModifiedDateTime = RockDateTime.Now;
                            model.ModifiedByPersonAliasId = personAliasId;
                        }

                        if ( model is Person )
                        {
                            var person = model as Person;
                            var transaction = new Rock.Transactions.SaveMetaphoneTransaction( person );
                            Rock.Transactions.RockQueue.TransactionQueue.Enqueue( transaction );
                        }
                    }
                }
                else if (entry.State == EntityState.Deleted)
                {
                    if ( !TriggerWorkflows( entity, WorkflowTriggerType.PreDelete, personAlias ) )
                    {
                        return null;
                    }
                }

                GetAuditDetails( dbContext, contextItem, personAliasId);
                updatedItems.Add(contextItem);
            }

            return updatedItems;
        }

        /// <summary>
        /// Creates audit logs and/or triggers workflows for items that were changed
        /// </summary>
        /// <param name="updatedItems">The updated items.</param>
        /// <param name="personAlias">The person alias.</param>
        protected virtual void RockPostSave( List<ContextItem> updatedItems, PersonAlias personAlias )
        {
            var audits = updatedItems.Select( i => i.Audit).ToList();
            if (audits.Any())
            {
                var transaction = new Rock.Transactions.AuditTransaction();
                transaction.Audits = audits;
                Rock.Transactions.RockQueue.TransactionQueue.Enqueue( transaction );
            }

            foreach( var item in updatedItems)
            {
                if (item.State == EntityState.Deleted)
                {
                    TriggerWorkflows( item.Entity, WorkflowTriggerType.PostDelete, personAlias );
                }
                else
                {
                    TriggerWorkflows( item.Entity, WorkflowTriggerType.PostSave, personAlias );
                }
            }
        }

        private bool TriggerWorkflows( IEntity entity, WorkflowTriggerType triggerType, PersonAlias personAlias )
        {
            Dictionary<string, PropertyInfo> properties = null;

            var rockContext = new RockContext();
            var workflowTypeService = new WorkflowTypeService( rockContext );
            var workflowService = new WorkflowService( rockContext );

            foreach ( var trigger in TriggerCache.Triggers( entity.TypeName, triggerType ).Where( t => t.IsActive == true ) )
            {
                bool match = true;

                if ( !string.IsNullOrWhiteSpace( trigger.EntityTypeQualifierColumn ) )
                {
                    if ( properties == null )
                    {
                        properties = new Dictionary<string, PropertyInfo>();
                        foreach ( PropertyInfo propertyInfo in entity.GetType().GetProperties() )
                        {
                            properties.Add( propertyInfo.Name.ToLower(), propertyInfo );
                        }
                    }

                    match = ( properties.ContainsKey( trigger.EntityTypeQualifierColumn.ToLower() ) &&
                        properties[trigger.EntityTypeQualifierColumn.ToLower()].GetValue( entity, null ).ToString()
                            == trigger.EntityTypeQualifierValue );
                }

                if ( match )
                {
                    if ( triggerType == WorkflowTriggerType.PreSave || triggerType == WorkflowTriggerType.PreDelete )
                    {
                        var workflowType = workflowTypeService.Get( trigger.WorkflowTypeId );

                        if ( workflowType != null )
                        {
                            var workflow = Rock.Model.Workflow.Activate( workflowType, trigger.WorkflowName );

                            List<string> workflowErrors;
                            if ( !workflow.Process( entity, out workflowErrors ) )
                            {
                                SaveErrorMessages.AddRange( workflowErrors );
                                return false;
                            }
                            else
                            {
                                if ( workflowType.IsPersisted )
                                {
                                    workflowService.Add( workflow );
                                    rockContext.SaveChanges();
                                }
                            }
                        }
                    }
                    else
                    {
                        var transaction = new Rock.Transactions.WorkflowTriggerTransaction();
                        transaction.Trigger = trigger;
                        transaction.Entity = entity.Clone();
                        transaction.PersonAlias = personAlias;
                        Rock.Transactions.RockQueue.TransactionQueue.Enqueue( transaction );
                    }
                }
            }
            return true;
        }

        private static void GetAuditDetails( DbContext dbContext, ContextItem item, int? personAliasId )
        {   
            // Get the base class (not the proxy class)
            Type rockEntityType = item.Entity.GetType();
            if ( rockEntityType.Namespace == "System.Data.Entity.DynamicProxies" )
            {
                rockEntityType = rockEntityType.BaseType;
            }

            // Check to make sure class does not have [NotAudited] attribute
            if (AuditClass (rockEntityType))
            {
                var dbEntity = dbContext.Entry( item.Entity );
                var audit = item.Audit;

                PropertyInfo[] properties = rockEntityType.GetProperties();

                foreach ( PropertyInfo propInfo in properties )
                {
                    // Check to make sure property does not have the [NotAudited] attribute
                    if ( AuditProperty( propInfo ) )
                    {
                        // If entire entity was added or deleted or this property was modified
                        var dbPropertyEntry = dbEntity.Property( propInfo.Name );
                        if ( dbPropertyEntry != null && (
                            dbEntity.State == EntityState.Added ||
                            dbEntity.State == EntityState.Deleted ||
                            dbPropertyEntry.IsModified ))
                        {
                            var currentValue = dbEntity.State == EntityState.Deleted ? string.Empty : dbPropertyEntry.CurrentValue;
                            var originalValue = dbEntity.State == EntityState.Added ? string.Empty : dbPropertyEntry.OriginalValue;

                            var detail = new AuditDetail();
                            detail.Property = propInfo.Name;
                            detail.CurrentValue = currentValue != null ? currentValue.ToString() : string.Empty;
                            detail.OriginalValue = originalValue != null ? originalValue.ToString() : string.Empty;
                            if (detail.CurrentValue != detail.OriginalValue)
                            {
                                audit.Details.Add( detail );
                            }
                        }
                    }
                }

                if ( audit.Details.Any() )
                {
                    var entityType = Rock.Web.Cache.EntityTypeCache.Read( rockEntityType );
                    if ( entityType != null )
                    {
                        string title = item.Entity.ToString();
                        if (string.IsNullOrWhiteSpace(title))
                        {
                            title = entityType.FriendlyName ?? string.Empty;
                        }
                        audit.DateTime = RockDateTime.Now;
                        audit.PersonAliasId = personAliasId;
                        audit.EntityTypeId = entityType.Id;
                        audit.EntityId = item.Entity.Id;
                        audit.Title = title.Truncate( 195 );
                    }
                }
            }

        }

        private static bool AuditClass( Type baseType )
        {
            var attribute = baseType.GetCustomAttribute( typeof( NotAuditedAttribute ) );
            return ( attribute == null );
        }        
        
        private static bool AuditProperty( PropertyInfo propertyInfo )
        {
            if ( propertyInfo.GetCustomAttribute( typeof( NotAuditedAttribute ) ) == null &&
                ( !propertyInfo.GetGetMethod().IsVirtual || propertyInfo.Name == "Id" || propertyInfo.Name == "Guid" || propertyInfo.Name == "Order" ) )
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// State of entity being changed during a context save
        /// </summary>
        protected class ContextItem
        {
            /// <summary>
            /// Gets or sets the entity.
            /// </summary>
            /// <value>
            /// The entity.
            /// </value>
            public IEntity Entity { get; set; }

            /// <summary>
            /// Gets or sets the state.
            /// </summary>
            /// <value>
            /// The state.
            /// </value>
            public EntityState State { get; set; }

            /// <summary>
            /// Gets or sets the audit.
            /// </summary>
            /// <value>
            /// The audit.
            /// </value>
            public Audit Audit { get; set; }

            /// <summary>
            /// Initializes a new instance of the <see cref="ContextItem"/> class.
            /// </summary>
            /// <param name="entity">The entity.</param>
            /// <param name="state">The state.</param>
            public ContextItem( IEntity entity, EntityState state )
            {
                Entity = entity;
                State = state;
                Audit = new Audit();

                switch ( state )
                {
                    case EntityState.Added:
                        {
                            Audit.AuditType = AuditType.Add;
                            break;
                        }
                    case EntityState.Deleted:
                        {
                            Audit.AuditType = AuditType.Delete;
                            break;
                        }
                    case EntityState.Modified:
                        {
                            Audit.AuditType = AuditType.Modify;
                            break;
                        }
                }

            }
        }
    
    }
}
