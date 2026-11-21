using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace MoviesCatalog.Presentation.Infrastructure
{
    public static class FileLogger
    {
        private static readonly string LogPath = Path.Combine(Directory.GetCurrentDirectory(), "Logs", "log.log");
        private static readonly SemaphoreSlim _lock = new SemaphoreSlim(1, 1);

        public static void ClearLog()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(LogPath)!);
            File.WriteAllText(LogPath, "=== NEW SESSION STARTED ===\n\n");
        }

        public static async Task LogAsync(string message)
        {
            await _lock.WaitAsync();
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(LogPath)!);

                // Use FileStream with FileShare.ReadWrite so multiple middlewares can append
                using var stream = new FileStream(LogPath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
                using var writer = new StreamWriter(stream);
                await writer.WriteLineAsync(message);
                await writer.WriteLineAsync();
            }
            finally
            {
                _lock.Release();
            }
        }
    }
}