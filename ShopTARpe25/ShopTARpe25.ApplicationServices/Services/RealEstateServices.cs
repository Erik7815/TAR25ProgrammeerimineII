using Microsoft.EntityFrameworkCore;
using ShopTARpe25.Core.Domain;
using ShopTARpe25.Core.Dto;
using ShopTARpe25.Core.ServiceInterface;
using ShopTARpe25.Data;


namespace ShopTARpe25.ApplicationServices.Services
{
    public class RealEstateServices : IRealEstateServices
    {
        private readonly ShopTARpe25Context _context;

        public RealEstateServices
            (
                ShopTARpe25Context context
            )
        {
            _context = context;
        }

        public async Task<RealEstate> Create(RealEstateDto dto)
        {
            RealEstate domain = new();

            domain.Id = Guid.NewGuid();
            domain.Area = dto.Area;
            domain.Address = dto.Address;
            domain.RoomNumber = dto.RoomNumber;
            domain.BuildingType = dto.BuildingType;
            domain.CreatedAt = DateTime.Now;
            domain.UpdatedAt = DateTime.Now;


            await _context.RealEstates.AddAsync(domain);
            await _context.SaveChangesAsync();

            return domain;
        }


        public async Task<RealEstate> DetailsAsync(Guid id)
        {
            var result = await _context.RealEstates
                .FirstOrDefaultAsync(x => x.Id == id);

            return result;
        }

        public async Task<RealEstate> Update(RealEstateDto dto)
        {
            RealEstate spaceship = new();

            spaceship.Id = dto.Id;
            spaceship.Area = dto.Area;
            spaceship.Address = dto.Address;
            spaceship.RoomNumber = dto. RoomNumber;
            spaceship.BuildingType = dto.BuildingType;
            spaceship.CreatedAt = dto.CreatedAt;
            spaceship.UpdatedAt = DateTime.Now;


            _context.RealEstates.Update(spaceship);
            await _context.SaveChangesAsync();

            return spaceship;
        }

        public async Task<RealEstate> Delete(Guid id)
        {
            var result = await _context.RealEstates
                .FirstOrDefaultAsync(x => x.Id == id);

            _context.RealEstates.Remove(result);
            await _context.SaveChangesAsync();

            return result;

        }
    }
}
