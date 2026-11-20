# Existing wrist-menu integration plan

User design decision: **add a tab to the game's existing wrist menu and reuse its existing cursor/controller input**. Do not create a second wrist menu, raycaster, controller cursor, or competing input module.

## Integration requirements
1. Locate and hook the existing wrist-menu host/tab registration point in the exact game's runtime.
2. Register an Item Spawner tab/page in that host.
3. Reuse the host's cursor, click/press events, and focus/visibility lifecycle. The tab should not create a new XR pointer.
4. Catalog loaded ItemDef assets and display searchable, categorized results; surface incomplete/loaded-only catalog status.
5. Spawn via verified game-native method only after the exact method signature and item/transform validity are confirmed.
6. Add quantity 1/5/10/25/50 and destinations: spawn point, hand, ground.
7. Fail closed and log actionable errors if host/cursor registration or spawning API cannot be resolved.

## Planned separation
- **Host adapter:** discovers/registers the existing menu tab and routes the game's cursor events.
- **Tab view:** search, category, item rows, quantity and destination controls. No input module of its own.
- **Catalog:** loaded item definitions, filtering, category grouping, refresh and status.
- **Spawn service:** guarded native API resolution, spawn request validation, result/error logging.
- **Quest validation:** test the tab's visibility, cursor click/selection, search, catalog, single spawn, and repeated quantities on headset.

## Current limitation
The repo's currently committed source does not contain the game's wrist-menu implementation or its runtime interop references. Exact host and cursor hook names cannot responsibly be guessed. This document records the integration direction; it is not proof the host hook or spawn calls work.
