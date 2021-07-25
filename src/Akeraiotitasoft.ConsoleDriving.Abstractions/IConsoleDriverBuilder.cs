// Copyright (c) .NET Foundation. All rights reserved.  (Original version)
// Copyright (c) Akeraiotitasoft LLC. All rights reserved.  For the ConsoleDriver derivative work.
// Licensed under the Apache License, Version 2.0. See License in the project root for license information.

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Akeraiotitasoft.ConsoleDriving
{
    /// <summary>
    /// The console driver builder interface
    /// </summary>
    public interface IConsoleDriverBuilder
    {
        /// <summary>
        /// A central location for sharing state between components during the host building process.
        /// </summary>
        IDictionary<object, object> Properties { get; }

        /// <summary>
        /// Set up the configuration for the builder itself. This will be used to initialize the <see cref="IConsoleDrivingEnvironment"/>
        /// for use later in the build process. This can be called multiple times and the results will be additive.
        /// </summary>
        /// <param name="configureDelegate">The delegate for configuring the <see cref="IConfigurationBuilder"/> that will be used
        /// to construct the <see cref="IConfiguration"/> for the console driver.</param>
        /// <returns>The same instance of the <see cref="IConsoleDriverBuilder"/> for chaining.</returns>
        IConsoleDriverBuilder ConfigureConsoleDriverConfiguration(Action<IConfigurationBuilder> configureDelegate);

        /// <summary>
        /// Sets up the configuration for the remainder of the build process and application. This can be called multiple times and
        /// the results will be additive. The results will be available at <see cref="ConsoleDriverBuilderContext.Configuration"/> for
        /// subsequent operations, as well as in <see cref="IConsoleDriver.Services"/>.
        /// </summary>
        /// <param name="configureDelegate">The delegate for configuring the <see cref="IConfigurationBuilder"/> that will be used
        /// to construct the <see cref="IConfiguration"/> for the application.</param>
        /// <returns>The same instance of the <see cref="IConsoleDriverBuilder"/> for chaining.</returns>
        IConsoleDriverBuilder ConfigureAppConfiguration(Action<ConsoleDriverBuilderContext, IConfigurationBuilder> configureDelegate);

        /// <summary>
        /// Adds services to the container. This can be called multiple times and the results will be additive.
        /// </summary>
        /// <param name="configureDelegate">The delegate for configuring the <see cref="IServiceCollection"/> that will be used
        /// to construct the <see cref="IServiceProvider"/>.</param>
        /// <returns>The same instance of the <see cref="IConsoleDriverBuilder"/> for chaining.</returns>
        IConsoleDriverBuilder ConfigureServices(Action<ConsoleDriverBuilderContext, IServiceCollection> configureDelegate);

        /// <summary>
        /// Overrides the factory used to create the service provider.
        /// </summary>
        /// <typeparam name="TContainerBuilder"></typeparam>
        /// <param name="factory"></param>
        /// <returns>The same instance of the <see cref="IConsoleDriverBuilder"/> for chaining.</returns>
        IConsoleDriverBuilder UseServiceProviderFactory<TContainerBuilder>(IServiceProviderFactory<TContainerBuilder> factory);

        /// <summary>
        /// Enables configuring the instantiated dependency container. This can be called multiple times and
        /// the results will be additive.
        /// </summary>
        /// <typeparam name="TContainerBuilder"></typeparam>
        /// <param name="configureDelegate"></param>
        /// <returns>The same instance of the <see cref="IConsoleDriverBuilder"/> for chaining.</returns>
        IConsoleDriverBuilder ConfigureContainer<TContainerBuilder>(Action<ConsoleDriverBuilderContext, TContainerBuilder> configureDelegate);

        /// <summary>
        /// Run the given actions to initialize the console dirver. This can only be called once.
        /// </summary>
        /// <returns>An initialized <see cref="IConsoleDriver"/></returns>
        IConsoleDriver Build();
    }
}
