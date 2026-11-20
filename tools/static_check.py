#!/usr/bin/env python3
"""Static sanity checks for the H3VR2 item-spawner source scaffold.

These checks intentionally do not claim to compile Unity/IL2CPP code.
"""
from pathlib import Path
import sys

ROOT = Path(__file__).resolve().parents[1]
SOURCES = list((ROOT / "Source").glob("*.cs"))
REQUIRED = [
    ROOT / "H3VR2ItemSpawner.csproj",
    ROOT / "README.md",
    ROOT / "SAFE_TEST_CHECKLIST.md",
    ROOT / "BUILD_CONFIGURATION.md",
]

errors = []
for path in REQUIRED:
    if not path.exists():
        errors.append(f"Missing required file: {path.relative_to(ROOT)}")
if not SOURCES:
    errors.append("No Source/*.cs files found")

for path in SOURCES:
    text = path.read_text(encoding="utf-8")
    balance = 0
    for ch in text:
        if ch == "{":
            balance += 1
        elif ch == "}":
            balance -= 1
        if balance < 0:
            errors.append(f"Unbalanced closing brace in {path.relative_to(ROOT)}")
            break
    if balance != 0:
        errors.append(f"Brace balance {balance} in {path.relative_to(ROOT)}")
    if "\x00" in text:
        errors.append(f"NUL byte found in {path.relative_to(ROOT)}")

if errors:
    print("\n".join("FAIL: " + e for e in errors))
    sys.exit(1)
print(f"Static scaffold checks passed ({len(SOURCES)} source files).")
print("This is not a game-runtime or C# compilation test.")
