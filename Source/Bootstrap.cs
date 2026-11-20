// H3VR2 Quest Item Spawner prototype — bootstrap
// Runtime compatibility and compilation are not yet verified.
using System;
using MelonLoader;
using UnityEngine;

[assembly: MelonInfo(typeof(H3VR2ItemSpawner.Bootstrap), "H3VR2 Item Spawner", "0.3.2", "Project")]
[assembly: MelonGame(null, null)]

namespace H3VR2ItemSpawner
{
    public sealed class Bootstrap : MelonMod
    {
        private bool _reported;

        public override void OnInitializeMelon()
        {
            LoggerInstance.Msg("Item Spawner prototype 0.3.2 initialized.");
            LoggerInstance.Warning("Runtime/game compatibility has not been verified.");
            WristMenuDiscovery.Run(s => LoggerInstance.Msg(s));
        }

        public override void OnUpdate()
        {
            if (_reported) return;
            try
            {
                var playerType = FindType("Footlong.Player");
                if (playerType == null) return;
                _reported = true;
                LoggerInstance.Msg("Found Footlong.Player type. This build is diagnostic only; menu attachment is not enabled yet.");
            }
            catch (Exception ex)
            {
                LoggerInstance.Error("Runtime probe failed: " + ex);
            }
        }

        private static Type FindType(string fullName)
        {
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                try
                {
                    var type = asm.GetType(fullName, false);
                    if (type != null) return type;
                }
                catch { /* Some IL2CPP assemblies may not support reflection queries. */ }
            }
            return null;
        }
    }
}