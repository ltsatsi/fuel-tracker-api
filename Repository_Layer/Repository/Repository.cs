using Domain_Layer.Data;
using Domain_Layer.Models;
using Microsoft.EntityFrameworkCore;
using Repository_Layer.IRepository;

namespace Repository_Layer.Repository
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<T> _dbSet;
        public Repository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _dbSet = _context.Set<T>();
        } // end constructor

        public async Task CreateAsync(T entity)
        {
            EntityNullException(entity, "entity is null in create: Repository");
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
        } // end method

        public async Task DeleteAsync(T entity)
        {
            EntityNullException(entity, "entity is null in delete: Repository");
            _context.Remove(entity);
            await _context.SaveChangesAsync();
        } // end method

        public async Task<IQueryable<T>> GetAllAsync()
        {
            return _dbSet.AsQueryable();
        } // end method

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FirstOrDefaultAsync(x => x.Id == id);
        } // end method

        public async Task<bool> IsExistAsync(int id)
        {
            return await _dbSet.AnyAsync(x => x.Id == id);     
        } // end method

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        } // end method

        public async Task UpdateAsync(T entity)
        {
            EntityNullException(entity, "entity is null in update: Repository");
            _context.Update(entity);
            await _context.SaveChangesAsync();
        } // end method

        // helpers
        void EntityNullException(T entity, string message)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(message);
            } // end if
        } // end method

    } // end class
} // end namespace
