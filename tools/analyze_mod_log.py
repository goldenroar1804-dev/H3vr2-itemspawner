#!/usr/bin/env python3
"""Offline triage for H3VR2 item-spawner diagnostic logs; never uploads log data."""
from pathlib import Path
import argparse
import re

PATTERNS = {
    "game_or_platform": re.compile(r"(game.?version|version|build|quest|android|il2cpp)", re.I),
    "loader": re.compile(r"(melonloader|bepinex|loader|mod.?loader|initializ)", re.I),
    "assembly": re.compile(r"(assembly|assemblies|dll|interop)", re.I),
    "candidate": re.compile(r"(CANDIDATE:|Footlong\\.|ItemDef)", re.I),
    "spawn": re.compile(r"(spawnitem|spawn|catalog|item.?def)", re.I),
    "menu": re.compile(r"(menu|wrist|tab|ui|canvas)", re.I),
    "failure": re.compile(r"(ERROR|EXCEPTION|FAILED|FATAL|WARN(?:ING)?|\\bWARN\\b)", re.I),
}

def analyze(text):
    lines = text.splitlines()
    groups = {key: [] for key in PATTERNS}
    for line in lines:
        stripped = line.strip()
        if not stripped:
            continue
        for key, pattern in PATTERNS.items():
            if pattern.search(stripped):
                groups[key].append(stripped)
    # Preserve unique lines in input order, avoiding noisy duplicate summaries.
    for key, values in groups.items():
        groups[key] = list(dict.fromkeys(values))
    return groups

def main():
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("log", type=Path, help="Local loader/game log file")
    parser.add_argument("--output", type=Path, default=None, help="Write summary to this path")
    args = parser.parse_args()
    if not args.log.is_file():
        raise SystemExit("Log file not found.")
    text = args.log.read_text(encoding="utf-8", errors="replace")
    groups = analyze(text)
    lines = [
        "H3VR2 Item Spawner diagnostic summary (offline; heuristic only)",
        "Input lines: " + str(len(text.splitlines())),
        "",
    ]
    for key, title in [
        ("game_or_platform", "Game/platform/version clues"),
        ("loader", "Loader/init clues"),
        ("assembly", "Assembly/reference clues"),
        ("candidate", "Type/API candidates"),
        ("menu", "Menu/UI clues"),
        ("spawn", "Catalog/spawn clues"),
        ("failure", "Warnings and error-like lines"),
    ]:
        lines += [title + " (" + str(len(groups[key])) + "):"]
        lines.extend("  " + item for item in groups[key][:150])
        lines.append("")
    lines += [
        "Next evidence to check:",
        "  - Confirm the exact game build and a documented loader compatibility route.",
        "  - Confirm loader initialization completed without fatal errors.",
        "  - Verify candidate APIs against runtime types; name matches are not proof.",
        "  - Do not treat this report as compilation, menu hookup, or spawn verification.",
        "  - Remove personal paths/tokens before sharing a log or report.",
    ]
    report = "\n".join(lines) + "\n"
    if args.output:
        args.output.write_text(report, encoding="utf-8")
        print("Saved report:", args.output)
    else:
        print(report)

if __name__ == "__main__":
    main()
