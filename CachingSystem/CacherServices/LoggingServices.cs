using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CacherServices
{
    public class LoggingServices : ILoggingServices
    {
        private readonly string path;
        private const string defaultPath = @"D:\epam-lab\Epam-Labs\CachingSystem\CacherServices\log\log.txt";

        public LoggingServices(string path = null)
            => this.path = string.IsNullOrEmpty(path) ? defaultPath : path;

        public void Log(string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                throw new ArgumentException("Fill log message");
            }

            using (StreamWriter streamWriter = new StreamWriter(path, true))
            {
                streamWriter.WriteLine(message);
                streamWriter.Close();
            }
        }
    }
}
