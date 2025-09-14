namespace TechChallengeFastFoodFunction.Model
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Role { get; set; } = string.Empty;
        public string Cpf { get; set; } = string.Empty;
    }
}
