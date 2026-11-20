# Completion gates and implementation backlog

Updated November 2026. This is a prioritized checklist; no unchecked item is implied complete.

## 0 — Compatibility (blocking)
- [ ] Identify target app package/version/build and CPU architecture.
- [ ] Find an explicitly supported mod-loader route for that game build.
- [ ] Validate the route from credible documentation/community evidence.
- [ ] Establish a reversible test procedure and backup.

## 1 — Build references (blocked by 0)
- [ ] Collect loader assemblies from a supported setup.
- [ ] Collect generated Unity/IL2CPP interop references for the exact build.
- [ ] Verify signatures and assembly versions.
- [ ] Set reference paths without committing proprietary binaries.

## 2 — Compile
- [ ] Compile the project against the verified reference set.
- [ ] Resolve compiler warnings/errors and document build command.
- [ ] Preserve build logs and target-version details.

## 3 — Diagnostics
- [ ] Verify bootstrap initialization in offline mode.
- [ ] Verify type-discovery output and cap/no-crash behavior.
- [ ] Verify adapter preparation without invoking spawn.
- [ ] Capture a diagnostic log and note limitations.

## 4 — Menu integration
- [ ] Identify an existing menu API and lifecycle.
- [ ] Confirm callback/input behavior and safe UI teardown.
- [ ] Attach a menu tab only after API evidence exists.
- [ ] Implement search, categories, quantity, destination, and recent list UI.
- [ ] Add cancel/close behavior and controller interaction feedback.

## 5 — Single spawn and batch
- [ ] Validate catalog completeness and categories.
- [ ] Test one item in offline mode at a safe location.
- [ ] Test destination transforms and orientation.
- [ ] Test quantity controls and spaced batches.
- [ ] Confirm failure handling, limits, and cleanup.

## 6 — Package
- [ ] Create package only from a successful verified build.
- [ ] Document install/remove/rollback procedure for supported loader.
- [ ] Include target build, checksums, and test report.
- [ ] Do not claim verified status until tests are recorded.

## Present state
Source scaffolding and source-integrity checks only. Loader compatibility, compilation, runtime invocation, UI hookup, and packaging remain unverified.
