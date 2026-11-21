// Destination helper: provides conservative, reusable placement transforms for a host integration.
// Host integration must supply the actual player/hand transforms from verified game APIs.
using System;
using UnityEngine;

namespace H3VR2ItemSpawner
{
    internal static class SpawnPlacement
    {
        private const float HandForwardDistance = 0.45f;
        private const float GroundForwardDistance = 1.2f;
        private const float GroundHeightOffset = 0.05f;

        internal static Vector3 ResolvePosition(SpawnDestination destination,
            Transform spawnPoint, Transform hand, Transform playerRoot)
        {
            switch (destination)
            {
                case SpawnDestination.SpawnPoint:
                    if (spawnPoint == null) throw new InvalidOperationException("Spawn point transform missing.");
                    return spawnPoint.position;
                case SpawnDestination.Hand:
                    if (hand == null) throw new InvalidOperationException("Hand transform missing.");
                    return hand.position + hand.forward * HandForwardDistance;
                case SpawnDestination.Ground:
                    if (playerRoot == null) throw new InvalidOperationException("Player transform missing.");
                    var forward = playerRoot.forward;
                    forward.y = 0f;
                    if (forward.sqrMagnitude < 0.0001f) forward = Vector3.forward;
                    forward.Normalize();
                    return playerRoot.position + forward * GroundForwardDistance + Vector3.down * GroundHeightOffset;
                default:
                    throw new ArgumentOutOfRangeException(nameof(destination));
            }
        }

        internal static Quaternion ResolveRotation(SpawnDestination destination,
            Transform spawnPoint, Transform hand, Transform playerRoot)
        {
            switch (destination)
            {
                case SpawnDestination.SpawnPoint:
                    if (spawnPoint == null) throw new InvalidOperationException("Spawn point transform missing.");
                    return spawnPoint.rotation;
                case SpawnDestination.Hand:
                    if (hand == null) throw new InvalidOperationException("Hand transform missing.");
                    return hand.rotation;
                case SpawnDestination.Ground:
                    if (playerRoot == null) throw new InvalidOperationException("Player transform missing.");
                    var forward = playerRoot.forward;
                    forward.y = 0f;
                    if (forward.sqrMagnitude < 0.0001f) forward = Vector3.forward;
                    return Quaternion.LookRotation(forward.normalized, Vector3.up);
                default:
                    throw new ArgumentOutOfRangeException(nameof(destination));
            }
        }
    }
}