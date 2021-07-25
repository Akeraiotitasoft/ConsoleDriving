// Copyright (c) .NET Foundation. All rights reserved.  (Original version)
// Copyright (c) Akeraiotitasoft LLC. All rights reserved.  For the ConsoleDriver derivative work.
// Licensed under the Apache License, Version 2.0. See License in the project root for license information.

using Microsoft.Extensions.FileProviders;

namespace Akeraiotitasoft.ConsoleDriving.Internal
{
    /// <summary>
    /// The console driving environment
    /// </summary>
    internal class ConsoleDrivingEnvironment : IConsoleDrivingEnvironment
    {
        /// <summary>
        /// The enviornment name
        /// </summary>
        public string EnvironmentName { get; set; }

        /// <summary>
        /// The application name
        /// </summary>
        public string ApplicationName { get; set; }

        /// <summary>
        /// Content Root Path
        /// </summary>
        public string ContentRootPath { get; set; }

        /// <summary>
        /// The content root file provider
        /// </summary>
        public IFileProvider ContentRootFileProvider { get; set; }
    }
}