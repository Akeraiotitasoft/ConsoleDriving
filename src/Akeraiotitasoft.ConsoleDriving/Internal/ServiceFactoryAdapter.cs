// Copyright (c) .NET Foundation. All rights reserved.  (Original version)
// Copyright (c) Akeraiotitasoft LLC. All rights reserved.  For the ConsoleDriver derivative work.
// Licensed under the Apache License, Version 2.0. See License in the project root for license information.

using System;
using Akeraiotitasoft.ConsoleDriving.Resources;
using Microsoft.Extensions.DependencyInjection;

namespace Akeraiotitasoft.ConsoleDriving.Internal
{
    internal class ServiceFactoryAdapter<TContainerBuilder> : IServiceFactoryAdapter
    {
        private IServiceProviderFactory<TContainerBuilder> _serviceProviderFactory;
        private readonly Func<ConsoleDriverBuilderContext> _contextResolver;
        private Func<ConsoleDriverBuilderContext, IServiceProviderFactory<TContainerBuilder>> _factoryResolver;

        public ServiceFactoryAdapter(IServiceProviderFactory<TContainerBuilder> serviceProviderFactory)
        {
            _serviceProviderFactory = serviceProviderFactory ?? throw new System.ArgumentNullException(nameof(serviceProviderFactory));
        }

        public ServiceFactoryAdapter(Func<ConsoleDriverBuilderContext> contextResolver, Func<ConsoleDriverBuilderContext, IServiceProviderFactory<TContainerBuilder>> factoryResolver)
        {
            _contextResolver = contextResolver ?? throw new ArgumentNullException(nameof(contextResolver));
            _factoryResolver = factoryResolver ?? throw new ArgumentNullException(nameof(factoryResolver));
        }

        public object CreateBuilder(IServiceCollection services)
        {
            if (_serviceProviderFactory == null)
            {
                _serviceProviderFactory = _factoryResolver(_contextResolver());

                if (_serviceProviderFactory == null)
                {
                    throw new InvalidOperationException(Strings.ResolverReturnedNull);
                }
            }
            return _serviceProviderFactory.CreateBuilder(services);
        }

        public IServiceProvider CreateServiceProvider(object containerBuilder)
        {
            if (_serviceProviderFactory == null)
            {
                throw new InvalidOperationException(Strings.CreateBuilderCallBeforeCreateServiceProvider);
            }

            return _serviceProviderFactory.CreateServiceProvider((TContainerBuilder)containerBuilder);
        }
    }
}
