using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace TodoPrototype
{
    class TaskCollection
    {
        private static TaskCollection taskCollection;
        private Dictionary<string, Task> Tasks;
        private BinaryFormatter Formatter;
        private const string ResourcePath = @"C://C#Basics/Prototype/TodoPrototype/PrototypeFiles/Todo.txt";

        public static TaskCollection Instance()
        {
            if (taskCollection == null)
            {
                taskCollection = new TaskCollection();
            }

            return taskCollection;
        }

        public Dictionary<int, string> Response(int code, string msg)
        {
            var Response = new Dictionary<int, string>();
            Response[code] = msg;

            return Response;
        }

        private TaskCollection()
        {
            this.Tasks = new Dictionary<string, Task>();
            this.Formatter = new BinaryFormatter();
        }

        public void PrintLabels()
        {
            if (this.Tasks.Count > 0)
            {
                foreach (Task task in Tasks.Values)
                {
                    Console.WriteLine(task.TaskLabel + "\n");
                }
            }
            else
            {
                Console.WriteLine("No tasks to display.");
            }
        }

        public Dictionary<int, string> Create(string label, string content)
        {
            if (this.Tasks.ContainsKey(label))
            {
                var response = Response(300, "This label already contains a task.");
                return response;
            }

            try
            {
                this.Tasks.Add(label, new TodoPrototype.Task(label, content));
                var response = Response(200, "Task created.");
                return response;
            }
            catch (Exception e)
            {
                var response = Response(500, e.Message);
                return response;
            }
        }

        public Dictionary<string, Task>.KeyCollection GetLabels()
        {
            var labels = Tasks.Keys;
            return labels;
        }

        public Dictionary<int, string> Edit(string label, string newContent)
        {
            if (this.Tasks.ContainsKey(label))
            {
                this.Tasks[label].TaskContent = newContent;
                var response = Response(200, "Task Edited.");
                return response;
            }
            else
            {
                var response = Response(500, "That label does not exist.");
                return response;
            }
        }

        public Dictionary<int, string> Delete(string label)
        {
            if (!this.Tasks.ContainsKey(label))
            {
                var response = Response(500, "No task under that label.");
                return response;
            }

            try
            {
                this.Tasks.Remove(label);
                var response = Response(200, "Task deleted.");
                return response;
            }
            catch (Exception e)
            {
                var response = Response(500, e.Message);
                return response;
            }
        }

        public Dictionary<int, string> Save()
        {
            string Path = TaskCollection.ResourcePath.Replace('/', System.IO.Path.DirectorySeparatorChar);

            try
            {
                FileStream writeStream = new FileStream(Path, FileMode.Create, FileAccess.Write);
                this.Formatter.Serialize(writeStream, this.Tasks);
                writeStream.Close();
                var response = Response(200, "Save confirmed.");
                return response;
            }
            catch (Exception e)
            {
                var response = Response(500, e.Message);
                return response;
            }
        }

        public Dictionary<int, string> Load()
        {
            string Path = TaskCollection.ResourcePath.Replace('/', System.IO.Path.DirectorySeparatorChar);
            var response = Response(200, "");

            if (File.Exists(Path))
            {
                try
                {
                    FileStream readStream = new FileStream(Path, FileMode.Open, FileAccess.Read);

                    // casting the deserialized object into a dictionary format
                    this.Tasks = (Dictionary<string, Task>) this.Formatter.Deserialize(readStream);
                    readStream.Close();
                    response[200] = "File load confirmed.";
                    return response;
                }
                catch (Exception e)
                {
                    response[500] = e.Message;
                    return response;
                }
            }

            response[500] = "File does not exist to load.";
            return response;
        }

        public Dictionary<int, string> Print(string label = "")
        {
            var response = Response(200, "");

            if (this.Tasks.Count > 0)
            {
                if (String.IsNullOrWhiteSpace(label))
                {
                    foreach (Task task in Tasks.Values)
                    {
                        Console.WriteLine(task.Label + ": " + task.Content);
                    }

                    response[200] = "Tasks returned.";
                    return response;
                }
                else
                {
                    if (this.Tasks.ContainsKey(label))
                    {
                        Console.WriteLine(String.Format("{0}: {1}\n", Tasks[label].TaskLabel, Tasks[label].TaskContent));
                        response[200] = "Tasks returned.";
                        return response;
                    }

                    response[500] = "No task under that label.";
                    return response;
                }
            }
            else
            {
                response[500] = "No tasks to view.";
                return response;
            }
        }
    }
}
