using Domain_Layer.Models;
using FuelTrackerAPI.Infrastructure;
using Repository_Layer.IRepository;
using Service_Layer.IService;

namespace Service_Layer.Service
{
    public class FuelService : ICustomService<Fuel>
    {
        private readonly IRepository<Fuel> _fuelRepository;
        public FuelService(IRepository<Fuel> fuelRepository)
        {
            _fuelRepository = fuelRepository ?? throw new ArgumentNullException(nameof(fuelRepository));
        }
        public async Task CreateAsync(Fuel entity)
        {
            try
            {
                await _fuelRepository.CreateAsync(entity);
                await _fuelRepository.SaveChangesAsync();
            } catch (Exception)
            {
                throw;
            }
        }

        public async Task DeleteAsync(Fuel entity)
        {
            try
            {
                await _fuelRepository.DeleteAsync(entity);
                await _fuelRepository.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IQueryable<Fuel>> GetAllAsync(QueryObject query)
        {
            try
            {
                var fuelRecords = await _fuelRepository.GetAllAsync();

                if (!string.IsNullOrWhiteSpace(query.Station))
                {
                    fuelRecords = fuelRecords.Where(f => f.Station.ToLower().Contains(query.Station.ToLower()));
                }

                if (!string.IsNullOrWhiteSpace(query.SortBy))
                {
                    if (query.SortBy.Equals("station", StringComparison.CurrentCultureIgnoreCase))
                    {
                        fuelRecords = query.IsDescending ? fuelRecords.OrderByDescending(f => f.Station).AsQueryable() : fuelRecords.OrderBy(f => f.Station).AsQueryable();
                    }
                }

                if (!string.IsNullOrWhiteSpace(query.SortBy))
                {
                    if (query.SortBy.Equals("price", StringComparison.CurrentCultureIgnoreCase))
                    {
                        fuelRecords = query.IsDescending ? fuelRecords.OrderByDescending(f => f.Price).AsQueryable() : fuelRecords.OrderBy(f => f.Price).AsQueryable();
                    }
                }

                if (!string.IsNullOrWhiteSpace(query.SortBy))
                {
                    if (query.SortBy.Equals("liter", StringComparison.CurrentCultureIgnoreCase))
                    {
                        fuelRecords = query.IsDescending ? fuelRecords.OrderByDescending(f => f.Liter).AsQueryable() : fuelRecords.OrderBy(f => f.Liter).AsQueryable();
                    }
                }

                int skipAmount = (query.PageNumber - 1) * query.PageSize;

                return fuelRecords.Skip(skipAmount).Take(query.PageSize);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Fuel> GetByIdAsync(int id)
        {
            try
            {
                return await _fuelRepository.GetByIdAsync(id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> IsExistAsync(int id)
        {
            try
            {
                return await _fuelRepository.IsExistAsync(id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdateAsync(Fuel entity)
        {
            try
            {
                await _fuelRepository.UpdateAsync(entity);
                await _fuelRepository.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    } // end class
} // end namespace
