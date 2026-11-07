using System;
using System.IO;
using System.Threading.Tasks;

namespace MoviesCatalog.Presentation.Infrastructure
{
    public static class FileLogger
    {
        private static readonly string LogPath = Path.Combine(Directory.GetCurrentDirectory(), "Logs", "log.log");

        public static void ClearLog()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(LogPath)!);
            File.WriteAllText(LogPath, "=== NEW SESSION STARTED ===\n\n");
        }

        public static async Task LogAsync(string message)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(LogPath)!);
            await File.AppendAllTextAsync(LogPath, message + "\n\n");
        }
    }
}