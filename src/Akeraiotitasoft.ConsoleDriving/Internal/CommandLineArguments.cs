// Copyright (c) .NET Foundation. All rights reserved.  (Original version)
// Copyright (c) Akeraiotitasoft LLC. All rights reserved.  For the ConsoleDriver derivative work.
// Licensed under the Apache License, Version 2.0. See License in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akeraiotitasoft.ConsoleDriving.Internal
{
    internal class CommandLineArguments : ICommandLineArguments
    {
        private readonly string[] _arguments;

        public CommandLineArguments(string[] args)
        {
            _arguments = args;
        }

        public string[] Arguments => _arguments;
    }
}
