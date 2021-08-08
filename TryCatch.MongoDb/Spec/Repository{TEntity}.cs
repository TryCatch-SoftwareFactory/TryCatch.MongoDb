// <copyright file="Repository{TEntity}.cs" company="TryCatch Software Factory">
// Copyright © TryCatch Software Factory All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

namespace TryCatch.MongoDb.Spec
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using TryCatch.MongoDb.Context;
    using TryCatch.Patterns.Repositories;
    using TryCatch.Patterns.Specifications;
    using TryCatch.Patterns.Specifications.Linq;
    using TryCatch.Validators;

    /// <summary>
    /// ISpecRepository implementation for Mongo DB.
    /// </summary>
    /// <typeparam name="TEntity">Type of document.</typeparam>
    public abstract class Repository<TEntity> : Linq.Repository<TEntity>, ISpecRepository<TEntity>
        where TEntity : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Repository{TEntity}"/> class.
        /// </summary>
        /// <param name="dbContext">Reference to Mongo DB context.</param>
        /// <param name="expressionsFactory">Reference to expressions factory.</param>
        protected Repository(IDbContext dbContext, IExpressionsFactory<TEntity> expressionsFactory)
            : base(dbContext, expressionsFactory)
        {
        }

        /// <inheritdoc/>
        public async Task<bool> DeleteAsync(ISpecification<TEntity> spec, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var where = spec as ILinqSpecification<TEntity>;

            ArgumentsValidator.ThrowIfIsNull(spec);

            var entityToDelete = await this.GetAsync(where.AsExpression(), cancellationToken).ConfigureAwait(false);

            if (entityToDelete != default(TEntity))
            {
                return await this.DeleteAsync(entityToDelete, cancellationToken).ConfigureAwait(false);
            }

            return false;
        }

        /// <inheritdoc/>
        public async Task<TEntity> GetAsync(ISpecification<TEntity> where, CancellationToken cancellationToken = default)
        {
            var spec = where as ILinqSpecification<TEntity>;

            ArgumentsValidator.ThrowIfIsNull(spec);

            return await this.GetAsync(spec.AsExpression(), cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task<long> GetCountAsync(ISpecification<TEntity> where = null, CancellationToken cancellationToken = default)
        {
            var spec = where as ILinqSpecification<TEntity>;

            return await this.GetCountAsync(spec?.AsExpression(), cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<TEntity>> GetPageAsync(
            int offset = 1,
            int limit = 1000,
            ISpecification<TEntity> where = null,
            ISortSpecification<TEntity> orderBy = null,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            ArgumentsValidator.ThrowIfIsLessThan(1, offset, $"Offset value is invalid: {offset}");
            ArgumentsValidator.ThrowIfIsLessThan(1, limit, $"Limit value is invalid: {limit}");

            var spec = where as ILinqSpecification<TEntity>;

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
