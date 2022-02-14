// <copyright file="DeleteVehicleSpec.cs" company="TryCatch Software Factory">
// Copyright © TryCatch Software Factory All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

namespace TryCatch.MongoDb.UnitTests.Mocks.Spec
{
    using System;
    using System.Linq.Expressions;
    using TryCatch.MongoDb.UnitTests.Mocks.Models;
    using TryCatch.Patterns.Specifications.Linq;
    using TryCatch.Validators;

    public class DeleteVehicleSpec : CompositeSpecification<Vehicle>, ILinqSpecification<Vehicle>
    {
        private readonly Guid id;

        public DeleteVehicleSpec(string id)
        {
            ArgumentsValidator.ThrowIfIsNullEmptyOrWhiteSpace(id);

            if (!Guid.TryParse(id, out this.id))
            {
                throw new ArgumentException("Invalid Id");
            }
        }

        public override Expression<Func<Vehicle, bool>> AsExpression() => x => x.Id == this.id;
    }
}
