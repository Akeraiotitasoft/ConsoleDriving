// Copyright (c) .NET Foundation. All rights reserved.  (Original version)
// Copyright (c) Akeraiotitasoft LLC. All rights reserved.  For the ConsoleDriver derivative work.
// Licensed under the Apache License, Version 2.0. See License in the project root for license information.

using System;

namespace Akeraiotitasoft.ConsoleDriving.Internal
{
    internal class ConfigureContainerAdapter<TContainerBuilder> : IConfigureContainerAdapter
    {
        private Action<ConsoleDriverBuilderContext, TContainerBuilder> _action;

        public ConfigureContainerAdapter(Action<ConsoleDriverBuilderContext, TContainerBuilder> action)
        {
            _action = action ?? throw new ArgumentNullException(nameof(action));
        }

        public void ConfigureContainer(ConsoleDriverBuilderContext consoleDriverContext, object containerBuilder)
        {
            _action(consoleDriverContext, (TContainerBuilder)containerBuilder);
        }
    }
}
