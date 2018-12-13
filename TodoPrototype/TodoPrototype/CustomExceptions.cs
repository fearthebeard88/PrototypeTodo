using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoPrototype
{
    class CustomExceptions : Exception
    {
        public CustomExceptions()
        {
        }

        public CustomExceptions(string message)
            : base(message)
        {
        }

        public CustomExceptions(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
