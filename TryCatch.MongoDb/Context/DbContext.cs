// <copyright file="DbContext.cs" company="TryCatch Software Factory">
// Copyright © TryCatch Software Factory All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

namespace TryCatch.MongoDb.Context
{
    using System;
    using System.Collections.Generic;
    using MongoDB.Driver;
    using TryCatch.Validators;

    /// <summary>
    /// Database context implementation for MongoDB repositories.
    /// </summary>
    public abstract class DbContext : IDbContext
    {
        private readonly IDictionary<Type, string> collections;

        private readonly IMongoDatabase database;

        /// <summary>
        /// Initializes a new instance of the <see cref="DbContext"/> class.
        /// </summary>
        /// <param name="database">The <see cref="IMongoDatabase"/> database reference.</param>
        /// <exception cref="ArgumentNullException">It is thrown if no database was specified as an argument.</exception>
        protected DbContext(IMongoDatabase database)
        {
            ArgumentsValidator.ThrowIfIsNull(database);

            this.database = database;

            this.collections = new Dictionary<Type, string>();
        }

        /// <summary>
        /// Allows getting the collection of documents associated with the document type specified as an argument.
        /// </summary>
        /// <typeparam name="TDocument">Type of the document.</typeparam>
        /// <returns>The <see cref="IMongoCollection{TDocument}"/> reference to the documents collection.</returns>
        public virtual IMongoCollection<TDocument> Get<TDocument>() => this.database.GetCollection<TDocument>(this.GetCollectionName<TDocument>());

        /// <summary>
        /// Allows getting the collection of documents associated with the document type specified as an argument.
        /// </summary>
        /// <typeparam name="TDocument">Type of the document.</typeparam>
        /// <param name="collectionName">The collection name.</param>
        /// <exception cref="ArgumentException">It is thrown if the collectionName is null, empty or whitespace.</exception>
        /// <returns>The <see cref="IMongoCollection{TDocument}"/> reference to the documents collection.</returns>
        public virtual IMongoCollection<TDocument> Get<TDocument>(string collectionName)
        {
            ArgumentsValidator.ThrowIfIsNullEmptyOrWhiteSpace(collectionName);

            return this.database.GetCollection<TDocument>(collectionName);
        }

        /// <summary>
        /// Allows adding a new document collection to the context.
        /// </summary>
        /// <typeparam name="TDocument">Type of document.</typeparam>
        /// <param name="collectionName">The collection name.</param>
        /// <exception cref="ArgumentException">It is thrown if the collectionName is null, empty or whitespace.</exception>
        /// <returns>True if the collection was added, false in another case.</returns>
        protected virtual bool AddCollection<TDocument>(string collectionName)
        {
            ArgumentsValidator.ThrowIfIsNullEmptyOrWhiteSpace(collectionName);

            return this.collections.TryAdd(typeof(TDocument), collectionName);
        }

        /// <summary>
        /// Allows getting the name of the collection associated with the type of document specified as an argument.
        /// </summary>
        /// <typeparam name="TDocument">The type of document.</typeparam>
        /// <exception cref="CollectionNotFoundException">It is thrown if no collection was associated with the specified document type.</exception>
        /// <returns>The name of associated collection.</returns>
        protected virtual string GetCollectionName<TDocument>()
        {
            if (!this.collections.TryGetValue(typeof(TDocument), out var name))
            {
                throw new CollectionNotFoundException($"Collection for {typeof(TDocument).Name} is not found");
            }

            return name;
        }
    }
}
