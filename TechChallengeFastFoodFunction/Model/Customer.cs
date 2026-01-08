using System.Diagnostics.CodeAnalysis;

namespace TechChallengeFastFoodFunction.Model

{
    [ExcludeFromCodeCoverage]
    public class Customer
    {
        public int Id { get; set; }
        public string Cpf { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public DateOnly BirthDay { get; set; }
    }
}
