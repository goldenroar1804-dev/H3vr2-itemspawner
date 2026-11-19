# Fix plan — Quest compatibility and runtime reliability

This is a source-fix checklist, not a claim that fixes have already been compiled or tested.

## Confirmed source issues to address
- [ ] Align the MelonMod assembly version with the source package version (the package is labeled 0.3.2, while the bootstrap metadata was observed as 0.3.1).
- [ ] Remove the assumption that `StandaloneInputModule` is a usable Quest controller fallback. Detect the actual XR UI input module available in the target runtime; if unavailable, fail visibly and log the missing dependency rather than showing a non-interactive menu.
- [ ] Guard reflected API discovery and invocation. Require the exact `Footlong.Items.SpawnItem(ItemDef, Vector3, Quaternion)` signature and return type before enabling spawn controls; report a clear error if it is absent.
- [ ] Validate each selected item and spawn transform before invocation; catch and log invocation exceptions without breaking the menu.
- [ ] Make catalog limitations visible. `Resources.FindObjectsOfTypeAll` may only return definitions loaded at that point, so the UI must not imply the catalog is complete.
- [ ] Test quantity limits, destination selection, and repeated spawning only after a single-item spawn is confirmed.

## External blockers
- [ ] Confirm a loader compatible with this exact H3VR2 Quest build. General Android/IL2CPP support is not proof of game compatibility.
- [ ] Obtain matching generated Unity/IL2CPP interop references.
- [ ] Compile against those exact references.
- [ ] Run on Quest 3 and verify menu attachment, controller interaction, catalog population, and spawning.

## Repository state
The C# source files from the v0.3.2 archive are not currently present in this GitHub repository. The repository currently contains project documentation only. Therefore, no source-code fix is claimed by this checklist.

## Release gate
Do not label this as a working mod or distribute an installable APK/DLL until it compiles against the target runtime and passes in-game tests. The browser preview is not a game mod.
