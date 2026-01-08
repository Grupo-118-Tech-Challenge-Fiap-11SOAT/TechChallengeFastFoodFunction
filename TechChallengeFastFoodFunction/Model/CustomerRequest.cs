using System.Diagnostics.CodeAnalysis;

namespace TechChallengeFastFoodFunction.Model
{
    [ExcludeFromCodeCoverage]
    public class CustomerRequest
    {
        public string Cpf { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public DateOnly BirthDay { get; set; }
    }
}
