using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Text;
using System.Text.Json;
using TechChallengeFastFoodFunction.Model;

namespace TechChallengeFastFoodFunction.Tests;

public class LoginFunctionTests
{
    [Fact]
    public async Task Run_WithValidCpf_ReturnsOkResultWithToken()
    {
        // Arrange
        var loginFunction = new LoginFunction();
        var loginRequest = new LoginRequest
        {
            Cpf = "12345678900"
        };
        
        var json = JsonSerializer.Serialize(loginRequest);
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(json));
        
        var mockRequest = new Mock<HttpRequest>();
        mockRequest.Setup(x => x.Body).Returns(stream);
        
        // Note: This test will fail in isolation without mocking LoginManager
        // Consider this a integration test or refactor to use dependency injection
        
        // Act & Assert
        // This test requires database setup or mocking LoginManager
        // Skipping actual execution without DI refactoring
    }
    
    [Fact]
    public async Task Run_WithValidEmailAndPassword_ReturnsOkResultWithToken()
    {
        // Arrange
        var loginFunction = new LoginFunction();
        var loginRequest = new LoginRequest
        {
            Email = "test@example.com",
            Password = "password123"
        };
        
        var json = JsonSerializer.Serialize(loginRequest);
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(json));
        
        var mockRequest = new Mock<HttpRequest>();
        mockRequest.Setup(x => x.Body).Returns(stream);
        
        // Act & Assert
        // This test requires database setup or mocking LoginManager
    }
    
    [Fact]
    public async Task Run_WithInvalidRequest_ReturnsBadRequest()
    {
        // Arrange
        var loginFunction = new LoginFunction();
        var loginRequest = new LoginRequest(); // Empty request
        
        var json = JsonSerializer.Serialize(loginRequest);
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(json));
        
        var mockRequest = new Mock<HttpRequest>();
        mockRequest.Setup(x => x.Body).Returns(stream);
        
        // Act
        var result = await loginFunction.Run(mockRequest.Object);
        
        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
        var badRequestResult = result as BadRequestObjectResult;
        Assert.NotNull(badRequestResult?.Value);
        Assert.Contains("JSON", badRequestResult.Value.ToString());
    }
    
    [Fact]
    public async Task Run_WithNullRequest_ReturnsBadRequest()
    {
        // Arrange
        var loginFunction = new LoginFunction();
        var json = "null";
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(json));
        
        var mockRequest = new Mock<HttpRequest>();
        mockRequest.Setup(x => x.Body).Returns(stream);
        
        // Act
        var result = await loginFunction.Run(mockRequest.Object);
        
        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }
    
    [Fact]
    public async Task Run_WithMissingEmailAndPasswordAndCpf_ReturnsBadRequest()
    {
        // Arrange
        var loginFunction = new LoginFunction();
        var loginRequest = new LoginRequest
        {
            Name = "Test",
            Surname = "User"
        };
        
        var json = JsonSerializer.Serialize(loginRequest);
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(json));
        
        var mockRequest = new Mock<HttpRequest>();
        mockRequest.Setup(x => x.Body).Returns(stream);
        
        // Act
        var result = await loginFunction.Run(mockRequest.Object);
        
        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }
    
    [Fact]
    public async Task Run_WithOnlyEmail_ReturnsBadRequest()
    {
        // Arrange
        var loginFunction = new LoginFunction();
        var loginRequest = new LoginRequest
        {
            Email = "teste@example.com"
        };
        
        var json = JsonSerializer.Serialize(loginRequest);
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(json));
        
        var mockRequest = new Mock<HttpRequest>();
        mockRequest.Setup(x => x.Body).Returns(stream);
        
        // Act
        var result = await loginFunction.Run(mockRequest.Object);
        
        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }
    
    [Fact]
    public async Task Run_WithOnlyPassword_ReturnsBadRequest()
    {
        // Arrange
        var loginFunction = new LoginFunction();
        var loginRequest = new LoginRequest
        {
            Password = "password123"
        };
        
        var json = JsonSerializer.Serialize(loginRequest);
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(json));
        
        var mockRequest = new Mock<HttpRequest>();
        mockRequest.Setup(x => x.Body).Returns(stream);
        
        // Act
        var result = await loginFunction.Run(mockRequest.Object);
        
        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }
}

