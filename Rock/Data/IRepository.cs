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
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using Rock.Model;

namespace Rock.Data
{
    /// <summary>
    /// Repository interface for POCO models
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// Gets an <see cref="IQueryable{T}"/> list of all models
        /// </summary>
        /// <returns></returns>
        IQueryable<T> AsQueryable();

        /// <summary>
        /// Gets an <see cref="IQueryable{T}"/> list of all models, 
        /// with optional eager loading of properties specified in includes
        /// </summary>
        /// <returns></returns>
        IQueryable<T> AsQueryable( string includes );

        /// <summary>
        /// Gets an <see cref="IEnumerable{T}"/> list of all models.
        /// </summary>
        /// <returns></returns>
        IEnumerable<T> GetAll();

        /// <summary>
        /// Gets an <see cref="IEnumerable{T}"/> list of models matching the where expression
        /// </summary>
        /// <param name="where">where expression</param>
        /// <returns></returns>
        IEnumerable<T> Find( Expression<Func<T, bool>> where );

        /// <summary>
        /// Gets the only model matching the where expression.  Throws an exception if more than one
        /// model match.
        /// </summary>
        /// <param name="where">where expression</param>
        /// <returns></returns>
        T Single( Expression<Func<T, bool>> where );

        /// <summary>
        /// Gets the first model matching the where expression.  Throws an exception if no models 
        /// match.
        /// </summary>
        /// <param name="where">where expression</param>
        /// <returns></returns>
        T First( Expression<Func<T, bool>> where );

        /// <summary>
        /// Gets the first model matching the where expression.  Returns null if no models 
        /// match.
        /// </summary>
        /// <param name="where">where expression</param>
        /// <returns></returns>
        T FirstOrDefault( Expression<Func<T, bool>> where );

        /// <summary>
        /// All the audits made to the entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        IQueryable<Audit> Audits( T entity );

        /// <summary>
        /// All the audits made to the entity.
        /// </summary>
        /// <param name="entityTypeId">The entity type id.</param>
        /// <param name="entityId">The entity id.</param>
        /// <returns></returns>
        IQueryable<Audit> Audits( int entityTypeId, int entityId );

        /// <summary>
        /// Adds the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void Add( T entity );

        /// <summary>
        /// Attaches the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void Attach( T entity );

        /// <summary>
        /// Sets the values.
        /// </summary>
        /// <param name="sourceItem">The source item.</param>
        /// <param name="targetItem">The target item.</param>
        void SetValues( T sourceItem, T targetItem );

        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void Delete( T entity );

        /// <summary>
        /// Saves any changes made in the current context
        /// </summary>
        /// <param name="PersonId">The person id.</param>
        /// <param name="audits">The audits.</param>
        /// <param name="errorMessages">The error messages.</param>
        /// <returns></returns>
        bool Save( PersonAlias personAlias, out List<Audit> audits, out List<string> errorMessages);

        /// <summary>
        /// Creates a raw query that will return entities
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        IEnumerable<T> ExecuteQuery( string query, params object[] parameters );

        /// <summary>
        /// Sets the configuration value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        void SetConfigurationValue( string key, string value );
    }

}