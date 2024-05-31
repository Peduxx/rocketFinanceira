namespace Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public Password Password { get; set; }

        public static User Create(
        string name,
        string email,
        string password)
        {
            return new()
            {
                Id = Guid.NewGuid(),
                Name = name,
                Email = email,
                Password = new Password(password)
            };
        }
    }
}