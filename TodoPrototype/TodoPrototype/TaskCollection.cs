using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace TodoPrototype
{
    class TaskCollection
    {
        private static TaskCollection taskCollection;
        private Dictionary<int, Task> Tasks;
        private BinaryFormatter Formatter;

        public static TaskCollection Instance()
        {
            if (taskCollection == null)
            {
                taskCollection = new TaskCollection();
            }

            return taskCollection;
        }

        private TaskCollection()
        {
            this.Tasks = new Dictionary<int, Task>();
            this.Formatter = new BinaryFormatter();
        }
    }
}
