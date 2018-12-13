using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Configuration;
using System.Text;
using System.Threading.Tasks;

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

        public static void Load()
        {
            try
            {
                if (!File.Exists(getLogFile()))
                {
                    var file = File.OpenWrite(getLogFile());
                    file.Close();
                }
                else
                {
                    var file = File.OpenWrite(getLogFile());
                    file.Close();
                }
            }
            catch (Exception e)
            {
                throw new CustomExceptions($"Error {e.Message} at {e.StackTrace}");
            }
            
        }

        public static Dictionary<int, bool> log(string msg)
        {
            var response = new Dictionary<int, bool>();
            if (File.Exists(Log.getLogFile()))
            {
                try
                {
                    string log = $"{DateTime.Now.ToString()}" + Environment.NewLine + $"{msg}" + Environment.NewLine;
                    File.AppendAllText(Log.getLogFile(), log);
                    response[200] = true;
                    return response;
                }
                catch (Exception e)
                {
                    response[500] = false;
                    return response;
                }
            }
            else
            {
                response[500] = false;
                return response;
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
                    Log.log(e.Message);
                }
            }
        }
    }
}
