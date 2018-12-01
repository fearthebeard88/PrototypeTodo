using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoPrototype
{
    [Serializable]
    class Task
    {
        public string TaskLabel;
        public string TaskContent;

        public Task(string TaskLabel = " ", string TaskContent = " ")
        {
            this.TaskLabel = TaskLabel;
            this.TaskContent = TaskContent;
        }

        public string Label
        {
            get { return this.TaskLabel; }
            set { this.TaskLabel = value; }
        }

        public string Content
        {
            get { return this.TaskContent; }
            set { this.TaskContent = value; }
        }
    }
}
