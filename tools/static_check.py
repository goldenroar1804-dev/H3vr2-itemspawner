#!/usr/bin/env python3
"""Integrity checks for experimental source; not compilation or runtime tests."""
from pathlib import Path
import json, sys, xml.etree.ElementTree as ET

ROOT = Path(__file__).resolve().parents[1]
SRC = ROOT / "Source"
SOURCES = sorted(SRC.glob("*.cs"))
errors = []
required = ["H3VR2ItemSpawner.csproj", "README.md", "SAFE_TEST_CHECKLIST.md",
            "BUILD_CONFIGURATION.md", "REMAINING_COMPLETION_GATES.md",
            "SOURCE_REVIEW_FINDINGS.md", "QUEST_COMPATIBILITY_RESEARCH.md",
            "MOD_STATUS.json", ".github/workflows/static-check.yml"]
for rel in required:
    if not (ROOT / rel).is_file(): errors.append("Missing required file: " + rel)
if not SOURCES: errors.append("No Source/*.cs files found")

def brace_balance(code):
    """Count braces outside comments, ordinary/verbatim strings, and char literals."""
    i, balance, state = 0, 0, "code"
    while i < len(code):
        ch = code[i]
        nxt = code[i+1] if i+1 < len(code) else ""
        if state == "code":
            if ch == "/" and nxt == "/": state="line"; i+=2; continue
            if ch == "/" and nxt == "*": state="block"; i+=2; continue
            if ch == "@" and nxt == '"': state="verbatim"; i+=2; continue
            if ch == '"': state="string"; i+=1; continue
            if ch == "'": state="char"; i+=1; continue
            if ch == "{": balance+=1
            elif ch == "}":
                balance-=1
                if balance < 0: return balance
        elif state == "line":
            if ch == "\\n": state="code"
        elif state == "block":
            if ch == "*" and nxt == "/": state="code"; i+=2; continue
        elif state == "string":
            if ch == "\\": i+=2; continue
            if ch == '"': state="code"
        elif state == "verbatim":
            if ch == '"' and nxt == '"': i+=2; continue
            if ch == '"': state="code"
        elif state == "char":
            if ch == "\\\\": i+=2; continue
            if ch == "'": state="code"
        i+=1
    if state in ("block", "string", "verbatim", "char"): return -999
    return balance

for path in SOURCES:
    code=path.read_text(encoding="utf-8")
    rel=str(path.relative_to(ROOT))
    if brace_balance(code)!=0: errors.append("Brace/lexical imbalance in "+rel)
    if "\\x00" in code: errors.append("NUL byte in "+rel)

contracts={
"Source/Bootstrap.cs":["OnInitializeMelon","WristMenuDiscovery.Run"],
"Source/ReflectionItemCatalog.cs":["Refresh","Filter","ItemDef"],
"Source/ReflectionSpawnService.cs":["Prepare","SpawnItem","Spawn"],
"Source/SpawnBatchCoordinator.cs":["ClampQuantity","SpawnBatch","BatchSpreadRadius"],
"Source/ItemSpawnerTabState.cs":["SetQuantity","MarkRecent","Visible"],
"Source/WristMenuDiscovery.cs":["Run","CANDIDATE"],
"Source/CatalogSnapshot.cs":["Replace","Categories","FindByName"],
"Source/ItemSpawnerController.cs":["Refresh","Visible","SpawnSelected","Select"],
"Source/SpawnPlacement.cs":["ResolvePosition","ResolveRotation","HandForwardDistance","GroundForwardDistance"]}
for rel, needles in contracts.items():
    p=ROOT/rel
    if not p.exists(): errors.append("Contract file missing: "+rel); continue
    content=p.read_text(encoding="utf-8")
    for needle in needles:
        if needle not in content: errors.append("Missing contract "+needle+" in "+rel)

# Explicit guardrails on diagnostic scanning: capped discovery, targeted type probes,
# and resilient handling of reflection failures must remain in the source.
probe=ROOT/"Source/WristMenuDiscovery.cs"
if probe.exists():
    code=probe.read_text(encoding="utf-8")
    for needle in ("candidates > 100", "membersLogged", "ReflectionTypeLoadException",
                   "Footlong.Items", "Footlong.Content.ItemDef"):
        if needle not in code: errors.append("Diagnostic safeguard/target missing: "+needle)
spawn=ROOT/"Source/ReflectionSpawnService.cs"
if spawn.exists():
    code=spawn.read_text(encoding="utf-8")
    for needle in ("typeof(Vector3)", "typeof(Quaternion)", "typeof(GameObject)",
                   "TargetInvocationException"):
        if needle not in code: errors.append("Spawn adapter guard missing: "+needle)

try: ET.parse(ROOT/"H3VR2ItemSpawner.csproj")
except Exception as exc: errors.append("Project XML invalid: "+str(exc))
try:
    status=json.loads((ROOT/"MOD_STATUS.json").read_text(encoding="utf-8"))
    if status.get("verified") is not False: errors.append("Status must explicitly remain unverified")
    feats=status.get("features",{})
    for key in ("actual_menu_registration","runtime_spawn_verified","compiled_mod","installable_package"):
        if feats.get(key) is not False: errors.append("Status must mark "+key+" false until verified")
except Exception as exc: errors.append("MOD_STATUS.json invalid: "+str(exc))

workflow=(ROOT/".github/workflows/static-check.yml")
if workflow.exists():
    wf=workflow.read_text(encoding="utf-8")
    for needle in ("tools/static_check.py","pull_request","push"):
        if needle not in wf: errors.append("Static-check workflow missing "+needle)

if errors:
    print("\n".join("FAIL: "+e for e in errors)); sys.exit(1)
print("Integrity checks passed: "+str(len(SOURCES))+" source files, contracts, project XML, status JSON.")
print("Not a C# compiler, loader compatibility check, or runtime validation.")
