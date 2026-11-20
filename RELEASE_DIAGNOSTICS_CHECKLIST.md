# Runtime diagnostics and release checklist

## Diagnostics
The future menu integration should surface these states rather than silently fail:
- host wrist-menu API not discovered
- cursor/input event not available
- catalog not initialized or empty
- catalog may be incomplete (loaded definitions only)
- spawn method unavailable
- invalid selection or destination
- each spawn success/failure, including batch totals

## Release verification
- [ ] Identify exact runtime loader and game version.
- [ ] Obtain matching interop references.
- [ ] Compile with exact references.
- [ ] Verify mod initializes without crashing.
- [ ] Register tab in original wrist menu.
- [ ] Cursor activates tab buttons.
- [ ] Search and categories work; refresh works.
- [ ] Spawn point, hand, and ground resolvers work.
- [ ] Single item spawn succeeds in offline testing.
- [ ] Quantity batches succeed; failures handled.
- [ ] Recent items and sorting work.
- [ ] UI prevents unintended batch spawning.
- [ ] Verify Quest performance and lifecycle.
- [ ] Record test logs and package only after verification.

## Current
Several utility source modules are prototypes committed in this repo, but they are not yet assembled into a compiled mod. Host/cursor hook and destination transforms remain unresolved.
