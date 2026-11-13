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
        private readonly IFileServices _fileServices;

        public RealEstateServices
            (
                ShopTARpe25Context context,
                IFileServices fileServices
            
            )
        {
            _context = context;
            _fileServices = fileServices;
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
            if (dto.Files != null)
            {
                _fileServices.UploadFilesToDatabase(dto, domain);
            }

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
            RealEstate estate = new();

            estate.Id = dto.Id;
            estate.Area = dto.Area;
            estate.Address = dto.Address;
            estate.RoomNumber = dto. RoomNumber;
            estate.BuildingType = dto.BuildingType;
            estate.CreatedAt = dto.CreatedAt;
            estate.UpdatedAt = DateTime.Now;


            _context.RealEstates.Update(estate);
            await _context.SaveChangesAsync();

            return estate;
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
