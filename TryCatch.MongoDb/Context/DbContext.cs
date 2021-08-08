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

    public abstract class DbContext : IDbContext
    {
        private readonly IDictionary<Type, string> collections;

        private readonly IMongoDatabase database;

        protected DbContext(IMongoDatabase database)
        {
            ArgumentsValidator.ThrowIfIsNull(database);

            this.database = database;

            this.collections = new Dictionary<Type, string>();
        }

        public IMongoCollection<TDocument> Get<TDocument>()
            where TDocument : class => this.database.GetCollection<TDocument>(this.GetCollectionName<TDocument>());

        protected bool AddCollection<TDocument>(string name)
            where TDocument : class
        {
            ArgumentsValidator.ThrowIfIsNullEmptyOrWhiteSpace(name);

            return this.collections.TryAdd(typeof(TDocument), name);
        }

        protected string GetCollectionName<TDocument>()
            where TDocument : class
        {
            if (!this.collections.TryGetValue(typeof(TDocument), out var name))
            {
                throw new CollectionNotFoundException($"Collection for {typeof(TDocument).Name} is not found");
            }

            return name;
        }
    }
}
