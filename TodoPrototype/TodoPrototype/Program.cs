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
                Console.WriteLine("View");
                Console.WriteLine("Add");
                Console.WriteLine("Delete");
                Console.WriteLine("Edit");
                Console.WriteLine("Exit");

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
                        case "VIEW":
                            Console.WriteLine("View chosen, viewing tasks.");
                            CurrentList.Print();
                            continue;
                        case "DELETE":
                            Console.WriteLine("Delete Chosen, please input the label of the task you wish to delete");
                            label = Console.ReadLine().Trim();
                            CurrentList.Delete(label);
                            CurrentList.Save();
                            continue;
                        case "EDIT":
                            Console.WriteLine("Edit chosen, please choose a task to edit.");
                            throw new SystemException("Not yet implemented.");
                        /*break*/
                        case "ADD":
                            Console.WriteLine("Add chosen, please input a label and content");
                            Console.Write("Label: ");
                            label = Console.ReadLine().Trim();
                            Console.Write("Content: ");
                            content = Console.ReadLine().Trim();
                            CurrentList.Create(label, content);
                            CurrentList.Save();
                            continue;
                        case "EXIT":
                            Console.WriteLine("Exiting now.");
                            exitFlag = true;
                            return;
                        default:
                            Console.WriteLine("Unknown input detected, please choose from the options above.");
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
