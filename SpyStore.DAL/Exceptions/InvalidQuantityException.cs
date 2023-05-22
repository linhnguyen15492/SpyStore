using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpyStore.DAL.Exceptions
{
    public class InvalidQuantityException : Exception
    {
        public InvalidQuantityException() { }
        public InvalidQuantityException(string message) : base(message) { }
        public InvalidQuantityException(string message, Exception innerException) : base(message, innerException) { }
    }
}
