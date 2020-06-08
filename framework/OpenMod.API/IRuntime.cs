﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using OpenMod.API.Ioc;
using Semver;

namespace OpenMod.API
{
    /// <summary>
    ///     Defines the OpenMod Runtime. This class is responsible for initializing OpenMod.
    /// </summary>
    [Service]
    public interface IRuntime
    {
        /// <summary>
        ///     Initializes the runtime.
        /// </summary>
        /// <returns></returns>
        Task InitAsync(ICollection<Assembly> openModHostAssemblies, IHostBuilder hostBuilder, RuntimeInitParameters parameters);

        /// <summary>
        ///     Shuts down OpenMod and disposes all services.
        /// </summary>
        Task ShutdownAsync();

        /// <summary>
        ///     Gets the OpenMod runtime version.
        /// </summary>
        SemVersion Version { get; }

        /// <summary>
        ///    Path to the OpenMod directory.
        /// </summary>
        string WorkingDirectory { get; }

        /// <summary>
        ///    Commandline arguments.
        /// </summary>
        string[] CommandlineArgs { get; }

        /// <summary>
        ///     The OpenMod configuration.
        /// </summary>
        IConfigurationRoot Configuration { get; }

        /// <summary>
        ///    The runtime status.
        /// </summary>
        RuntimeStatus Status { get; }

    }

    public enum RuntimeStatus
    {
        Initializing,
        Initialized,
        Unloaded,
        Crashed
    }

    public class RuntimeInitParameters
    {
        public string WorkingDirectory;
        public string[] CommandlineArgs;
    }
}
