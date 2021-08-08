// <copyright file="VehiclesCommandRepository.cs" company="TryCatch Software Factory">
// Copyright © TryCatch Software Factory All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

namespace TryCatch.MongoDb.UnitTests.Mocks.Linq
{
    using TryCatch.MongoDb;
    using TryCatch.MongoDb.UnitTests.Mocks.Models;

    public class VehiclesCommandRepository : CommandRepository<Vehicle>
    {
        public VehiclesCommandRepository(VehiclesContext dbContext, VehiclesExpressionFactory expressionFactory)
            : base(dbContext, expressionFactory)
        {
        }
    }
}
