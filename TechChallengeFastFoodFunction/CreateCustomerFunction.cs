using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using TechChallengeFastFoodFunction.Model;
using TechChallengeFastFoodFunction.Repository;

namespace TechChallengeFastFoodFunction;

public class CreateCustomerFunction
{
    private readonly ILogger<CreateCustomerFunction> _logger;

    public CreateCustomerFunction(ILogger<CreateCustomerFunction> logger)
    {
        _logger = logger;
    }

    [Function("CreateCustomerFunction")]
    public async Task<IActionResult> RunAsync([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req)
    {
        try
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            var loginData = JsonSerializer.Deserialize<CustomerRequest>(requestBody);
            if (!ValidateRequestModel(loginData))
            {
                return new BadRequestObjectResult("Por favor, passe um JSON v�lido no corpo da requisi��o.");
            }

            var loginManager = new Manager.LoginManager(new UserRepository());
            if (await loginManager.CreateCustomer(loginData.Cpf, loginData.Name, loginData.Surname, loginData.Email, loginData.BirthDay))
                return new CreatedResult();
            else
                return new BadRequestObjectResult("Erro ao criar usu�rio.");
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message, "Erro ao processar a requisi��o.");
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
    }

    private bool ValidateRequestModel(CustomerRequest loginData)
    {
        if (loginData == null || string.IsNullOrEmpty(loginData.Cpf) || string.IsNullOrEmpty(loginData.Name) || string.IsNullOrEmpty(loginData.Surname) || string.IsNullOrEmpty(loginData.Email) || loginData.BirthDay == default)
        {
            return false;
        }
        return true;
    }
}