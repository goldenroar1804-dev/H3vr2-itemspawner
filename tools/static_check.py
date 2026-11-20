#!/usr/bin/env python3
"""Source-integrity checks for the experimental scaffold. Not a C# compiler."""
from pathlib import Path
import sys

ROOT = Path(__file__).resolve().parents[1]
SRC = ROOT / "Source"
SOURCES = sorted(SRC.glob("*.cs"))
REQUIRED_FILES = [
    "H3VR2ItemSpawner.csproj", "README.md", "SAFE_TEST_CHECKLIST.md",
    "BUILD_CONFIGURATION.md", "REMAINING_COMPLETION_GATES.md",
    "SOURCE_REVIEW_FINDINGS.md",
]
errors = []
for rel in REQUIRED_FILES:
    if not (ROOT / rel).is_file():
        errors.append("Missing required file: " + rel)
if not SOURCES:
    errors.append("No Source/*.cs files found")

def brace_balance(code):
    i, balance, state = 0, 0, "code"
    while i < len(code):
        ch, nxt = code[i], code[i + 1] if i + 1 < len(code) else ""
        if state == "code":
            if ch == "/" and nxt == "/": state = "line"; i += 2; continue
            if ch == "/" and nxt == "*": state = "block"; i += 2; continue
            if ch == '"': state = "string"; i += 1; continue
            if ch == "'": state = "char"; i += 1; continue
            if ch == "{": balance += 1
            elif ch == "}": balance -= 1
            if balance < 0: return balance
        elif state == "line":
            if ch == "\n": state = "code"
        elif state == "block":
            if ch == "*" and nxt == "/": state = "code"; i += 2; continue
        elif state in ("string", "char"):
            term = '"' if state == "string" else "'"
            if ch == "\\": i += 2; continue
            if ch == term: state = "code"
        i += 1
    return balance

for path in SOURCES:
    code = path.read_text(encoding="utf-8")
    if brace_balance(code) != 0:
        errors.append("Brace imbalance in " + str(path.relative_to(ROOT)))
    if "\x00" in code:
        errors.append("NUL byte in " + str(path.relative_to(ROOT)))

# Required scaffold contract: ensure the key building blocks still exist.
contracts = {
    "Source/Bootstrap.cs": ["OnInitializeMelon", "WristMenuDiscovery.Run"],
    "Source/ReflectionItemCatalog.cs": ["Refresh", "Filter", "ItemDef"],
    "Source/ReflectionSpawnService.cs": ["Prepare", "SpawnItem", "Spawn"],
    "Source/SpawnBatchCoordinator.cs": ["ClampQuantity", "SpawnBatch", "BatchSpreadRadius"],
    "Source/ItemSpawnerTabState.cs": ["SetQuantity", "MarkRecent", "Visible"],
    "Source/WristMenuDiscovery.cs": ["Run", "CANDIDATE"],
}
for rel, needles in contracts.items():
    path = ROOT / rel
    if not path.exists():
        errors.append("Contract file missing: " + rel)
        continue
    content = path.read_text(encoding="utf-8")
    for needle in needles:
        if needle not in content:
            errors.append("Missing scaffold contract " + needle + " in " + rel)

if errors:
    print("\n".join("FAIL: " + error for error in errors))
    sys.exit(1)
print("Source integrity checks passed (" + str(len(SOURCES)) + " C# source files).")
print("Contract checks passed. This is not compilation or game-runtime validation.")
