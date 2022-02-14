// <copyright file="MongoDbFixture.cs" company="TryCatch Software Factory">
// Copyright © TryCatch Software Factory All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

namespace TryCatch.MongoDb.UnitTests.Fixtures
{
    using System;
    using Mongo2Go;
    using MongoDB.Driver;
    using TryCatch.MongoDb.UnitTests.Mocks;
    using TryCatch.MongoDb.UnitTests.Mocks.Models;

    public class MongoDbFixture : IDisposable
    {
        private const string DatabaseName = "dbName";

#pragma warning disable IDE0044 // Agregar modificador de solo lectura
        private static object dbLock = new object();
#pragma warning restore IDE0044 // Agregar modificador de solo lectura

        private static int counter;

        private static MongoDbRunner runner;

        private readonly IMongoDatabase database;

        private bool isDisposed;

        public MongoDbFixture()
        {
            lock (dbLock)
            {
                if (counter < 1)
                {
                    runner = MongoDbRunner.Start(singleNodeReplSet: true);

                    var client = new MongoClient(runner.ConnectionString);

                    this.database = client.GetDatabase(DatabaseName);

                    Given.SeedDatabase(this.database);
                }
                else
                {
                    var client = new MongoClient(runner.ConnectionString);

                    this.database = client.GetDatabase(DatabaseName);
                }

                counter++;
            }
        }

        public VehiclesContext Context => new VehiclesContext(this.database);

        public void Dispose()
        {
            this.Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this.isDisposed)
            {
                return;
            }

            if (disposing)
            {
                counter--;

                if (counter < 1)
                {
                    runner.Dispose();
                }
            }

            this.isDisposed = true;
        }
    }
}
