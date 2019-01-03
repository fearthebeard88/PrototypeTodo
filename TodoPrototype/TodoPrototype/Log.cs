using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Configuration;
using System.Security;
using System.Security.Permissions;
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

        public static bool Load()
        {
            try
            {
                var file = File.OpenWrite(getLogFile());
                file.Close();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
            
        }

        public static bool log(string msg)
        {
            var response = new Dictionary<int, bool>();
            if (File.Exists(Log.getLogFile()))
            {
                try
                {
                    string log = $"{DateTime.Now.ToString()}" + Environment.NewLine + $"{msg}" + Environment.NewLine;
                    File.AppendAllText(Log.getLogFile(), log);
                    return true;
                }
                catch (Exception e)
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
                    Log.log(e.Message);
                }
            }
        }
    }
}
