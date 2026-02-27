using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scheduler_desktop_app.Exceptions
{
    internal class CustomerOperationException : Exception
    {
        public string Operation { get; }

        public CustomerOperationException(string operation, string message)
            : base(message)
        {
            Operation = operation;
        }

        public CustomerOperationException(string operation, string message
            , Exception innerException) : base(message, innerException)
        {
            Operation = operation;
        }
    }
}
