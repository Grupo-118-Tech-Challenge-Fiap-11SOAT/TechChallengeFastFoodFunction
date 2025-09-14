using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using TechChallengeFastFoodFunction.Model;
using TechChallengeFastFoodFunction.Repository;

namespace TechChallengeFastFoodFunction.Manager
{
    public class LoginManager
    {
        public OkObjectResult GenerateJwtToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JwtKey")));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(
                    int.Parse(Environment.GetEnvironmentVariable("JwtExpirationMinutes"))
                ),
                SigningCredentials = creds,
                Issuer = Environment.GetEnvironmentVariable("JwtIssuer"),
                Audience = Environment.GetEnvironmentVariable("JwtAudience")
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));

            return new OkObjectResult(new { token });
        }

        public async Task<(bool, User?)> CanLoginByUserId(string username, string password)
        {
            var userRepository = new UserRepository();

            var user = await userRepository.GetUserByUsernameAndPassAsync(username);
            if (user == null)
            {
                return (false, user);
            }

            if (!VerifyPassword(password, user.Password))
            {
                return (false, user);
            }

            return (user != null, user);
        }

        public async Task<(bool, User?)> CanLoginByCpf(string cpf)
        {
            var userRepository = new UserRepository();

            var user = await userRepository.GetUserByCpfAsync(cpf);
            if (user == null)
            {
                return (false, user);
            }

            return (user != null, user);
        }

        private bool VerifyPassword(string password, string storedHash)
        {
            var secretKey = Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("SecurityKey"));
            using var hmac = new HMACSHA512(secretKey);
            var computedHash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(password)));
            return computedHash == storedHash;
        }

        public async Task<bool> CreateUser(string username, string password, string cpf = "")
        {
            try
            {
                var userRepository = new UserRepository();

                var user = new User
                {
                    Name = username,
                    Password = CreatePasswordHash(password),
                    Cpf = cpf,
                    Role = "User"
                };

                return await userRepository.CreateUserAsync(user) != null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string CreatePasswordHash(string password)
        {
            var secretKey = Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("SecurityKey"));
            using var hmac = new HMACSHA512(secretKey);
            var passwordHash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(password)));
            return passwordHash;
        }
    }
}
