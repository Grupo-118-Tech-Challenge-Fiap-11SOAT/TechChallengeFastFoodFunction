using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using TechChallengeFastFoodFunction.Model;
using TechChallengeFastFoodFunction.Repository;

namespace TechChallengeFastFoodFunction;

public class CreateEmployeeFunction
{
    private readonly ILogger<CreateEmployeeFunction> _log;
    public CreateEmployeeFunction(ILogger<CreateEmployeeFunction> log) => _log = log;

    [Function("CreateEmployeeFunction")]
    public async Task<IActionResult> RunAsync([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req)
    {
        try
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            var loginData = JsonSerializer.Deserialize<LoginRequest>(requestBody);
            if (!ValidateRequestModel(loginData))
            {
                return new BadRequestObjectResult("Por favor, passe um JSON v�lido no corpo da requisi��o.");
            }

            var loginManager = new Manager.LoginManager(new UserRepository());
            if (await loginManager.CreateEmployee(loginData.Name,loginData.Surname,loginData.Email, loginData.Password, loginData.Role,loginData.Cpf, loginData.BirthDay))
                return new CreatedResult();
            else
                return new BadRequestObjectResult("Erro ao criar usu�rio.");
        }
        catch (Exception e)
        {
            _log.LogError(e.Message, "Erro ao processar a requisi��o.");
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
    }

    private bool ValidateRequestModel(LoginRequest loginData)
    {
        if (loginData == null || string.IsNullOrEmpty(loginData.Name) || string.IsNullOrEmpty(loginData.Surname) || string.IsNullOrEmpty(loginData.Email) || string.IsNullOrEmpty(loginData.Password) || string.IsNullOrEmpty(loginData.Role) || string.IsNullOrEmpty(loginData.Cpf) || loginData.BirthDay == default)
        {
            return false;
        }
        return true;
    }
}