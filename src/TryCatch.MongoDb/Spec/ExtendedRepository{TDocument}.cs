// <copyright file="ExtendedRepository{TDocument}.cs" company="TryCatch Software Factory">
// Copyright © TryCatch Software Factory All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

namespace TryCatch.MongoDb.Spec
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using TryCatch.MongoDb.Context;
    using TryCatch.Patterns.Specifications;
    using TryCatch.Patterns.Specifications.Linq;
    using TryCatch.Validators;

    /// <summary>
    /// Extended repository base on specifications.
    /// </summary>
    /// <typeparam name="TDocument">Type of document.</typeparam>
    public abstract class ExtendedRepository<TDocument> : Linq.ExtendedRepository<TDocument>, Patterns.Repositories.Spec.IExtendedRepository<TDocument>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExtendedRepository{TDocument}"/> class.
        /// </summary>
        /// <param name="dbContext">Reference to Mongo DB context.</param>
        protected ExtendedRepository(IDbContext dbContext)
            : base(dbContext)
        {
        }

        /// <inheritdoc/>
        public async Task<bool> DeleteAsync(ISpecification<TDocument> spec, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var where = spec as ILinqSpecification<TDocument>;

            ArgumentsValidator.ThrowIfIsNull(spec);

            var result = await this.Documents.DeleteOneAsync(where.AsExpression(), cancellationToken).ConfigureAwait(false);

            return result.DeletedCount > 0;
        }

        /// <inheritdoc/>
        public async Task<TDocument> GetAsync(ISpecification<TDocument> spec, CancellationToken cancellationToken = default)
        {
            var specs = spec as ILinqSpecification<TDocument>;

            ArgumentsValidator.ThrowIfIsNull(spec);

            return await this.GetAsync(specs.AsExpression(), cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task<long> GetCountAsync(ISpecification<TDocument> spec = null, CancellationToken cancellationToken = default)
        {
            var specs = spec as ILinqSpecification<TDocument>;

            return await this.GetCountAsync(specs?.AsExpression(), cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<TDocument>> GetPageAsync(
            int offset = 1,
            int limit = 1000,
            ISpecification<TDocument> where = null,
            ISortSpecification<TDocument> orderBy = null,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            ArgumentsValidator.ThrowIfIsLessThan(1, offset, $"Offset value is invalid: {offset}");
            ArgumentsValidator.ThrowIfIsLessThan(1, limit, $"Limit value is invalid: {limit}");

            var spec = where as ILinqSpecification<TDocument>;

            var orderAsAscending = (orderBy != null) && orderBy.IsAscending();

            return await this.GetPageAsync(
                offset,
                limit,
                spec?.AsExpression(),
                orderBy?.AsExpression(),
                orderAsAscending,
                cancellationToken)
                .ConfigureAwait(false);
        }
    }
}
