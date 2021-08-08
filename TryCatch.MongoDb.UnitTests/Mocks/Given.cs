// <copyright file="Given.cs" company="TryCatch Software Factory">
// Copyright © TryCatch Software Factory All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

namespace TryCatch.MongoDb.UnitTests.Mocks
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using MongoDB.Driver;
    using TryCatch.MongoDb.UnitTests.Mocks.Models;
    using TryCatch.MongoDb.UnitTests.Mocks.Spec;
    using TryCatch.Patterns.Specifications;

    public static class Given
    {
        private const string DefaultName = "read-Name-0";

        public static IEnumerable<Vehicle> GetVehicles => new[]
        {
            new Vehicle() { Id = new Guid("4b9d0962-a6cd-40dd-be3a-bd4ea98cc6f2"), Name = "read-Name-0" },
            new Vehicle() { Id = new Guid("9f653036-eff6-470e-9585-b3e010a9280b"), Name = "read-Name-1" },
            new Vehicle() { Id = new Guid("bb6b3e86-4721-4d26-8211-f491e6669dce"), Name = "read-Name-2" },
            new Vehicle() { Id = new Guid("05251b62-4742-415f-9ed5-a7e4e7c5e5da"), Name = "read-Name-3" },
            new Vehicle() { Id = new Guid("d9252ba7-fed7-41ce-966c-40bad7697067"), Name = "read-Name-4" },
            new Vehicle() { Id = new Guid("b32dab62-44e2-4557-a636-9fe5f287961c"), Name = "read-Name-5" },
            new Vehicle() { Id = new Guid("5a874846-ee41-468d-9af8-13bf98de7394"), Name = "read-Name-6" },
            new Vehicle() { Id = new Guid("72ce172a-97ab-4f35-a203-08c5a6b7bfea"), Name = "read-Name-7" },
            new Vehicle() { Id = new Guid("d3d5649f-caad-49be-b009-0a706cba4326"), Name = "read-Name-8" },
            new Vehicle() { Id = new Guid("f4c1d8a6-9cb3-400c-aadc-879a11573f10"), Name = "read-Name-9" },
            new Vehicle() { Id = new Guid("d631fd0e-f333-4847-8321-12ed2ecb8468"), Name = "read-Name-01" },
            new Vehicle() { Id = new Guid("d0e004af-97f8-4dd6-b72f-2916b55509b3"), Name = "read-Name-02" },
            new Vehicle() { Id = new Guid("f59312e2-b5b3-48aa-87a5-795641580622"), Name = "read-Name-03" },
            new Vehicle() { Id = new Guid("720a791e-85ff-4f04-bb15-9dd32902567c"), Name = "read-Name-04" },
            new Vehicle() { Id = new Guid("9bcb7ba0-16cf-4ae9-808a-8708d5aca3fc"), Name = "read-Name-05" },
            new Vehicle() { Id = new Guid("95652ebd-104c-41fa-8306-13e9e5955194"), Name = "read-Name-06" },
            new Vehicle() { Id = new Guid("07c8ded5-1e68-4983-9d4a-a6e46da68dd2"), Name = "read-Name-07" },
            new Vehicle() { Id = new Guid("888743f2-14df-4e74-b456-be7f450043f1"), Name = "read-Name-08" },
            new Vehicle() { Id = new Guid("854578a3-bf13-4a9f-b8e3-2644cad1fc8f"), Name = "read-Name-09" },
            new Vehicle() { Id = new Guid("2b64e2a7-8c7c-4eed-a5dc-83149839b81c"), Name = "read-Name-010" },
            new Vehicle() { Id = new Guid("41b0c644-916b-4b50-9ee1-bd126341ce15"), Name = "read-Name-011" },
            new Vehicle() { Id = new Guid("b3f87f6c-3b7e-43f6-8f63-822cd2b41371"), Name = "read-Name-012" },
            new Vehicle() { Id = new Guid("7f67fb77-0603-4d05-88eb-b6285f4c13b5"), Name = "read-Name-013" },
            new Vehicle() { Id = new Guid("a39bb01d-6a3c-4e60-b4a6-2d5e40a0ce71"), Name = "read-Name-014" },
            new Vehicle() { Id = new Guid("f4471e37-1934-4a06-b25a-b2810a3d267a"), Name = "read-Name-015" },
            new Vehicle() { Id = new Guid("76e5d03d-c787-4e82-a36d-cdf959e90951"), Name = "read-Name-016" },
            new Vehicle() { Id = new Guid("fcc59ca9-3c20-429d-8829-e7cd2c0f8dbc"), Name = "read-Name-017" },
            new Vehicle() { Id = new Guid("15f5ddbe-16f7-48ee-a070-ef182d329b22"), Name = "read-Name-018" },
            new Vehicle() { Id = new Guid("48f7d1cf-fab1-4b58-81a0-152f35c801a9"), Name = "read-Name-019" },
        };

        public static Vehicle GetVehicle => GetVehicles.AsQueryable().First(ReadWhere);

        public static Expression<Func<Vehicle, bool>> NotFoundWhere => (x) => x.Id == NotFoundVehicle.Id;

        public static Expression<Func<Vehicle, bool>> ReadWhere => (x) => x.Name == DefaultName;

        public static Expression<Func<Vehicle, bool>> ListWhere => (x) => x.Name.Contains("read-Name");

        public static Expression<Func<Vehicle, object>> OrderBy => (x) => x.Name;

        public static ISpecification<Vehicle> SpecReadWhere => new ReadVehicleSpec(DefaultName);

        public static ISpecification<Vehicle> SpecListWhere => new ListVehiclesSpec("read-Name");

        public static ISortSpecification<Vehicle> SpecOrderBy => new SortVehicleSpec(true, "Name");

        public static ISortSpecification<Vehicle> SpecOrderByDesc => new SortVehicleSpec(false, "Name");

        public static Vehicle VehicleToDeleteLinqCommandTest => new Vehicle
        {
            Id = new Guid("9e0d05c6-c702-4427-95bb-927654d87833"),
            Name = "vehicle-2-delete",
        };

        public static Vehicle VehicleToDeleteLinqRepositoryTest => new Vehicle
        {
            Id = new Guid("666963e8-30eb-4a13-b797-9a4d38717b80"),
            Name = "vehicle-2-delete",
        };

        public static Vehicle NotFoundVehicle => new Vehicle
        {
            Id = new Guid("b1c200f6-79f6-4c11-ab7e-13b41a9270e0"),
            Name = "not-found-vehicle-to-delete",
        };

        public static ISpecification<Vehicle> VehicleToDeleteSpecRepositoryTest => new DeleteVehicleSpec("0ca4b2ff-f3f6-4906-b261-3731fdee8f9b");

        public static ISpecification<Vehicle> NotFoundVehicleSpec => new DeleteVehicleSpec("b1c200f6-79f6-4c11-ab7e-13b41a9270e0");

        public static Vehicle VehicleToUpdate => new Vehicle
        {
            Id = new Guid("4afb30d4-98ed-4d18-9a48-58dc87009471"),
            Name = "vehicle-2-update",
        };

        public static IEnumerable<Vehicle> VehiclesToDeleteLinqCommandTest => new[]
        {
            new Vehicle() { Id = new Guid("07e3a3cb-5d8f-41cb-b496-f8b308d3321b"), Name = "Vehicles-to-delete" },
            new Vehicle() { Id = new Guid("1748373d-9a52-4cf0-ad9a-429e8b2284a9"), Name = "Vehicles-to-delete" },
            new Vehicle() { Id = new Guid("26052647-88ce-4c75-a754-9d1dbd71b5cd"), Name = "Vehicles-to-delete" },
            new Vehicle() { Id = new Guid("81536479-847d-47c0-96db-cdc2f94d4f26"), Name = "Vehicles-to-delete" },
            new Vehicle() { Id = new Guid("141dc072-0f7b-4e4e-93cd-99634933bfd6"), Name = "Vehicles-to-delete" },
            new Vehicle() { Id = new Guid("c49507c4-54a1-46f7-a133-4107b86aa293"), Name = "Vehicles-to-delete" },
            new Vehicle() { Id = new Guid("08a19c98-9183-402b-a308-d5c7ccebe093"), Name = "Vehicles-to-delete" },
        };

        public static IEnumerable<Vehicle> VehiclesToDeleteLinqRepositoryTest => new[]
        {
            new Vehicle() { Id = new Guid("56e6de90-a07f-46b5-916b-d8ccbacbd9b4"), Name = "Vehicles-to-delete" },
            new Vehicle() { Id = new Guid("3f498c54-86aa-4d30-8439-d0b8de6483e9"), Name = "Vehicles-to-delete" },
            new Vehicle() { Id = new Guid("f0feb54d-4b26-4b06-930b-dd283d384c93"), Name = "Vehicles-to-delete" },
            new Vehicle() { Id = new Guid("9cb6cd05-22cd-4a84-a966-417995cd8c54"), Name = "Vehicles-to-delete" },
            new Vehicle() { Id = new Guid("3af9550a-46fc-44d8-b0aa-6ddc29ff40b4"), Name = "Vehicles-to-delete" },
            new Vehicle() { Id = new Guid("4b772390-3057-4767-a380-710b582c95d0"), Name = "Vehicles-to-delete" },
            new Vehicle() { Id = new Guid("1cfa17a8-122f-4511-a77e-f9e950d85f3d"), Name = "Vehicles-to-delete" },
        };

        public static IEnumerable<Vehicle> VehiclesToDeleteSpecCommandTest => new[]
        {
            new Vehicle() { Id = new Guid("273ffc9d-509f-445f-8dda-df4c944112fb"), Name = "Vehicles-to-delete" },
            new Vehicle() { Id = new Guid("5fc900eb-b874-4b61-8130-e83f04e7a861"), Name = "Vehicles-to-delete" },
            new Vehicle() { Id = new Guid("e7e45698-f719-40d6-85bd-2e6334322731"), Name = "Vehicles-to-delete" },
            new Vehicle() { Id = new Guid("0482234a-8b64-478d-a9e6-ea68ff1e546c"), Name = "Vehicles-to-delete" },
            new Vehicle() { Id = new Guid("32f4e15c-b3ce-480d-b5a1-cb8608f6cfbe"), Name = "Vehicles-to-delete" },
            new Vehicle() { Id = new Guid("7f47018a-18a0-4926-9de4-cdb6155db047"), Name = "Vehicles-to-delete" },
            new Vehicle() { Id = new Guid("280eeec4-f9bb-4c3e-96f2-7ec0ca371ab4"), Name = "Vehicles-to-delete" },
        };

        public static IEnumerable<Vehicle> VehiclesToDeleteSpecRepositoryTest => new[]
        {
            new Vehicle() { Id = new Guid("8b5dd544-b8ba-4512-85b6-94e9a51497e6"), Name = "Vehicles-to-delete" },
            new Vehicle() { Id = new Guid("f2ca41a2-74d2-4fd5-9c4b-7b36b90c6d37"), Name = "Vehicles-to-delete" },
            new Vehicle() { Id = new Guid("59f836db-ed20-4363-a386-79b083145d34"), Name = "Vehicles-to-delete" },
            new Vehicle() { Id = new Guid("b8f36665-8afb-47e6-b663-785de17e8d0e"), Name = "Vehicles-to-delete" },
            new Vehicle() { Id = new Guid("9fafbe5d-bb42-4b24-90cd-5ae27bb09996"), Name = "Vehicles-to-delete" },
            new Vehicle() { Id = new Guid("76d054c8-1cf1-4a99-b659-e8f274f50e26"), Name = "Vehicles-to-delete" },
            new Vehicle() { Id = new Guid("c7c1434a-10d9-4cb6-af6b-cc8e11269600"), Name = "Vehicles-to-delete" },
        };

        public static IEnumerable<Vehicle> VehiclesToUpdate => new[]
        {
            new Vehicle() { Id = new Guid("642be69c-0518-466f-94fb-f8ec8ff1d625"), Name = "Vehicles-to-update" },
            new Vehicle() { Id = new Guid("1702e9da-21c0-47d6-af63-528ddb10e1a4"), Name = "Vehicles-to-update" },
            new Vehicle() { Id = new Guid("9ede017f-cc04-4043-a552-4989a9f8630f"), Name = "Vehicles-to-update" },
            new Vehicle() { Id = new Guid("c5f7c104-821f-4b67-ab5b-bdf746a383b1"), Name = "Vehicles-to-update" },
            new Vehicle() { Id = new Guid("387555f3-e33f-4bf5-8374-a076cf912299"), Name = "Vehicles-to-update" },
            new Vehicle() { Id = new Guid("dd2ddd7b-b105-4e30-bf89-71be436664ad"), Name = "Vehicles-to-update" },
        };

        public static IEnumerable<Vehicle> GetFilteredVehicles() => GetVehicles.AsQueryable().Where(ListWhere).ToList();

        public static void SeedDatabase(IMongoDatabase database)
        {
            var collection = database.GetCollection<Vehicle>(typeof(Vehicle).Name);

            InsertManyOptions options = null;
            InsertOneOptions oneOptions = null;

            collection.InsertMany(GetVehicles, options);

            collection.InsertMany(VehiclesToDeleteLinqCommandTest, options);
            collection.InsertMany(VehiclesToDeleteLinqRepositoryTest, options);
            collection.InsertMany(VehiclesToDeleteSpecCommandTest, options);
            collection.InsertMany(VehiclesToDeleteSpecRepositoryTest, options);
            collection.InsertMany(VehiclesToUpdate, options);

            collection.InsertOne(VehicleToDeleteLinqCommandTest, oneOptions);
            collection.InsertOne(VehicleToDeleteLinqRepositoryTest, oneOptions);
            collection.InsertOne(VehicleToUpdate, oneOptions);

            var vehicleToDeleteSpectRepositoryTest = new Vehicle()
            {
                Id = new Guid("0ca4b2ff-f3f6-4906-b261-3731fdee8f9b"),
                Name = "vehicle-to-delete-spec-repo",
            };

            collection.InsertOne(vehicleToDeleteSpectRepositoryTest, oneOptions);
        }
    }
}
