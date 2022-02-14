// <copyright file="ReadingRepository{TDocument}.cs" company="TryCatch Software Factory">
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
    /// Reading Repository based on Specifications.
    /// </summary>
    /// <typeparam name="TDocument">Type of document.</typeparam>
    public abstract class ReadingRepository<TDocument> : Linq.ReadingRepository<TDocument>, Patterns.Repositories.Spec.IReadingRepository<TDocument>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadingRepository{TDocument}"/> class.
        /// </summary>
        /// <param name="dbContext">IDbContext reference.</param>
        protected ReadingRepository(IDbContext dbContext)
            : base(dbContext)
        {
        }

        /// <inheritdoc/>
        public async virtual Task<TDocument> GetAsync(ISpecification<TDocument> spec, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var specs = spec as ILinqSpecification<TDocument>;

            ArgumentsValidator.ThrowIfIsNull(specs);

            return await this.GetAsync(specs.AsExpression(), cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async virtual Task<long> GetCountAsync(ISpecification<TDocument> spec = null, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var specs = spec as ILinqSpecification<TDocument>;

            return await this.GetCountAsync(specs?.AsExpression(), cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async virtual Task<IEnumerable<TDocument>> GetPageAsync(
            int offset = 1,
            int limit = 1000,
            ISpecification<TDocument> where = null,
            ISortSpecification<TDocument> orderBy = null,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            ArgumentsValidator.ThrowIfIsLessThan(1, offset, $"Offset value is invalid: {offset}");
            ArgumentsValidator.ThrowIfIsLessThan(1, limit, $"Limit value is invalid: {limit}");

            var specs = where as ILinqSpecification<TDocument>;

            return await this.GetPageAsync(
                offset,
                limit,
                specs?.AsExpression(),
                orderBy?.AsExpression(),
                orderBy is null || orderBy.IsAscending(),
                cancellationToken).ConfigureAwait(false);
        }
    }
}
