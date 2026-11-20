// Reflection diagnostics to locate the game's existing wrist-menu tab and input host.
// Candidate discovery only; no menu is patched or replaced.
using System;
using System.Linq;
using System.Reflection;

namespace H3VR2ItemSpawner
{
    internal static class WristMenuDiscovery
    {
        private static readonly string[] Keywords = { "BladeMenu", "Wrist", "Cursor", "Interaction", "MenuController", "ItemSpawner" };
        internal static void Run(Action<string> log)
        {
            int candidates = 0, membersLogged = 0;
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                Type[] types;
                try { types = assembly.GetTypes(); }
                catch (ReflectionTypeLoadException ex) { types = ex.Types.Where(t => t != null).ToArray(); }
                catch { continue; }
                foreach (var type in types)
                {
                    string name = type.FullName ?? type.Name;
                    if (!Keywords.Any(k => name.IndexOf(k, StringComparison.OrdinalIgnoreCase) >= 0)) continue;
                    if (++candidates > 100) { log("Discovery capped at 100 types."); return; }
                    log("CANDIDATE: " + name);
                    try {
                        var members = type.GetMembers(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);
                        foreach (var member in members) {
                            if (membersLogged >= 500) { log("Member output capped at 500."); return; }
                            string m = member.Name;
                            bool relevant = Keywords.Any(k => m.IndexOf(k, StringComparison.OrdinalIgnoreCase) >= 0) ||
                                m.IndexOf("Tab", StringComparison.OrdinalIgnoreCase) >= 0 ||
                                m.IndexOf("Button", StringComparison.OrdinalIgnoreCase) >= 0 ||
                                m.IndexOf("Spawn", StringComparison.OrdinalIgnoreCase) >= 0 ||
                                m.IndexOf("Open", StringComparison.OrdinalIgnoreCase) >= 0 ||
                                m.IndexOf("Close", StringComparison.OrdinalIgnoreCase) >= 0;
                            if (!relevant) continue;
                            if (member is MethodInfo method) {
                                string args = string.Join(", ", method.GetParameters().Select(p => p.ParameterType.Name + " " + p.Name));
                                log("  METHOD: " + method.ReturnType.Name + " " + m + "(" + args + ")");
                            } else if (member is PropertyInfo prop) log("  PROPERTY: " + prop.PropertyType.Name + " " + m);
                            else if (member is FieldInfo field) log("  FIELD: " + field.FieldType.Name + " " + m);
                            membersLogged++;
                        }
                    } catch { }
                }
            }
            log("Discovery complete. Types=" + candidates + ", relevant members=" + membersLogged + ". This output is diagnostic, not proof of hook compatibility.");
        }
    }
}