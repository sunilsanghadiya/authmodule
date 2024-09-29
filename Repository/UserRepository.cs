using authmodule.Db;
using authmodule.Entitis;
using authmodule.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace authmodule.Repository
{
    public interface IUserRepository
    {
        Task<List<Users>?> GetAllUsers(); 
        Task<Users?> Login(LoginDto loginDto);
        Task<Users?> Register(Users users);
        Task<Users?> GetUserByEmailId(string emailId);
    }
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        
        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<List<Users>?> GetAllUsers() 
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<Users?> Login(LoginDto loginDto)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Email == loginDto.Email && x.Password == loginDto.Password && !x.IsDeleted && x.IsActive);
        }

        public async Task<Users?> GetUserByEmailId(string emailId) 
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == emailId && !u.IsDeleted);
        }

        public async Task<Users?> Register(Users users)
        {
            if(users.ID == 0)
            {
                _context.Users.Add(users);
            }
            else 
            {
                _context.Users.Update(users);
            }

            await _context.SaveChangesAsync();

            return users;
        }

    }
}