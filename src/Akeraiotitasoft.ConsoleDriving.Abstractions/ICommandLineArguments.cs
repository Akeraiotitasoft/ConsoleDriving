// Copyright (c) .NET Foundation. All rights reserved.  (Original version)
// Copyright (c) Akeraiotitasoft LLC. All rights reserved.  For the ConsoleDriver derivative work.
// Licensed under the Apache License, Version 2.0. See License in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akeraiotitasoft.ConsoleDriving
{
    /// <summary>
    /// The command line arguments so that they can be injected
    /// </summary>
    public interface ICommandLineArguments
    {
        /// <summary>
        /// Command Line Arguments
        /// </summary>
        string[] Arguments { get; }
    }
}
