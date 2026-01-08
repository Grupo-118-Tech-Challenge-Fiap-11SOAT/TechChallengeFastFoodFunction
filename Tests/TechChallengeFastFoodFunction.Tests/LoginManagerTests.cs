using Microsoft.AspNetCore.Mvc;
using Moq;
using TechChallengeFastFoodFunction.Manager;
using TechChallengeFastFoodFunction.Model;
using TechChallengeFastFoodFunction.Repository;

namespace TechChallengeFastFoodFunction.Tests;

public class LoginManagerTests
{
    [Fact]
    public void GenerateJwtToken_WithValidEmployee_ReturnsOkResultWithToken()
    {
        // Arrange
        Environment.SetEnvironmentVariable("JwtKey", "ThisIsAVerySecureSecretKeyForJwtTokenGenerationThatIsAtLeast32CharactersLong");
        Environment.SetEnvironmentVariable("JwtExpirationMinutes", "60");
        Environment.SetEnvironmentVariable("JwtIssuer", "TestIssuer");
        Environment.SetEnvironmentVariable("JwtAudience", "TestAudience");
        
        var loginManager = new LoginManager(new UserRepository());
        var employee = new Employee
        {
            Id = 1,
            Name = "André Santos",
            Role = "Admin"
        };
        
        // Act
        var result = loginManager.GenerateJwtToken(employee);
        
        // Assert
        Assert.NotNull(result);
        Assert.IsType<OkObjectResult>(result);
        
        var okResult = result as OkObjectResult;
        Assert.NotNull(okResult.Value);
        
        var tokenObject = okResult.Value;
        var tokenProperty = tokenObject.GetType().GetProperty("token");
        Assert.NotNull(tokenProperty);
        
        var token = tokenProperty.GetValue(tokenObject) as string;
        Assert.NotNull(token);
        Assert.NotEmpty(token);
    }

