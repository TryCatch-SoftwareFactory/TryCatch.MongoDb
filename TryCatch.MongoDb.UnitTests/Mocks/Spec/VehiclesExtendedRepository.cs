// <copyright file="VehiclesExtendedRepository.cs" company="TryCatch Software Factory">
// Copyright © TryCatch Software Factory All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

namespace TryCatch.MongoDb.UnitTests.Mocks.Spec
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using TryCatch.MongoDb.Context;
    using TryCatch.MongoDb.Spec;
    using TryCatch.MongoDb.UnitTests.Mocks.Models;

    public class VehiclesExtendedRepository : ExtendedRepository<Vehicle>
    {
        public VehiclesExtendedRepository(IDbContext dbContext)
            : base(dbContext)
        {
        }

        protected override Expression<Func<Vehicle, object>> GetDefaultOrderByQuery() => (x) => x.Name;

        protected override Expression<Func<Vehicle, bool>> GetDefaultQuery() => (x) => x.Name.Contains("read-Name");

        protected override Expression<Func<Vehicle, bool>> GetManyQuery(IEnumerable<Vehicle> documents)
        {
            var ids = documents.Select(x => x.Id);

            return (x) => ids.Contains(x.Id);
        }

        protected override Expression<Func<Vehicle, bool>> GetQuery(Vehicle document) => (x) => x.Id == document.Id;
    }
}
