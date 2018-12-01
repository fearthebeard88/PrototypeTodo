using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

                switch (input)
                {
                    case "VIEW":
                        Console.WriteLine("View chosen, viewing tasks.");
                        // code here
                        break;
                    case "REMOVE":
                        Console.WriteLine("Delete chosen, choose tasks for deletion.");
                        // remove method
                        break;
                    case "EDIT":
                        Console.WriteLine("Edit chosen, please choose a task to edit.");
                        // edit method
                        break;
                    case "ADD":
                        Console.WriteLine("Add chosen, please add a task.");
                        // add method
                        break;
                    case "EXIT":
                        Console.WriteLine("Exiting now.");
                        exitFlag = true;
                        return;
                    default:
                        Console.WriteLine("Unknown input detected, please choose from the options above.");
                        break;
                }
            }
        }
    }
}
