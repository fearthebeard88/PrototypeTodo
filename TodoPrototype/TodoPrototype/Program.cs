using System;
using System.CodeDom;
using System.Collections.Generic;
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
        static void Main(string[] args)
        {
            TaskCollection CurrentList = TaskCollection.Instance();
            bool exitFlag = false;
            while (!exitFlag)
            {
                Console.WriteLine("\nPlease choose from the options below. Please note you can include a \n" +
                                  "label after the option to use that option on just the task with that label.\n");
                Console.WriteLine("View");
                Console.WriteLine("Add");
                Console.WriteLine("Delete");
                Console.WriteLine("Edit");
                Console.WriteLine("Exit\n");

                Console.WriteLine("Current Tasks:");
                CurrentList.Load();
                CurrentList.PrintLabels();

                var input = Console.ReadLine().Trim();
                if (String.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("Invalid response, try again.");
                    continue;
                }

                try
                {
                    string label = "";
                    string content = "";
                    var inputArray = input.Split(' ');
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
                    switch (input)
                    {
                        case "VIEW":
                            BeepAndClear();
                            Console.WriteLine("View");
                            if (inputDictionary.ContainsKey("label"))
                            {
                                CurrentList.Print(inputDictionary["label"]);
                                Console.WriteLine();
                            }
                            else
                            {
                                CurrentList.Print();
                                Console.WriteLine();
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
                                Console.Write("Label of task to be deleted: ");
                                CurrentList.PrintLabels();
                                label = Console.ReadLine().Trim();
                                BeepAndClear();
                            }

                            CurrentList.Delete(label);
                            CurrentList.Save();
                            break;
                        case "EDIT":
                            BeepAndClear();
                            Console.WriteLine("Edit");
                            if (inputDictionary.ContainsKey("label"))
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
                                Console.Write("New content: ");
                                content = Console.ReadLine();
                                CurrentList.Edit(label, content);
                                CurrentList.Save();
                            }
                            else
                            {
                                throw new SystemException(String.Format("There is no task with label of {0}.", label));
                            }
                            
                            break;
                        case "ADD":
                            BeepAndClear();
                            Console.WriteLine("Add");
                            if (inputDictionary.ContainsKey("label"))
                            {
                                label = inputDictionary["label"];
                            }
                            else
                            {
                                Console.Write("Label: ");
                                label = Console.ReadLine().Trim();
                            }
                            
                            Console.Write("Content: ");
                            content = Console.ReadLine().Trim();
                            CurrentList.Create(label, content);
                            CurrentList.Save();
                            BeepAndClear();
                            break;
                        case "EXIT":
                            BeepAndClear();
                            Console.WriteLine("Exiting now...");
                            exitFlag = true;
                            Environment.Exit(0);
                            return;
                        default:
                            BeepAndClear();
                            Console.WriteLine("Unknown input detected, please choose from the options below.");
                            Console.Beep();
                             break;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("There was an error processing your request. Error: {0}", e.Message);
                    Console.Beep();
                    continue;
                }
            }
        }

        public static void BeepAndClear()
        {
            Console.Clear();
            Console.Beep();
        }
    }
}
