// <copyright file="DbContextTests.cs" company="TryCatch Software Factory">
// Copyright © TryCatch Software Factory All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

namespace TryCatch.MongoDb.UnitTests.Context
{
    using System;
    using FluentAssertions;
    using MongoDB.Driver;
    using TryCatch.MongoDb.Context;
    using TryCatch.MongoDb.UnitTests.Mocks.Models;
    using Xunit;

    public class DbContextTests
    {
        private readonly VehiclesContext sut;

        public DbContextTests()
        {
            var databse = NSubstitute.Substitute.For<IMongoDatabase>();

            this.sut = new VehiclesContext(databse);
        }

        [Fact]
        public void Construct_Without_Database_Ref()
        {
            // Arrange
            IMongoDatabase database = null;

            // Act
            Action actual = () => _ = new VehiclesContext(database);

            // Asserts
            actual.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Add_Collection_Ok()
        {
            // Arrange

            // Act
            var actual = this.sut.Add<Train>(typeof(Train).Name);

            // Asserts
            actual.Should().BeTrue();
        }

        [Fact]
        public void Add_Twice_Collection_Ok()
        {
            // Arrange

            // Act
            var actual = this.sut.Add<Vehicle>(typeof(Vehicle).Name);

            // Asserts
            actual.Should().BeFalse();
        }

        [Fact]
        public void Get_Collection_Ok()
        {
            // Arrange

            // Act
            var actual = this.sut.Get<Vehicle>();

            // Asserts
            actual.Should().NotBeNull();
        }

        [Fact]
        public void Get_Not_Found_Collection_Ok()
        {
            // Arrange

            // Act
            Action actual = () => _ = this.sut.Get<Train>();

            // Asserts
            actual.Should().Throw<CollectionNotFoundException>();
        }
    }
}
