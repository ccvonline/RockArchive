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
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using com.ccvonline.CommandCenter.Model;

namespace com.ccvonline.CommandCenter.Data
{
    /// <summary>
    /// 
    /// </summary>
    public partial class CommandCenterContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandCenterContext"/> class.
        /// </summary>
        public CommandCenterContext()
            : base( "RockContext" )
        {
            // intentionally left blank
        }

        /// <summary>
        /// This method is called when the model for a derived context has been initialized, but
        /// before the model has been locked down and used to initialize the context.  The default
        /// implementation of this method does nothing, but it can be overridden in a derived class
        /// such that the model can be further configured before it is locked down.
        /// </summary>
        /// <param name="modelBuilder">The builder that defines the model for the context being created.</param>
        /// <remarks>
        /// Typically, this method is called only once when the first instance of a derived context
        /// is created.  The model for that context is then cached and is for all further instances of
        /// the context in the app domain.  This caching can be disabled by setting the ModelCaching
        /// property on the given ModelBuidler, but note that this can seriously degrade performance.
        /// More control over caching is provided through use of the DbModelBuilder and DbContextFactory
        /// classes directly.
        /// </remarks>
        protected override void OnModelCreating( DbModelBuilder modelBuilder )
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            // Model Configurations
            modelBuilder.Configurations.Add( new RecordingConfiguration() );
        }
        
        #region Models

        /// <summary>
        /// Gets or sets the commandCenter competencies.
        /// </summary>
        /// <value>
        /// The commandCenter competencies.
        /// </value>
        public DbSet<Recording> Recordings { get; set; }

        #endregion
    }
}
