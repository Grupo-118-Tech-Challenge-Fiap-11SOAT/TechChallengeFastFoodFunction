using TechChallengeFastFoodFunction.Model;

namespace TechChallengeFastFoodFunction.Repository;

public interface IUserRepository
{
    Task<Employee?> GetUserByUsernameAndPassAsync(string username);
    Task<Employee?> GetUserByCpfAsync(string cpf);
    Task<Employee?> CreateEmployeeAsync(Employee employee);
    Task<Customer?> CreateCustomerAsync(Customer customer);
}

