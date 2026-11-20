# Safe testing checklist

This checklist is for validating the source prototype without risking the installed game.

## Gate 0 — build inputs (not yet satisfied)
- [ ] Confirm the exact game build and a compatible loader using a documented, reversible setup.
- [ ] Obtain the matching Unity/IL2CPP interop references.
- [ ] Create/verify a project file and compile without warnings that indicate missing or mismatched APIs.
- [ ] Verify the compiled artifact is a mod DLL—not an APK patch.

## Gate 1 — diagnostics only
- [ ] Run the loader with the diagnostic bootstrap only.
- [ ] Confirm the mod log shows initialization and the runtime probe.
- [ ] Capture errors and runtime type-discovery results.
- [ ] Confirm the game launches and exits normally before proceeding.

## Gate 2 — catalog, read-only
- [ ] Populate catalog from loaded ItemDef assets.
- [ ] Verify category/name extraction and search.
- [ ] Verify refresh doesn't cause a hang or repeated allocations at an unreasonable rate.

## Gate 3 — single-item spawn
- [ ] Confirm the exact reflected SpawnItem overload at runtime.
- [ ] Test only one harmless item in offline play.
- [ ] Verify placement, orientation, and cleanup; do not test batches yet.

## Gate 4 — controlled batches and UI
- [ ] Confirm destination resolvers and menu attachment are reliable.
- [ ] Start with quantity 1, then 5; increase only after successful tests.
- [ ] Confirm cancellation and failure reporting.
- [ ] Test menu visibility and controller input without blocking gameplay.

## Stop conditions
Stop and remove/disable the mod if the game fails to launch, crashes, corrupts saved data,
or repeatedly throws errors. Do not use APK patching or loader installation experiments
without a backup and a verified compatibility route.

**Current status:** Gates 0–4 are pending. The repository contains source scaffolding and
metadata-derived API observations only; it is not a compiled or tested mod.
