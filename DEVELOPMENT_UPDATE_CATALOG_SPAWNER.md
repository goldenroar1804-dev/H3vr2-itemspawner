# Development update: reflection catalog and spawn adapter

Added initial independent modules:
- Runtime catalog: enumerates loaded ItemDef assets, reads a likely category/name, supports filtering.
- Spawn service: resolves the exact metadata-derived SpawnItem(ItemDef, Vector3, Quaternion) overload and validates definition before invocation, with exception logging.

These are integration building blocks, not tested game-ready functionality. Catalog completeness, categories, method resolution, and actual spawning need runtime verification. The modules do not yet connect to the existing wrist-menu host or cursor because the exact host hook is not known.
