// Writes capped, non-invasive runtime inventory to persistent storage for compatibility review.
using System;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace H3VR2ItemSpawner
{
    internal static class RuntimeDiagnostics
    {
        private const int MaxReportChars = 120000;
        private static readonly StringWriter Buffer = new StringWriter();
        private static bool _finished;
        internal static string ReportPath { get; private set; }

        internal static void Record(string line)
        {
            if (_finished) return;
            if (Buffer.GetStringBuilder().Length >= MaxReportChars)
            {
                Buffer.WriteLine("[REPORT CAP REACHED]");
                _finished = true;
                return;
            }
            Buffer.WriteLine(line ?? "(null)");
        }

        internal static void Run(Action<string> log)
        {
            Record("=== H3VR2 Item Spawner Runtime Diagnostic ===");
            Record("Prototype: 0.3.4-source; diagnostic only; no game modifications requested.");
            Record("UTC: " + DateTime.UtcNow.ToString("O"));
            Record("Unity: " + Application.unityVersion);
            Record("Persistent path: " + Application.persistentDataPath);
            Record("Runtime: " + Environment.Version);
            foreach (Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                try { Record("ASSEMBLY: " + asm.FullName); }
                catch (Exception ex) { Record("ASSEMBLY READ ERROR: " + ex.Message); }
            }
            Record("=== Focused type lookup ===");
            foreach (string name in new[] { "Footlong.Items", "Footlong.Content.ItemDef",
                "Footlong.Content.ItemSpawnerDef", "Footlong.Player" })
            {
                Type t = FindType(name);
                Record(t == null ? "MISSING: " + name : "FOUND: " + t.FullName + " [" + t.Assembly.GetName().Name + "]");
            }
            Record("=== Diagnostic disclaimer ===");
            Record("No spawn API invocation, menu patching, or gameplay changes performed.");
            Record("Type inventory may be incomplete in IL2CPP runtimes.");
            log?.Invoke("Runtime diagnostic inventory recorded.");
        }

        internal static void Finish()
        {
            if (ReportPath != null) return;
            try
            {
                string folder = Application.persistentDataPath;
                if (string.IsNullOrWhiteSpace(folder)) folder = Path.GetTempPath();
                Directory.CreateDirectory(folder);
                ReportPath = Path.Combine(folder, "H3VR2ItemSpawner-diagnostics.txt");
                File.WriteAllText(ReportPath, Buffer.ToString());
            }
            catch (Exception ex)
            {
                ReportPath = "(save failed: " + ex.Message + ")";
            }
        }

        private static Type FindType(string fullName)
        {
            foreach (Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
                try { Type t = asm.GetType(fullName, false); if (t != null) return t; } catch { }
            return null;
        }
    }
}
