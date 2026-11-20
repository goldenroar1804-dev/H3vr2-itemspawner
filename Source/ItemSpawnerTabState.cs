// UI state model for the eventual existing-wrist-menu tab. Not a Unity UI prefab.
using System;
using System.Collections.Generic;
using System.Linq;

namespace H3VR2ItemSpawner
{
    internal sealed class ItemSpawnerTabState
    {
        internal string Search = "";
        internal string Category = "All";
        internal int Quantity = 1;
        internal SpawnDestination Destination = SpawnDestination.SpawnPoint;
        internal CatalogEntry Selected;
        internal readonly List<CatalogEntry> Recent = new List<CatalogEntry>();
        internal string Status = "Not connected to game menu.";

        internal void SetQuantity(int q)
        {
            Quantity = q == 5 || q == 10 || q == 25 || q == 50 ? q : 1;
        }

        internal void MarkRecent(CatalogEntry entry)
        {
            if (entry == null) return;
            Recent.RemoveAll(x => x != null && x.Definition == entry.Definition);
            Recent.Insert(0, entry);
            if (Recent.Count > 12) Recent.RemoveRange(12, Recent.Count - 12);
        }

        internal IEnumerable<CatalogEntry> Visible(IEnumerable<CatalogEntry> catalog)
        {
            if (Category == "Recently Spawned") return Recent.ToList();
            return ReflectionItemCatalog.Filter(catalog, Search, Category)
                .OrderBy(x => x.Name, StringComparer.OrdinalIgnoreCase);
        }
    }
}