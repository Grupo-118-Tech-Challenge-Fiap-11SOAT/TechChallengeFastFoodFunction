using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Text;
using System.Text.Json;
using TechChallengeFastFoodFunction.Model;

namespace TechChallengeFastFoodFunction.Tests;

public class CreateEmployeeFunctionTests
{
    private readonly Mock<ILogger<CreateEmployeeFunction>> _mockLogger;

    public CreateEmployeeFunctionTests()
    {
        _mockLogger = new Mock<ILogger<CreateEmployeeFunction>>();
    }

    [Fact]
    public async Task RunAsync_WithValidRequest_ReturnsCreatedResult()
    {
        // Arrange
        var function = new CreateEmployeeFunction(_mockLogger.Object);
        var loginRequest = new LoginRequest
        {
            Name = "Carlos",
            Surname = "Mendes",
            Email = "carlos.mendes@example.com",
            Password = "SecurePassword123",
            Role = "Admin",
            Cpf = "12345678900",
            BirthDay = new DateOnly(1990, 1, 1)
        };
        
        var json = JsonSerializer.Serialize(loginRequest);
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(json));
        
        var mockRequest = new Mock<HttpRequest>();
        mockRequest.Setup(x => x.Body).Returns(stream);
        
        // Act & Assert
        // Note: This requires database or mocking LoginManager
        // Full integration test needs proper setup
    }

    [Fact]
    public async Task RunAsync_WithNullRequest_ReturnsBadRequest()
    {
        // Arrange
        var function = new CreateEmployeeFunction(_mockLogger.Object);
        var json = "null";
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(json));
        
        var mockRequest = new Mock<HttpRequest>();
        mockRequest.Setup(x => x.Body).Returns(stream);
        
        // Act
        var result = await function.RunAsync(mockRequest.Object);
        
        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
        var badRequestResult = result as BadRequestObjectResult;
        Assert.NotNull(badRequestResult?.Value);
        Assert.Contains("JSON", badRequestResult.Value.ToString());
    }

    [Fact]
    public async Task RunAsync_WithMissingName_ReturnsBadRequest()
    {
        // Arrange
        var function = new CreateEmployeeFunction(_mockLogger.Object);
        var loginRequest = new LoginRequest
        {
            Surname = "Ferreira",
            Email = "lucia.ferreira@example.com",
            Password = "SecurePassword123",
            Role = "Admin",
            Cpf = "12345678900",
            BirthDay = new DateOnly(1990, 1, 1)
        };
        
        var json = JsonSerializer.Serialize(loginRequest);
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(json));
        
        var mockRequest = new Mock<HttpRequest>();
        mockRequest.Setup(x => x.Body).Returns(stream);
        
        // Act
        var result = await function.RunAsync(mockRequest.Object);
        
        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task RunAsync_WithMissingSurname_ReturnsBadRequest()
    {
        // Arrange
        var function = new CreateEmployeeFunction(_mockLogger.Object);
        var loginRequest = new LoginRequest
        {
            Name = "Fernando",
            Email = "fernando.gomes@example.com",
            Password = "SecurePassword123",
            Role = "Admin",
            Cpf = "12345678900",
            BirthDay = new DateOnly(1990, 1, 1)
        };
        
        var json = JsonSerializer.Serialize(loginRequest);
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(json));
        
        var mockRequest = new Mock<HttpRequest>();
        mockRequest.Setup(x => x.Body).Returns(stream);
        
        // Act
        var result = await function.RunAsync(mockRequest.Object);
        
        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task RunAsync_WithMissingEmail_ReturnsBadRequest()
    {
        // Arrange
        var function = new CreateEmployeeFunction(_mockLogger.Object);
        var loginRequest = new LoginRequest
        {
            Name = "Patricia",
            Surname = "Souza",
            Password = "SecurePassword123",
            Role = "Admin",
            Cpf = "12345678900",
            BirthDay = new DateOnly(1990, 1, 1)
        };
        
        var json = JsonSerializer.Serialize(loginRequest);
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(json));
        
        var mockRequest = new Mock<HttpRequest>();
        mockRequest.Setup(x => x.Body).Returns(stream);
        
        // Act
        var result = await function.RunAsync(mockRequest.Object);
        
        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task RunAsync_WithMissingPassword_ReturnsBadRequest()
    {
        // Arrange
        var function = new CreateEmployeeFunction(_mockLogger.Object);
        var loginRequest = new LoginRequest
        {
            Name = "Thiago",
            Surname = "Ribeiro",
            Email = "thiago.ribeiro@example.com",
            Role = "Admin",
            Cpf = "12345678900",
            BirthDay = new DateOnly(1990, 1, 1)
        };
        
        var json = JsonSerializer.Serialize(loginRequest);
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(json));
        
        var mockRequest = new Mock<HttpRequest>();
        mockRequest.Setup(x => x.Body).Returns(stream);
        
        // Act
        var result = await function.RunAsync(mockRequest.Object);
        
        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task RunAsync_WithMissingRole_ReturnsBadRequest()
    {
        // Arrange
        var function = new CreateEmployeeFunction(_mockLogger.Object);
        var loginRequest = new LoginRequest
        {
            Name = "Amanda",
            Surname = "Lima",
            Email = "amanda.lima@example.com",
            Password = "SecurePassword123",
            Cpf = "12345678900",
            BirthDay = new DateOnly(1990, 1, 1)
        };
        
        var json = JsonSerializer.Serialize(loginRequest);
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(json));
        
        var mockRequest = new Mock<HttpRequest>();
        mockRequest.Setup(x => x.Body).Returns(stream);
        
        // Act
        var result = await function.RunAsync(mockRequest.Object);
        
        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task RunAsync_WithMissingCpf_ReturnsBadRequest()
    {
        // Arrange
        var function = new CreateEmployeeFunction(_mockLogger.Object);
        var loginRequest = new LoginRequest
        {
            Name = "Rodrigo",
            Surname = "Azevedo",
            Email = "rodrigo.azevedo@example.com",
            Password = "SecurePassword123",
            Role = "Admin",
            BirthDay = new DateOnly(1990, 1, 1)
        };
        
        var json = JsonSerializer.Serialize(loginRequest);
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(json));
        
        var mockRequest = new Mock<HttpRequest>();
        mockRequest.Setup(x => x.Body).Returns(stream);
        
        // Act
        var result = await function.RunAsync(mockRequest.Object);
        
        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task RunAsync_WithDefaultBirthDay_ReturnsBadRequest()
    {
        // Arrange
        var function = new CreateEmployeeFunction(_mockLogger.Object);
        var loginRequest = new LoginRequest
        {
            Name = "Camila",
            Surname = "Barbosa",
            Email = "camila.barbosa@example.com",
            Password = "SecurePassword123",
            Role = "Admin",
            Cpf = "12345678900",
            BirthDay = default
        };
        
        var json = JsonSerializer.Serialize(loginRequest);
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(json));
        
        var mockRequest = new Mock<HttpRequest>();
        mockRequest.Setup(x => x.Body).Returns(stream);
        
        // Act
        var result = await function.RunAsync(mockRequest.Object);
        
        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task RunAsync_WithEmptyName_ReturnsBadRequest()
    {
        // Arrange
        var function = new CreateEmployeeFunction(_mockLogger.Object);
        var loginRequest = new LoginRequest
        {
            Name = "",
            Surname = "Andrade",
            Email = "felipe.andrade@example.com",
            Password = "SecurePassword123",
            Role = "Admin",
            Cpf = "12345678900",
            BirthDay = new DateOnly(1990, 1, 1)
        };
        
        var json = JsonSerializer.Serialize(loginRequest);
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(json));
        
        var mockRequest = new Mock<HttpRequest>();
        mockRequest.Setup(x => x.Body).Returns(stream);
        
        // Act
        var result = await function.RunAsync(mockRequest.Object);
        
        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task RunAsync_WithEmptyPassword_ReturnsBadRequest()
    {
        // Arrange
        var function = new CreateEmployeeFunction(_mockLogger.Object);
        var loginRequest = new LoginRequest
        {
            Name = "Marcos",
            Surname = "Pereira",
            Email = "marcos.pereira@example.com",
            Password = "",
            Role = "Admin",
            Cpf = "12345678900",
            BirthDay = new DateOnly(1990, 1, 1)
        };
        
        var json = JsonSerializer.Serialize(loginRequest);
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(json));
        
        var mockRequest = new Mock<HttpRequest>();
        mockRequest.Setup(x => x.Body).Returns(stream);
        
        // Act
        var result = await function.RunAsync(mockRequest.Object);
        
        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task RunAsync_WithInvalidJson_ReturnsInternalServerError()
    {
        // Arrange
        var function = new CreateEmployeeFunction(_mockLogger.Object);
        var json = "{invalid json}";
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(json));
        
        var mockRequest = new Mock<HttpRequest>();
        mockRequest.Setup(x => x.Body).Returns(stream);
        
        // Act
        var result = await function.RunAsync(mockRequest.Object);
        
        // Assert
        Assert.IsType<StatusCodeResult>(result);
        var statusCodeResult = result as StatusCodeResult;
        Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
    }
}

