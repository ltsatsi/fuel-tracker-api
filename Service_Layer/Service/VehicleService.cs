using Domain_Layer.Models;
using FuelTrackerAPI.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Repository_Layer.IRepository;
using Service_Layer.IService;

namespace Service_Layer.Service
{
    public class VehicleService : ICustomService<Vehicle>
    {
        private readonly IRepository<Vehicle> _vehicleRepository;
        public VehicleService(IRepository<Vehicle> vehicleRepository)
        {
            _vehicleRepository = vehicleRepository ?? throw new ArgumentNullException(nameof(vehicleRepository));
        }

        public async Task CreateAsync(Vehicle entity)
        {
            try
            {
                await _vehicleRepository.CreateAsync(entity);
                await _vehicleRepository.SaveChangesAsync();
            } catch (Exception)
            {
                throw;
            }
        }

        public async Task DeleteAsync(Vehicle entity)
        {
            try
            {
                await _vehicleRepository.DeleteAsync(entity);
                await _vehicleRepository.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IQueryable<Vehicle>> GetAllAsync(QueryObject query)
        {
            try
            {   
                var vehicles = await _vehicleRepository.GetAllAsync();
                var vehiclesFuelIncluded = vehicles.Include(v => v.FuelRecords).AsQueryable();

                if (!string.IsNullOrWhiteSpace(query.Registration))
                {
                    vehiclesFuelIncluded = vehiclesFuelIncluded.Where(v => v.Registration.Contains(query.Registration.ToUpper())).AsQueryable();
                }

                if (!string.IsNullOrWhiteSpace(query.Make))
                {
                    vehiclesFuelIncluded = vehiclesFuelIncluded.Where(v => v.Make.ToLower().Contains(query.Make.ToLower())).AsQueryable();
                }

                if (query.Year != null)
                {
                    vehiclesFuelIncluded = vehiclesFuelIncluded.Where(v => v.Year.Equals(query.Year)).AsQueryable();
                }

                if (!string.IsNullOrWhiteSpace(query.SortBy))
                {
                    if (query.SortBy.Equals("make", StringComparison.CurrentCultureIgnoreCase))
                    {
                        vehiclesFuelIncluded = query.IsDescending ? vehiclesFuelIncluded.OrderByDescending(v => v.Make).AsQueryable() : vehiclesFuelIncluded.OrderBy(v => v.Make).AsQueryable();
                    }
                }

                if (!string.IsNullOrWhiteSpace(query.SortBy))
                {
                    if (query.SortBy.ToLower().Equals("year", StringComparison.CurrentCultureIgnoreCase))
                    {
                        vehiclesFuelIncluded = query.IsDescending ? vehiclesFuelIncluded.OrderByDescending(v => v.Year).AsQueryable() : vehiclesFuelIncluded.OrderBy(v => v.Year).AsQueryable();
                    }
                }

                int skipAmount = (query.PageNumber - 1) * query.PageSize;

                return vehiclesFuelIncluded.Skip(skipAmount).Take(query.PageSize);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Vehicle> GetByIdAsync(int id)
        {
            try
            {
                var vehicles = await _vehicleRepository.GetAllAsync();
                var vehicleFuelIncluded = await vehicles.Include(v => v.FuelRecords).FirstOrDefaultAsync(v => v.Id == id);

                return vehicleFuelIncluded;
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
                return await _vehicleRepository.IsExistAsync(id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdateAsync(Vehicle entity)
        {
            try
            {
                await _vehicleRepository.UpdateAsync(entity);
                await _vehicleRepository.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    } // end class
} // end namespace
