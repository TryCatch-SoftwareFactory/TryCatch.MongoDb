﻿// <copyright file="VehiclesRepository.cs" company="TryCatch Software Factory">
// Copyright © TryCatch Software Factory All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

namespace TryCatch.MongoDb.UnitTests.Mocks.Linq
{
    using TryCatch.MongoDb.Context;
    using TryCatch.MongoDb.Linq;
    using TryCatch.MongoDb.UnitTests.Mocks.Models;

    public class VehiclesRepository : Repository<Vehicle>
    {
        public VehiclesRepository(IDbContext dbContext, VehiclesExpressionFactory expressionFactory)
            : base(dbContext, expressionFactory)
        {
        }
    }
}
