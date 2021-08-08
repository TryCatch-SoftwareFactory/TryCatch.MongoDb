// <copyright file="QueryRepository{TEntity}.cs" company="TryCatch Software Factory">
// Copyright © TryCatch Software Factory All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

namespace TryCatch.MongoDb.Linq
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;
    using MongoDB.Driver;
    using TryCatch.MongoDb.Context;
    using TryCatch.Patterns.Repositories;
    using TryCatch.Validators;

    /// <summary>
    /// Query Repository class base for IQueryRepository interface.
    /// </summary>
    /// <typeparam name="TEntity">Type of document.</typeparam>
    public abstract class QueryRepository<TEntity> : ILinqQueryRepository<TEntity>
        where TEntity : class
    {
        private const int DefaultLimit = 1000;

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryRepository{TEntity}"/> class.
        /// </summary>
        /// <param name="dbContext">IDbContext reference.</param>
        /// <param name="expressionsFactory">Reference to expressions factory.</param>
        protected QueryRepository(IDbContext dbContext, IExpressionsFactory<TEntity> expressionsFactory)
        {
            ArgumentsValidator.ThrowIfIsNull(dbContext);
            ArgumentsValidator.ThrowIfIsNull(expressionsFactory);

            this.ExpressionsFactory = expressionsFactory;
            this.Documents = dbContext.Get<TEntity>();
        }

        protected IMongoCollection<TEntity> Documents { get; }

        protected IExpressionsFactory<TEntity> ExpressionsFactory { get; }

        /// <inheritdoc/>
        public async virtual Task<TEntity> GetAsync(
            Expression<Func<TEntity, bool>> where,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            ArgumentsValidator.ThrowIfIsNull(where);

            var options = new FindOptions<TEntity> { };

            var result = await this.Documents
                .FindAsync(where, options, cancellationToken)
                .ConfigureAwait(false);

            return await result
                .FirstOrDefaultAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async virtual Task<long> GetCountAsync(
            Expression<Func<TEntity, bool>> where = null,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            where ??= this.ExpressionsFactory.GetWhereByQueryName(QueriesNames.DefaultCount);

            return await this.Documents
                .CountDocumentsAsync(where, new CountOptions(), cancellationToken)
                .ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async virtual Task<IEnumerable<TEntity>> GetPageAsync(
            int offset = 1,
            int limit = DefaultLimit,
            Expression<Func<TEntity, bool>> where = null,
            Expression<Func<TEntity, object>> orderBy = null,
            bool orderAsAscending = true,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            ArgumentsValidator.ThrowIfIsLessThan(1, offset, $"Offset value is invalid: {offset}");
            ArgumentsValidator.ThrowIfIsLessThan(1, limit, $"Limit value is invalid: {limit}");

            where ??= this.ExpressionsFactory.GetWhereByQueryName(QueriesNames.DefaultPage);
            orderBy ??= this.ExpressionsFactory.GetSortByByQueryName(QueriesNames.DefaultPage);

            var options = new FindOptions<TEntity>()
            {
                Skip = offset > 1 ? offset : 0,
                Limit = limit,
                Sort = GetSortDefinition(orderAsAscending, orderBy),
            };

            var result = await this.Documents
                .FindAsync(where, options, cancellationToken)
                .ConfigureAwait(false);

            return await result
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        private static SortDefinition<TEntity> GetSortDefinition(
            bool orderAsAscending,
            Expression<Func<TEntity, object>> orderBy) => orderAsAscending
                ? Builders<TEntity>.Sort.Ascending(orderBy)
                : Builders<TEntity>.Sort.Descending(orderBy);
    }
}
