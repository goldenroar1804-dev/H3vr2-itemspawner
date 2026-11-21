// Explicit integration boundary for a future verified menu adapter.
// This intentionally does not hook or alter game UI without confirmed APIs.
using System;
using System.Collections.Generic;
using UnityEngine;

namespace H3VR2ItemSpawner
{
    internal interface IItemSpawnerMenuHost
    {
        bool IsAvailable { get; }
        Transform SpawnPoint { get; }
        Transform PlayerRoot { get; }
        Transform RightHand { get; }
        void RegisterTab(string title, Action drawCallback);
    }

    internal sealed class MenuIntegrationBridge
    {
        private readonly ItemSpawnerController _controller;
        private IItemSpawnerMenuHost _host;

        internal MenuIntegrationBridge(ItemSpawnerController controller)
        {
            _controller = controller ?? throw new ArgumentNullException(nameof(controller));
        }

        internal bool Attach(IItemSpawnerMenuHost host)
        {
            if (host == null || !host.IsAvailable)
            {
                _controller.State.Status = "Menu integration unavailable: host not verified.";
                return false;
            }
            // Registration intentionally withheld until the game-specific draw callback
            // and input controls are implemented against verified APIs.
            _host = host;
            _controller.State.Status = "Host detected; tab registration pending verified UI adapter.";
            return false;
        }

        internal Vector3 ResolvePosition(SpawnDestination destination)
        {
            if (_host == null || !_host.IsAvailable) throw new InvalidOperationException("Menu/player host unavailable.");
            return SpawnPlacement.ResolvePosition(destination, _host.SpawnPoint, _host.RightHand, _host.PlayerRoot);
        }

        internal Quaternion ResolveRotation(SpawnDestination destination)
        {
            if (_host == null || !_host.IsAvailable) throw new InvalidOperationException("Menu/player host unavailable.");
            return SpawnPlacement.ResolveRotation(destination, _host.SpawnPoint, _host.RightHand, _host.PlayerRoot);
        }
    }
}