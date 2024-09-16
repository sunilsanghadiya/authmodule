using authmodule.Db;
using authmodule.Entitis;
using authmodule.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace authmodule.Repository
{
    public interface IUserRepository
    {
        Task<List<Users>?> GetAllUsers(); 
        Task<Users> Login(LoginDto loginDto);
        Task<Users> Register(Users users);
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

        public async Task<Users> Login(LoginDto loginDto)
        {
            Users? user = new();
            if(loginDto != null)
            {
                user = _context.Users.Where(x => x.Email == loginDto.Email && x.Password == loginDto.Password && !x.IsDeleted).FirstOrDefault();
            }
            return user;
        }

        public async Task<Users> Register(Users users)
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