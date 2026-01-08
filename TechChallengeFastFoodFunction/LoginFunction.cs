using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using System.Text.Json;
using TechChallengeFastFoodFunction.Manager;
using TechChallengeFastFoodFunction.Model;
using TechChallengeFastFoodFunction.Repository;

namespace TechChallengeFastFoodFunction;

public class LoginFunction
{
    [Function("Login")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req)
    {
        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

        var loginData = JsonSerializer.Deserialize<LoginRequest>(requestBody);
        
        if (!ValidateRequestModel(loginData))
        {
            return new BadRequestObjectResult("Por favor, passe um JSON v�lido no corpo da requisi��o.");
        }

        var loginManager = new LoginManager(new UserRepository());
        Employee? user = new();
        bool canLogin;

        try
        {
            if (!string.IsNullOrEmpty(loginData.Cpf))
                (canLogin, user) = await loginManager.CanLoginByCpf(loginData.Cpf);
            else
                (canLogin, user) = await loginManager.CanLoginByUserId(loginData.Email, loginData.Password);
        }
        catch (Exception)
        {
            throw;
        }

        if (canLogin && user != null)
        {
            return loginManager.GenerateJwtToken(user);
        }
        else
        {

            return new UnauthorizedResult();
        }
    }

    private bool ValidateRequestModel(LoginRequest loginData)
    {
        if (loginData == null || (string.IsNullOrEmpty(loginData.Email) || string.IsNullOrEmpty(loginData.Password)) && string.IsNullOrEmpty(loginData.Cpf))
        {
            return false;
        }
        return true;
    }
}
