using System;

namespace Account.Business.Exceptions
{
    public class AccountException : Exception
    {
        public AccountException(string message) : base(message) { }
    }
}
