namespace Infrastructure.Authentication
{
    public sealed class JwtProvider(
        IOptions<JwtOptions> jwtOptions
        ) : IJwtProvider
    {
        private readonly JwtOptions _jwtOptions = jwtOptions.Value;

        public async Task<string> GenerateAsync(User user)
        {
            var claims = new Claim[] {
                new("IdUser", user.Id.ToString()),
                new(JwtRegisteredClaimNames.Email, user.Email),
        };

            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_jwtOptions.SecretKey)),
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _jwtOptions.Issuer,
                _jwtOptions.Audience,
                claims,
                null,
                DateTime.UtcNow.AddHours(8),
                signingCredentials);

            string tokenValue = new JwtSecurityTokenHandler()
                .WriteToken(token);

            return tokenValue;
        }
    }

}
