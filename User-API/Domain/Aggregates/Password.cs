namespace Domain.Aggregates
{
    public class Password
    {
        public Password() { }
        public Password(string password)
        {
            Salt = GenerateSalt();
            Value = Generate(password, Salt);
        }

        public Password(string password, byte[] salt)
        {
            Salt = salt;
            Value = Generate(password, Salt);
        }

        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid IdUser { get; set; }
        public string Value { get; set; }
        public byte[] Salt { get; set; }

        private static byte[] GenerateSalt()
        {
            byte[] salt = new byte[16];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }

        private static string Generate(string password, byte[] salt)
        {
            byte[] saltedPassword = Encoding.UTF8.GetBytes(password).Concat(salt).ToArray();

            byte[] hashBytes = SHA256.HashData(saltedPassword);
            return Convert.ToBase64String(hashBytes);
        }

        public bool Compare(string password)
        {
            string hashedPassword = Generate(password, Salt);
            return hashedPassword == Value;
        }
    }
}
