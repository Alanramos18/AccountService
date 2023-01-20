namespace Account.Business.Helpers
{
    public interface IEncryption
    {
        /// <summary>
        ///     Hash password.
        /// </summary>
        /// <param name="password">Password to hash</param>
        /// <returns>Hashed Password</returns>
        string Hash(string password);

        /// <summary>
        ///     Verify if password match.
        /// </summary>
        /// <param name="password">Password to hash</param>
        /// <param name="hash">Hashed password</param>
        /// <returns>True is password matches, false if not</returns>
        bool Verify(string password, string hash);
    }
}
