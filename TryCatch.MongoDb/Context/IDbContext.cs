// <copyright file="IDbContext.cs" company="TryCatch Software Factory">
// Copyright © TryCatch Software Factory All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

namespace TryCatch.MongoDb.Context
{
    using MongoDB.Driver;

    /// <summary>
    /// Interface of Mongo DB context.
    /// </summary>
    public interface IDbContext
    {
        /// <summary>
        /// Gets a reference to TDocument MongoCollection.
        /// </summary>
        /// <typeparam name="TDocument">Type of Mongo Db Document.</typeparam>
        /// <returns>TDocument MongoDB collection.</returns>
        IMongoCollection<TDocument> Get<TDocument>();

        /// <summary>
        /// Gets a reference to TDocument MongoCollection.
        /// </summary>
        /// <typeparam name="TDocument">Type of Mongo Db Document.</typeparam>
        /// <param name="collectionName">The name of the collection.</param>
        /// <returns>TDocument MongoDB collection.</returns>
        IMongoCollection<TDocument> Get<TDocument>(string collectionName);
    }
}
