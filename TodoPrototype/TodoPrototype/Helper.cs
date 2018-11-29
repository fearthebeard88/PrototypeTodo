using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoPrototype
{
    class Helper
    {
        public static string GetLabel()
        {
            Console.WriteLine("What is the name of this task?");
            var label = Console.ReadLine().Trim();

            return String.IsNullOrWhiteSpace(label) ? "Unknown" : label;
        }

        public static string GetContent()
        {
            Console.WriteLine("What is the task?");
            var content = Console.ReadLine().Trim();

            return String.IsNullOrWhiteSpace(content) ? "Unknown" : content;
        }
    }
}
