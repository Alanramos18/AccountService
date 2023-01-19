namespace Account.Business.Helpers
{
    internal class Encryption : IEncryption
    {
        public Encryption()
        {

        }

        /// <inheritdoc/>
        public string Hash(string password)
        {
            var hashed = BCrypt.Net.BCrypt.HashPassword(password);

            return hashed;
        }

        /// <inheritdoc/>
        public bool Verify(string password, string hash)
        {
            bool verified = BCrypt.Net.BCrypt.Verify(password, hash);

            return verified;
        }
    }
}
