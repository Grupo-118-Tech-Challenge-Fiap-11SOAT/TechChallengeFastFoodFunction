using Microsoft.AspNetCore.Mvc;
using TechChallengeFastFoodFunction.Manager;
using TechChallengeFastFoodFunction.Model;

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
        
        var loginManager = new LoginManager();
        var employee = new Employee
        {
            Id = 1,
            Name = "John Doe",
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
        
        var loginManager = new LoginManager();
        var employee = new Employee
        {
            Id = 1,
            Name = "Jane Smith",
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
        
        var loginManager = new LoginManager();
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
        
        var loginManager = new LoginManager();
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
        
        var loginManager = new LoginManager();
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
        
        var loginManager = new LoginManager();
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
        
        var loginManager = new LoginManager();
        
        // Act
        var (canLogin, user) = await loginManager.CanLoginByUserId("nonexisting@example.com", "password");
        
        // Assert
        Assert.False(canLogin);
        Assert.Null(user);
    }

    [Fact]
    public async Task CanLoginByCpf_WithNonExistingCpf_ReturnsFalse()
    {
        // Arrange
        Environment.SetEnvironmentVariable("SqlConnectionString", null);
        
        var loginManager = new LoginManager();
        
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
        
        var loginManager = new LoginManager();
        
        // Act
        var result = await loginManager.CreateEmployee(
            "John",
            "Doe",
            "john.doe@example.com",
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
        
        var loginManager = new LoginManager();
        
        // Act
        var result = await loginManager.CreateCustomer(
            "12345678900",
            "John",
            "Doe",
            "john.doe@example.com",
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
        
        var loginManager = new LoginManager();
        var employee1 = new Employee { Id = 1, Name = "Employee1", Role = "Admin" };
        var employee2 = new Employee { Id = 2, Name = "Employee2", Role = "User" };
        
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
}

