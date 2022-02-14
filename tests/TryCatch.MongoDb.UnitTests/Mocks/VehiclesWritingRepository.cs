// <copyright file="VehiclesWritingRepository.cs" company="TryCatch Software Factory">
// Copyright © TryCatch Software Factory All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

namespace TryCatch.MongoDb.UnitTests.Mocks
{
    using System;
    using System.Linq.Expressions;
    using TryCatch.MongoDb.UnitTests.Mocks.Models;
    using TryCatch.MongoDb.Writers;

    public class VehiclesWritingRepository : WritingRepository<Vehicle>
    {
        public VehiclesWritingRepository(VehiclesContext dbContext)
            : base(dbContext)
        {
        }

        protected override Expression<Func<Vehicle, bool>> GetQuery(Vehicle document) => (x) => x.Id == document.Id;
    }
}
