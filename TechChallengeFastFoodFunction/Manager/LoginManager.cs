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
        public OkObjectResult GenerateJwtToken(Employee user)
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

        public async Task<(bool, Employee?)> CanLoginByUserId(string username, string password)
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

        public async Task<(bool, Employee?)> CanLoginByCpf(string cpf)
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

        public async Task<bool> CreateEmployee(string name, string surname, string email, string password, string role, string cpf, DateOnly birthDay)
        {
            try
            {
                var userRepository = new UserRepository();

                var employee = new Employee
                {
                    Name = name,
                    Surname = surname,
                    Email = email,
                    Password = CreatePasswordHash(password),
                    Cpf = cpf,
                    Role = role,
                    BirthDay = birthDay
                };

                return await userRepository.CreateEmployeeAsync(employee) != null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> CreateCustomer(string cpf, string name, string surname, string email, DateOnly birthDay)
        {
            try
            {
                var userRepository = new UserRepository();

                var customer = new Customer
                {
                    Name = name,
                    Surname = surname,
                    Cpf = cpf,
                    Email = email,
                    BirthDay = birthDay
                };

                return await userRepository.CreateCustomerAsync(customer) != null;
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
