using FuelTrackerAPI.Infrastructure;

namespace Service_Layer.IService
{
    public interface ICustomService<T> where T : class
    {
        Task<IQueryable<T>> GetAllAsync(QueryObject query);
        Task<T> GetByIdAsync(int id);
        Task CreateAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task<bool> IsExistAsync(int id);  
    } // end interface
} // end namespace
