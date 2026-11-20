// Minimal spawn confirmation policy for tab UI.
namespace H3VR2ItemSpawner
{
    internal static class SpawnConfirmationPolicy
    {
        // Recommend confirmation for batches above 10; menu host should implement dialog.
        internal static bool RequiresConfirmation(int count) => count > 10;
        internal static bool IsAllowedQuantity(int count) =>
            count == 1 || count == 5 || count == 10 || count == 25 || count == 50;
    }
}