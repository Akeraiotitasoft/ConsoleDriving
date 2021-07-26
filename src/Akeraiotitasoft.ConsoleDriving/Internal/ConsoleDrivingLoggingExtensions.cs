// Copyright (c) .NET Foundation. All rights reserved.  (Original version)
// Copyright (c) Akeraiotitasoft LLC. All rights reserved.  For the ConsoleDriver derivative work.
// Licensed under the Apache License, Version 2.0. See License in the project root for license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using Microsoft.Extensions.Logging;

namespace Akeraiotitasoft.ConsoleDriving.Internal
{
    internal static class ConsoleDrivingLoggingExtensions
    {
        public static void Starting(this ILogger logger)
        {
            if (logger.IsEnabled(LogLevel.Debug))
            {
                logger.LogDebug(
                   eventId: LoggerEventIds.Starting,
                   message: "Hosting starting");
            }
        }

        public static void Stopped(this ILogger logger)
        {
            if (logger.IsEnabled(LogLevel.Debug))
            {
                logger.LogDebug(
                    eventId: LoggerEventIds.Stopped,
                    message: "Hosting stopped");
            }
        }
    }
}
