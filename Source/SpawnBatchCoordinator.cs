// Spawn batch coordinator. Destination selection is intentionally expressed as an
// input transform supplied by the future wrist-menu host integration.
using System;
using UnityEngine;

namespace H3VR2ItemSpawner
{
    internal enum SpawnDestination { SpawnPoint, Hand, Ground }

    internal static class SpawnBatchCoordinator
    {
        private const float BatchSpreadRadius = 0.24f;
        private const int MaximumBatchSize = 50;

        // UI presets only; invalid values fall back to a single spawn.
        internal static int ClampQuantity(int quantity)
        {
            if (quantity == 5 || quantity == 10 || quantity == 25 || quantity == 50)
                return Math.Min(quantity, MaximumBatchSize);
            return 1;
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
            Vector3 basePos;
            Quaternion rot;
            try {
                // Resolve once so the batch remains grouped even if the hand/player moves.
                basePos = positionResolver(destination);
                rot = rotationResolver(destination);
            } catch (Exception ex) {
                log?.Invoke("Batch spawn cancelled: destination resolution failed: " + ex.Message);
                return 0;
            }

            for (int i = 0; i < count; i++)
            {
                try
                {
                    // Compact, evenly spaced circle around the resolved destination.
                    float angle = (Mathf.PI * 2f * i) / count;
                    Vector3 offset = count == 1 ? Vector3.zero :
                        new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle)) * BatchSpreadRadius;
                    Vector3 pos = basePos + rot * offset;
                    if (ReflectionSpawnService.Spawn(itemDef, pos, rot, log) != null) spawned++;
                }
                catch (Exception ex) { log?.Invoke("Batch item " + (i + 1) + " failed: " + ex.Message); }
            }
            log?.Invoke("Batch result: " + spawned + "/" + count + " spawned.");
            return spawned;
        }
    }
}