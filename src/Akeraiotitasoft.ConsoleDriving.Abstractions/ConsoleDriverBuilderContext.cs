// Copyright (c) .NET Foundation. All rights reserved.  (Original version)
// Copyright (c) Akeraiotitasoft LLC. All rights reserved.  For the ConsoleDriver derivative work.
// Licensed under the Apache License, Version 2.0. See License in the project root for license information.

using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akeraiotitasoft.ConsoleDriving
{
    /// <summary>
    /// Context containing the common services on the <see cref="IConsoleDriver" />. Some properties may be null until set by the <see cref="IConsoleDriver" />.
    /// </summary>
    public class ConsoleDriverBuilderContext
    {
        /// <summary>
        /// The constructor
        /// </summary>
        /// <param name="properties"></param>
        public ConsoleDriverBuilderContext(IDictionary<object, object> properties)
        {
            Properties = properties ?? throw new System.ArgumentNullException(nameof(properties));
        }

        /// <summary>
        /// The <see cref="IConsoleDrivingEnvironment" /> initialized by the <see cref="IConsoleDriver" />.
        /// </summary>
        public IConsoleDrivingEnvironment ConsoleDrivingEnvironment { get; set; }

        /// <summary>
        /// The <see cref="IConfiguration" /> containing the merged configuration of the application and the <see cref="IConsoleDrivingEnvironment" />.
        /// </summary>
        public IConfiguration Configuration { get; set; }

        /// <summary>
        /// A central location for sharing state between components during the host building process.
        /// </summary>
        public IDictionary<object, object> Properties { get; }
    }
}
