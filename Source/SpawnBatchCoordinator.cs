// Spawn batch coordinator. Destination selection is intentionally expressed as an
// input transform supplied by the future wrist-menu host integration.
using System;
using UnityEngine;

namespace H3VR2ItemSpawner
{
    internal enum SpawnDestination { SpawnPoint, Hand, Ground }

    internal static class SpawnBatchCoordinator
    {
        internal static int ClampQuantity(int quantity)
        {
            if (quantity <= 1) return 1;
            if (quantity <= 5) return 5;
            if (quantity <= 10) return 10;
            if (quantity <= 25) return 25;
            return 50;
        }

        internal static int SpawnBatch(object itemDef, int requestedQuantity,
            SpawnDestination destination, Func<SpawnDestination, Vector3> positionResolver,
            Func<SpawnDestination, Quaternion> rotationResolver, Action<string> log)
        {
            if (itemDef == null) { log?.Invoke("Batch spawn cancelled: no item selected."); return 0; }
            if (positionResolver == null || rotationResolver == null) {
                log?.Invoke("Batch spawn cancelled: destination resolver unavailable."); return 0;
            }
            int count = ClampQuantity(requestedQuantity);
            int spawned = 0;
            for (int i = 0; i < count; i++)
            {
                try
                {
                    Vector3 pos = positionResolver(destination);
                    Quaternion rot = rotationResolver(destination);
                    if (ReflectionSpawnService.Spawn(itemDef, pos, rot, log) != null) spawned++;
                }
                catch (Exception ex) { log?.Invoke("Batch item " + (i + 1) + " failed: " + ex.Message); }
            }
            log?.Invoke("Batch result: " + spawned + "/" + count + " spawned.");
            return spawned;
        }
    }
}