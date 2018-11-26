using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoPrototype
{
    class Task
    {
        public int TaskID;
        public string TaskLabel;
        public string TaskContent;
        public bool Parent;
        public bool Child;

        private Task(int TaskID, string TaskLabel, string TaskContent, bool Parent = false, bool Child = false)
        {
            this.TaskID = TaskID;
            this.TaskLabel = TaskLabel;
            this.TaskContent = TaskContent;
            this.Parent = Parent;
            this.Child = Child;
        }
    }
}
