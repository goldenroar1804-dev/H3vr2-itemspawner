// Guarded reflection probe for the metadata-derived Footlong.Items.SpawnItem overload.
// This deliberately does not spawn items until runtime behavior is verified.
using System;
using System.Reflection;
using UnityEngine;

namespace H3VR2ItemSpawner
{
    internal static class SpawnApiProbe
    {
        private const string ItemsTypeName = "Footlong.Items";
        private const string ItemDefTypeName = "Footlong.Content.ItemDef";

        internal static MethodInfo ResolveSpawnMethod(Action<string> log)
        {
            try
            {
                Type itemsType = FindType(ItemsTypeName);
                Type itemDefType = FindType(ItemDefTypeName);
                if (itemsType == null || itemDefType == null)
                {
                    log?.Invoke("Spawn API probe: required types not found.");
                    return null;
                }

                MethodInfo method = itemsType.GetMethod(
                    "SpawnItem",
                    BindingFlags.Public | BindingFlags.Static,
                    null,
                    new[] { itemDefType, typeof(Vector3), typeof(Quaternion) },
                    null);

                if (method == null || method.ReturnType != typeof(GameObject))
                {
                    log?.Invoke("Spawn API probe: exact overload/return type not found.");
                    return null;
                }

                log?.Invoke("Spawn API probe: exact overload resolved (not runtime-tested).");
                return method;
            }
            catch (Exception ex)
            {
                log?.Invoke("Spawn API probe failed: " + ex.Message);
                return null;
            }
        }

        private static Type FindType(string fullName)
        {
            foreach (Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                try
                {
                    Type found = asm.GetType(fullName, false);
                    if (found != null) return found;
                }
                catch { }
            }
            return null;
        }
    }
}