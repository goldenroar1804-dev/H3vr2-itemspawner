# H3VR2 Quest Item Spawner — development tracker

This repository tracks the Quest 3 item-spawner mod prototype. It is **not an installable mod yet**.

## Current prototype

The v0.3.2 source package contains:
- A wrist-mounted Unity UI with item search and category filters.
- Quantity selection: 1, 5, 10, 25, or 50.
- Spawn destinations: spawn point, right-hand target, or ground raycast.
- A runtime catalog adapter for loaded `Footlong.Content.ItemDef` assets.
- A read-only reflection probe for logging runtime API signatures.

## APK metadata finding

The supplied APK's IL2CPP metadata (version 39) was decoded for this overload:

```csharp
Footlong.Items.SpawnItem(
    Footlong.Content.ItemDef item,
    UnityEngine.Vector3 position,
    UnityEngine.Quaternion rotation
) -> UnityEngine.GameObject
```

This is a **metadata-derived finding**, not a live runtime test. The adapter checks the exact overload before it enables spawning.

## Blocking requirements

1. Confirm a mod loader that works with this exact H3VR2 Quest package and game version.
2. Obtain that loader's generated IL2CPP/Unity interop assemblies for this exact build.
3. Compile the C# project against those references.
4. Test menu attachment, catalog population, and a single spawn in offline play.
5. Only then package and install the mod.

H3VR2's developer stated that modding was not supported at launch and that a proper framework would take additional work. A Quest-specific supported modding path has not been confirmed. LemonLoader supports Android/IL2CPP in general, but that does not prove compatibility with H3VR2.

## Safety / installation status

- No game APK has been modified or signed.
- No compiled DLL or installable package is available.
- Do not patch or reinstall H3VR2 based on this prototype. Back up app data before any future loader experiment.
- The browser preview is only a UI mockup and does not spawn game items.

## Source package

The current source prototype is version 0.3.2. It contains the C# scripts, LemonLoader bootstrap experiment, project file, API findings, and UI preview. The source must still be compiled and tested against a confirmed loader/runtime before it can be called a working mod.


## Build setup

A configurable project file and build-reference instructions are now included (`H3VR2ItemSpawner.csproj` and `BUILD_CONFIGURATION.md`). They make local compilation setup easier but do not indicate a successful build or verified loader compatibility.
