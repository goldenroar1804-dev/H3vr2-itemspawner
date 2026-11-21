// Reflection diagnostics: dump narrowly targeted Footlong API signatures, plus likely menu types.
using System;
using System.Linq;
using System.Reflection;

namespace H3VR2ItemSpawner
{
    internal static class WristMenuDiscovery
    {
        private static readonly string[] Keywords = { "BladeMenu", "Wrist", "Cursor", "Interaction", "MenuController", "ItemSpawner", "Spawner", "Spawn", "Inventory", "Catalog", "Footlong" };
        private static readonly string[] TargetTypes = { "Footlong.Items", "Footlong.Content.ItemDef", "Footlong.Content.ItemSpawnerDef", "Footlong.Player" };

        internal static void Run(Action<string> log)
        {
            int candidates = 0, membersLogged = 0;
            foreach (string target in TargetTypes)
            {
                Type found = FindType(target);
                if (found == null) { log("TARGET TYPE MISSING: " + target); continue; }
                log("TARGET TYPE: " + found.FullName + " [" + found.Assembly.GetName().Name + "]");
                DumpMembers(found, log, ref membersLogged, 600, true);
            }
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                Type[] types;
                try { types = assembly.GetTypes(); }
                catch (ReflectionTypeLoadException ex) { types = ex.Types.Where(t => t != null).ToArray(); }
                catch { continue; }
                foreach (Type type in types)
                {
                    string name = type.FullName ?? type.Name;
                    if (!Keywords.Any(k => name.IndexOf(k, StringComparison.OrdinalIgnoreCase) >= 0)) continue;
                    if (++candidates > 160) { log("Discovery capped at 160 candidate types."); break; }
                    log("CANDIDATE: " + name);
                    DumpMembers(type, log, ref membersLogged, 800, false);
                }
            }
            log("Discovery complete. Candidate types=" + candidates + ", member output=" + membersLogged + ". Diagnostic only.");\n            log("Probe scope: expanded keyword match for inventory/catalog/spawn/menu candidates; no methods invoked.");
        }

        private static void DumpMembers(Type type, Action<string> log, ref int total, int cap, bool dumpAll)
        {
            try {
                var members = type.GetMembers(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);
                foreach (MemberInfo member in members) {
                    if (total >= cap) { log("Member output capped."); return; }
                    string n = member.Name;
                    bool relevant = dumpAll || Keywords.Any(k => n.IndexOf(k, StringComparison.OrdinalIgnoreCase) >= 0) ||
                        new[] { "Tab", "Button", "Spawn", "Open", "Close", "Item", "Category", "Name", "Prefab", "Asset", "Hand" }
                        .Any(k => n.IndexOf(k, StringComparison.OrdinalIgnoreCase) >= 0);
                    if (!relevant) continue;
                    if (member is MethodInfo m) {
                        string args = string.Join(", ", m.GetParameters().Select(p => p.ParameterType.FullName + " " + p.Name));
                        log("  METHOD: " + m.ReturnType.FullName + " " + n + "(" + args + ")");
                    } else if (member is PropertyInfo p) log("  PROPERTY: " + p.PropertyType.FullName + " " + n);
                    else if (member is FieldInfo f) log("  FIELD: " + f.FieldType.FullName + " " + n);
                    total++;
                }
            } catch (Exception ex) { log("Member scan failed: " + ex.Message); }
        }

        private static Type FindType(string fullName)
        {
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
                try { Type t = assembly.GetType(fullName, false); if (t != null) return t; } catch { }
            return null;
        }
    }
}
