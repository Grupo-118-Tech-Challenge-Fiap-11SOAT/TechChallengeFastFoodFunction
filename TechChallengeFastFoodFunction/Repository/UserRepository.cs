using Microsoft.Data.SqlClient;
using TechChallengeFastFoodFunction.Model;

namespace TechChallengeFastFoodFunction.Repository;

public class UserRepository
{
    private readonly string _connectionString;

    // O construtor lê a string de conexão das variáveis de ambiente.
    public UserRepository()
    {
        _connectionString = Environment.GetEnvironmentVariable("SqlConnectionString");
    }

    public async Task<User> GetUserByUsernameAndPassAsync(string username)
    {
        if (string.IsNullOrEmpty(_connectionString))
        {
            return null;
        }

        // A instrução "using" garante que a conexão e o comando sejam fechados e descartados.
        using (var connection = new SqlConnection(_connectionString))
        {
            // A consulta SQL parametrizada previne ataques de injeção de SQL.
            var sql = "SELECT Name, Email, Password, Role FROM Employees WHERE Email = @Email";
            using (var command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@Email", username);

                try
                {
                    await connection.OpenAsync();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            // Se encontrar o usuário, retorna o objeto User.
                            return new User
                            {
                                Name = reader["Name"].ToString(),
                                Password = reader["Password"].ToString(),
                                Role = reader["Role"].ToString(),
                                Email = reader["Email"].ToString()
                            };
                        }
                    }
                }
                catch (SqlException)
                {
                }
            }
        }
        return null;
    }

    public async Task<User> GetUserByCpfAsync(string cpf)
    {
        if (string.IsNullOrEmpty(_connectionString))
        {
            return null;
        }

        // A instrução "using" garante que a conexão e o comando sejam fechados e descartados.
        using (var connection = new SqlConnection(_connectionString))
        {
            // A consulta SQL parametrizada previne ataques de injeção de SQL.
            var sql = "SELECT Name, Cpf FROM Customers WHERE Cpf = @Cpf";
            using (var command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@Cpf", cpf);

                try
                {
                    await connection.OpenAsync();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new User
                            {
                                Name = reader["Name"].ToString(),
                                Cpf = reader["Cpf"].ToString(),
                            };
                        }
                    }
                }
                catch (SqlException)
                {
                    throw;
                }
            }
        }
        return null;
    }

    public async Task<User> CreateUserAsync(User user)
    {
        if (string.IsNullOrEmpty(_connectionString))
        {
            return null;
        }

        try
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = "INSERT INTO Users (Username, Password, Role, Cpf) OUTPUT INSERTED.Id VALUES (@Username, @Password, @Role, @Cpf)";
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Username", user.Name);
                    command.Parameters.AddWithValue("@Password", user.Password);
                    command.Parameters.AddWithValue("@Role", string.IsNullOrWhiteSpace(user.Role) ? "" : user.Role);
                    command.Parameters.AddWithValue("@Cpf", string.IsNullOrWhiteSpace(user.Cpf) ? "" : user.Password);
                    try
                    {
                        await connection.OpenAsync();
                        user.Id = (int)await command.ExecuteScalarAsync();
                        return user;
                    }
                    catch (SqlException)
                    {
                        return null;
                    }
                }
            }
        }
        catch (Exception)
        {
            throw;
        }
    }
}
