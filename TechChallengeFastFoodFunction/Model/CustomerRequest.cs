namespace TechChallengeFastFoodFunction.Model
{
    public class CustomerRequest
    {
        public string Cpf { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public DateOnly BirthDay { get; set; }
    }
}
