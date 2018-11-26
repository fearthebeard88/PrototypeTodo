using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace TodoPrototype
{
    class CRUD
    {
        private const string Path = @"c:\C#Basics\Prototype\TodoPrototype\PrototypeFiles\Todos.txt";

        public static string GetPath()
        {
            var rawPath = CRUD.Path;
            var DS = System.IO.Path.DirectorySeparatorChar;
            var path = rawPath.Replace('\\', DS);

            return path;
        }

        public static string GetFile()
        {
            var Path = CRUD.GetPath();

            if (!File.Exists(Path))
            {
                var newFile = File.Create(Path);
                newFile.Close();
            }

            var contents = File.ReadAllText(Path);

            return contents;
        }

        public static void View()
        {
            var content = GetFile();

            Console.WriteLine(content);
        }

    }
}
