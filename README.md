# H3VR2 Quest Item Spawner — source scaffold

**Status: experimental, source-only. Not compiled, installed, or tested in-game.**

This prototype separates catalog discovery, spawn-adapter reflection, batch/state logic, placement helpers, and runtime diagnostics.

## Components
- Loader bootstrap and guarded initialization diagnostics (loader compatibility unverified).
- Reflection catalog discovery and search/filter state.
- Metadata-derived spawn adapter and API signature probe.
- Batch quantity limits, grouped placement, and per-item error isolation.
- Read-only menu/API discovery; no actual menu patching.
- Python source integrity checks (not compilation/runtime tests).

## Latest development
Runtime diagnostics now specifically enumerate the metadata-derived `Footlong.Items`, `Footlong.Content.ItemDef`, `Footlong.Content.ItemSpawnerDef`, and `Footlong.Player` signatures and fields, where discoverable. They also scan likely menu-related types. Output is capped and guarded to reduce crash risk. The results still must be captured from the exact live game runtime to confirm signatures and compatibility.

## Compatibility gate
The target Quest package/version must have a documented compatible loader. Generic Android/IL2CPP support is not proof of compatibility. Do not install, patch, or sign the game based solely on this scaffold.

## Build prerequisites after compatibility is verified
- Compatible loader assemblies.
- Game-version-specific Unity/IL2CPP interop references.
- Matching build tools and target configuration.
- Backed-up test environment and offline test plan.

See `BUILD_CONFIGURATION.md`, `QUEST_COMPATIBILITY_RESEARCH.md`, `REMAINING_COMPLETION_GATES.md`, and `FINAL_INTEGRATION_INPUT.md`.

## Validation
Run `python3 tools/static_check.py`. Passing means only source integrity checks passed; it does not prove compilation or runtime behavior.

## Milestones
1. Confirm loader/package/version compatibility.
2. Acquire matching generated interop references.
3. Compile and resolve errors.
4. Validate diagnostic initialization and runtime probe.
5. Implement menu hookup against verified APIs.
6. Validate catalog and single spawn; then batches, cleanup, stability, and packaging.

## November 20 update
Expanded the read-only runtime probe to log targeted Footlong signatures plus likely menu APIs. This is diagnostic groundwork, not an installable mod.

## Update tracking

Repository changes are recorded in [`DEVELOPMENT_LOG.md`](DEVELOPMENT_LOG.md). This log records development work and validation results; it does not imply that the scaffold has been compiled or tested in-game.
