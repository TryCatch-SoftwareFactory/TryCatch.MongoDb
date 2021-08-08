// <copyright file="VehiclesExpressionFactory.cs" company="TryCatch Software Factory">
// Copyright © TryCatch Software Factory All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

namespace TryCatch.MongoDb.UnitTests.Mocks
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using TryCatch.MongoDb.UnitTests.Mocks.Models;
    using TryCatch.Validators;

    public class VehiclesExpressionFactory : IExpressionsFactory<Vehicle>
    {
        public Expression<Func<Vehicle, object>> GetSortByByQueryName(string queryName) =>
            queryName switch
            {
                _ => (x) => x.Name,
            };

        public Expression<Func<Vehicle, bool>> GetWhereByQueryName(string queryName, Vehicle entity = null) =>
            queryName switch
            {
                QueriesNames.DefaultGet => (x) => x.Id == entity.Id,
                QueriesNames.UpdateOne => (x) => x.Id == entity.Id,
                QueriesNames.DeleteOne => (x) => x.Id == entity.Id,
                _ => (x) => x.Name.Contains("read-Name")
            };

        public Expression<Func<Vehicle, bool>> GetWhereByQueryName(string queryName, IEnumerable<Vehicle> documents)
        {
            ArgumentsValidator.ThrowIfIsNull(documents);

            var ids = documents.Select(x => x.Id);

            return queryName switch
            {
                QueriesNames.DeleteMany => (x) => ids.Contains(x.Id),
                _ => null,
            };
        }
    }
}
