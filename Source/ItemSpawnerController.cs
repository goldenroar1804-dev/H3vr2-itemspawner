// Controller facade for future menu integration. Contains no UI or player-specific hooks.
using System;
using System.Collections.Generic;
using UnityEngine;

namespace H3VR2ItemSpawner
{
    internal sealed class ItemSpawnerController
    {
        private readonly CatalogSnapshot _catalog = new CatalogSnapshot();
        private readonly ItemSpawnerTabState _state = new ItemSpawnerTabState();
        private readonly Action<string> _log;

        internal ItemSpawnerTabState State => _state;
        internal IReadOnlyList<CatalogEntry> Catalog => _catalog.Entries;

        internal ItemSpawnerController(Action<string> log)
        {
            _log = log;
        }

        internal void Refresh()
        {
            _catalog.Replace(ReflectionItemCatalog.Refresh(_log));
            if (_state.Selected != null &&
                _catalog.FindByName(_state.Selected.Name) == null)
                _state.Selected = null;
            _state.Status = "Catalog: " + _catalog.Count + " definitions loaded (may be incomplete).";
        }

        internal IEnumerable<CatalogEntry> Visible()
        {
            return _state.Visible(_catalog.Entries);
        }

        internal IEnumerable<string> Categories()
        {
            var values = new List<string> { "All", "Recently Spawned" };
            values.AddRange(_catalog.Categories());
            return values;
        }

        internal bool Select(CatalogEntry entry)
        {
            if (entry == null || entry.Definition == null)
            {
                _state.Selected = null;
                _state.Status = "Selection cleared.";
                return false;
            }
            _state.Selected = entry;
            _state.Status = "Selected: " + entry.Name;
            return true;
        }

        internal int SpawnSelected(Func<SpawnDestination, Vector3> positionResolver,
            Func<SpawnDestination, Quaternion> rotationResolver)
        {
            if (_state.Selected == null)
            {
                _state.Status = "Choose an item first.";
                _log?.Invoke(_state.Status);
                return 0;
            }
            int spawned = SpawnBatchCoordinator.SpawnBatch(_state.Selected.Definition,
                _state.Quantity, _state.Destination, positionResolver, rotationResolver, _log);
            if (spawned > 0) _state.MarkRecent(_state.Selected);
            _state.Status = spawned > 0 ? "Spawned " + spawned + " item(s)." : "Spawn failed or was cancelled.";
            return spawned;
        }
    }
}