using System.Net;
using authmodule.Entitis;
using authmodule.Helpers;
using authmodule.Models;
using authmodule.Models.DTOs;
using authmodule.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

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
        private readonly IMapper _mapper;

        public UserService(
            IUserRepository userRepository, 
            IMapper mapper) 
        {
            _userRepository = userRepository;
            _mapper = mapper;
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
                Users? loginUser = new();

                Users? userByEmail = await _userRepository.GetUserByEmailId(loginDto.Email);  

                //verify either user auhorized or not
                bool isVerifiedUser = CustomPasswordHasher.VerifyPassword(loginDto.Password, userByEmail.Password);

                if(isVerifiedUser) {
                    loginDto.Password = userByEmail.Password;
                    loginUser = await _userRepository.Login(loginDto);
                }else {
                    return new Result("Unauthorized user.");
                }
            
                if(loginUser == null) {
                    return new Result($"An error occurred while getting user : {loginDto.Email.ToString()}", null, HttpStatusCode.InternalServerError);
                }

                LoginResponse? loginResponse = _mapper.Map<LoginResponse>(loginUser);

                #region Generate AccessToken for User
                
                loginResponse.AccessToken = JwtTokenHelper.GenerateJwtToken(loginResponse?.Email, "sdfs^&&#%GFHeystr6wecewr673674rfhsdvfyu3r46R%E%TSFdsdfsdf");
                #endregion

                result.ResultObject = loginResponse;
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
                if(!registerDto.Email.Contains("@"))
                {
                    return new Result($"Please provide valid email address", registerDto.Email.ToString());
                }
                if(string.IsNullOrWhiteSpace(registerDto.Email))
                {
                    return new Result($"Please provide email address", registerDto.Email.ToString());
                }
                if(string.IsNullOrWhiteSpace(registerDto.Password))
                {
                    return new Result($"Please provide Password", registerDto.Password.ToString());
                }
                if(registerDto.Password.Length < 8)
                {
                    return new Result($"Please provide atleast 8 character Password", registerDto.Password.ToString());
                }
                #endregion

                Result? result = new();

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
                    Password = CustomPasswordHasher.HashPassword(registerDto.Password)
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