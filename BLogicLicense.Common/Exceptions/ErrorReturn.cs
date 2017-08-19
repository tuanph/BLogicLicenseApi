using System;

namespace BLogicLicense.Common.Exceptions
{
    public class ErrorReturn : Exception
    {
        public ErrorReturn()
        {

        }
        public ErrorReturn(string message) : base(message)
        {
        }
    }
}
