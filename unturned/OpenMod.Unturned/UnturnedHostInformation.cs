﻿using OpenMod.Core.Helpers;
using OpenMod.Extensions.Games.Abstractions;
using SDG.Unturned;
using Semver;

namespace OpenMod.Unturned
{
    public class UnturnedHostInformation : IGameHostInformation
    {
        public UnturnedHostInformation()
        {
            HostVersion = VersionHelper.ParseAssemblyVersion(GetType().Assembly);

        }

        public SemVersion HostVersion { get; }

        public string HostName { get; } = "OpenMod for Unturned";

        public bool IsClient { get; } = Provider.isClient;

        public bool IsServer { get; } = Provider.isServer || Dedicator.isDedicated || Dedicator.isStandaloneDedicatedServer;

        public ushort? ServerPort { get; } = Provider.port;

        public string ServerInstance { get; } = Dedicator.serverID;

        public string ServerName { get; } = Provider.serverName;

        public string GameDisplayName { get; } = Provider.APP_NAME;

        public string GameVersion { get; } = Provider.APP_VERSION;
    }
}