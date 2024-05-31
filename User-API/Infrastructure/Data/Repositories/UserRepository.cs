namespace Infrastructure.Data.Repositories
{
    public class UserRepository(DataContext context) : IUserRepository
    {
        private readonly DataContext _context = context;

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _context.User.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task SaveAsync(User user)
        {
            _context.User.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(string email, string newEmail)
        {
            var user = await _context.User.FirstOrDefaultAsync(u => u.Email == email);

            user.Email = newEmail;

            await _context.SaveChangesAsync();
        }

        public async Task<string> GetPasswordByIdUserAsync(Guid userId)
        {
            var password = await _context.Password.FirstOrDefaultAsync(p => p.IdUser == userId);

            return password!.Value;
        }

        public async Task<byte[]> GetSaltByIdUserAsync(Guid userId)
        {
            var password = await _context.Password.FirstOrDefaultAsync(p => p.IdUser == userId);

            return password.Salt;
        }
    }
}