# H3VR2 Quest modding compatibility check (November 20, 2026)

## Findings
- H3VR2 is an IL2CPP game and the developer has said a mod framework was not available at launch.
- The developer has publicly indicated that the framework/platform scope remains undecided; Quest support has not been confirmed.
- Generic Quest IL2CPP patching tools (including QuestPatcher) do not demonstrate compatibility with this specific game build.
- H3VR (the original PC game) modding templates, BepInEx instructions, and MeatKit documentation must not be assumed to work for H3VR2 Quest. H3VR2 is a separate game and runtime.

## References
- Steam FAQ thread: https://steamcommunity.com/app/4632350/discussions/0/844005195821141165/
- QuestPatcher project: https://github.com/Lauriethefish/QuestPatcher
- Community H3VR modding template (original game): https://github.com/H3VR-Modding/H3VRPluginTemplate

## Project decision
Keep the prototype source-only and diagnostic until a loader/runtime route is demonstrated on the exact H3VR2 Quest package. Do not make APK changes or install a loader based on generic compatibility claims. The catalog/spawn adapters remain speculative and require game-specific interop plus validation.
