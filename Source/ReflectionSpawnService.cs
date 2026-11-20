// Reflection-based spawn adapter; invokes only the exact metadata-derived overload.
using System;
using System.Reflection;
using UnityEngine;

namespace H3VR2ItemSpawner
{
    internal static class ReflectionSpawnService
    {
        private static MethodInfo _method;
        private static Type _itemType;

        internal static bool Prepare(Action<string> log)
        {
            try {
                _itemType = FindType("Footlong.Content.ItemDef");
                Type itemsType = FindType("Footlong.Items");
                if (_itemType == null || itemsType == null) { log?.Invoke("Spawn service: API types unavailable."); return false; }
                _method = itemsType.GetMethod("SpawnItem", BindingFlags.Public | BindingFlags.Static, null,
                    new[] { _itemType, typeof(Vector3), typeof(Quaternion) }, null);
                if (_method == null || _method.ReturnType != typeof(GameObject)) {
                    log?.Invoke("Spawn service: exact SpawnItem overload was not resolved."); return false;
                }
                log?.Invoke("Spawn service prepared. Invocation remains unverified.");
                return true;
            } catch (Exception ex) { log?.Invoke("Spawn service preparation failed: " + ex.Message); return false; }
        }

        internal static GameObject Spawn(object definition, Vector3 position, Quaternion rotation, Action<string> log)
        {
            try {
                if (_method == null && !Prepare(log)) return null;
                if (definition == null || !_itemType.IsInstanceOfType(definition)) { log?.Invoke("Spawn rejected: invalid item definition."); return null; }
                var result = _method.Invoke(null, new[] { definition, (object)position, rotation }) as GameObject;
                if (result == null) log?.Invoke("Spawn call returned null.");
                return result;
            } catch (TargetInvocationException ex) {
                log?.Invoke("Game spawn API threw: " + (ex.InnerException != null ? ex.InnerException.Message : ex.Message)); return null;
            } catch (Exception ex) { log?.Invoke("Spawn failed: " + ex.Message); return null; }
        }

        private static Type FindType(string name)
        {
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
                try { var t = asm.GetType(name, false); if (t != null) return t; } catch { }
            return null;
        }
    }
}