// <copyright file="VehiclesContext.cs" company="TryCatch Software Factory">
// Copyright © TryCatch Software Factory All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

namespace TryCatch.MongoDb.UnitTests.Mocks.Models
{
    using MongoDB.Driver;
    using TryCatch.MongoDb.Context;

    public class VehiclesContext : DbContext
    {
        public VehiclesContext(IMongoDatabase database)
            : base(database)
        {
            this.AddCollection<Vehicle>(typeof(Vehicle).Name);
        }

        public bool Add<TDocument>(string name) => this.AddCollection<TDocument>(name);
    }
}
