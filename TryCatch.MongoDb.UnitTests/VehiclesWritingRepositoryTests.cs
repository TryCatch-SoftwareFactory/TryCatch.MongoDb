// <copyright file="VehiclesWritingRepositoryTests.cs" company="TryCatch Software Factory">
// Copyright © TryCatch Software Factory All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

namespace TryCatch.MongoDb.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using FluentAssertions;
    using TryCatch.MongoDb.UnitTests.Fixtures;
    using TryCatch.MongoDb.UnitTests.Mocks;
    using TryCatch.MongoDb.UnitTests.Mocks.Models;
    using Xunit;

    public class VehiclesWritingRepositoryTests : IClassFixture<MongoDbFixture>
    {
        private const string TestName = "LINQ-WRITING-REPOSITORY-TEST";

        private readonly VehiclesWritingRepository sut;

        public VehiclesWritingRepositoryTests(MongoDbFixture mongoDbTest)
        {
            this.sut = new VehiclesWritingRepository(mongoDbTest.Context);
        }

        [Fact]
        public async Task Create_without_entity()
        {
            // Arrange
            Vehicle entity = null;

            // Act
            Func<Task> act = async () => await this.sut.CreateAsync(entity).ConfigureAwait(false);

            // Asserts
            await act.Should().ThrowAsync<ArgumentNullException>().ConfigureAwait(false);
        }

        [Fact]
        public async Task Create_ok()
        {
            // Arrange
            var entity = DocumentsFactory.GetDocument<Vehicle>();

            // Act
            var actual = await this.sut.CreateAsync(entity).ConfigureAwait(false);

            // Asserts
            actual.Should().BeTrue();
        }

        [Fact]
        public async Task CreateOrUpdate_without_entity()
        {
            // Arrange
            Vehicle entity = null;

            // Act
            Func<Task> act = async () => await this.sut.CreateOrUpdateAsync(entity).ConfigureAwait(false);

            // Asserts
            await act.Should().ThrowAsync<ArgumentNullException>().ConfigureAwait(false);
        }

        [Fact]
        public async Task CreateOrUpdate_new_entity_ok()
        {
            // Arrange
            var entity = DocumentsFactory.GetDocument<Vehicle>();

            // Act
            var actual = await this.sut.CreateOrUpdateAsync(entity).ConfigureAwait(false);

            // Asserts
            actual.Should().BeTrue();
        }

        [Fact]
        public async Task CreateOrUpdate_old_entity_ok()
        {
            // Arrange
            var entity = Given.VehicleToUpdate;

            entity.Name = $"{entity.Name}-MODIFIED-{TestName}-2";

            // Act
            var actual = await this.sut.CreateOrUpdateAsync(entity).ConfigureAwait(false);

            // Asserts
            actual.Should().BeTrue();
        }

        [Fact]
        public async Task Update_without_entity()
        {
            // Arrange
            Vehicle entity = null;

            // Act
            Func<Task> act = async () => await this.sut.UpdateAsync(entity).ConfigureAwait(false);

            // Asserts
            await act.Should().ThrowAsync<ArgumentNullException>().ConfigureAwait(false);
        }

        [Fact]
        public async Task Update_with_non_exist_entity()
        {
            // Arrange
            var entity = DocumentsFactory.GetDocument<Vehicle>();

            // Act
            var actual = await this.sut.UpdateAsync(entity).ConfigureAwait(false);

            // Asserts
            actual.Should().BeFalse();
        }

        [Fact]
        public async Task Update_ok()
        {
            // Arrange
            var entity = Given.VehicleToUpdate;

            entity.Name = $"{entity.Name}-MODIFIED-{TestName}";

            // Act
            var actual = await this.sut.UpdateAsync(entity).ConfigureAwait(false);

            // Asserts
            actual.Should().BeTrue();
        }

        [Fact]
        public async Task Delete_without_entity()
        {
            // Arrange
            Vehicle entity = null;

            // Act
            Func<Task> act = async () => await this.sut.DeleteAsync(entity).ConfigureAwait(false);

            // Asserts
            await act.Should().ThrowAsync<ArgumentNullException>().ConfigureAwait(false);
        }

        [Fact]
        public async Task Delete_entity_ok()
        {
            // Arrange
            var entity = Given.VehicleToDeleteLinqWritingTest;

            // Act
            var actual = await this.sut.DeleteAsync(entity).ConfigureAwait(false);

            // Asserts
            actual.Should().BeTrue();
        }
    }
}