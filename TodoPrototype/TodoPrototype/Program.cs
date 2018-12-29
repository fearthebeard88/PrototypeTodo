using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;

namespace TodoPrototype
{
    class Program
    {
        static void Main(string[] args)
        {
            TaskCollection CurrentList = TaskCollection.Instance();
            bool exitFlag = false;
            {
                try
                {
                    Log.Load();
                }
                catch (CustomExceptions e)
                {
                    Console.WriteLine($"The following error/s occured when attempting to set up log file.\n{e.Message}");
                    return;
                }
            }

            while (!exitFlag)
            {
                DisplayMenu();

                var results = CurrentList.Load();
                if (results.ContainsKey(500))
                {
                    Console.WriteLine(results[500]);
                    return;
                }

                CurrentList.PrintLabels();

                var input = Console.ReadLine().Trim();
                if (String.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("Invalid response, try again.");
                    continue;
                }

                string label = "";
                string content = "";
                var character = new char[] {' '};
                var inputArray = input.Split(character, 3, StringSplitOptions.RemoveEmptyEntries);
                Dictionary<string, string> inputDictionary = new Dictionary<string, string>();
                inputDictionary["input"] = inputArray[0].Trim().ToUpper();

                // first argument is the action, second would be label, third is content
                if (inputArray.Length > 3)
                {
                    Console.WriteLine("Too many arguments given.");
                    continue;
                }
                else if (inputArray.Length > 2)
                {
                    inputDictionary["label"] = inputArray[1].Trim();
                    inputDictionary["content"] = inputArray[2].Trim();
                }
                else if (inputArray.Length > 1)
                {
                    inputDictionary["label"] = inputArray[1].Trim();
                }

                input = inputDictionary["input"];
                Dictionary<int, string> response = new Dictionary<int, string>();
                switch (input)
                {
                    case "VIEW":
                        BeepAndClear();
                        Console.WriteLine("View\n");
                        if (inputDictionary.ContainsKey("label"))
                        {
                            response = CurrentList.Print(inputDictionary["label"]);
                            if (response.ContainsKey(500))
                            {
                                Console.WriteLine(response[500]);
                                Log.log(response[500]);
                            }
                        }
                        else
                        {
                            response = CurrentList.Print();
                            if (response.ContainsKey(500))
                            {
                                Console.WriteLine(response[500]);
                                Log.log(response[500]);
                            }
                        }

                        break;
                    case "DELETE":
                        BeepAndClear();
                        Console.WriteLine("Delete");
                        if (inputDictionary.ContainsKey("label"))
                        {
                            label = inputDictionary["label"];
                        }
                        else
                        {
                            Console.WriteLine("Label of task to be deleted: ");
                            CurrentList.PrintLabels();
                            label = Console.ReadLine().Trim();
                        }

                        response = CurrentList.Delete(label);
                        if (response.ContainsKey(500))
                        {
                            Log.log(response[500]);
                        }

                        response = CurrentList.Save();
                        if (response.ContainsKey(500))
                        {
                            Log.log(response[500]);
                        }

                        BeepAndClear();
                        break;
                    case "EDIT":
                        BeepAndClear();
                        Console.WriteLine("Edit");
                        if (inputDictionary.ContainsKey("content") && inputDictionary.ContainsKey("label"))
                        {
                            content = inputDictionary["content"];
                            label = inputDictionary["label"];
                        }

                        else if (inputDictionary.ContainsKey("label"))
                        {
                            label = inputDictionary["label"];
                        }
                        else
                        {
                            Console.Write("Label of task to edit: ");
                            CurrentList.PrintLabels();
                            label = Console.ReadLine();
                        }

                        var labelsCollection = CurrentList.GetLabels();
                        if (labelsCollection.Contains(label))
                        {
                            if (!inputDictionary.ContainsKey("content"))
                            {
                                Console.Write("New content: ");
                                content = Console.ReadLine();
                            }
                            
                            response = CurrentList.Edit(label, content);
                            if (response.ContainsKey(500))
                            {
                                Console.WriteLine(response[500]);
                                Log.log(response[500]);
                            }

                            response = CurrentList.Save();
                            if (response.ContainsKey(500))
                            {
                                Console.WriteLine(response[500]);
                                Log.log(response[500]);
                            }

                        }
                        else
                        {
                            Console.WriteLine("Invalid label provided.");
                            Log.log("Invalid label provided.");
                        }
                        
                        break;
                    case "ADD":
                        BeepAndClear();
                        Console.WriteLine("Add");
                        if (inputDictionary.ContainsKey("content") && inputDictionary.ContainsKey("label"))
                        {
                            content = inputDictionary["content"];
                            label = inputDictionary["label"];
                        }

                        else if (inputDictionary.ContainsKey("label"))
                        {
                            label = inputDictionary["label"];
                        }
                        else
                        {
                            Console.Write("Label: ");
                            label = Console.ReadLine().Trim();
                        }

                        if (!inputDictionary.ContainsKey("content"))
                        {
                            Console.Write("Content: ");
                            content = Console.ReadLine().Trim();
                        }
                       
                        response = CurrentList.Create(label, content);
                        if (response.ContainsKey(500))
                        {
                            Console.WriteLine(response[500]);
                            Log.log(response[500]);
                        }

                        response = CurrentList.Save();
                        if (response.ContainsKey(500))
                        {
                            Console.WriteLine(response[500]);
                            Log.log(response[500]);
                        }

                        break;
                    case "CHILDREN":
                        BeepAndClear();

                        var exit = false;
                        while (!exit)
                        {
                            Console.WriteLine("Please pick an option from the parent/child task menu.\n");


                            DisplayChildrenMenu();
                            CurrentList.printTasksWithChildren();
                            var childInput = Console.ReadLine().Trim();
                            if (!String.IsNullOrWhiteSpace(childInput))
                            {
                                var delimiter = new char[] { ' ' };
                                var childInputArray = childInput.Split(delimiter, 4, StringSplitOptions.RemoveEmptyEntries);
                                if (childInputArray[0].ToUpper() == "BACK")
                                {
                                    BeepAndClear();
                                    break;
                                }

                                BeepAndClear();
                                RunChildInput(childInputArray, ref CurrentList);
                            }
                        }
                       
                        break;
                    case "LOGS":
                        Console.WriteLine("Deleting logs file now.");
                        Log.DeleteLogs();
                        return;
                    case "EXIT":
                        Console.WriteLine("Exiting now...");
                        exitFlag = true;
                        Environment.Exit(0);
                        return;
                    default:
                        BeepAndClear();
                        Console.WriteLine("Unknown input detected, please choose from the options below.");
                        break;
                }
            }
        }

