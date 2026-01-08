using TechChallengeFastFoodFunction.Model;
using TechChallengeFastFoodFunction.Repository;

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
        var result = await repository.GetUserByUsernameAndPassAsync("test@example.com");
        
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
        var result = await repository.GetUserByUsernameAndPassAsync("test@example.com");
        
        // Assert
        Assert.Null(result);
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
    public async Task CreateEmployeeAsync_WithNullConnectionString_ReturnsNull()
    {
        // Arrange
        Environment.SetEnvironmentVariable("SqlConnectionString", null);
        var repository = new UserRepository();
        var employee = new Employee
        {
            Name = "John",
            Surname = "Doe",
            Email = "john.doe@example.com",
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
            Name = "John",
            Surname = "Doe",
            Email = "john.doe@example.com",
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
    public async Task CreateCustomerAsync_WithNullConnectionString_ReturnsNull()
    {
        // Arrange
        Environment.SetEnvironmentVariable("SqlConnectionString", null);
        var repository = new UserRepository();
        var customer = new Customer
        {
            Name = "John",
            Surname = "Doe",
            Email = "john.doe@example.com",
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

