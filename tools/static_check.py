#!/usr/bin/env python3
"""Static sanity checks for the H3VR2 item-spawner scaffold (not a compiler)."""
from pathlib import Path
import sys

ROOT = Path(__file__).resolve().parents[1]
SOURCES = sorted((ROOT / "Source").glob("*.cs"))
REQUIRED = [ROOT / "H3VR2ItemSpawner.csproj", ROOT / "README.md",
 ROOT / "SAFE_TEST_CHECKLIST.md", ROOT / "BUILD_CONFIGURATION.md",
 ROOT / "REMAINING_COMPLETION_GATES.md"]
errors = []
for path in REQUIRED:
    if not path.exists(): errors.append("Missing required file: " + str(path.relative_to(ROOT)))
if not SOURCES: errors.append("No Source/*.cs files found")

def brace_balance(code):
    """Count braces outside comments, strings, and character literals."""
    i = balance = 0
    state = "code"
    while i < len(code):
        ch = code[i]; nxt = code[i + 1] if i + 1 < len(code) else ""
        if state == "code":
            if ch == "/" and nxt == "/": state = "line_comment"; i += 2; continue
            if ch == "/" and nxt == "*": state = "block_comment"; i += 2; continue
            if ch == '"': state = "string"; i += 1; continue
            if ch == "'": state = "char"; i += 1; continue
            if ch == "{": balance += 1
            elif ch == "}": balance -= 1
            if balance < 0: return balance
        elif state == "line_comment":
            if ch == "\n": state = "code"
        elif state == "block_comment":
            if ch == "*" and nxt == "/": state = "code"; i += 2; continue
        elif state in ("string", "char"):
            term = '"' if state == "string" else "'"
            if ch == "\\": i += 2; continue
            if ch == term: state = "code"
        i += 1
    return balance

for path in SOURCES:
    code = path.read_text(encoding="utf-8")
    bal = brace_balance(code)
    if bal != 0: errors.append("Brace balance " + str(bal) + " in " + str(path.relative_to(ROOT)))
    if "\x00" in code: errors.append("NUL byte found in " + str(path.relative_to(ROOT)))
if errors:
    print("\n".join("FAIL: " + e for e in errors)); sys.exit(1)
print("Static scaffold checks passed (" + str(len(SOURCES)) + " source files).")
print("This is not a C# compilation or game-runtime test.")
