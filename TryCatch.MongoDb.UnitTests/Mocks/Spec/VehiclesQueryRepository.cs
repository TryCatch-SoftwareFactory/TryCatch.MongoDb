// <copyright file="VehiclesQueryRepository.cs" company="TryCatch Software Factory">
// Copyright © TryCatch Software Factory All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

namespace TryCatch.MongoDb.UnitTests.Mocks.Spec
{
    using TryCatch.MongoDb.Context;
    using TryCatch.MongoDb.Spec;
    using TryCatch.MongoDb.UnitTests.Mocks.Models;

    public class VehiclesQueryRepository : QueryRepository<Vehicle>
    {
        public VehiclesQueryRepository(IDbContext dbContext, VehiclesExpressionFactory expressionFactory)
            : base(dbContext, expressionFactory)
        {
        }
    }
}
