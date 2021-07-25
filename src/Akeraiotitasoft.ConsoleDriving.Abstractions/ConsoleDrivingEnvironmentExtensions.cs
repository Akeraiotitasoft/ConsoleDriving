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
    /// Extension methods for <see cref="IConsoleDrivingEnvironment"/>.
    /// </summary>
    public static class ConsoleDrivingEnvironmentExtensions
    {
        /// <summary>
        /// Checks if the current console driving environment name is <see cref="EnvironmentName.Development"/>.
        /// </summary>
        /// <param name="consoleDriverEnvironment">An instance of <see cref="IConsoleDrivingEnvironment"/>.</param>
        /// <returns>True if the environment name is <see cref="EnvironmentName.Development"/>, otherwise false.</returns>
        public static bool IsDevelopment(this IConsoleDrivingEnvironment consoleDriverEnvironment)
        {
            if (consoleDriverEnvironment == null)
            {
                throw new ArgumentNullException(nameof(consoleDriverEnvironment));
            }

            return consoleDriverEnvironment.IsEnvironment(EnvironmentName.Development);
        }

        /// <summary>
        /// Checks if the current console driving environment name is <see cref="EnvironmentName.Staging"/>.
        /// </summary>
        /// <param name="consoleDriverEnvironment">An instance of <see cref="IConsoleDrivingEnvironment"/>.</param>
        /// <returns>True if the environment name is <see cref="EnvironmentName.Staging"/>, otherwise false.</returns>
        public static bool IsStaging(this IConsoleDrivingEnvironment consoleDriverEnvironment)
        {
            if (consoleDriverEnvironment == null)
            {
                throw new ArgumentNullException(nameof(consoleDriverEnvironment));
            }

            return consoleDriverEnvironment.IsEnvironment(EnvironmentName.Staging);
        }

        /// <summary>
        /// Checks if the current console driving environment name is <see cref="EnvironmentName.Production"/>.
        /// </summary>
        /// <param name="consoleDriverEnvironment">An instance of <see cref="IConsoleDrivingEnvironment"/>.</param>
        /// <returns>True if the environment name is <see cref="EnvironmentName.Production"/>, otherwise false.</returns>
        public static bool IsProduction(this IConsoleDrivingEnvironment consoleDriverEnvironment)
        {
            if (consoleDriverEnvironment == null)
            {
                throw new ArgumentNullException(nameof(consoleDriverEnvironment));
            }

            return consoleDriverEnvironment.IsEnvironment(EnvironmentName.Production);
        }

        /// <summary>
        /// Compares the current hosting environment name against the specified value.
        /// </summary>
        /// <param name="consoleDriverEnvironment">An instance of <see cref="IConsoleDrivingEnvironment"/>.</param>
        /// <param name="environmentName">Environment name to validate against.</param>
        /// <returns>True if the specified name is the same as the current environment, otherwise false.</returns>
        public static bool IsEnvironment(
            this IConsoleDrivingEnvironment consoleDriverEnvironment,
            string environmentName)
        {
            if (consoleDriverEnvironment == null)
            {
                throw new ArgumentNullException(nameof(consoleDriverEnvironment));
            }

            return string.Equals(
                consoleDriverEnvironment.EnvironmentName,
                environmentName,
                StringComparison.OrdinalIgnoreCase);
        }
    }
}
