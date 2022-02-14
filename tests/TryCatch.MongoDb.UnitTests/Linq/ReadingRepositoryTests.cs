// <copyright file="ReadingRepositoryTests.cs" company="TryCatch Software Factory">
// Copyright © TryCatch Software Factory All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

namespace TryCatch.MongoDb.UnitTests.Linq
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using FluentAssertions;
    using TryCatch.MongoDb.UnitTests.Fixtures;
    using TryCatch.MongoDb.UnitTests.Mocks;
    using TryCatch.MongoDb.UnitTests.Mocks.Linq;
    using TryCatch.MongoDb.UnitTests.Mocks.Models;
    using Xunit;

    public class ReadingRepositoryTests : IClassFixture<MongoDbFixture>
    {
        private readonly VehiclesReadingRepository sut;

        public ReadingRepositoryTests(MongoDbFixture mongoDbTest)
        {
            this.sut = new VehiclesReadingRepository(mongoDbTest.Context);
        }

        [Fact]
        public async Task GetAsync_without_where()
        {
            // Arrange
            Expression<Func<Vehicle, bool>> where = null;

            // Act
            Func<Task> act = async () => await this.sut.GetAsync(where).ConfigureAwait(false);

            // Asserts
            await act.Should().ThrowAsync<ArgumentNullException>().ConfigureAwait(false);
        }

        [Fact]
        public async Task GetAsync_ok()
        {
            // Arrange
            var expectedEntity = Given.GetVehicle;

            // Act
            var actual = await this.sut.GetAsync(Given.ReadWhere).ConfigureAwait(false);

            // Asserts
            actual.Should().BeEquivalentTo(expectedEntity);
        }

        [Fact]
        public async Task GetCountAsync_without_where()
        {
            // Arrange
            Expression<Func<Vehicle, bool>> where = null;
            var expectedLength = Given.GetVehicles.Count();

            // Act
            var actual = await this.sut.GetCountAsync(where).ConfigureAwait(false);

            // Asserts
            actual.Should().Be(expectedLength);
        }

        [Fact]
        public async Task GetCountAsync_with_where()
        {
            // Arrange
            var expected = 1;

            // Act
            var actual = await this.sut.GetCountAsync(Given.ReadWhere).ConfigureAwait(false);

            // Asserts
            actual.Should().Be(expected);
        }

        [Theory]
        [InlineData(-1, 1)]
        [InlineData(1, -1)]
        public async Task GetPageAsync_with_invalid_args(int offset, int limit)
        {
            // Arrange

            // Act
            Func<Task> act = async () => await this.sut
                .GetPageAsync(offset, limit)
                .ConfigureAwait(false);

            // Asserts
            await act.Should()
                .ThrowAsync<ArgumentOutOfRangeException>()
                .ConfigureAwait(false);
        }

        [Theory]
        [InlineData(2, 2, 2)]
        [InlineData(1, 1, 1)]
        public async Task GetPageAsync_with_valid_args(int offset, int limit, int expectedLength)
        {
            // Arrange

            // Act
            var actual = await this.sut
                .GetPageAsync(limit: limit, offset: offset)
                .ConfigureAwait(false);

            // Asserts
            actual.Should().HaveCount(expectedLength);
        }

        [Fact]
        public async Task GetPageAsync_with_where()
        {
            // Arrange
            var where = Given.ListWhere;
            var expected = Given.GetFilteredVehicles();

            // Act
            var actual = await this.sut
                .GetPageAsync(where: where)
                .ConfigureAwait(false);

            // Asserts
            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task GetPageAsync_with_default_where()
        {
            // Arrange
            var expected = Given.GetVehicles;

            // Act
            var actual = await this.sut
                .GetPageAsync()
                .ConfigureAwait(false);

            // Asserts
            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task GetPageAsync_orderBy_field_asc()
        {
            // Arrange
            var orderBy = Given.OrderBy;
            var expected = Given.GetVehicles.OrderBy(x => x.Name);

            // Act
            var actual = await this.sut
                .GetPageAsync(orderBy: orderBy)
                .ConfigureAwait(false);

            // Asserts
            actual.Should().BeEquivalentTo(expected).And.BeInAscendingOrder(x => x.Name);
        }

        [Fact]
        public async Task GetPageAsync_orderBy_field_desc()
        {
            // Arrange
            var orderBy = Given.OrderBy;
            var expected = Given.GetVehicles;

            // Act
            var actual = await this.sut
                .GetPageAsync(
                    orderBy: orderBy,
                    orderAsAscending: false)
                .ConfigureAwait(false);

            // Asserts
            actual.Should().BeEquivalentTo(expected).And.BeInDescendingOrder(x => x.Name);
        }
    }
}
