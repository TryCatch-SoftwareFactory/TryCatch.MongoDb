// <copyright file="CommandRepositoryTests.cs" company="TryCatch Software Factory">
// Copyright © TryCatch Software Factory All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

namespace TryCatch.MongoDb.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using FluentAssertions;
    using TryCatch.MongoDb.UnitTests.Fixtures;
    using TryCatch.MongoDb.UnitTests.Mocks;
    using TryCatch.MongoDb.UnitTests.Mocks.Linq;
    using TryCatch.MongoDb.UnitTests.Mocks.Models;
    using Xunit;

    public class CommandRepositoryTests : IClassFixture<MongoDbFixture>
    {
        private const string TestName = "LINQ-COMMAND-REPOSITORY-TEST";

        private readonly VehiclesCommandRepository sut;

        public CommandRepositoryTests(MongoDbFixture mongoDbTest)
        {
            var factory = new VehiclesExpressionFactory();

            this.sut = new VehiclesCommandRepository(mongoDbTest.Context, factory);
        }

        [Fact]
        public async Task Add_without_entity()
        {
            // Arrange
            Vehicle entity = null;

            // Act
            Func<Task> act = async () => await this.sut.AddAsync(entity).ConfigureAwait(false);

            // Asserts
            await act.Should().ThrowAsync<ArgumentNullException>().ConfigureAwait(false);
        }

        [Fact]
        public async Task Add_ok()
        {
            // Arrange
            var entity = DocumentsFactory.GetDocument<Vehicle>();

            // Act
            var actual = await this.sut.AddAsync(entity).ConfigureAwait(false);

            // Asserts
            actual.Should().BeTrue();
        }

        [Fact]
        public async Task AddOrUpdate_without_entity()
        {
            // Arrange
            Vehicle entity = null;

            // Act
            Func<Task> act = async () => await this.sut.AddOrUpdateAsync(entity).ConfigureAwait(false);

            // Asserts
            await act.Should().ThrowAsync<ArgumentNullException>().ConfigureAwait(false);
        }

        [Fact]
        public async Task AddOrUpdate_new_entity_ok()
        {
            // Arrange
            var entity = DocumentsFactory.GetDocument<Vehicle>();

            // Act
            var actual = await this.sut.AddOrUpdateAsync(entity).ConfigureAwait(false);

            // Asserts
            actual.Should().BeTrue();
        }

        [Fact]
        public async Task AddOrUpdate_old_entity_ok()
        {
            // Arrange
            var entity = Given.VehicleToUpdate;

            entity.Name = $"{entity.Name}-MODIFIED-{TestName}-2";

            // Act
            var actual = await this.sut.AddOrUpdateAsync(entity).ConfigureAwait(false);

            // Asserts
            actual.Should().BeTrue();
        }

        [Fact]
        public async Task Add_without_entities()
        {
            // Arrange
            IEnumerable<Vehicle> entities = null;

            // Act
            Func<Task> act = async () => await this.sut.AddAsync(entities).ConfigureAwait(false);

            // Asserts
            await act.Should().ThrowAsync<ArgumentNullException>().ConfigureAwait(false);
        }

        [Fact]
        public async Task Add_with_empty_entities()
        {
            // Arrange
            var entities = Array.Empty<Vehicle>();

            // Act
            var actual = await this.sut.AddAsync(entities).ConfigureAwait(false);

            // Asserts
            actual.Should().BeFalse();
        }

        [Fact]
        public async Task Add_entities_ok()
        {
            // Arrange
            var entities = DocumentsFactory.GetDocuments<Vehicle>(10);

            // Act
            var actual = await this.sut.AddAsync(entities).ConfigureAwait(false);

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
        public async Task Update_without_entities()
        {
            // Arrange
            IEnumerable<Vehicle> entities = null;

            // Act
            Func<Task> act = async () => await this.sut.UpdateAsync(entities).ConfigureAwait(false);

            // Asserts
            await act.Should().ThrowAsync<ArgumentNullException>().ConfigureAwait(false);
        }

        [Fact]
        public async Task Update_with_empty_entities()
        {
            // Arrange
            var entities = Array.Empty<Vehicle>() as IEnumerable<Vehicle>;

            // Act
            var actual = await this.sut.UpdateAsync(entities).ConfigureAwait(false);

            // Asserts
            actual.Should().BeFalse();
        }

        [Fact]
        public async Task Update_with_non_exists_entities()
        {
            // Arrange
            var entities = DocumentsFactory.GetDocuments<Vehicle>(10);

            // Act
            var actual = await this.sut.UpdateAsync(entities).ConfigureAwait(false);

            // Asserts
            actual.Should().BeFalse();
        }

        [Fact]
        public async Task Update_entities_ok()
        {
            // Arrange
            var entities = Given.VehiclesToUpdate;

            entities = entities.Select(x => new Vehicle() { Id = x.Id, Name = $"{x.Name}-modified-{TestName}" });

            // Act
            var actual = await this.sut.UpdateAsync(entities).ConfigureAwait(false);

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
            var entity = Given.VehicleToDeleteLinqCommandTest;

            // Act
            var actual = await this.sut.DeleteAsync(entity).ConfigureAwait(false);

            // Asserts
            actual.Should().BeTrue();
        }

        [Fact]
        public async Task Delete_without_entities()
        {
            // Arrange
            IEnumerable<Vehicle> entities = null;

            // Act
            Func<Task> act = async () => await this.sut.AddAsync(entities).ConfigureAwait(false);

            // Asserts
            await act.Should().ThrowAsync<ArgumentNullException>().ConfigureAwait(false);
        }

        [Fact]
        public async Task Delete_with_empty_entities()
        {
            // Arrange
            var entities = Array.Empty<Vehicle>();

            // Act
            var actual = await this.sut.DeleteAsync(entities).ConfigureAwait(false);

            // Asserts
            actual.Should().BeFalse();
        }

        [Fact]
        public async Task Delete_entities_ok()
        {
            // Arrange
            var entities = Given.VehiclesToDeleteLinqCommandTest;

            // Act
            var actual = await this.sut.DeleteAsync(entities).ConfigureAwait(false);

            // Asserts
            actual.Should().BeTrue();
        }
    }
}
