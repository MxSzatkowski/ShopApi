using System;

namespace ShopsApi.Exceptions

{
    public class NotFound : Exception
    {
        public NotFound(string message) : base(message)
        {

        }
    }
}
