﻿using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using OpenMod.API;
using OpenMod.API.Prioritization;

namespace OpenMod.Core.Commands.OpenModCommands
{
    [Command("restart", Priority = Priority.Lowest)]
    [CommandDescription("Restarts the server.")]
    public class CommandRestart : Command
    {
        private readonly IOpenModHost m_Host;
        private readonly ILogger<CommandRestart> m_Logger;
        private static Process? s_ChildProcess;

        public CommandRestart(
            IServiceProvider serviceProvider,
            IOpenModHost host,
            ILogger<CommandRestart> logger) : base(serviceProvider)
        {
            m_Host = host;
            m_Logger = logger;
        }

        protected override Task OnExecuteAsync()
        {
            var args = Environment.GetCommandLineArgs();
            var binaryPath = Process.GetCurrentProcess().MainModule.FileName;

            string arguments = string.Empty;

            if (args.Length > 1)
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    arguments = string.Join(" ", args
                        .Skip(1)
                        .Select(d =>
                        {
                            if (d.Contains(" "))
                            {
                                return "\"" + d.Replace("\"", "^\"") + "\"";
                            }

                            return d.Replace("\"", "^\"");
                        }));
                }
                else
                {
                    arguments = string.Join(" ", args.Skip(1)
                        .Select(d => d.Replace("\"", "\\\"")));
                }
            }

            m_Logger.LogInformation($"Executing command after shutdown: {Environment.NewLine}{binaryPath} {arguments}");

            var startInfo = new ProcessStartInfo(binaryPath, arguments)
            {
                UseShellExecute = false,
                WorkingDirectory = Path.GetDirectoryName(binaryPath)
            };

            foreach (DictionaryEntry kv in Environment.GetEnvironmentVariables())
            {
                if (startInfo.EnvironmentVariables.ContainsKey(kv.Key.ToString()))
                {
                    startInfo.EnvironmentVariables[kv.Key.ToString()] = kv.Value.ToString();
                }
                else
                {
                    startInfo.EnvironmentVariables.Add(kv.Key.ToString(), kv.Value.ToString());
                }
            }

            // Not required on Windows
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) || RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;
            }

            s_ChildProcess = Process.Start(startInfo);
            Task.Run(m_Host.ShutdownAsync);

            return Task.CompletedTask;
        }

        private static void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            s_ChildProcess?.WaitForExit();
        }
    }
}