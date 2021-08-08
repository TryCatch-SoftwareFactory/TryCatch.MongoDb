// <copyright file="CollectionNotFoundException.cs" company="TryCatch Software Factory">
// Copyright © TryCatch Software Factory All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

namespace TryCatch.MongoDb.Context
{
    using System;

    /// <summary>
    /// Represents errors that occur when Documents collection are requested but it has not registered yet.
    /// </summary>
    public class CollectionNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CollectionNotFoundException"/> class.
        /// </summary>
        public CollectionNotFoundException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CollectionNotFoundException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public CollectionNotFoundException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CollectionNotFoundException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The <see cref="Exception"/> that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
        public CollectionNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
