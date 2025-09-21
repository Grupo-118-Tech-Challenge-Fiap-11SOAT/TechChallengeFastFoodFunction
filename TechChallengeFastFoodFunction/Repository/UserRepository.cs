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

    public async Task<Employee> GetUserByUsernameAndPassAsync(string username)
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
                            return new Employee
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
                    throw;
                }
            }
        }
        return null;
    }

    public async Task<Employee> GetUserByCpfAsync(string cpf)
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
                            return new Employee
                            {
                                Name = reader["Name"].ToString(),
                                Cpf = reader["Cpf"].ToString(),
                                Role = "Customer"
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

    public async Task<Employee> CreateEmployeeAsync(Employee employee)
    {
        if (string.IsNullOrEmpty(_connectionString))
        {
            return null;
        }

        try
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = "INSERT INTO Employees (Name, Surname, Email, Password, Role, Cpf, BirthDay) OUTPUT INSERTED.Id VALUES (@Name, @Surname, @Email, @Password, @Role, @Cpf, @BirthDay)";
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Name", employee.Name);
                    command.Parameters.AddWithValue("@Surname", employee.Surname);
                    command.Parameters.AddWithValue("@Email", employee.Email);
                    command.Parameters.AddWithValue("@Password", employee.Password);
                    command.Parameters.AddWithValue("@Role", employee.Role);
                    command.Parameters.AddWithValue("@Cpf", employee.Cpf);
                    command.Parameters.AddWithValue("@BirthDay", employee.BirthDay);
                    try
                    {
                        await connection.OpenAsync();
                        employee.Id = (int)await command.ExecuteScalarAsync();
                        return employee;
                    }
                    catch (SqlException)
                    {
                        throw;
                    }
                }
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<Customer> CreateCustomerAsync(Customer customer)
    {
        if (string.IsNullOrEmpty(_connectionString))
        {
            return null;
        }

        try
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = "INSERT INTO Customers (Name, Surname,email, Cpf, BirthDay) OUTPUT INSERTED.Id VALUES (@Name, @Surname, @Email, @Cpf, @BirthDay)";
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Name", customer.Name);
                    command.Parameters.AddWithValue("@Surname", customer.Surname);
                    command.Parameters.AddWithValue("@Email", customer.Email);
                    command.Parameters.AddWithValue("@Cpf", customer.Cpf);
                    command.Parameters.AddWithValue("@BirthDay", customer.BirthDay);
                    try
                    {
                        await connection.OpenAsync();
                        customer.Id = (int)await command.ExecuteScalarAsync();
                        return customer;
                    }
                    catch (SqlException)
                    {
                        throw;
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
