// Copyright (c) .NET Foundation. All rights reserved.  (Original version)
// Copyright (c) Akeraiotitasoft LLC. All rights reserved.  For the ConsoleDriver derivative work.
// Licensed under the Apache License, Version 2.0. See License in the project root for license information.

using Akeraiotitasoft.ConsoleDriving.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Akeraiotitasoft.ConsoleDriving
{
    /// <summary>
    /// The Console Driver Builder
    /// </summary>
    public class ConsoleDriverBuilder : IConsoleDriverBuilder
    {
        /// <summary>
        /// The constructor
        /// </summary>
        /// <param name="args">The command line arguments</param>
        public ConsoleDriverBuilder(string[] args)
        {
            _commandLineArguments = new CommandLineArguments(args);
        }

        private List<Action<IConfigurationBuilder>> _configureConsoleDriverConfigActions = new List<Action<IConfigurationBuilder>>();
        private List<Action<ConsoleDriverBuilderContext, IConfigurationBuilder>> _configureAppConfigActions = new List<Action<ConsoleDriverBuilderContext, IConfigurationBuilder>>();
        private List<Action<ConsoleDriverBuilderContext, IServiceCollection>> _configureServicesActions = new List<Action<ConsoleDriverBuilderContext, IServiceCollection>>();
        private List<IConfigureContainerAdapter> _configureContainerActions = new List<IConfigureContainerAdapter>();
        private IServiceFactoryAdapter _serviceProviderFactory = new ServiceFactoryAdapter<IServiceCollection>(new DefaultServiceProviderFactory());
        private bool _consoleDriverBuilt;
        private IConfiguration _consoleDriverConfiguration;
        private IConfiguration _appConfiguration;
        private ConsoleDriverBuilderContext _consoleDriverBuilderContext;
        private IConsoleDrivingEnvironment _consoleDrivingEnvironment;
        private IServiceProvider _appServices;
        private ICommandLineArguments _commandLineArguments;

        /// <summary>
        /// A central location for sharing state between components during the console driver building process.
        /// </summary>
        public IDictionary<object, object> Properties { get; } = new Dictionary<object, object>();

        /// <summary>
        /// Set up the configuration for the builder itself. This will be used to initialize the <see cref="IConsoleDrivingEnvironment"/>
        /// for use later in the build process. This can be called multiple times and the results will be additive.
        /// </summary>
        /// <param name="configureDelegate"></param>
        /// <returns>The same instance of the <see cref="IConsoleDriverBuilder"/> for chaining.</returns>
        public IConsoleDriverBuilder ConfigureConsoleDriverConfiguration(Action<IConfigurationBuilder> configureDelegate)
        {
            _configureConsoleDriverConfigActions.Add(configureDelegate ?? throw new ArgumentNullException(nameof(configureDelegate)));
            return this;
        }

        /// <summary>
        /// Sets up the configuration for the remainder of the build process and application. This can be called multiple times and
        /// the results will be additive. The results will be available at <see cref="ConsoleDriverBuilderContext.Configuration"/> for
        /// subsequent operations, as well as in <see cref="IConsoleDriver.Services"/>.
        /// </summary>
        /// <param name="configureDelegate"></param>
        /// <returns>The same instance of the <see cref="IConsoleDriverBuilder"/> for chaining.</returns>
        public IConsoleDriverBuilder ConfigureAppConfiguration(Action<ConsoleDriverBuilderContext, IConfigurationBuilder> configureDelegate)
        {
            _configureAppConfigActions.Add(configureDelegate ?? throw new ArgumentNullException(nameof(configureDelegate)));
            return this;
        }

        /// <summary>
        /// Adds services to the container. This can be called multiple times and the results will be additive.
        /// </summary>
        /// <param name="configureDelegate"></param>
        /// <returns>The same instance of the <see cref="IConsoleDriverBuilder"/> for chaining.</returns>
        public IConsoleDriverBuilder ConfigureServices(Action<ConsoleDriverBuilderContext, IServiceCollection> configureDelegate)
        {
            _configureServicesActions.Add(configureDelegate ?? throw new ArgumentNullException(nameof(configureDelegate)));
            return this;
        }

        /// <summary>
        /// Overrides the factory used to create the service provider.
        /// </summary>
        /// <typeparam name="TContainerBuilder"></typeparam>
        /// <param name="factory"></param>
        /// <returns>The same instance of the <see cref="IConsoleDriverBuilder"/> for chaining.</returns>
        public IConsoleDriverBuilder UseServiceProviderFactory<TContainerBuilder>(IServiceProviderFactory<TContainerBuilder> factory)
        {
            _serviceProviderFactory = new ServiceFactoryAdapter<TContainerBuilder>(factory ?? throw new ArgumentNullException(nameof(factory)));
            return this;
        }

        /// <summary>
        /// Overrides the factory used to create the service provider.
        /// </summary>
        /// <param name="factory">A factory used for creating service providers.</param>
        /// <typeparam name="TContainerBuilder">The type of the builder to create.</typeparam>
        /// <returns>The same instance of the <see cref="IConsoleDriverBuilder"/> for chaining.</returns>
        public IConsoleDriverBuilder UseServiceProviderFactory<TContainerBuilder>(Func<ConsoleDriverBuilderContext, IServiceProviderFactory<TContainerBuilder>> factory)
        {
            _serviceProviderFactory = new ServiceFactoryAdapter<TContainerBuilder>(() => _consoleDriverBuilderContext, factory ?? throw new ArgumentNullException(nameof(factory)));
            return this;
        }

        /// <summary>
        /// Enables configuring the instantiated dependency container. This can be called multiple times and
        /// the results will be additive.
        /// </summary>
        /// <typeparam name="TContainerBuilder"></typeparam>
        /// <param name="configureDelegate"></param>
        /// <returns>The same instance of the <see cref="IConsoleDriverBuilder"/> for chaining.</returns>
        public IConsoleDriverBuilder ConfigureContainer<TContainerBuilder>(Action<ConsoleDriverBuilderContext, TContainerBuilder> configureDelegate)
        {
            _configureContainerActions.Add(new ConfigureContainerAdapter<TContainerBuilder>(configureDelegate
                ?? throw new ArgumentNullException(nameof(configureDelegate))));
            return this;
        }

        /// <summary>
        /// Run the given actions to initialize the console driver. This can only be called once.
        /// </summary>
        /// <returns>An initialized <see cref="IConsoleDriver"/></returns>
        public IConsoleDriver Build()
        {
            if (_consoleDriverBuilt)
            {
                throw new InvalidOperationException("Build can only be called once.");
            }
            _consoleDriverBuilt = true;

            BuildConsoleDriverConfiguration();
            CreateConsoleDrivingEnvironment();
            CreateConsoleDriverBuilderContext();
            BuildAppConfiguration();
            CreateServiceProvider();

            return _appServices.GetRequiredService<IConsoleDriver>();
        }

        private void BuildConsoleDriverConfiguration()
        {
            var configBuilder = new ConfigurationBuilder();
            foreach (var buildAction in _configureConsoleDriverConfigActions)
            {
                buildAction(configBuilder);
            }
            _consoleDriverConfiguration = configBuilder.Build();
        }

        private void CreateConsoleDrivingEnvironment()
        {
            _consoleDrivingEnvironment = new ConsoleDrivingEnvironment()
            {
                ApplicationName = _consoleDriverConfiguration[ConsoleDriverDefaults.ApplicationKey],
                EnvironmentName = _consoleDriverConfiguration[ConsoleDriverDefaults.EnvironmentKey] ?? EnvironmentName.Production,
                ContentRootPath = ResolveContentRootPath(_consoleDriverConfiguration[ConsoleDriverDefaults.ContentRootKey], AppContext.BaseDirectory),
            };
            _consoleDrivingEnvironment.ContentRootFileProvider = new PhysicalFileProvider(_consoleDrivingEnvironment.ContentRootPath);
        }

        private string ResolveContentRootPath(string contentRootPath, string basePath)
        {
            if (string.IsNullOrEmpty(contentRootPath))
            {
                return basePath;
            }
            if (Path.IsPathRooted(contentRootPath))
            {
                return contentRootPath;
            }
            return Path.Combine(Path.GetFullPath(basePath), contentRootPath);
        }

        private void CreateConsoleDriverBuilderContext()
        {
            _consoleDriverBuilderContext = new ConsoleDriverBuilderContext(Properties)
            {
                ConsoleDrivingEnvironment = _consoleDrivingEnvironment,
                Configuration = _consoleDriverConfiguration
            };
        }

        private void BuildAppConfiguration()
        {
            var configBuilder = new ConfigurationBuilder();
            configBuilder.AddConfiguration(_consoleDriverConfiguration);
            foreach (var buildAction in _configureAppConfigActions)
            {
                buildAction(_consoleDriverBuilderContext, configBuilder);
            }
            _appConfiguration = configBuilder.Build();
            _consoleDriverBuilderContext.Configuration = _appConfiguration;
        }

        private void CreateServiceProvider()
        {
            var services = new ServiceCollection();
            services.AddSingleton(_commandLineArguments);
            services.AddSingleton(_consoleDrivingEnvironment);
            services.AddSingleton(_consoleDriverBuilderContext);
            services.AddSingleton(_appConfiguration);
            services.AddSingleton<IConsoleDriver, Akeraiotitasoft.ConsoleDriving.Internal.ConsoleDriver>();
            services.AddOptions();
            services.AddLogging();

            foreach (var configureServicesAction in _configureServicesActions)
            {
                configureServicesAction(_consoleDriverBuilderContext, services);
            }

            var containerBuilder = _serviceProviderFactory.CreateBuilder(services);

            foreach (var containerAction in _configureContainerActions)
            {
                containerAction.ConfigureContainer(_consoleDriverBuilderContext, containerBuilder);
            }

            _appServices = _serviceProviderFactory.CreateServiceProvider(containerBuilder);

            if (_appServices == null)
            {
                throw new InvalidOperationException($"The IServiceProviderFactory returned a null IServiceProvider.");
            }
        }
    }
}
