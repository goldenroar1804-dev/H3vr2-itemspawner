# Remaining completion gates — honest status and actionable route

This project cannot be called complete until the gates below are verified against the user's exact Quest game build.

## Gate A: loader compatibility
- Do not assume generic Android IL2CPP loaders are compatible with this app.
- Confirm loader support for the package identifier, game version, architecture, and metadata/runtime layout.
- Avoid APK patching or installation until there is a documented, reversible, and compatible path.

## Gate B: interop references
- Obtain generated Unity and IL2CPP interop references from a compatible loader installation for the exact game build.
- Verify required types and method signatures against runtime-generated references, not only APK metadata.
- Keep references outside the repository if they are proprietary or user-specific.

## Gate C: compile and runtime validation
- Build against the verified reference set.
- Resolve compiler errors before attempting install.
- Start with diagnostic logging only; verify initialization and menu discovery.
- Validate catalog and single-item spawning in offline play before batch spawning.

## Gate D: menu and package
- Menu hookup requires verified existing menu APIs and interaction callbacks.
- Package only after a successful build and controlled runtime tests.
- Record build provenance, target version, and test outcomes. Never label an untested artifact as verified.

## Current limitations
The repository contains source scaffolding and static checks. These gates are not complete. The project has not been built, installed, or tested in-game. This document is an implementation plan, not a claim of completion.
