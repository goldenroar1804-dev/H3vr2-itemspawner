// Runtime item catalog using reflection; exact game compatibility is unverified.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace H3VR2ItemSpawner
{
    internal sealed class CatalogEntry
    {
        internal object Definition;
        internal string Name;
        internal string Category;
    }

    internal static class ReflectionItemCatalog
    {
        internal static List<CatalogEntry> Refresh(Action<string> log)
        {
            var result = new List<CatalogEntry>();
            try
            {
                Type defType = FindType("Footlong.Content.ItemDef");
                if (defType == null) { log?.Invoke("Catalog: ItemDef type unavailable."); return result; }
                UnityEngine.Object[] assets = Resources.FindObjectsOfTypeAll(defType);
                foreach (var asset in assets)
                {
                    if (asset == null) continue;
                    string name = asset.name;
                    string category = ReadCategory(asset);
                    result.Add(new CatalogEntry { Definition = asset, Name = name, Category = category });
                }
                result = result.OrderBy(x => x.Category).ThenBy(x => x.Name, StringComparer.OrdinalIgnoreCase).ToList();
                log?.Invoke("Catalog refreshed: " + result.Count + " loaded definitions (may be incomplete).");
            }
            catch (Exception ex) { log?.Invoke("Catalog refresh failed: " + ex.Message); }
            return result;
        }

        internal static List<CatalogEntry> Filter(IEnumerable<CatalogEntry> entries, string search, string category)
        {
            search = search ?? "";
            category = category ?? "";
            return entries.Where(x =>
                (category.Length == 0 || category == "All" || string.Equals(x.Category, category, StringComparison.OrdinalIgnoreCase)) &&
                (search.Length == 0 || x.Name.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0))
                .ToList();
        }

        private static string ReadCategory(object asset)
        {
            try {
                var t = asset.GetType();
                foreach (var memberName in new[] { "Category", "Type", "ItemType" }) {
                    var p = t.GetProperty(memberName, BindingFlags.Public | BindingFlags.Instance);
                    if (p != null && p.GetIndexParameters().Length == 0) {
                        var v = p.GetValue(asset, null);
                        if (v != null) return v.ToString();
                    }
                    var f = t.GetField(memberName, BindingFlags.Public | BindingFlags.Instance);
                    if (f != null) { var v = f.GetValue(asset); if (v != null) return v.ToString(); }
                }
            } catch { }
            return "Uncategorized";
        }

        private static Type FindType(string name)
        {
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
                try { var t = asm.GetType(name, false); if (t != null) return t; } catch { }
            return null;
        }
    }
}