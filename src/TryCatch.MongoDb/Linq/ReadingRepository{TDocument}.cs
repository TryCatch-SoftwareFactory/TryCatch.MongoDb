// <copyright file="ReadingRepository{TDocument}.cs" company="TryCatch Software Factory">
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
    using TryCatch.Patterns.Repositories.Linq;
    using TryCatch.Validators;

    /// <summary>
    /// Abstract reading repository of IReadingRepository interface.
    /// </summary>
    /// <typeparam name="TDocument">Type of document.</typeparam>
    public abstract class ReadingRepository<TDocument> : IReadingRepository<TDocument>
    {
        private const int DefaultLimit = 1000;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadingRepository{TDocument}"/> class.
        /// </summary>
        /// <param name="dbContext">IDbContext reference.</param>
        protected ReadingRepository(IDbContext dbContext)
        {
            ArgumentsValidator.ThrowIfIsNull(dbContext);

            this.Documents = dbContext.Get<TDocument>();
        }

        protected IMongoCollection<TDocument> Documents { get; }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public async virtual Task<long> GetCountAsync(Expression<Func<TDocument, bool>> spec = null, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            spec ??= this.GetDefaultQuery();

            return await this.Documents
                .CountDocumentsAsync(spec, new CountOptions(), cancellationToken)
                .ConfigureAwait(false);
        }

        /// <inheritdoc/>
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

        protected abstract Expression<Func<TDocument, bool>> GetDefaultQuery();

        protected abstract Expression<Func<TDocument, object>> GetDefaultOrderByQuery();

        private static SortDefinition<TDocument> GetSortDefinition(
            bool orderAsAscending,
            Expression<Func<TDocument, object>> orderBy) => orderAsAscending
                ? Builders<TDocument>.Sort.Ascending(orderBy)
                : Builders<TDocument>.Sort.Descending(orderBy);
    }
}
