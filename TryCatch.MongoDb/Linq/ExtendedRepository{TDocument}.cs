// <copyright file="ExtendedRepository{TDocument}.cs" company="TryCatch Software Factory">
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
    using TryCatch.Patterns.Repositories.Linq;
    using TryCatch.Validators;

    /// <summary>
    /// Abstract repository of IRepository{TDocument}.
    /// </summary>
    /// <typeparam name="TDocument">Type of document.</typeparam>
    public abstract class ExtendedRepository<TDocument> : IExtendedRepository<TDocument>
    {
        private const int DefaultLimit = 1000;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExtendedRepository{TDocument}"/> class.
        /// </summary>
        /// <param name="dbContext">IDbContext reference.</param>
        protected ExtendedRepository(IDbContext dbContext)
        {
            ArgumentsValidator.ThrowIfIsNull(dbContext);

            this.Documents = dbContext.Get<TDocument>();
        }

        protected IMongoCollection<TDocument> Documents { get; }

        /// <inheritdoc />
        public async virtual Task<bool> CreateAsync(TDocument entity, CancellationToken cancellationToken = default)
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
        public async virtual Task<bool> CreateAsync(IEnumerable<TDocument> entities, CancellationToken cancellationToken = default)
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
        public async virtual Task<bool> CreateOrUpdateAsync(TDocument entity, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            ArgumentsValidator.ThrowIfIsNull(entity);

            var where = this.GetQuery(entity);

            var options = new ReplaceOptions()
            {
                IsUpsert = true,
            };

            var result = await this.Documents
                .ReplaceOneAsync(where, entity, options, cancellationToken)
                .ConfigureAwait(false);

            return result.ModifiedCount > 0 || (result.UpsertedId != null);
        }

        /// <inheritdoc />
        public async virtual Task<bool> DeleteAsync(TDocument entity, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            ArgumentsValidator.ThrowIfIsNull(entity);

            var where = this.GetQuery(entity);

            var result = await this.Documents.DeleteOneAsync(where, cancellationToken).ConfigureAwait(false);

            return result.DeletedCount > 0;
        }

        /// <inheritdoc />
        public async virtual Task<bool> DeleteAsync(IEnumerable<TDocument> entities, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            ArgumentsValidator.ThrowIfIsNull(entities);

            var resultFlag = false;

            if (entities.Any())
            {
                var where = this.GetManyQuery(entities);

                var result = await this.Documents
                    .DeleteManyAsync(where, cancellationToken)
                    .ConfigureAwait(false);

                resultFlag = result.DeletedCount == entities.LongCount();
            }

            return resultFlag;
        }

        /// <inheritdoc />
        public async virtual Task<TDocument> GetAsync(Expression<Func<TDocument, bool>> spec, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            ArgumentsValidator.ThrowIfIsNull(spec);

            var options = new FindOptions<TDocument> { };

            var result = await this.Documents
                .FindAsync(spec, options, cancellationToken)
                .ConfigureAwait(false);

            return await result
                .FirstOrDefaultAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async virtual Task<long> GetCountAsync(Expression<Func<TDocument, bool>> spec = null, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            spec ??= this.GetDefaultQuery();

            return await this.Documents
                .CountDocumentsAsync(spec, new CountOptions(), cancellationToken)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async virtual Task<IEnumerable<TDocument>> GetPageAsync(
            int offset = 1,
            int limit = DefaultLimit,
            Expression<Func<TDocument, bool>> where = null,
            Expression<Func<TDocument, object>> orderBy = null,
            bool orderAsAscending = true,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            ArgumentsValidator.ThrowIfIsLessThan(1, offset, $"Offset value is invalid: {offset}");
            ArgumentsValidator.ThrowIfIsLessThan(1, limit, $"Limit value is invalid: {limit}");

            where ??= this.GetDefaultQuery();
            orderBy ??= this.GetDefaultOrderByQuery();

            var options = new FindOptions<TDocument>()
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
        public async Task<bool> UpdateAsync(TDocument entity, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            ArgumentsValidator.ThrowIfIsNull(entity);

            var where = this.GetQuery(entity);

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
        public async Task<bool> UpdateAsync(IEnumerable<TDocument> entities, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            ArgumentsValidator.ThrowIfIsNull(entities);

            var resultFlag = false;

            if (entities.Any())
            {
                var options = new BulkWriteOptions() { IsOrdered = false };

                var updates = entities
                    .Select(x => new ReplaceOneModel<TDocument>(this.GetQuery(x), x))
                    .ToList<WriteModel<TDocument>>();

                var result = await this.Documents
                    .BulkWriteAsync(updates, options, cancellationToken)
                    .ConfigureAwait(false);

                resultFlag = result.ModifiedCount == entities.LongCount();
            }

            return resultFlag;
        }

        protected abstract Expression<Func<TDocument, bool>> GetQuery(TDocument document);

        protected abstract Expression<Func<TDocument, bool>> GetManyQuery(IEnumerable<TDocument> documents);

        protected abstract Expression<Func<TDocument, bool>> GetDefaultQuery();

        protected abstract Expression<Func<TDocument, object>> GetDefaultOrderByQuery();

        private static SortDefinition<TDocument> GetSortDefinition(
            bool orderAsAscending,
            Expression<Func<TDocument, object>> orderBy) => orderAsAscending
                ? Builders<TDocument>.Sort.Ascending(orderBy)
                : Builders<TDocument>.Sort.Descending(orderBy);
    }
}