        private static void RunChildInput(string[] childInputArray, ref TaskCollection CurrentList)
        {
            Dictionary<string, string> inputDictionary = new Dictionary<string, string>();
            inputDictionary["input"] = childInputArray[0].ToUpper();
            if (childInputArray.Length == 4)
            {
                inputDictionary["parent"] = childInputArray[1];
                inputDictionary["child"] = childInputArray[2];
                inputDictionary["content"] = childInputArray[3];
            }
            else if (childInputArray.Length == 3)
            {
                inputDictionary["parent"] = childInputArray[1];
                inputDictionary["child"] = childInputArray[2];
            }
            else if (childInputArray.Length == 2)
            {
                inputDictionary["parent"] = childInputArray[1];
            }
            else if (childInputArray.Length > 4)
            {
                Console.WriteLine("Too many arguments.");
                return;
            }

            string parent;
            string child;
            string content;

            switch (inputDictionary["input"])
            {
                case "VIEW":
                    CurrentList.printFullChildTasks();
                    Console.WriteLine();
                    break;
                case "ADD":
                    if (inputDictionary.ContainsKey("parent") && inputDictionary.ContainsKey("child"))
                    {
                        parent = inputDictionary["parent"];
                        child = inputDictionary["child"];
                    }
                    else if (inputDictionary.ContainsKey("parent"))
                    {
                        parent = inputDictionary["parent"];
                        Console.WriteLine("Label of child: ");
                        CurrentList.PrintLabels();
                        child = Console.ReadLine().Trim();
                    }
                    else
                    {
                        Console.WriteLine("Label of parent task: ");
                        CurrentList.PrintLabels();
                        parent = Console.ReadLine().Trim();
                        Console.WriteLine("Label of child: ");
                        CurrentList.PrintLabels();
                        child = Console.ReadLine().Trim();
                    }

                    if (String.IsNullOrWhiteSpace(parent) || String.IsNullOrWhiteSpace(child))
                    {
                        Console.WriteLine("Input not accepted, please specify a parent label and child label for this action.");
                        return;
                    }
                    
                    CurrentList.setChildTask(CurrentList.Tasks[parent], CurrentList.Tasks[child]);
                    CurrentList.Save();
                    BeepAndClear();
                    break;
                case "DELETE":
                    if (inputDictionary.ContainsKey("parent") && inputDictionary.ContainsKey("child"))
                    {
                        parent = inputDictionary["parent"];
                        child = inputDictionary["child"];
                    }
                    else if (inputDictionary.ContainsKey("parent"))
                    {
                        parent = inputDictionary["parent"];
                        Console.WriteLine("Label of child to be deleted: ");
                        CurrentList.printTasksWithChildren();
                        child = Console.ReadLine().Trim();
                    }
                    else
                    {
                        Console.WriteLine("Label of parent task: ");
                        CurrentList.printTasksWithChildren();
                        parent = Console.ReadLine().Trim();
                        Console.WriteLine("Label of child to be deleted: ");
                        CurrentList.printTasksWithChildren();
                        child = Console.ReadLine().Trim();
                    }

                    if (String.IsNullOrWhiteSpace(parent) || String.IsNullOrWhiteSpace(child))
                    {
                        Console.WriteLine("Input not accepted, please specify a parent label and child label for this action.");
                        return;
                    }
                    
                    CurrentList.RemoveChild(parent, child);
                    BeepAndClear();
                    break;
                case "EDIT":
                    if (inputDictionary.ContainsKey("parent") && inputDictionary.ContainsKey("child") &&
                        inputDictionary.ContainsKey("content"))
                    {
                        parent = inputDictionary["parent"];
                        child = inputDictionary["child"];
                        content = inputDictionary["content"];
                    }
                    else if (inputDictionary.ContainsKey("parent") && inputDictionary.ContainsKey("child"))
                    {
                        parent = inputDictionary["parent"];
                        child = inputDictionary["child"];
                        Console.WriteLine("New content: ");
                        content = Console.ReadLine().Trim();
                    }
                    else if (inputDictionary.ContainsKey("parent"))
                    {
                        parent = inputDictionary["parent"];
                        Console.WriteLine("Label of child task to be edited: ");
                        CurrentList.printTasksWithChildren();
                        child = Console.ReadLine().Trim();
                        Console.WriteLine("New content: ");
                        content = Console.ReadLine().Trim();
                    }
                    else
                    {
                        Console.WriteLine("Label of parent task: ");
                        CurrentList.printTasksWithChildren();
                        parent = Console.ReadLine().Trim();
                        Console.WriteLine("Label of child task to be edited: ");
                        CurrentList.printTasksWithChildren();
                        child = Console.ReadLine().Trim();
                        Console.WriteLine("New content: ");
                        content = Console.ReadLine().Trim();
                    }

                    if (String.IsNullOrWhiteSpace(parent) || String.IsNullOrWhiteSpace(child) ||
                        String.IsNullOrWhiteSpace(content))
                    {
                        Console.WriteLine("Input not accepted, please specify a parent label and child label and content for this action.");
                        return;
                    }
                    
                    CurrentList.editChildTask(parent, child, content);
                    CurrentList.Save();
                    BeepAndClear();
                    break;
                case "PARENT":
                    if (inputDictionary.ContainsKey("parent"))
                    {
                        child = inputDictionary["parent"];
                    }
                    else
                    {
                        Console.WriteLine("Label of child task: ");
                        CurrentList.printTasksWithChildren();
                        child = Console.ReadLine().Trim();
                    }

                    if (String.IsNullOrWhiteSpace(child))
                    {
                        Console.WriteLine("Input not accepted, please specify a child label for this action.");
                        return;
                    }
                    
                    CurrentList.printParentTask(child);
                    break;
                default:
                    Console.WriteLine("Input not accepted, please try again.");
                    break;
            }
        }

