# The one thing needed to unlock the final integration

The source work is in place, but the mod cannot safely be completed against guessed game interfaces.

## If/when you can provide one item
From Quest, provide **a loader log after launching H3VR2 once with the mod-loader installed** (or the loader's documented support page for this exact H3VR2 version). If no loader is installed or supported, do not install a random one.

A log should ideally contain:
- game version/build and Quest platform
- loader version and initialization result
- loaded assemblies / type-probe output
- any errors

Do not send account credentials, private tokens, or unrelated personal information.

## What is already scaffolded
Catalog discovery/filtering, a metadata-derived spawn adapter, destination/batch logic, quantity safeguards, menu integration boundary, diagnostics, and source integrity checks.

## What is not yet confirmed
- that this game build supports the chosen loader
- that the reflection signatures match live runtime types
- that the catalog query returns usable definitions
- that menu registration can be implemented against the actual UI
- successful compile, in-game spawn test, and package install

**Do not install a DLL or package from this repository until those checks are completed.**
