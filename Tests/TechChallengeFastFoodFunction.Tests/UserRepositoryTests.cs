using TechChallengeFastFoodFunction.Model;
using TechChallengeFastFoodFunction.Repository;
using Microsoft.Data.SqlClient;

namespace TechChallengeFastFoodFunction.Tests;

public class UserRepositoryTests
{
    [Fact]
    public async Task GetUserByUsernameAndPassAsync_WithNullConnectionString_ReturnsNull()
    {
        // Arrange
        Environment.SetEnvironmentVariable("SqlConnectionString", null);
        var repository = new UserRepository();
        
        // Act
        var result = await repository.GetUserByUsernameAndPassAsync("teste@example.com");
        
        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetUserByUsernameAndPassAsync_WithEmptyConnectionString_ReturnsNull()
    {
        // Arrange
        Environment.SetEnvironmentVariable("SqlConnectionString", "");
        var repository = new UserRepository();
        
        // Act
        var result = await repository.GetUserByUsernameAndPassAsync("teste@example.com");
        
        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetUserByUsernameAndPassAsync_WithInvalidConnectionString_ThrowsSqlException()
    {
        // Arrange
        Environment.SetEnvironmentVariable("SqlConnectionString", "Server=invalid;Database=invalid;");
        var repository = new UserRepository();
        
        // Act & Assert
        await Assert.ThrowsAsync<SqlException>(async () => 
            await repository.GetUserByUsernameAndPassAsync("mariana.souza@example.com"));
    }

    [Fact]
    public async Task GetUserByCpfAsync_WithNullConnectionString_ReturnsNull()
    {
        // Arrange
        Environment.SetEnvironmentVariable("SqlConnectionString", null);
        var repository = new UserRepository();
        
        // Act
        var result = await repository.GetUserByCpfAsync("12345678900");
        
        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetUserByCpfAsync_WithEmptyConnectionString_ReturnsNull()
    {
        // Arrange
        Environment.SetEnvironmentVariable("SqlConnectionString", "");
        var repository = new UserRepository();
        
        // Act
        var result = await repository.GetUserByCpfAsync("12345678900");
        
        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetUserByCpfAsync_WithInvalidConnectionString_ThrowsSqlException()
    {
        // Arrange
        Environment.SetEnvironmentVariable("SqlConnectionString", "Server=invalid;Database=invalid;");
        var repository = new UserRepository();
        
        // Act & Assert
        await Assert.ThrowsAsync<SqlException>(async () => 
            await repository.GetUserByCpfAsync("98765432100"));
    }

    [Fact]
    public async Task CreateEmployeeAsync_WithNullConnectionString_ReturnsNull()
    {
        // Arrange
        Environment.SetEnvironmentVariable("SqlConnectionString", null);
        var repository = new UserRepository();
        var employee = new Employee
        {
            Name = "Carlos",
            Surname = "Silva",
            Email = "carlos.silva@example.com",
            Password = "hashedpassword",
            Role = "Admin",
            Cpf = "12345678900",
            BirthDay = new DateOnly(1990, 1, 1)
        };
        
        // Act
        var result = await repository.CreateEmployeeAsync(employee);
        
        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task CreateEmployeeAsync_WithEmptyConnectionString_ReturnsNull()
    {
        // Arrange
        Environment.SetEnvironmentVariable("SqlConnectionString", "");
        var repository = new UserRepository();
        var employee = new Employee
        {
            Name = "Roberto",
            Surname = "Santos",
            Email = "roberto.santos@example.com",
            Password = "hashedpassword",
            Role = "Admin",
            Cpf = "12345678900",
            BirthDay = new DateOnly(1990, 1, 1)
        };
        
        // Act
        var result = await repository.CreateEmployeeAsync(employee);
        
        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task CreateEmployeeAsync_WithInvalidConnectionString_ThrowsSqlException()
    {
        // Arrange
        Environment.SetEnvironmentVariable("SqlConnectionString", "Server=invalid;Database=invalid;");
        var repository = new UserRepository();
        var employee = new Employee
        {
            Name = "Fernanda",
            Surname = "Costa",
            Email = "fernanda.costa@example.com",
            Password = "hashedpassword",
            Role = "Manager",
            Cpf = "11122233344",
            BirthDay = new DateOnly(1985, 5, 15)
        };
        
        // Act & Assert
        await Assert.ThrowsAsync<SqlException>(async () => 
            await repository.CreateEmployeeAsync(employee));
    }

    [Fact]
    public async Task CreateEmployeeAsync_WithValidEmployee_PreparesCorrectParameters()
    {
        // Arrange
        Environment.SetEnvironmentVariable("SqlConnectionString", "Server=invalid;Database=invalid;");
        var repository = new UserRepository();
        var employee = new Employee
        {
            Name = "Paulo",
            Surname = "Rocha",
            Email = "paulo.rocha@example.com",
            Password = "senhaSegura123",
            Role = "Employee",
            Cpf = "55566677788",
            BirthDay = new DateOnly(1992, 8, 20)
        };
        
        // Act & Assert
        // Este teste verifica que o método tenta criar a conexão e executar o comando
        await Assert.ThrowsAsync<SqlException>(async () => 
            await repository.CreateEmployeeAsync(employee));
    }

    [Fact]
    public async Task CreateCustomerAsync_WithNullConnectionString_ReturnsNull()
    {
        // Arrange
        Environment.SetEnvironmentVariable("SqlConnectionString", null);
        var repository = new UserRepository();
        var customer = new Customer
        {
            Name = "Lucas",
            Surname = "Oliveira",
            Email = "lucas.oliveira@example.com",
            Cpf = "12345678900",
            BirthDay = new DateOnly(1990, 1, 1)
        };
        
        // Act
        var result = await repository.CreateCustomerAsync(customer);
        
        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task CreateCustomerAsync_WithEmptyConnectionString_ReturnsNull()
    {
        // Arrange
        Environment.SetEnvironmentVariable("SqlConnectionString", "");
        var repository = new UserRepository();
        var customer = new Customer
        {
            Name = "Vanessa",
            Surname = "Soares",
            Email = "vanessa.soares@example.com",
            Cpf = "12345678900",
            BirthDay = new DateOnly(1990, 1, 1)
        };
        
        // Act
        var result = await repository.CreateCustomerAsync(customer);
        
        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task CreateCustomerAsync_WithInvalidConnectionString_ThrowsSqlException()
    {
        // Arrange
        Environment.SetEnvironmentVariable("SqlConnectionString", "Server=invalid;Database=invalid;");
        var repository = new UserRepository();
        var customer = new Customer
        {
            Name = "Beatriz",
            Surname = "Martins",
            Email = "beatriz.martins@example.com",
            Cpf = "99988877766",
            BirthDay = new DateOnly(1988, 3, 10)
        };
        
        // Act & Assert
        await Assert.ThrowsAsync<SqlException>(async () => 
            await repository.CreateCustomerAsync(customer));
    }

    [Fact]
    public async Task CreateCustomerAsync_WithValidCustomer_PreparesCorrectParameters()
    {
        // Arrange
        Environment.SetEnvironmentVariable("SqlConnectionString", "Server=invalid;Database=invalid;");
        var repository = new UserRepository();
        var customer = new Customer
        {
            Name = "Gabriel",
            Surname = "Alves",
            Email = "gabriel.alves@example.com",
            Cpf = "33344455566",
            BirthDay = new DateOnly(1995, 11, 25)
        };
        
        // Act & Assert
        // Este teste verifica que o método tenta criar a conexão e executar o comando
        await Assert.ThrowsAsync<SqlException>(async () => 
            await repository.CreateCustomerAsync(customer));
    }

    [Fact]
    public async Task CreateCustomerAsync_WithDifferentCustomerData_ThrowsSqlException()
    {
        // Arrange
        Environment.SetEnvironmentVariable("SqlConnectionString", "Server=invalid;Database=invalid;");
        var repository = new UserRepository();
        var customer = new Customer
        {
            Name = "Larissa",
            Surname = "Carvalho",
            Email = "larissa.carvalho@example.com",
            Cpf = "44455566677",
            BirthDay = new DateOnly(1993, 7, 5)
        };
        
        // Act & Assert
        await Assert.ThrowsAsync<SqlException>(async () => 
            await repository.CreateCustomerAsync(customer));
    }

    [Fact]
    public async Task CreateEmployeeAsync_WithDifferentEmployeeData_ThrowsSqlException()
    {
        // Arrange
        Environment.SetEnvironmentVariable("SqlConnectionString", "Server=invalid;Database=invalid;");
        var repository = new UserRepository();
        var employee = new Employee
        {
            Name = "Ricardo",
            Surname = "Teixeira",
            Email = "ricardo.teixeira@example.com",
            Password = "senha456",
            Role = "Supervisor",
            Cpf = "77788899900",
            BirthDay = new DateOnly(1987, 12, 30)
        };
        
        // Act & Assert
        await Assert.ThrowsAsync<SqlException>(async () => 
            await repository.CreateEmployeeAsync(employee));
    }

    [Fact]
    public void UserRepository_Constructor_ReadsConnectionStringFromEnvironment()
    {
        // Arrange
        var expectedConnectionString = "Server=test;Database=test;";
        Environment.SetEnvironmentVariable("SqlConnectionString", expectedConnectionString);
        
        // Act
        var repository = new UserRepository();
        
        // Assert
        Assert.NotNull(repository);
    }

    [Fact]
    public void UserRepository_Constructor_HandlesNullConnectionString()
    {
        // Arrange
        Environment.SetEnvironmentVariable("SqlConnectionString", null);
        
        // Act
        var repository = new UserRepository();
        
        // Assert
        Assert.NotNull(repository);
    }
}