        private static void DisplayMenu()
        {
            Console.WriteLine("\nPlease choose from the options below using the following syntax:\n" +
                              "Action or Action Label or Action Label Content. \n");
            Console.WriteLine("View(Include optional label)");
            Console.WriteLine("Add(Include optional label or label with content)");
            Console.WriteLine("Delete(Include optional label)");
            Console.WriteLine("Edit(-Label-Content)");
            Console.WriteLine("Children");
            Console.WriteLine("Logs(This is will clear the log file.)");
            Console.WriteLine("Exit\n");

            Console.WriteLine("Current Tasks:");
        }

        private static void DisplayChildrenMenu()
        {
            Console.WriteLine("\nChoose from the actions below to modify a tasks child.\n");
            Console.WriteLine("View(Shows all tasks with children)");
            Console.WriteLine("Add(Include optional parent label and child label)");
            Console.WriteLine("Delete(Include optional parent label and child label)");
            Console.WriteLine("Edit(Include optional parent label, child label, and content)");
            Console.WriteLine("Parent(Include optional child label)");
            Console.WriteLine("Back(Go back to the main menu");

            Console.WriteLine("Current Tasks with Children: \n");
        }

        private static void BeepAndClear()
        {
            Console.Clear();
            Console.Beep();
        }
    }
}
