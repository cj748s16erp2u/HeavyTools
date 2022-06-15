using System;
using System.IO;
using System.Text;

namespace eLog.HeavyTools.ImportBase
{
    public class Logger : IDisposable
    {
        private FileStream logStream;
        private StreamWriter logWriter;
        private FileStream errorStream;
        private StreamWriter errorWriter;

        public string LogFile { get; private set; }
        public string ErrorFile { get; private set; }

        public Logger(string directory, string prefix)
        {
            var now = DateTime.Now;
            var fileTimestamp = $"{prefix}_{now:yyMMdd.HHmmss}";
            this.LogFile = Path.Combine(directory, $"{fileTimestamp}_log.txt");
            this.ErrorFile = Path.Combine(directory, $"{fileTimestamp}_err.txt");

            this.logStream = new FileStream(this.LogFile, FileMode.Create, FileAccess.ReadWrite, FileShare.Read);
            this.logWriter = new StreamWriter(this.logStream, Encoding.UTF8);
            this.errorStream = new FileStream(this.ErrorFile, FileMode.Create, FileAccess.ReadWrite, FileShare.Read);
            this.errorWriter = new StreamWriter(this.errorStream, Encoding.UTF8);
        }

        public void Log(string message = null)
        {
            Console.Write(message);

            this.logWriter.Write(message);
        }

        public void LogErrorLine(string message)
        {
            var foregroundColor = Console.ForegroundColor;
            try
            {
                Console.ForegroundColor = ConsoleColor.Red;
                this.LogLine(message);
            }
            finally
            {
                Console.ForegroundColor = foregroundColor;
            }

            this.errorWriter.WriteLine(message);
        }

        public void LogLine(string message = null)
        {
            Console.WriteLine(message);

            this.logWriter.WriteLine(message);
        }

        public void FlushFiles()
        {
            this.logWriter.Flush();
            this.errorWriter.Flush();
        }

        public void Dispose()
        {
            this.logWriter?.Flush();
            this.logWriter?.Dispose();
            this.logWriter = null;
            this.logStream = null;

            this.errorWriter?.Flush();
            this.errorWriter?.Dispose();
            this.errorWriter = null;
            this.errorStream = null;
        }
    }
}
