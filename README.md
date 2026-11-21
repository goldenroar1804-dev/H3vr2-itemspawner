# H3VR2 Quest Item Spawner — source scaffold

**Status: experimental, source-only. Not compiled, installed, or tested in-game.**

This repository is a prototype for an offline-use Quest item-spawner mod. It is deliberately separated into catalog, spawn-adapter, batch, state, and diagnostic components so future work can be tested independently.

## Current source inventory

| Component | Current state |
|---|---|
| Loader bootstrap | Experimental MelonLoader entry point; compatibility unverified |
| Catalog | Reflection-based lookup/filtering scaffold |
| Spawn adapter | Reflection adapter for a metadata-derived method signature; unverified |
| Batch coordinator | Quantity clamping, destination-resolver interface, spacing, per-item error isolation |
| Menu discovery | Read-only reflection diagnostics; does not attach a menu |
| UI state | Search/category/quantity/destination/recent state; not wired to UI |
| Static checks | Python source integrity checks; not a compiler or runtime test |

## Safety and compatibility gate

The exact Quest package/version must have a documented compatible loader before attempting installation. Generic Android/IL2CPP support is not proof of game compatibility. Do not patch, sign, or reinstall the game based solely on this scaffold.

## Build inputs (only once compatibility is verified)

- Compatible loader's assemblies.
- Game-version-specific Unity/IL2CPP interop references.
- Matching .NET build tools and target configuration.
- A backed-up test environment and offline test plan.

See `BUILD_CONFIGURATION.md`, `QUEST_COMPATIBILITY_RESEARCH.md`, and `REMAINING_COMPLETION_GATES.md`.

## Validation

Run the static checker:

```bash
python3 tools/static_check.py
```

It checks source structure and expected scaffold components. Passing is **not** evidence of C# compilation or in-game behavior.

## Milestones

1. Confirm loader/package/version compatibility (blocked).
2. Acquire matching generated interop references (blocked by 1).
3. Compile and resolve errors (blocked by 1–2).
4. Validate diagnostic initialization and menu discovery (blocked by 1–3).
5. Attach an actual UI and test catalog + single spawn (blocked by verified APIs).
6. Test batches, cleanup, stability, then package (blocked by 1–5).



## November 20 maintenance update

The source-integrity checker now receives lexical and workflow checks in addition to scaffold contracts. The mod remains diagnostic-only until exact Quest loader compatibility and API references can be verified. No installable artifact is claimed.
