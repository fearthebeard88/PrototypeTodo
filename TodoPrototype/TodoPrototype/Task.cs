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
        public Task Parent;
        public Dictionary<string, Task> Child = new Dictionary<string, Task>();

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

        public Task TaskParent
        {
            get { return this.Parent; }
            set { this.TaskParent = value; }
        }

        public void setChild(string label, Task Child)
        {
            this.Child[label] = Child;
        }

        public Task getChild(string label)
        {
            if (this.Child.ContainsKey(label))
            {
                return Child[label];
            }

            return null;
        }

        public string getChildLabel(Task child)
        {
            return child.Label;
        }
    }
}
