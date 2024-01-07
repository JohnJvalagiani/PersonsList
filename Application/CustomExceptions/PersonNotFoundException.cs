using System;
using System.Collections.Generic;
using System.Text;

namespace Core.CustomExceptions
{
   public class PersonNotFoundException : Exception
    {
        public PersonNotFoundException()
        {
        }

        public PersonNotFoundException(string message)
            : base(message)
        {
        }

        public PersonNotFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
