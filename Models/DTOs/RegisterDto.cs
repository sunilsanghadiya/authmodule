namespace authmodule.Models.DTOs
{
    public class RegisterDto
    {
        public int Id { get; set; }
        public string? FirstName { get; set;}
        public string? LastName { get; set;}
        public string Email { get; set;} = string.Empty;
        public string MobileNumber { get; set;} = string.Empty;
        public bool Gender { get; set; }
        public DateOnly BirthDate { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}