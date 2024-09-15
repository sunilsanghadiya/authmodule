using authmodule.Db;
using authmodule.Repository;
using Microsoft.AspNetCore.Http.HttpResults;

namespace authmodule.Services
{
    public interface ITempDataService 
    {
        Task<object> GetData();
    }
    public class TempDataService : ITempDataService 
    {
        private readonly ITempDataRepository _tempDataRepository;

        public TempDataService(ITempDataRepository tempDataRepository) {
            _tempDataRepository = tempDataRepository;
        }

        public async Task<object> GetData() 
        {
            try 
            {
                object? result = await _tempDataRepository.GetData();
                return result;
            }
            catch(Exception ex)
            {
                throw new Exception($"An error occurred while ger", ex);
            }
        }
    }
}