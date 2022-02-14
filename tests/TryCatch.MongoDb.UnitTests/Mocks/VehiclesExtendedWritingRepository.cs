// <copyright file="VehiclesExtendedWritingRepository.cs" company="TryCatch Software Factory">
// Copyright © TryCatch Software Factory All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

namespace TryCatch.MongoDb.UnitTests.Mocks.Linq
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using TryCatch.MongoDb.UnitTests.Mocks.Models;
    using TryCatch.MongoDb.Writers;

    public class VehiclesExtendedWritingRepository : ExtendedWritingRepository<Vehicle>
    {
        public VehiclesExtendedWritingRepository(VehiclesContext dbContext)
            : base(dbContext)
        {
        }

        protected override Expression<Func<Vehicle, bool>> GetManyQuery(IEnumerable<Vehicle> documents)
        {
            var ids = documents.Select(x => x.Id);

            return (x) => ids.Contains(x.Id);
        }

        protected override Expression<Func<Vehicle, bool>> GetQuery(Vehicle document) => (x) => x.Id == document.Id;
    }
}
