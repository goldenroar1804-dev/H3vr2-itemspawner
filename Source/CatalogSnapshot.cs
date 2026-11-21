// Safer catalog snapshot and lookup helpers. Runtime object enumeration may be incomplete.
using System;
using System.Collections.Generic;
using System.Linq;

namespace H3VR2ItemSpawner
{
    internal sealed class CatalogSnapshot
    {
        private readonly List<CatalogEntry> _entries = new List<CatalogEntry>();
        internal IReadOnlyList<CatalogEntry> Entries => _entries;
        internal int Count => _entries.Count;

        internal void Replace(IEnumerable<CatalogEntry> incoming)
        {
            _entries.Clear();
            if (incoming == null) return;
            foreach (var e in incoming)
                if (e != null && e.Definition != null) _entries.Add(e);
        }

        internal IEnumerable<string> Categories()
        {
            return _entries.Select(e => e.Category ?? "Uncategorized")
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .OrderBy(s => s, StringComparer.OrdinalIgnoreCase);
        }

        internal IEnumerable<CatalogEntry> Query(string text, string category)
        {
            return ReflectionItemCatalog.Filter(_entries, text, category);
        }

        internal CatalogEntry FindByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return null;
            return _entries.FirstOrDefault(e => string.Equals(e.Name, name, StringComparison.OrdinalIgnoreCase));
        }
    }
}