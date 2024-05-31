namespace Domain.Ports
{
    public interface IUserRepository
    {
        Task<User> GetByEmailAsync(string email);
        Task SaveAsync(User user);
        Task UpdateAsync(string email, string newEmail);
        Task<string> GetPasswordByIdUserAsync(Guid userId);
        Task<byte[]> GetSaltByIdUserAsync(Guid userId);
    }
}
