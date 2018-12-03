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
                Console.WriteLine("Please choose from the options below. Please note you can include a \n" +
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
                    var inputArray = input.Split(' ');
                    string arguments = "";
                    if (inputArray.Length > 1 && !(inputArray.Length > 2))
                    {
                        input = inputArray[0].ToUpper();
                        arguments = inputArray[1].Trim();
                    }
                    else if (inputArray.Length > 2)
                    {
                        throw new SystemException("More than one additional argument is not supported at this time.");
                    }
                    else
                    {
                        input = inputArray[0].ToUpper();
                    }

                    string label = "";
                    string content = "";

                    switch (input)
                    {
                        case "VIEW":
                            BeepAndClear();
                            Console.WriteLine("View");
                            if (arguments.Trim().Length > 0)
                            {
                                CurrentList.Print(arguments.Trim());
                                Console.WriteLine();
                            }
                            else
                            {
                                CurrentList.Print();
                                Console.WriteLine();
                            }
                            
                            continue;
                        case "DELETE":
                            BeepAndClear();
                            Console.WriteLine("Delete");
                            if (arguments.Trim().Length > 0)
                            {
                                label = arguments.Trim();
                            }
                            else
                            {
                                Console.Write("Label of task to be deleted: ");
                                label = Console.ReadLine().Trim();
                            }

                            CurrentList.Delete(label);
                            CurrentList.Save();
                            continue;
                        case "EDIT":
                            BeepAndClear();
                            Console.WriteLine("Edit");
                            if (arguments.Trim().Length > 0)
                            {
                                label = arguments.Trim();
                            }
                            else
                            {
                                Console.Write("Label of task to edit: ");
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
                            
                            continue;
                        case "ADD":
                            BeepAndClear();
                            Console.WriteLine("Add");
                            if (arguments.Trim().Length > 0)
                            {
                                label = arguments.Trim();
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
                            continue;
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
                            continue;
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
