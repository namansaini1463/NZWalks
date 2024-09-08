using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using WebApiNZwalks.Data;
using WebApiNZwalks.Models.Domain;

namespace WebApiNZwalks.Repositories
{
    public class SQLRegionRepository : IRegionRepository
    {
        private readonly NZWalksDbContext DbContext; 
        public SQLRegionRepository(NZWalksDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public async Task<Region> CreateAsync(Region region)
        {
            await DbContext.Regions.AddAsync(region);
            await DbContext.SaveChangesAsync();

            return region;  
        }

        public async Task<Region?> DeleteAsync(Guid id)
        {
            var exisitingRegion = await DbContext.Regions.FirstOrDefaultAsync(region => region.Id == id);

            if (exisitingRegion == null) {
                return null;
            }

            DbContext.Regions.Remove(exisitingRegion);
            await DbContext.SaveChangesAsync();

            return exisitingRegion;
        }

        public async Task<List<Region>> GetAllAsync()
        {
            return await DbContext.Regions.ToListAsync();
        }

        public async Task<Region?> GetByIdAsync(Guid id)
        {
            return await DbContext.Regions.FirstOrDefaultAsync(region => region.Id == id);
        }

        public async Task<Region?> UpdateAsync(Guid id, Region region)
        {
            var existingRegion = await DbContext.Regions.FirstOrDefaultAsync(region => region.Id == id);

            if(existingRegion == null)
            {
                return null;
            }

            existingRegion.Code = region.Code;
            existingRegion.Name = region.Name;
            existingRegion.RegionImageUrl = region.RegionImageUrl;

            await DbContext.SaveChangesAsync();

            return existingRegion;
        }
    }
}
