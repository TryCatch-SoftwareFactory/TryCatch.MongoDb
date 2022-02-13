// <copyright file="WritingRepository{TDocument}.cs" company="TryCatch Software Factory">
// Copyright © TryCatch Software Factory All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

namespace TryCatch.MongoDb.Writers
{
    using System;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;
    using MongoDB.Driver;
    using TryCatch.MongoDb.Context;
    using TryCatch.Patterns.Repositories;
    using TryCatch.Validators;

    /// <summary>
    /// Abstract writing repository. Allows working with Mongo DB Documents.
    /// </summary>
    /// <typeparam name="TDocument">Type of document.</typeparam>
    public abstract class WritingRepository<TDocument> : IWritingRepository<TDocument>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WritingRepository{TDocument}"/> class.
        /// </summary>
        /// <param name="dbContext">IDbContext reference.</param>
        protected WritingRepository(IDbContext dbContext)
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

        protected abstract Expression<Func<TDocument, bool>> GetQuery(TDocument document);
    }
}
