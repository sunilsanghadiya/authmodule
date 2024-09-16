using authmodule.Entitis;
using authmodule.Models.DTOs;
using authmodule.Repository;

namespace authmodule.Services
{
    public interface IUserService 
    {
        Task<Result> GetUsers();
        Task<Result> Login(LoginDto loginDto);
        Task<Result> Register(RegisterDto registerDto);
    }

    public class UserService : IUserService 
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository) 
        {
            _userRepository = userRepository;
        }

        public async Task<Result> GetUsers()
        {
            try
            {
                Result? result = new();
                List<Users>? users = await _userRepository.GetAllUsers();

                result.ResultObject = users;
                return result;
            }
            catch(Exception ex)
            {
                return new Result($"An error occurred while getting get user list: ", ex.Message);
            }
        }

        public async Task<Result> Login(LoginDto loginDto)
        {
            try
            {

                if(string.IsNullOrWhiteSpace(loginDto.Email))
                {
                    return new Result($"Please provide email address", loginDto.Email.ToString());
                }
                if(string.IsNullOrWhiteSpace(loginDto.Password))
                {
                    return new Result($"Please provide Password address", loginDto.Password.ToString());
                }

                Result? result = new();
                Users? loginUser = await _userRepository.Login(loginDto);

                result.ResultObject = loginUser;
                return result;
            }
            catch(Exception ex)
            {
                return new Result($"An error occurred while getting get user list: ", ex.Message);
            }
        }

        public async Task<Result> Register(RegisterDto registerDto)
        {
            try 
            {
                #region API VALIDATIONS
                if(string.IsNullOrWhiteSpace(registerDto.Email))
                {
                    return new Result($"Please provide email address", registerDto.Email.ToString());
                }
                if(string.IsNullOrWhiteSpace(registerDto.Password))
                {
                    return new Result($"Please provide Password address", registerDto.Password.ToString());
                }
                #endregion

                Result? result = new();

                PasswordHasher? passwordHasher = new();

                Users? newUser = new()
                {
                    FirstName = registerDto.FirstName,
                    LastName = registerDto.LastName,
                    Email = registerDto.Email,
                    MobileNumber = registerDto.MobileNumber,
                    Gender = registerDto.Gender,
                    BirthDate = registerDto.BirthDate,
                    IsActive = true,
                    IsAdmin = false,
                    Created = DateTime.UtcNow,
                    Modified = null,
                    ModifiedBy = 0,
                    IsDeleted = false,
                    Password = passwordHasher.HashPassword(registerDto.Password)
                };

                newUser = await _userRepository.Register(newUser);

                result.ResultObject = newUser;
                return result;
            }
            catch(Exception ex)
            {
                return new Result($"An error occurred while register a user: ", ex);
            }
        }
    }
}