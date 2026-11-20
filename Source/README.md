# H3VR2 Quest Item Spawner — source prototype

This is an experimental source scaffold, not a compiled or tested game mod.

## Status
- Bootstrap assembly version aligned to 0.3.2.
- Bootstrap logs initialization and checks for the metadata-derived player type.
- Guarded reflection probe checks the exact metadata-derived SpawnItem signature.
- Item spawning and wrist UI attachment remain disabled pending confirmed loader/runtime compatibility and interop references.

## Build prerequisites
This source requires the target game's matching IL2CPP/Unity interop references and a compatible mod loader. These are intentionally not bundled. It has not been compiled or tested on Quest.

## Important
Do not install this as a DLL yet. The bootstrap is diagnostic-only and is not a functioning item-spawner menu.
