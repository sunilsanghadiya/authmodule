namespace authmodule.Models.DTOs
{
    public class RegisterDto
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string Email { get; set;} = string.Empty;
        public string MobileNumber { get; set;} = string.Empty;
        public int Gender { get; set; }
        public DateTime BirthDate { get; set; }
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}

/*
    Gender INT
    1 = male
    2 = female
    3 = transgender
    4 = prefer not to say
*/