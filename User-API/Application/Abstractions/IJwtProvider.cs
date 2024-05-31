namespace Application.Abstractions
{
    public interface IJwtProvider
    {
        Task<string> GenerateAsync(UserEntity user);
    }
}