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
        public Task Child;

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
            set { this.Parent = value; }
        }

        public Task TaskChild
        {
            get { return this.Child; }
            set { this.Child = value; }
        }

        public string ParentLabel
        {
            get { return this.TaskParent.Label; }
            set { this.TaskParent.Label = value; }
        }

        public string ChildLabel
        {
            get { return this.TaskChild.Label; }
            set { this.TaskChild.Label = value; }
        }
    }
}
