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

namespace TodoPrototype
{
    class Program
    {
        // PrintLabels is the only method that does not return anything

        static void Main(string[] args)
        {
            TaskCollection CurrentList = TaskCollection.Instance();
            bool exitFlag = false;
            var logger = Log.Load();
            if (logger.ContainsKey(500))
            {
                Console.WriteLine(logger[500]);
                return;
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
                var inputArray = input.Split('-');
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

        private static void DisplayMenu()
        {
            Console.WriteLine("\nPlease choose from the options below using the following syntax:\n" +
                              "Action or Action-Label or Action-Label-Content. \n");
            Console.WriteLine("View(-Label)");
            Console.WriteLine("Add(-Label-Content)");
            Console.WriteLine("Delete(-Label)");
            Console.WriteLine("Edit(-Label-Content)");
            Console.WriteLine("Logs(This is will clear the log file.)");
            Console.WriteLine("Exit\n");

            Console.WriteLine("Current Tasks:");
        }

        public static void BeepAndClear()
        {
            Console.Clear();
            Console.Beep();
        }
    }
}
