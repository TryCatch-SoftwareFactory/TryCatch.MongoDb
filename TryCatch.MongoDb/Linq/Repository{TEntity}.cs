// <copyright file="Repository{TEntity}.cs" company="TryCatch Software Factory">
// Copyright © TryCatch Software Factory All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

namespace TryCatch.MongoDb.Linq
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;
    using MongoDB.Driver;
    using TryCatch.MongoDb.Context;
    using TryCatch.Patterns.Repositories;
    using TryCatch.Validators;

    /// <summary>
    /// Abstract implementation of repository.
    /// </summary>
    /// <typeparam name="TEntity">Type of document.</typeparam>
    public abstract class Repository<TEntity> : ILinqRepository<TEntity>
        where TEntity : class
    {
        private const int DefaultLimit = 1000;

        /// <summary>
        /// Initializes a new instance of the <see cref="Repository{TEntity}"/> class.
        /// </summary>
        /// <param name="dbContext">IDbContext reference.</param>
        /// <param name="expressionsFactory">Reference to expressions factory.</param>
        protected Repository(IDbContext dbContext, IExpressionsFactory<TEntity> expressionsFactory)
        {
            ArgumentsValidator.ThrowIfIsNull(dbContext);
            ArgumentsValidator.ThrowIfIsNull(expressionsFactory);

            this.ExpressionsFactory = expressionsFactory;
            this.Documents = dbContext.Get<TEntity>();
        }

        protected IMongoCollection<TEntity> Documents { get; }

        protected IExpressionsFactory<TEntity> ExpressionsFactory { get; }

        /// <inheritdoc />
        public async Task<bool> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            ArgumentsValidator.ThrowIfIsNull(entity);

            InsertOneOptions options = null;

            await this.Documents
                .InsertOneAsync(entity, options, cancellationToken)
                .ConfigureAwait(false);

            return true;
        }

        /// <inheritdoc />
        public async Task<bool> AddAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            ArgumentsValidator.ThrowIfIsNull(entities);

            var options = new InsertManyOptions()
            {
                IsOrdered = false,
            };

            var result = false;

            if (entities.Any())
            {
                await this.Documents
                    .InsertManyAsync(entities, options, cancellationToken)
                    .ConfigureAwait(false);

                result = true;
            }

            return result;
        }

        /// <inheritdoc />
        public async Task<bool> AddOrUpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            ArgumentsValidator.ThrowIfIsNull(entity);

            var where = this.ExpressionsFactory.GetWhereByQueryName(QueriesNames.UpdateOne, entity);

            var exists = await this.Documents
                .CountDocumentsAsync(where, new CountOptions(), cancellationToken)
                .ConfigureAwait(false);

            var resultFlag = false;

            if (exists == 1)
            {
                resultFlag = await this.UpdateAsync(entity, cancellationToken).ConfigureAwait(false);
            }
            else if (exists == 0)
            {
                resultFlag = await this.AddAsync(entity, cancellationToken).ConfigureAwait(false);
            }

            return resultFlag;
        }

        /// <inheritdoc />
        public async Task<bool> DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            ArgumentsValidator.ThrowIfIsNull(entity);

            var where = this.ExpressionsFactory.GetWhereByQueryName(QueriesNames.DeleteOne, entity);

            var result = await this.Documents.DeleteOneAsync(where, cancellationToken).ConfigureAwait(false);

            return result.DeletedCount > 0;
        }

        /// <inheritdoc />
        public async Task<bool> DeleteAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            ArgumentsValidator.ThrowIfIsNull(entities);

            var resultFlag = false;

            if (entities.Any())
            {
                var where = this.ExpressionsFactory.GetWhereByQueryName(QueriesNames.DeleteMany, entities);

                var result = await this.Documents
                    .DeleteManyAsync(where, cancellationToken)
                    .ConfigureAwait(false);

                resultFlag = result.DeletedCount == entities.LongCount();
            }

            return resultFlag;
        }

        /// <inheritdoc />
        public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> where, CancellationToken cancellationToken = default)
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

        /// <inheritdoc />
        public async Task<long> GetCountAsync(Expression<Func<TEntity, bool>> where = null, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            where ??= this.ExpressionsFactory.GetWhereByQueryName(QueriesNames.DefaultCount);

            return await this.Documents
                .CountDocumentsAsync(where, new CountOptions(), cancellationToken)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<TEntity>> GetPageAsync(
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

        /// <inheritdoc />
        public async Task<bool> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            ArgumentsValidator.ThrowIfIsNull(entity);

            var where = this.ExpressionsFactory.GetWhereByQueryName(QueriesNames.UpdateOne, entity);

            var options = new ReplaceOptions()
            {
                IsUpsert = true,
            };

            var result = await this.Documents
                .ReplaceOneAsync(where, entity, options, cancellationToken)
                .ConfigureAwait(false);

            return result.ModifiedCount == 1;
        }

        /// <inheritdoc />
        public async Task<bool> UpdateAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            ArgumentsValidator.ThrowIfIsNull(entities);

            var resultFlag = false;

            if (entities.Any())
            {
                var options = new BulkWriteOptions() { IsOrdered = false };

                var updates = entities
                    .Select(x =>
                    {
                        var where = this.ExpressionsFactory.GetWhereByQueryName(QueriesNames.UpdateOne, x);

                        return new ReplaceOneModel<TEntity>(where, x);
                    })
                    .ToList<WriteModel<TEntity>>();

                var result = await this.Documents
                    .BulkWriteAsync(updates, options, cancellationToken)
                    .ConfigureAwait(false);

                resultFlag = result.ModifiedCount == entities.LongCount();
            }

            return resultFlag;
        }

        private static SortDefinition<TEntity> GetSortDefinition(
            bool orderAsAscending,
            Expression<Func<TEntity, object>> orderBy) => orderAsAscending
                ? Builders<TEntity>.Sort.Ascending(orderBy)
                : Builders<TEntity>.Sort.Descending(orderBy);
    }
}
