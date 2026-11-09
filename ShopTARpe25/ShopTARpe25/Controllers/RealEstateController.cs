using Microsoft.AspNetCore.Mvc;
using ShopTARpe25.Core.Dto;
using ShopTARpe25.Core.ServiceInterface;
using ShopTARpe25.Data;
using ShopTARpe25.Models.Realestate;

namespace ShopTARpe25.Controllers
{
    public class RealEstateController : Controller
    {
        private readonly ShopTARpe25Context _context;
        private readonly IRealEstateServices _realEstateService; 

        public RealEstateController
            (
                IRealEstateServices realEstateService,
                ShopTARpe25Context context,
                IFileServices fileService
            )
        {
            _realEstateService = realEstateService;
            _context = context;
        }


        public IActionResult Index()
        {
            var result = _context.RealEstates
                .Select(x => new RealEstateIndexViewModel
                {
                    Id = x.Id,
                    Area = x.Area,
                    Address = x.Address,
                    RoomNumber = x.RoomNumber,
                    BuildingType = x.BuildingType,
                });

            return View(result);
        }

        [HttpGet]
        public IActionResult Create()
        {
            RealEstateCreateUpdateViewModel vm = new();

            return View("CreateUpdate", vm);
        }

        [HttpPost]
        public async Task<IActionResult> Create(RealEstateCreateUpdateViewModel vm)
        {

            var dto = new RealEstateDto
            {
                Id = vm.Id,
                Area = vm.Area,
                Address = vm.Address,
                RoomNumber = vm.RoomNumber,
                BuildingType = vm.BuildingType,
            };

            var result = await _realEstateService.Create(dto);
            if (result == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var spaceship = await _realEstateService.DetailsAsync(id);

            if (spaceship == null)
            {
                return NotFound();
            }

            var vm = new RealEstateDetailsViewModel();

            vm.Id = spaceship.Id;
            vm.Area = spaceship.Area;
            vm.Address = spaceship.Address;
            vm.RoomNumber = spaceship.RoomNumber;
            vm.BuildingType = spaceship.BuildingType;
            vm.CreatedAt = spaceship.CreatedAt;
            vm.UpdatedAt = spaceship.UpdatedAt;

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var spaceship = await _realEstateService.DetailsAsync(id);

            if (spaceship == null)
            {
                return NotFound();
            }

            var vm = new RealEstateCreateUpdateViewModel();

            vm.Id = spaceship.Id;
            vm.Area = spaceship.Area;
            vm.Address = spaceship.Address;
            vm.RoomNumber = spaceship.RoomNumber  ;
            vm.BuildingType = spaceship.BuildingType;
            vm.CreatedAt = spaceship.CreatedAt;
            vm.UpdatedAt = spaceship.UpdatedAt;
  

            return View("CreateUpdate", vm);
        }

        [HttpPost]
        public async Task<IActionResult> Update(RealEstateCreateUpdateViewModel vm)
        {
            var dto = new RealEstateDto()
            {
                Id = vm.Id,
                Area = vm.Area,
                Address = vm.Address,
                RoomNumber = vm.RoomNumber,
                BuildingType = vm.BuildingType,
                CreatedAt = vm.CreatedAt,
                UpdatedAt = vm.UpdatedAt
            };

            var result = await _realEstateService.Update(dto);

            if (result == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var spaceship = await _realEstateService.DetailsAsync(id);

            if (spaceship == null)
            {
                return NotFound();
            }

          

            var vm = new RealEstateDeleteViewModel();

            vm.Id = spaceship.Id;
            vm.Area = spaceship.Area;
            vm.Address = spaceship.Address;
            vm.RoomNumber = spaceship.RoomNumber;
            vm.BuildingType = spaceship.BuildingType;
            vm.CreatedAt = spaceship.CreatedAt;
            vm.UpdatedAt = spaceship.UpdatedAt;

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmation(Guid id)
        {
            var result = await _realEstateService.Delete(id);

            if (result == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }

       
    }
}
