﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OpenMod.Core.Plugins;
using OpenMod.Unturned.Plugins;

[assembly: PluginMetadata(Author = "OpenMod", DisplayName = "Unturned Sample Plugin", Id = "UnturnedSamplePlugin", Version = "1.0.0")]

namespace OpenMod.Unturned.SamplePlugin
{
    [UsedImplicitly]
    public class UnturnedSamplePlugin : OpenModUnturnedPlugin
    {
        private readonly ILogger<UnturnedSamplePlugin> m_Logger;
        private readonly IConfiguration m_Configuration;

        public UnturnedSamplePlugin(
            ILogger<UnturnedSamplePlugin> logger, 
            IConfiguration configuration,
            IServiceProvider serviceProvider) : base(serviceProvider)
        {
            m_Logger = logger;
            m_Configuration = configuration;
        }

        protected override async UniTask OnLoadAsync()
        {
            m_Logger.LogInformation("SampleUnturnedPlugin has been loaded.");
            m_Logger.LogInformation($"Configuration value: {m_Configuration["somevalue"]}");

            await UniTask.SwitchToThreadPool();
            m_Logger.LogInformation($"SwitchToThreadPool Thread: {Thread.CurrentThread.ManagedThreadId}");

            await UniTask.Yield();
            m_Logger.LogInformation($"Yield Thread: {Thread.CurrentThread.ManagedThreadId}");

            await UniTask.SwitchToTaskPool();
            m_Logger.LogInformation($"SwitchToTaskPool Thread: {Thread.CurrentThread.ManagedThreadId}");

            await UniTask.SwitchToMainThread();
            m_Logger.LogInformation($"SwitchToMainThread Thread: {Thread.CurrentThread.ManagedThreadId}");

            m_Logger.LogInformation("Waiting 300 frames (~ 5 seconds)");
            await UniTask.DelayFrame(300);
            m_Logger.LogInformation("Waiting done.");

            m_Logger.LogInformation("Waiting 2 seconds via UniTask.Delay");
            await UniTask.Delay(TimeSpan.FromSeconds(2));
            m_Logger.LogInformation("Waiting done.");

            m_Logger.LogInformation("Waiting 2 seconds via Task.Delay");
            await Task.Delay(TimeSpan.FromSeconds(2));
            m_Logger.LogInformation("Waiting done.");
        }
    }
}
