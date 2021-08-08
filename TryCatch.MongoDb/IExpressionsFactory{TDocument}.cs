// <copyright file="IExpressionsFactory{TDocument}.cs" company="TryCatch Software Factory">
// Copyright © TryCatch Software Factory All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

namespace TryCatch.MongoDb
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    /// <summary>
    /// Factory of queries expression.
    /// </summary>
    /// <typeparam name="TDocument">Type of entities to use on expression.</typeparam>
    public interface IExpressionsFactory<TDocument>
        where TDocument : class
    {
        /// <summary>
        /// Gets a reference to filter expression for a specific query.
        /// </summary>
        /// <param name="queryName">Query name.</param>
        /// <param name="entity">Reference to the entity to filter(optional).</param>
        /// <returns>Filter expression.</returns>
        Expression<Func<TDocument, bool>> GetWhereByQueryName(string queryName, TDocument entity = default);

        /// <summary>
        /// Gets a reference to filter expression for a specific query.
        /// </summary>
        /// <param name="queryName">Query name.</param>
        /// <param name="documents">Reference to the documents collection to filter(optional).</param>
        /// <returns>Filter expression.</returns>
        Expression<Func<TDocument, bool>> GetWhereByQueryName(string queryName, IEnumerable<TDocument> documents);

        /// <summary>
        /// Gets a reference to sort by expression for a specific query.
        /// </summary>
        /// <param name="queryName">Query name.</param>
        /// <returns>SortBy expression.</returns>
        Expression<Func<TDocument, object>> GetSortByByQueryName(string queryName);
    }
}
