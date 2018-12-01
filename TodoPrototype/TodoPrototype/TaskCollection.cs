using System;
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
                    Console.WriteLine(task.TaskLabel);
                }
            }
            else
            {
                Console.WriteLine("No tasks to display.");
            }
        }

        public void Create(string label, string content)
        {
            if (this.Tasks.ContainsKey(label))
            {
                Console.WriteLine("{0} already exists, please choose a new task label.", label);
                return;
            }

            try
            {
                this.Tasks.Add(label, new TodoPrototype.Task(label, content));
            }
            catch (Exception e)
            {
                Console.WriteLine("Error encountered when adding new task. Error: {0}", e.Message);
                throw;
            }
        }

        public Dictionary<string, Task>.KeyCollection GetLabels()
        {
            var labels = Tasks.Keys;
            return labels;
        }

        public void Edit(string label, string newContent)
        {
            if (this.Tasks.ContainsKey(label))
            {
                try
                {
                    this.Tasks[label].TaskContent = newContent;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error encountered when editing task with label: {0}. Error: {1}", label, e.Message);
                    throw;
                }
            }
            else
            {
                Console.WriteLine("There is no tast with label: {0}", label);
            }
        }

        public void Delete(string label)
        {
            if (!this.Tasks.ContainsKey(label))
            {
                Console.WriteLine("No matching task with label: {0}", label);
                return;
            }

            try
            {
                this.Tasks.Remove(label);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error encountered when removing task with label: {0}. Error: {1}", label, e.Message);
                throw;
            }
        }

        public void Save()
        {
            string Path = TaskCollection.ResourcePath.Replace('/', System.IO.Path.DirectorySeparatorChar);

            try
            {
                FileStream writeStream = new FileStream(Path, FileMode.Create, FileAccess.Write);
                this.Formatter.Serialize(writeStream, this.Tasks);
                writeStream.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to save tasks. Error: {0}", e.Message);
                throw;
            }
        }

        public void Load()
        {
            string Path = TaskCollection.ResourcePath.Replace('/', System.IO.Path.DirectorySeparatorChar);

            if (File.Exists(Path))
            {
                try
                {
                    FileStream readStream = new FileStream(Path, FileMode.Open, FileAccess.Read);

                    // casting the deserialized object into a dictionary format
                    this.Tasks = (Dictionary<string, Task>) this.Formatter.Deserialize(readStream);
                    readStream.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Unable to load the task collection. Error: {0}", e.Message);
                    throw;
                }
            }
        }

        public void Print(string label = "")
        {

            if (this.Tasks.Count > 0)
            {
                if (String.IsNullOrWhiteSpace(label))
                {
                    foreach (Task task in Tasks.Values)
                    {
                        Console.WriteLine(task.Label + ": " + task.Content);
                    }
                }
                else
                {
                    if (this.Tasks.ContainsKey(label))
                    {
                        Console.WriteLine("{0}: {1}", Tasks[label].TaskLabel, Tasks[label].TaskContent);
                    }
                }
            }
            else
            {
                Console.WriteLine("There are no tasks to view yet.");
            }
        }
    }
}
