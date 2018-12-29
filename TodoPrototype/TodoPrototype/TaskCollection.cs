using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
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
        public Dictionary<string, Task> Tasks;
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

        internal Dictionary<int, string> Response(int code, string msg)
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

        internal void PrintLabels()
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

        internal Dictionary<int, string> Create(string label, string content)
        {
            if (this.Tasks.ContainsKey(label))
            {
                var response = Response(500, $"{label} already contains a task.");
                return response;
            }

            try
            {
                this.Tasks.Add(label, new TodoPrototype.Task(label, content));
                var response = Response(200, $"{label} created successfully.");
                return response;
            }
            catch (Exception e)
            {
                var response = Response(500, $"Error: {e.Message} at {e.StackTrace}");
                return response;
            }
        }

        internal Dictionary<string, Task>.KeyCollection GetLabels()
        {
            var labels = Tasks.Keys;
            return labels;
        }

        internal Dictionary<int, string> Edit(string label, string newContent)
        {
            if (this.Tasks.ContainsKey(label))
            {
                this.Tasks[label].TaskContent = newContent;
                var response = Response(200, $"{label} changed successfully.");
                return response;
            }
            else
            {
                var response = Response(500, $"{label} does not exist to edit.");
                return response;
            }
        }

        internal Dictionary<int, string> Delete(string label)
        {
            if (!this.Tasks.ContainsKey(label))
            {
                var response = Response(500, $"{label} does not exist, unable to delete.");
                return response;
            }

            try
            {
                this.Tasks.Remove(label);
                var response = Response(200, $"{label} deleted successfully.");
                return response;
            }
            catch (Exception e)
            {
                var response = Response(500, $"Error: {e.Message} at {e.StackTrace}");
                return response;
            }
        }

        internal Dictionary<int, string> Save()
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
                var response = Response(500, $"Error: {e.Message} at {e.StackTrace}");
                return response;
            }
        }

        internal Dictionary<int, string> Load()
        {
            string Path = TaskCollection.ResourcePath.Replace('/', System.IO.Path.DirectorySeparatorChar);
            var response = new Dictionary<int, string>();

            if (File.Exists(Path))
            {
                try
                {
                    FileStream readStream = new FileStream(Path, FileMode.Open, FileAccess.Read);

                    // casting the deserialized object into a dictionary format
                    this.Tasks = (Dictionary<string, Task>) this.Formatter.Deserialize(readStream);
                    readStream.Close();
                    response = Response(200, "File load confirmed.");
                    return response;
                }
                catch (Exception e)
                {
                    response = Response(500, $"Error: {e.Message} at {e.StackTrace}");
                    return response;
                }
            }

            response = Response(500, "File does not exist to load.");
            return response;
        }

        internal Dictionary<int, string> Print(string label = "")
        {
            var response = new Dictionary<int, string>();

            if (this.Tasks.Count > 0)
            {
                if (String.IsNullOrWhiteSpace(label))
                {
                    foreach (Task task in Tasks.Values)
                    {
                        if (!(task.Child.Count > 0))
                        {
                            Console.WriteLine(task.Label + ": " + task.Content + "-> No child tasks.");
                        }
                        else
                        {
                            var childrenLabels = this.getChildLabels(task);
                            foreach (var child in childrenLabels)
                            {
                                Console.WriteLine(task.Label + ": " + task.Content + "->Child task: " + child);
                            }
                        }
                    }

                    response = Response(200, "Tasks returned.");
                    return response;
                }
                else
                {
                    if (this.Tasks.ContainsKey(label))
                    {
                        Console.WriteLine($"{Tasks[label].TaskLabel}: {Tasks[label].TaskContent}\n");
                        response = Response(200, $"{label} returned.");
                        return response;
                    }

                    response = Response(500, $"{label} does not exist.");
                    return response;
                }
            }
            else
            {
                response = Response(500, "No tasks to view.");
                return response;
            }
        }

        internal string[] getChildLabels(Task parent)
        {
            if (Tasks.ContainsKey(parent.Label) && parent.Child.Count > 0)
            {
                var childLabels = new string[parent.Child.Count];
                var children = parent.Child.Values.ToArray();
                for (int i = 0; i < children.Length; i++)
                {
                    childLabels[i] = children[i].Label;
                }

                return childLabels;
            }

            return null;
        }

        internal bool hasChildren(Task parent)
        {
            if (parent.Child.Count > 0)
            {
                return true;
            }

            return false;
        }

        internal void printTasksWithChildren()
        {
            if (!(Tasks.Count > 0))
            {
                Console.WriteLine("No tasks to display.");
                return;
            }

            foreach (var task in Tasks.Values)
            {
                if (this.hasChildren(task))
                {
                    Console.Write($"{task.Label}: ");
                    foreach (var child in task.Child)
                    {
                        Console.WriteLine(child.Key);
                    }
                }
            }
        }

        internal void setChildTask(Task parent, Task child)
        {
            if (Tasks.ContainsKey(parent.Label) && Tasks.ContainsKey(child.Label) && Tasks[child.Label].Parent == null)
            {
                parent.setChild(child.Label, child);
                child.Parent = parent;
            }
        }

        internal void printFullChildTasks()
        {
            if (Tasks.Count > 0)
            {
                foreach (var task in Tasks.Values)
                {
                    if (hasChildren(task))
                    {
                        Console.Write($"{task.Label}: ");
                        foreach (var child in task.Child)
                        {
                            var childLabel = child.Value.getChildLabel(child.Value);
                            var childContent = child.Value.Content;
                            Console.WriteLine($"{childLabel}: {childContent}");
                        }
                    }
                }
            }
        }

        internal void RemoveChild(string parent, string child)
        {
            if (Tasks.ContainsKey(parent) && Tasks.ContainsKey(child))
            {
                Tasks[parent].Child.Remove(Tasks[child].Label);
                Tasks[child].Parent = null;
            }
        }

        internal void printParentTask(string child)
        {
            if (Tasks.ContainsKey(child))
            {
                var parent = Tasks[child].Parent;
                Console.WriteLine($"{parent.Label}: {parent.Content}");
            }
        }

        internal void editChildTask(string parent, string child, string content)
        {
            if (Tasks.ContainsKey(parent) && Tasks.ContainsKey(child))
            {
                var childTask = Tasks[parent].getChild(child);
                childTask.Content = content;
            }
        }
    }
}
