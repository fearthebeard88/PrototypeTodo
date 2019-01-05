using System;
using System.IO;

namespace TodoPrototype
{
    class Log
    {
        private const string ResourcePath = @"C://C#Basics/Prototype/TodoPrototype/PrototypeFiles/system.log";

        public static string getLogFile()
        {
            var log = Log.ResourcePath.Replace('/', Path.DirectorySeparatorChar);

            return log;
        }

        public static bool Load()
        {
            try
            {
                var file = File.OpenWrite(getLogFile());
                file.Close();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public static bool log(string msg)
        {
            if (File.Exists(Log.getLogFile()) && !String.IsNullOrWhiteSpace(msg))
            {
                string log = $"{DateTime.Now.ToString()}" + Environment.NewLine + $"{msg}" + Environment.NewLine;
                string file = getLogFile();

                try
                {
                    File.AppendAllText(file, log);
                    return true;
                }
                catch (UnauthorizedAccessException)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public static void DeleteLogs()
        {
            if (File.Exists(Log.getLogFile()))
            {
                try
                {
                    File.Delete(Log.getLogFile());
                }
                catch (Exception e)
                {
                    Console.WriteLine("Unable to delete the log file, please check the log file for more details.");
                    if (!Log.log(e.Message))
                    {
                        throw;
                    }
                }
            }
        }
    }
}
