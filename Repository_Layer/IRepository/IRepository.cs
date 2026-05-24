using Domain_Layer.Models;

namespace Repository_Layer.IRepository
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task<IQueryable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task CreateAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task<bool> IsExistAsync(int id);
        Task SaveChangesAsync();
    } // end interface
} // end namespace
