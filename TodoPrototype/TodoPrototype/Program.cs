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
            // create a todo list with parent and children tasks
            TaskCollection CurrentList = TaskCollection.Instance();
            bool exitFlag = false;
            while (!exitFlag)
            {
                Console.WriteLine("Please specify an action.  Choices are the following: \n");
                Console.WriteLine("View All");
                Console.WriteLine("View Task");
                Console.WriteLine("Add");
                Console.WriteLine("Delete");
                Console.WriteLine("Edit");
                Console.WriteLine("Exit\n");

                var input = Console.ReadLine().Trim().ToUpper();
                if (String.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("Invalid response, try again.");
                    continue;
                }

                try
                {
                    CurrentList.Load();
                    string label;
                    string content;

                    switch (input)
                    {
                        case "VIEW ALL":
                            Console.Clear();
                            Console.WriteLine("View chosen, viewing tasks.");
                            CurrentList.Print();
                            Console.Beep();
                            continue;
                        case "VIEW TASK":
                            Console.Clear();
                            Console.WriteLine("Which task do you want to view?");
                            label = Console.ReadLine();
                            CurrentList.Print(label);
                            Console.Beep();
                            continue;
                        case "DELETE":
                            Console.Clear();
                            Console.WriteLine("Delete Chosen, please input the label of the task you wish to delete");
                            label = Console.ReadLine().Trim();
                            CurrentList.Delete(label);
                            CurrentList.Save();
                            Console.Beep();
                            continue;
                        case "EDIT":
                            Console.Clear();
                            Console.WriteLine("Edit chosen, please choose a task to edit.");
                            Console.Write("Label of task to edit: ");
                            label = Console.ReadLine();
                            Console.Write("New content: ");
                            content = Console.ReadLine();
                            CurrentList.Edit(label, content);
                            CurrentList.Save();
                            Console.Beep();
                            continue;
                        /*break*/
                        case "ADD":
                            Console.Clear();
                            Console.WriteLine("Add chosen, please input a label and content");
                            Console.Write("Label: ");
                            label = Console.ReadLine().Trim();
                            Console.Write("Content: ");
                            content = Console.ReadLine().Trim();
                            CurrentList.Create(label, content);
                            CurrentList.Save();
                            Console.Beep();
                            continue;
                        case "EXIT":
                            Console.Clear();
                            Console.WriteLine("Exiting now.");
                            exitFlag = true;
                            Console.Beep();
                            return;
                        default:
                            Console.Clear();
                            Console.WriteLine("Unknown input detected, please choose from the options below.");
                            Console.Beep();
                            continue;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error handling request. Error: {0}", e.Message);
                }
            }
        }
    }
}
