// Reflection diagnostics to locate the game's existing wrist-menu tab and input host.
// Does not patch, create, or replace any menu. Candidate discovery only.
using System;
using System.Linq;
using System.Reflection;

namespace H3VR2ItemSpawner
{
    internal static class WristMenuDiscovery
    {
        private static readonly string[] Keywords = { "BladeMenu", "Wrist", "Cursor", "Interaction", "MenuController" };

        internal static void Run(Action<string> log)
        {
            int candidates = 0;
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
                    if (++candidates > 80) { log("Menu discovery capped at 80 candidate types."); return; }
                    log("Menu candidate type: " + name);
                    try
                    {
                        var members = type.GetMembers(BindingFlags.Instance | BindingFlags.Static |
                            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);
                        foreach (var member in members)
                        {
                            string m = member.Name;
                            if (Keywords.Any(k => m.IndexOf(k, StringComparison.OrdinalIgnoreCase) >= 0) ||
                                m.IndexOf("Tab", StringComparison.OrdinalIgnoreCase) >= 0 ||
                                m.IndexOf("Button", StringComparison.OrdinalIgnoreCase) >= 0)
                                log("  member: " + member.MemberType + " " + m);
                        }
                    }
                    catch { }
                }
            }
            log("Menu discovery finished; candidate types logged: " + candidates +
                ". Names alone do not confirm an attachable menu hook.");
        }
    }
}