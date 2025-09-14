using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using TechChallengeFastFoodFunction.Model;

namespace TechChallengeFastFoodFunction;

public class CreateUserFunction
{
    private readonly ILogger<CreateUserFunction> _log;
    public CreateUserFunction(ILogger<CreateUserFunction> log) => _log = log;

    [Function("CreateUserFunction")]
    public async Task<IActionResult> RunAsync([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req)
    {
        try
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            var loginData = JsonSerializer.Deserialize<LoginRequest>(requestBody);
            if (loginData == null || (string.IsNullOrEmpty(loginData.Email) && string.IsNullOrEmpty(loginData.Password) && string.IsNullOrEmpty(loginData.Cpf)) || string.IsNullOrEmpty(loginData.Email) || string.IsNullOrEmpty(loginData.Password))
            {
                return new BadRequestObjectResult("Por favor, passe um JSON válido no corpo da requisição.");
            }

            var loginManager = new Manager.LoginManager();
            if (await loginManager.CreateUser(loginData.Email, loginData.Password, loginData.Cpf))
                return new CreatedResult();
            else
                return new BadRequestObjectResult("Erro ao criar usuário.");
        }
        catch (Exception e)
        {
            _log.LogError(e.Message, "Erro ao processar a requisição.");
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
    }
}