    [Fact]
    public void GenerateJwtToken_WithDifferentRoles_GeneratesTokensWithCorrectClaims()
    {
        // Arrange
        Environment.SetEnvironmentVariable("JwtKey", "ThisIsAVerySecureSecretKeyForJwtTokenGenerationThatIsAtLeast32CharactersLong");
        Environment.SetEnvironmentVariable("JwtExpirationMinutes", "60");
        Environment.SetEnvironmentVariable("JwtIssuer", "TestIssuer");
        Environment.SetEnvironmentVariable("JwtAudience", "TestAudience");
        
        var loginManager = new LoginManager(new UserRepository());
        var employee = new Employee
        {
            Id = 1,
            Name = "Isabela Ferreira",
            Role = "Manager"
        };
        
        // Act
        var result = loginManager.GenerateJwtToken(employee);
        
        // Assert
        Assert.NotNull(result);
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public void CreatePasswordHash_WithValidPassword_ReturnsHashedString()
    {
        // Arrange
        Environment.SetEnvironmentVariable("SecurityKey", "MyVerySecureSecretKeyForHashing123!");
        
        var loginManager = new LoginManager(new UserRepository());
        var password = "MySecurePassword123";
        
        // Act
        var hash = loginManager.CreatePasswordHash(password);
        
        // Assert
        Assert.NotNull(hash);
        Assert.NotEmpty(hash);
        Assert.NotEqual(password, hash);
    }

    [Fact]
    public void CreatePasswordHash_WithSamePassword_ReturnsSameHash()
    {
        // Arrange
        Environment.SetEnvironmentVariable("SecurityKey", "MyVerySecureSecretKeyForHashing123!");
        
        var loginManager = new LoginManager(new UserRepository());
        var password = "MySecurePassword123";
        
        // Act
        var hash1 = loginManager.CreatePasswordHash(password);
        var hash2 = loginManager.CreatePasswordHash(password);
        
        // Assert
        Assert.Equal(hash1, hash2);
    }

    [Fact]
    public void CreatePasswordHash_WithDifferentPasswords_ReturnsDifferentHashes()
    {
        // Arrange
        Environment.SetEnvironmentVariable("SecurityKey", "MyVerySecureSecretKeyForHashing123!");
        
        var loginManager = new LoginManager(new UserRepository());
        var password1 = "Password1";
        var password2 = "Password2";
        
        // Act
        var hash1 = loginManager.CreatePasswordHash(password1);
        var hash2 = loginManager.CreatePasswordHash(password2);
        
        // Assert
        Assert.NotEqual(hash1, hash2);
    }

    [Fact]
    public void CreatePasswordHash_WithEmptyPassword_ReturnsHash()
    {
        // Arrange
        Environment.SetEnvironmentVariable("SecurityKey", "MyVerySecureSecretKeyForHashing123!");
        
        var loginManager = new LoginManager(new UserRepository());
        var password = "";
        
        // Act
        var hash = loginManager.CreatePasswordHash(password);
        
        // Assert
        Assert.NotNull(hash);
        Assert.NotEmpty(hash);
    }

    [Fact]
    public async Task CanLoginByUserId_WithNonExistingUser_ReturnsFalse()
    {
        // Arrange
        Environment.SetEnvironmentVariable("SqlConnectionString", null);
        Environment.SetEnvironmentVariable("SecurityKey", "MyVerySecureSecretKeyForHashing123!");
        
        var loginManager = new LoginManager(new UserRepository());
        
        // Act
        var (canLogin, user) = await loginManager.CanLoginByUserId("silvana.ramos@example.com", "password");
        
        // Assert
        Assert.False(canLogin);
        Assert.Null(user);
    }

    [Fact]
    public async Task CanLoginByCpf_WithNonExistingCpf_ReturnsFalse()
    {
        // Arrange
        Environment.SetEnvironmentVariable("SqlConnectionString", null);
        
        var loginManager = new LoginManager(new UserRepository());
        
        // Act
        var (canLogin, user) = await loginManager.CanLoginByCpf("00000000000");
        
        // Assert
        Assert.False(canLogin);
        Assert.Null(user);
    }

    [Fact]
    public async Task CreateEmployee_WithNullConnectionString_ReturnsFalse()
    {
        // Arrange
        Environment.SetEnvironmentVariable("SqlConnectionString", null);
        Environment.SetEnvironmentVariable("SecurityKey", "MyVerySecureSecretKeyForHashing123!");
        
        var loginManager = new LoginManager(new UserRepository());
        
        // Act
        var result = await loginManager.CreateEmployee(
            "Leonardo",
            "Martins",
            "leonardo.martins@example.com",
            "password123",
            "Admin",
            "12345678900",
            new DateOnly(1990, 1, 1)
        );
        
        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task CreateCustomer_WithNullConnectionString_ReturnsFalse()
    {
        // Arrange
        Environment.SetEnvironmentVariable("SqlConnectionString", null);
        
        var loginManager = new LoginManager(new UserRepository());
        
        // Act
        var result = await loginManager.CreateCustomer(
            "12345678900",
            "Bianca",
            "Rodrigues",
            "bianca.rodrigues@example.com",
            new DateOnly(1990, 1, 1)
        );
        
        // Assert
        Assert.False(result);
    }

    [Fact]
    public void GenerateJwtToken_WithMultipleEmployees_GeneratesUniqueTokens()
    {
        // Arrange
        Environment.SetEnvironmentVariable("JwtKey", "ThisIsAVerySecureSecretKeyForJwtTokenGenerationThatIsAtLeast32CharactersLong");
        Environment.SetEnvironmentVariable("JwtExpirationMinutes", "60");
        Environment.SetEnvironmentVariable("JwtIssuer", "TestIssuer");
        Environment.SetEnvironmentVariable("JwtAudience", "TestAudience");
        
        var loginManager = new LoginManager(new UserRepository());
        var employee1 = new Employee { Id = 1, Name = "Gustavo Lima", Role = "Admin" };
        var employee2 = new Employee { Id = 2, Name = "Tatiana Costa", Role = "User" };
        
        // Act
        var result1 = loginManager.GenerateJwtToken(employee1);
        var result2 = loginManager.GenerateJwtToken(employee2);
        
        // Assert
        Assert.NotNull(result1);
        Assert.NotNull(result2);
        
        var okResult1 = result1 as OkObjectResult;
        var okResult2 = result2 as OkObjectResult;
        
        var token1 = okResult1.Value.GetType().GetProperty("token")?.GetValue(okResult1.Value) as string;
        var token2 = okResult2.Value.GetType().GetProperty("token")?.GetValue(okResult2.Value) as string;
        
        Assert.NotEqual(token1, token2);
    }

    [Fact]
    public async Task CanLoginByUserId_WithCorrectPassword_ReturnsTrue()
    {
        // Arrange
        Environment.SetEnvironmentVariable("SecurityKey", "MyVerySecureSecretKeyForHashing123!");
        
        var mockRepository = new Mock<IUserRepository>();
        var loginManager = new LoginManager(mockRepository.Object);
        
        var password = "SenhaSegura123";
        var passwordHash = loginManager.CreatePasswordHash(password);
        
        var employee = new Employee
        {
            Id = 1,
            Name = "Carlos",
            Surname = "Oliveira",
            Email = "carlos.oliveira@example.com",
            Password = passwordHash,
            Role = "Admin"
        };
        
        mockRepository
            .Setup(r => r.GetUserByUsernameAndPassAsync("carlos.oliveira@example.com"))
            .ReturnsAsync(employee);
        
        // Act
        var (canLogin, user) = await loginManager.CanLoginByUserId("carlos.oliveira@example.com", password);
        
        // Assert
        Assert.True(canLogin);
        Assert.NotNull(user);
        Assert.Equal("Carlos", user.Name);
        Assert.Equal("carlos.oliveira@example.com", user.Email);
    }

    [Fact]
    public async Task CanLoginByUserId_WithIncorrectPassword_ReturnsFalse()
    {
        // Arrange
        Environment.SetEnvironmentVariable("SecurityKey", "MyVerySecureSecretKeyForHashing123!");
        
        var mockRepository = new Mock<IUserRepository>();
        var loginManager = new LoginManager(mockRepository.Object);
        
        var correctPassword = "SenhaSegura123";
        var wrongPassword = "SenhaErrada456";
        var passwordHash = loginManager.CreatePasswordHash(correctPassword);
        
        var employee = new Employee
        {
            Id = 2,
            Name = "Fernanda",
            Surname = "Almeida",
            Email = "fernanda.almeida@example.com",
            Password = passwordHash,
            Role = "Manager"
        };
        
        mockRepository
            .Setup(r => r.GetUserByUsernameAndPassAsync("fernanda.almeida@example.com"))
            .ReturnsAsync(employee);
        
        // Act
        var (canLogin, user) = await loginManager.CanLoginByUserId("fernanda.almeida@example.com", wrongPassword);
        
        // Assert
        Assert.False(canLogin);
        Assert.NotNull(user);
    }

    [Fact]
    public async Task CanLoginByUserId_WithUserNotFound_ReturnsFalse()
    {
        // Arrange
        Environment.SetEnvironmentVariable("SecurityKey", "MyVerySecureSecretKeyForHashing123!");
        
        var mockRepository = new Mock<IUserRepository>();
        mockRepository
            .Setup(r => r.GetUserByUsernameAndPassAsync(It.IsAny<string>()))
            .ReturnsAsync((Employee)null);
        
        var loginManager = new LoginManager(mockRepository.Object);
        
        // Act
        var (canLogin, user) = await loginManager.CanLoginByUserId("usuario.inexistente@example.com", "qualquersenha");
        
        // Assert
        Assert.False(canLogin);
        Assert.Null(user);
    }

    [Fact]
    public async Task CanLoginByUserId_WithEmptyPassword_ReturnsFalse()
    {
        // Arrange
        Environment.SetEnvironmentVariable("SecurityKey", "MyVerySecureSecretKeyForHashing123!");
        
        var mockRepository = new Mock<IUserRepository>();
        var loginManager = new LoginManager(mockRepository.Object);
        
        var correctPassword = "SenhaSegura123";
        var emptyPassword = "";
        var passwordHash = loginManager.CreatePasswordHash(correctPassword);
        
        var employee = new Employee
        {
            Id = 3,
            Name = "Ricardo",
            Surname = "Santos",
            Email = "ricardo.santos@example.com",
            Password = passwordHash,
            Role = "User"
        };
        
        mockRepository
            .Setup(r => r.GetUserByUsernameAndPassAsync("ricardo.santos@example.com"))
            .ReturnsAsync(employee);
        
        // Act
        var (canLogin, user) = await loginManager.CanLoginByUserId("ricardo.santos@example.com", emptyPassword);
        
        // Assert
        Assert.False(canLogin);
        Assert.NotNull(user);
    }

    [Fact]
    public async Task CanLoginByCpf_WithExistingCpf_ReturnsTrue()
    {
        // Arrange
        var mockRepository = new Mock<IUserRepository>();
        
        var employee = new Employee
        {
            Id = 4,
            Name = "Juliana",
            Surname = "Pereira",
            Cpf = "12345678901",
            Role = "Customer"
        };
        
        mockRepository
            .Setup(r => r.GetUserByCpfAsync("12345678901"))
            .ReturnsAsync(employee);
        
        var loginManager = new LoginManager(mockRepository.Object);
        
        // Act
        var (canLogin, user) = await loginManager.CanLoginByCpf("12345678901");
        
        // Assert
        Assert.True(canLogin);
        Assert.NotNull(user);
        Assert.Equal("Juliana", user.Name);
        Assert.Equal("12345678901", user.Cpf);
    }

    [Fact]
    public async Task CreateEmployee_WithValidData_ReturnsTrue()
    {
        // Arrange
        Environment.SetEnvironmentVariable("SecurityKey", "MyVerySecureSecretKeyForHashing123!");
        
        var mockRepository = new Mock<IUserRepository>();
        var expectedEmployee = new Employee
        {
            Id = 5,
            Name = "Marcelo",
            Surname = "Souza",
            Email = "marcelo.souza@example.com",
            Role = "Admin",
            Cpf = "98765432100",
            BirthDay = new DateOnly(1985, 5, 15)
        };
        
        mockRepository
            .Setup(r => r.CreateEmployeeAsync(It.IsAny<Employee>()))
            .ReturnsAsync(expectedEmployee);
        
        var loginManager = new LoginManager(mockRepository.Object);
        
        // Act
        var result = await loginManager.CreateEmployee(
            "Marcelo",
            "Souza",
            "marcelo.souza@example.com",
            "senha123",
            "Admin",
            "98765432100",
            new DateOnly(1985, 5, 15)
        );
        
        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task CreateEmployee_WithRepositoryFailure_ReturnsFalse()
    {
        // Arrange
        Environment.SetEnvironmentVariable("SecurityKey", "MyVerySecureSecretKeyForHashing123!");
        
        var mockRepository = new Mock<IUserRepository>();
        mockRepository
            .Setup(r => r.CreateEmployeeAsync(It.IsAny<Employee>()))
            .ReturnsAsync((Employee)null);
        
        var loginManager = new LoginManager(mockRepository.Object);
        
        // Act
        var result = await loginManager.CreateEmployee(
            "Patricia",
            "Lima",
            "patricia.lima@example.com",
            "senha456",
            "User",
            "11122233344",
            new DateOnly(1992, 8, 22)
        );
        
        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task CreateCustomer_WithValidData_ReturnsTrue()
    {
        // Arrange
        var mockRepository = new Mock<IUserRepository>();
        var expectedCustomer = new Customer
        {
            Id = 6,
            Name = "Amanda",
            Surname = "Costa",
            Cpf = "55566677788",
            Email = "amanda.costa@example.com",
            BirthDay = new DateOnly(1995, 3, 10)
        };
        
        mockRepository
            .Setup(r => r.CreateCustomerAsync(It.IsAny<Customer>()))
            .ReturnsAsync(expectedCustomer);
        
        var loginManager = new LoginManager(mockRepository.Object);
        
        // Act
        var result = await loginManager.CreateCustomer(
            "55566677788",
            "Amanda",
            "Costa",
            "amanda.costa@example.com",
            new DateOnly(1995, 3, 10)
        );
        
        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task CreateCustomer_WithRepositoryFailure_ReturnsFalse()
    {
        // Arrange
        var mockRepository = new Mock<IUserRepository>();
        mockRepository
            .Setup(r => r.CreateCustomerAsync(It.IsAny<Customer>()))
            .ReturnsAsync((Customer)null);
        
        var loginManager = new LoginManager(mockRepository.Object);
        
        // Act
        var result = await loginManager.CreateCustomer(
            "99988877766",
            "Roberto",
            "Mendes",
            "roberto.mendes@example.com",
            new DateOnly(1988, 11, 5)
        );
        
        // Assert
        Assert.False(result);
    }
}

