# Build configuration (source prototype)

This project expects local reference assemblies from the exact game build and loader; it does not bundle them.

## Windows build
1. Install .NET SDK 6 or newer.
2. Obtain a documented, compatible loader for this exact game build. Compatibility is not assumed.
3. Set:
   - `H3VR2ManagedDir`: folder containing Unity/game managed or generated interop reference DLLs.
   - `MelonLoaderDir`: folder containing MelonLoader.dll.
4. Run:
   ```powershell
   dotnet build H3VR2ItemSpawner.csproj -p:H3VR2ManagedDir="C:\path\to\managed" -p:MelonLoaderDir="C:\path\to\loader"
   ```

## Important
- This file only organizes reference paths; it cannot confirm API compatibility.
- Unity and IL2CPP generated bindings vary between builds and loaders.
- Missing references will cause compilation errors; do not substitute random DLLs.
- Current scripts remain a diagnostic/scaffolding prototype. Menu integration and spawn behavior have not been validated in-game.
