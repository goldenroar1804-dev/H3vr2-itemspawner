# APK integration gate — 2026-11-20

## Current state
The repository contains a reflection-oriented source prototype and metadata-derived API notes. It is not a functional APK modification. The target package is an IL2CPP Unity Android application; metadata alone does not establish a supported runtime extension point.

## Required inputs for a legitimate, testable integration
- The exact original APK for the target Quest build (upload to this chat).
- Confirmation that the build permits user-installed extensions or a supported mod loader.
- Loader/runtime compatibility and matching generated interop references, if the game provides an extension path.
- Build tooling and signing configuration for a test package.

## Validation sequence
1. Record APK package name, version, architecture, and hashes; retain the original unchanged.
2. Confirm an extension path compatible with the exact build before compiling integration code.
3. Compile the prototype against matching references, with diagnostics enabled.
4. Run a no-op startup test first; verify logs and stability.
5. Test catalog enumeration, single-item spawn, then quantities and destination modes.
6. Package/sign only through a compatible, documented extension/build workflow. Never claim the package is test-ready until it installs and runs on headset.

## Scope
This tracker does not contain instructions for bypassing protections, defeating integrity checks, or patching proprietary game internals. It focuses on compatible extension and test workflows.

## Next step
Upload the original APK. Do not re-upload metadata files unless they have changed; prior metadata analysis remains recorded in the repository.
