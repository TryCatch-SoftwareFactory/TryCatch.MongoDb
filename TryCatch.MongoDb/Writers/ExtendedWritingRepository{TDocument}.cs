// <copyright file="ExtendedWritingRepository{TDocument}.cs" company="TryCatch Software Factory">
// Copyright © TryCatch Software Factory All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

namespace TryCatch.MongoDb.Writers
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
    /// Abstract extended writing repository. Allows working with Mongo DB Documents.
    /// </summary>
    /// <typeparam name="TDocument">Type of document.</typeparam>
    public abstract class ExtendedWritingRepository<TDocument> : IExtendedWritingRepository<TDocument>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExtendedWritingRepository{TDocument}"/> class.
        /// </summary>
        /// <param name="dbContext">IDbContext reference.</param>
        protected ExtendedWritingRepository(IDbContext dbContext)
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

            return result.ModifiedCount > 0 || result.UpsertedId != null;
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
        public async virtual Task<bool> UpdateAsync(TDocument entity, CancellationToken cancellationToken = default)
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
        public async virtual Task<bool> UpdateAsync(IEnumerable<TDocument> entities, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            ArgumentsValidator.ThrowIfIsNull(entities);

            var resultFlag = false;

            if (entities.Any())
            {
                var options = new BulkWriteOptions()
                {
                    IsOrdered = false,
                };

                var updates = entities
                    .Select(x => new ReplaceOneModel<TDocument>(this.GetQuery(x), x) { IsUpsert = true })
                    .ToList<WriteModel<TDocument>>();

                var result = await this.Documents
                    .BulkWriteAsync(updates, options, cancellationToken)
                    .ConfigureAwait(false);

                resultFlag = (result.ModifiedCount + result.InsertedCount) == entities.LongCount();
            }

            return resultFlag;
        }

        protected abstract Expression<Func<TDocument, bool>> GetQuery(TDocument document);

        protected abstract Expression<Func<TDocument, bool>> GetManyQuery(IEnumerable<TDocument> documents);
    }
}
