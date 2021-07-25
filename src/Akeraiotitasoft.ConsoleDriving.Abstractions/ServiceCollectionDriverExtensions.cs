// Copyright (c) .NET Foundation. All rights reserved.  (Original version)
// Copyright (c) Akeraiotitasoft LLC. All rights reserved.  For the ConsoleDriver derivative work.
// Licensed under the Apache License, Version 2.0. See License in the project root for license information.

using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akeraiotitasoft.ConsoleDriving
{
    /// <summary>
    /// Extension methods for <see cref="IServiceCollection"/>
    /// </summary>
    public static class ServiceCollectionDriverExtensions
    {
        /// <summary>
        /// Add an <see cref="IDriver"/> registration for the given type.
        /// </summary>
        /// <typeparam name="TDriver">An <see cref="IDriver"/> to register.</typeparam>
        /// <param name="serviceCollection">The <see cref="IServiceCollection"/> to register with.</param>
        /// <returns>The original <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddDriver<TDriver>(this IServiceCollection serviceCollection)
            where TDriver : class, IDriver
        {
            serviceCollection.AddTransient<IDriver, TDriver>();
            return serviceCollection;
        }
    }
}
