// Copyright (c) .NET Foundation. All rights reserved.  (Original version)
// Copyright (c) Akeraiotitasoft LLC. All rights reserved.  For the ConsoleDriver derivative work.
// Licensed under the Apache License, Version 2.0. See License in the project root for license information.

namespace Akeraiotitasoft.ConsoleDriving.Internal
{
    internal interface IConfigureContainerAdapter
    {
        void ConfigureContainer(ConsoleDriverBuilderContext consoleDriverContext, object containerBuilder);
    }
}