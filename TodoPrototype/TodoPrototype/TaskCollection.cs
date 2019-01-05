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
            Dictionary<int, string> response;
            if (this.Tasks.ContainsKey(label))
            {
                return Response(500, $"{label} already contains a task.");
            }
            else
            {
                if (!String.IsNullOrWhiteSpace(label) && !String.IsNullOrWhiteSpace(content))
                {
                    this.Tasks.Add(label, new TodoPrototype.Task(label, content));
                    return Response(200, $"{label} created successfully.");
                }

                return Response(500, "Unable to create new task.");
            }
        }

        internal Dictionary<string, Task>.KeyCollection GetLabels()
        {
            return Tasks.Keys;
        }

        internal Dictionary<int, string> Edit(string label, string newContent)
        {
            if (!String.IsNullOrWhiteSpace(label) && !String.IsNullOrWhiteSpace(newContent))
            {
                if (this.Tasks.ContainsKey(label))
                {
                    this.Tasks[label].TaskContent = newContent;
                    return Response(200, $"{label} changed successfully.");
                }
                else
                {
                    return Response(500, $"{label} does not exist to edit.");
                }
            }
            else
            {
                return Response(500, "No label or content provided.");
            }
            
        }

        internal Dictionary<int, string> Delete(string label)
        {
            if (!String.IsNullOrWhiteSpace(label))
            {
                Dictionary<int, string> response;
                if (!this.Tasks.ContainsKey(label))
                {
                    return Response(500, $"{label} does not exist, unable to delete.");
                }

                this.Tasks.Remove(label);
                return Response(200, $"{label} deleted successfully.");
            }

            return Response(500, "No label provided.");
        }

        internal Dictionary<int, string> Save()
        {
            FileStream writeStream;
            string Path = TaskCollection.ResourcePath.Replace('/', System.IO.Path.DirectorySeparatorChar);
            
            if (File.Exists(Path))
            {
                try
                {
                    writeStream = new FileStream(Path, FileMode.Create, FileAccess.Write);
                }
                catch (Exception e)
                {
                    return Response(500, $"Error: {e.Message} at {e.StackTrace}");
                }

                try
                {
                    this.Formatter.Serialize(writeStream, this.Tasks);
                }
                catch(System.Runtime.Serialization.SerializationException e)
                {
                    return Response(500, e.Message);
                }
                //catch(System.Security.SecurityException e)
                //{
                //    return Response(500, e.Message);
                //}
                
                writeStream.Close();
                return Response(200, "Save confirmed.");
            }
            else
            {
                return Response(500, "This file does not exist, unable to save.");
            }
            
        }

        internal Dictionary<int, string> Load()
        {
            string Path = TaskCollection.ResourcePath.Replace('/', System.IO.Path.DirectorySeparatorChar);
            FileStream readStream;

            if (File.Exists(Path))
            {
                try
                {
                    readStream = new FileStream(Path, FileMode.Open, FileAccess.Read);
                }
                catch (System.Security.SecurityException e)
                {
                    return Response(500, $"Error: {e.Message} at {e.StackTrace}");
                }

                try
                {
                    this.Tasks = (Dictionary<string, Task>)this.Formatter.Deserialize(readStream);
                }
                catch (System.Security.SecurityException e)
                {
                    return Response(500, e.Message);
                }
                //catch (System.Runtime.Serialization.SerializationException e)
                //{
                //    return Response(500, e.Message);
                //}

                readStream.Close();
                return Response(200, "File load confirmed.");
            }

            return Response(500, "File does not exist to load");
        }

        internal Dictionary<int, string> Print(string label = "")
        {
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

                    return Response(200, "Tasks returned.");
                }
                else
                {
                    if (this.Tasks.ContainsKey(label))
                    {
                        Console.WriteLine($"{Tasks[label].TaskLabel}: {Tasks[label].TaskContent}\n");
                        return Response(200, $"{label} returned.");
                    }

                    return Response(500, $"{label} does not exist.");
                }
            }
            else
            {
                return Response(500, "No tasks to view.");
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

        // add response dictionary to children task methods
        // log non 200 responses
        // investigate errors when adding a child task to a parent task
    }
}
