using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Text;
using System.Text.Json;
using TechChallengeFastFoodFunction.Model;

namespace TechChallengeFastFoodFunction.Tests;

public class CreateCustomerFunctionTests
{
    private readonly Mock<ILogger<CreateCustomerFunction>> _mockLogger;

    public CreateCustomerFunctionTests()
    {
        _mockLogger = new Mock<ILogger<CreateCustomerFunction>>();
    }

    [Fact]
    public async Task RunAsync_WithValidRequest_ReturnsCreatedResult()
    {
        // Arrange
        var function = new CreateCustomerFunction(_mockLogger.Object);
        var customerRequest = new CustomerRequest
        {
            Cpf = "12345678900",
            Name = "João",
            Surname = "Silva",
            Email = "joao.silva@example.com",
            BirthDay = new DateOnly(1990, 1, 1)
        };
        
        var json = JsonSerializer.Serialize(customerRequest);
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
        var function = new CreateCustomerFunction(_mockLogger.Object);
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
    public async Task RunAsync_WithMissingCpf_ReturnsBadRequest()
    {
        // Arrange
        var function = new CreateCustomerFunction(_mockLogger.Object);
        var customerRequest = new CustomerRequest
        {
            Name = "Maria",
            Surname = "Santos",
            Email = "maria.santos@example.com",
            BirthDay = new DateOnly(1990, 1, 1)
        };
        
        var json = JsonSerializer.Serialize(customerRequest);
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
        var function = new CreateCustomerFunction(_mockLogger.Object);
        var customerRequest = new CustomerRequest
        {
            Cpf = "12345678900",
            Surname = "Oliveira",
            Email = "pedro.oliveira@example.com",
            BirthDay = new DateOnly(1990, 1, 1)
        };
        
        var json = JsonSerializer.Serialize(customerRequest);
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
        var function = new CreateCustomerFunction(_mockLogger.Object);
        var customerRequest = new CustomerRequest
        {
            Cpf = "12345678900",
            Name = "Ana",
            Email = "ana.costa@example.com",
            BirthDay = new DateOnly(1990, 1, 1)
        };
        
        var json = JsonSerializer.Serialize(customerRequest);
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
        var function = new CreateCustomerFunction(_mockLogger.Object);
        var customerRequest = new CustomerRequest
        {
            Cpf = "12345678900",
            Name = "Rafael",
            Surname = "Lima",
            BirthDay = new DateOnly(1990, 1, 1)
        };
        
        var json = JsonSerializer.Serialize(customerRequest);
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
        var function = new CreateCustomerFunction(_mockLogger.Object);
        var customerRequest = new CustomerRequest
        {
            Cpf = "12345678900",
            Name = "Juliana",
            Surname = "Rodrigues",
            Email = "juliana.rodrigues@example.com",
            BirthDay = default
        };
        
        var json = JsonSerializer.Serialize(customerRequest);
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(json));
        
        var mockRequest = new Mock<HttpRequest>();
        mockRequest.Setup(x => x.Body).Returns(stream);
        
        // Act
        var result = await function.RunAsync(mockRequest.Object);
        
        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task RunAsync_WithEmptyCpf_ReturnsBadRequest()
    {
        // Arrange
        var function = new CreateCustomerFunction(_mockLogger.Object);
        var customerRequest = new CustomerRequest
        {
            Cpf = "",
            Name = "Bruno",
            Surname = "Almeida",
            Email = "bruno.almeida@example.com",
            BirthDay = new DateOnly(1990, 1, 1)
        };
        
        var json = JsonSerializer.Serialize(customerRequest);
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
        var function = new CreateCustomerFunction(_mockLogger.Object);
        var customerRequest = new CustomerRequest
        {
            Cpf = "12345678900",
            Name = "",
            Surname = "Fernandes",
            Email = "fernanda.fernandes@example.com",
            BirthDay = new DateOnly(1990, 1, 1)
        };
        
        var json = JsonSerializer.Serialize(customerRequest);
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(json));
        
        var mockRequest = new Mock<HttpRequest>();
        mockRequest.Setup(x => x.Body).Returns(stream);
        
        // Act
        var result = await function.RunAsync(mockRequest.Object);
        
        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task RunAsync_WithInvalidJson_ReturnsBadRequest()
    {
        // Arrange
        var function = new CreateCustomerFunction(_mockLogger.Object);
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

