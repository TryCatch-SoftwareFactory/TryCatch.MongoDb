// <copyright file="VehiclesRepository.cs" company="TryCatch Software Factory">
// Copyright © TryCatch Software Factory All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

namespace TryCatch.MongoDb.UnitTests.Mocks.Spec
{
    using System;
    using System.Linq.Expressions;
    using TryCatch.MongoDb.Context;
    using TryCatch.MongoDb.Spec;
    using TryCatch.MongoDb.UnitTests.Mocks.Models;

    public class VehiclesRepository : Repository<Vehicle>
    {
        public VehiclesRepository(IDbContext dbContext)
            : base(dbContext)
        {
        }

        protected override Expression<Func<Vehicle, object>> GetDefaultOrderByQuery() => (x) => x.Name;

        protected override Expression<Func<Vehicle, bool>> GetDefaultQuery() => (x) => x.Name.Contains("read-Name");

        protected override Expression<Func<Vehicle, bool>> GetQuery(Vehicle document) => (x) => x.Id == document.Id;
    }
}
