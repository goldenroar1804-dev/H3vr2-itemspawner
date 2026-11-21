#!/usr/bin/env python3
"""Summarize diagnostic output from the prototype; no game log is uploaded."""
from pathlib import Path
import argparse
import re

def analyze(text):
    types, methods, warnings, errors = [], [], [], []
    for line in text.splitlines():
        s = line.strip()
        if "CANDIDATE:" in s: types.append(s.split("CANDIDATE:", 1)[1].strip())
        if "METHOD:" in s or "PROPERTY:" in s or "FIELD:" in s: methods.append(s)
        if "WARNING" in s.upper() or "WARN:" in s.upper(): warnings.append(s)
        if "ERROR" in s.upper() or "EXCEPTION" in s.upper() or "FAILED" in s.upper(): errors.append(s)
    return types, methods, warnings, errors

def main():
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("log", type=Path)
    parser.add_argument("--output", type=Path, default=None)
    args = parser.parse_args()
    if not args.log.is_file():
        raise SystemExit("Log file not found.")
    text = args.log.read_text(encoding="utf-8", errors="replace")
    types, methods, warnings, errors = analyze(text)
    lines = [
        "H3VR2 Item Spawner diagnostic summary (offline analysis only)",
        "Types discovered: " + str(len(types)),
        "Relevant member lines: " + str(len(methods)),
        "Warnings: " + str(len(warnings)),
        "Error-like lines: " + str(len(errors)),
        "",
        "Candidate types:",
    ]
    lines.extend("  " + x for x in types[:100])
    lines += ["", "Relevant members:"]
    lines.extend("  " + x for x in methods[:500])
    lines += ["", "Warnings:"]
    lines.extend("  " + x for x in warnings[:100])
    lines += ["", "Error-like lines:"]
    lines.extend("  " + x for x in errors[:100])
    lines += ["", "This summary is heuristic; it does not establish compatibility or successful execution."]
    report = "\n".join(lines) + "\n"
    if args.output:
        args.output.write_text(report, encoding="utf-8")
        print("Saved report:", args.output)
    else:
        print(report)

if __name__ == "__main__":
    main()
