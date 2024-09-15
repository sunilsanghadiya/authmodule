using authmodule.Db;
using authmodule.Entitis;
using Microsoft.EntityFrameworkCore;

namespace authmodule.Repository
{
    public interface ITempDataRepository
    {
        Task<List<TempData>?> GetData();
    }
    public class TempDataRepository : ITempDataRepository
    {
        private readonly ApplicationDbContext _context;
        public TempDataRepository(ApplicationDbContext context) {
            _context = context;
        } 

        public async Task<List<TempData>?> GetData() 
        {
            return await _context.TempData.ToListAsync();
        }
        
    }

}