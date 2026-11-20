# Integration contract — wrist menu tab

The tab integration should implement this behavior:
- Register a single Item Spawner page with the existing wrist menu.
- Do not create a custom controller ray, cursor, or input module.
- Route the game's cursor click/selection events to the tab's buttons and item rows.
- Keep tab state: search text, category, selected definition, quantity (1/5/10/25/50), and destination.
- On spawn action, call SpawnBatchCoordinator with destination resolvers supplied by the existing menu/game context.
- If host registration, cursor event wiring, or destination resolution is missing, disable the spawn button and show/log the reason.
- Keep the catalog refresh action separate from opening the tab to avoid repeatedly enumerating resources.

## Integration is blocked on actual runtime host
Exact menu class, tab-registration API, cursor event and destination transforms must be identified from matching runtime assemblies or a live diagnostic capture. Do not guess names and claim integration.
