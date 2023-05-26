using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpyStore.DAL.Exceptions
{
    // The ShoppingCartRepo will throw a custom exception when trying to add more items to the cart than are available in inventory.
    public class InvalidQuantityException : Exception
    {
        public InvalidQuantityException() { }
        public InvalidQuantityException(string message) : base(message) { }
        public InvalidQuantityException(string message, Exception innerException) : base(message, innerException) { }
    }
}
