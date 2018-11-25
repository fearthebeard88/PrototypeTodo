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
            bool hasInput = false;
            while (!hasInput)
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
                        CRUD.View();
                        hasInput = true;
                        break;
                    case "REMOVE":
                        Console.WriteLine("Delete chosen, choose tasks for deletion.");
                        // remove method
                        hasInput = true;
                        break;
                    case "EDIT":
                        Console.WriteLine("Edit chosen, please choose a task to edit.");
                        // edit method
                        hasInput = true;
                        break;
                    case "ADD":
                        Console.WriteLine("Add chosen, please add a task.");
                        // add method
                        hasInput = true;
                        break;
                    case "EXIT":
                        Console.WriteLine("Exiting now.");
                        return;
                    default:
                        Console.WriteLine("Unknown input detected, please choose from the options above.");
                        break;
                }
            }
        }
    }
}
