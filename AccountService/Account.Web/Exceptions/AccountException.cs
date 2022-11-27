using System;

namespace Account.Web.Exceptions
{
    public class AccountException : Exception
    {
        public AccountException(string message) : base(message) { }
    }
}
