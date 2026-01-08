using TechChallengeFastFoodFunction.Model;

namespace TechChallengeFastFoodFunction.Tests;

public class ModelTests
{
    [Fact]
    public void Customer_Properties_CanBeSetAndGet()
    {
        // Arrange & Act
        var customer = new Customer
        {
            Id = 1,
            Cpf = "12345678900",
            Name = "Paulo",
            Surname = "Henrique",
            Email = "paulo.henrique@example.com",
            BirthDay = new DateOnly(1990, 1, 1)
        };
        
        // Assert
        Assert.Equal(1, customer.Id);
        Assert.Equal("12345678900", customer.Cpf);
        Assert.Equal("Paulo", customer.Name);
        Assert.Equal("Henrique", customer.Surname);
        Assert.Equal("paulo.henrique@example.com", customer.Email);
        Assert.Equal(new DateOnly(1990, 1, 1), customer.BirthDay);
    }

    [Fact]
    public void Employee_Properties_CanBeSetAndGet()
    {
        // Arrange & Act
        var employee = new Employee
        {
            Id = 1,
            Name = "Vitória",
            Surname = "Moreira",
            Password = "hashedpassword",
            Email = "vitoria.moreira@example.com",
            Role = "Admin",
            Cpf = "09876543211",
            BirthDay = new DateOnly(1985, 5, 15)
        };
        
        // Assert
        Assert.Equal(1, employee.Id);
        Assert.Equal("Vitória", employee.Name);
        Assert.Equal("Moreira", employee.Surname);
        Assert.Equal("hashedpassword", employee.Password);
        Assert.Equal("vitoria.moreira@example.com", employee.Email);
        Assert.Equal("Admin", employee.Role);
        Assert.Equal("09876543211", employee.Cpf);
        Assert.Equal(new DateOnly(1985, 5, 15), employee.BirthDay);
    }

    [Fact]
    public void Employee_DefaultValues_AreCorrect()
    {
        // Arrange & Act
        var employee = new Employee();
        
        // Assert
        Assert.Equal(string.Empty, employee.Role);
        Assert.Equal(string.Empty, employee.Cpf);
        Assert.Equal(0, employee.Id);
    }

    [Fact]
    public void LoginRequest_Properties_CanBeSetAndGet()
    {
        // Arrange & Act
        var loginRequest = new LoginRequest
        {
            Name = "Aline",
            Surname = "Barbosa",
            Email = "aline.barbosa@example.com",
            Password = "password123",
            Cpf = "11111111111",
            Role = "User",
            BirthDay = new DateOnly(1995, 3, 20)
        };
        
        // Assert
        Assert.Equal("Aline", loginRequest.Name);
        Assert.Equal("Barbosa", loginRequest.Surname);
        Assert.Equal("aline.barbosa@example.com", loginRequest.Email);
        Assert.Equal("password123", loginRequest.Password);
        Assert.Equal("11111111111", loginRequest.Cpf);
        Assert.Equal("User", loginRequest.Role);
        Assert.Equal(new DateOnly(1995, 3, 20), loginRequest.BirthDay);
    }

    [Fact]
    public void CustomerRequest_Properties_CanBeSetAndGet()
    {
        // Arrange & Act
        var customerRequest = new CustomerRequest
        {
            Cpf = "22222222222",
            Name = "Eduardo",
            Surname = "Nascimento",
            Email = "eduardo.nascimento@example.com",
            BirthDay = new DateOnly(1988, 7, 10)
        };
        
        // Assert
        Assert.Equal("22222222222", customerRequest.Cpf);
        Assert.Equal("Eduardo", customerRequest.Name);
        Assert.Equal("Nascimento", customerRequest.Surname);
        Assert.Equal("eduardo.nascimento@example.com", customerRequest.Email);
        Assert.Equal(new DateOnly(1988, 7, 10), customerRequest.BirthDay);
    }

    [Fact]
    public void Customer_DefaultValues_AreCorrect()
    {
        // Arrange & Act
        var customer = new Customer();
        
        // Assert
        Assert.Equal(0, customer.Id);
        Assert.Null(customer.Cpf);
        Assert.Null(customer.Name);
        Assert.Null(customer.Surname);
        Assert.Null(customer.Email);
        Assert.Equal(default(DateOnly), customer.BirthDay);
    }

    [Fact]
    public void LoginRequest_DefaultValues_AreCorrect()
    {
        // Arrange & Act
        var loginRequest = new LoginRequest();
        
        // Assert
        Assert.Null(loginRequest.Name);
        Assert.Null(loginRequest.Surname);
        Assert.Null(loginRequest.Email);
        Assert.Null(loginRequest.Password);
        Assert.Null(loginRequest.Cpf);
        Assert.Null(loginRequest.Role);
        Assert.Equal(default(DateOnly), loginRequest.BirthDay);
    }

    [Fact]
    public void CustomerRequest_DefaultValues_AreCorrect()
    {
        // Arrange & Act
        var customerRequest = new CustomerRequest();
        
        // Assert
        Assert.Null(customerRequest.Cpf);
        Assert.Null(customerRequest.Name);
        Assert.Null(customerRequest.Surname);
        Assert.Null(customerRequest.Email);
        Assert.Equal(default(DateOnly), customerRequest.BirthDay);
    }

    [Fact]
    public void Employee_RoleProperty_CanBeModified()
    {
        // Arrange
        var employee = new Employee { Role = "User" };
        
        // Act
        employee.Role = "Admin";
        
        // Assert
        Assert.Equal("Admin", employee.Role);
    }

    [Fact]
    public void Employee_CpfProperty_CanBeModified()
    {
        // Arrange
        var employee = new Employee { Cpf = "11111111111" };
        
        // Act
        employee.Cpf = "22222222222";
        
        // Assert
        Assert.Equal("22222222222", employee.Cpf);
    }

    [Fact]
    public void Customer_BirthDayProperty_AcceptsDifferentDates()
    {
        // Arrange & Act
        var customer = new Customer
        {
            BirthDay = new DateOnly(2000, 12, 31)
        };
        
        // Assert
        Assert.Equal(2000, customer.BirthDay.Year);
        Assert.Equal(12, customer.BirthDay.Month);
        Assert.Equal(31, customer.BirthDay.Day);
    }

    [Fact]
    public void LoginRequest_PartialData_IsValid()
    {
        // Arrange & Act
        var loginRequest = new LoginRequest
        {
            Email = "claudia.soares@example.com",
            Password = "password"
        };
        
        // Assert
        Assert.Equal("claudia.soares@example.com", loginRequest.Email);
        Assert.Equal("password", loginRequest.Password);
        Assert.Null(loginRequest.Cpf);
    }

    [Fact]
    public void CustomerRequest_WithMinimalData_IsValid()
    {
        // Arrange & Act
        var customerRequest = new CustomerRequest
        {
            Cpf = "33333333333"
        };
        
        // Assert
        Assert.Equal("33333333333", customerRequest.Cpf);
        Assert.Null(customerRequest.Name);
    }
}

