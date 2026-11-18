using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopTARpe25.Core.Dto;
using ShopTARpe25.Core.ServiceInterface;
using ShopTARpe25.Data;
using ShopTARpe25.Models.Realestate;
using ShopTARpe25.Models.Spaceship;
using static System.Net.Mime.MediaTypeNames;

namespace ShopTARpe25.Controllers
{
    public class RealEstateController : Controller
    {
        private readonly ShopTARpe25Context _context;
        private readonly IRealEstateServices _realEstateService;
        private readonly IFileServices _fileServices;

        public RealEstateController
            (
                IRealEstateServices realEstateService,
                ShopTARpe25Context context,
                IFileServices fileService
            )
        {
            _realEstateService = realEstateService;
            _context = context;
            _fileServices = fileService;
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
                Files = vm.Files,
                Image = vm.Image
                .Select(x => new FileToDatabaseDto
                {
                    Id = x.ImageId,
                    ImageData = x.ImageData,
                    ImageTitle = x.ImageTitle,
                    RealEstateId = x.RealEstateId
                }).ToArray()
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
            var result = await _realEstateService.DetailsAsync(id);

            if (result == null)
            {
                return NotFound();
            }

            //siin toimub info edasi liikumine dto-st viewModelini
            RealEstateImageViewModel[] images = await FileFromDatabase(id);

            var vm = new RealEstateDetailsViewModel();

            vm.Id = result.Id;
            vm.Area = result.Area;
            vm.Address = result.Address;
            vm.RoomNumber = result.RoomNumber;
            vm.BuildingType = result.BuildingType;
            vm.CreatedAt = result.CreatedAt;
            vm.UpdatedAt = result.UpdatedAt;
            vm.Images.AddRange(images);

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var result = await _realEstateService.DetailsAsync(id);

            if (result == null)
            {
                return NotFound();
            }
            RealEstateImageViewModel[] images = await FileFromDatabase(id);

            var vm = new RealEstateCreateUpdateViewModel();

            vm.Id = result.Id;
            vm.Area = result.Area;
            vm.Address = result.Address;
            vm.RoomNumber = result.RoomNumber  ;
            vm.BuildingType = result.BuildingType;
            vm.CreatedAt = result.CreatedAt;
            vm.UpdatedAt = result.UpdatedAt;
            vm.Image.AddRange(images);


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
                UpdatedAt = vm.UpdatedAt,
                Files = vm.Files,
                Image = vm.Image
                .Select(x => new FileToDatabaseDto
                 {
                     Id = x.ImageId,
                     ImageData = x.ImageData,
                     ImageTitle = x.ImageTitle,
                     RealEstateId = x.RealEstateId
                 }).ToArray()

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
            var result = await _realEstateService.DetailsAsync(id);

            if (result == null)
            {
                return NotFound();
            }

            RealEstateImageViewModel[] images = await FileFromDatabase(id);


            var vm = new RealEstateDeleteViewModel();

            vm.Id = result.Id;
            vm.Area = result.Area;
            vm.Address = result.Address;
            vm.RoomNumber = result.RoomNumber;
            vm.BuildingType = result.BuildingType;
            vm.CreatedAt = result.CreatedAt;
            vm.UpdatedAt = result.UpdatedAt;
            vm.Images.AddRange(images);

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

       private async Task<RealEstateImageViewModel[]> FileFromDatabase(Guid id)
        {
            var result = await _context.FileToDatabases
                .Where(x => x.RealEstateId == id)
                .Select(y => new RealEstateImageViewModel
                {
                ImageId = y.Id,
                RealEstateId = y.Id,
                ImageData = y.ImageData,
                ImageTitle = y.ImageTitle,
                //string.Format tähenda et see muudab andmed string anmdetüübiks
                Image = string.Format("data:image/gif;base64, {0}", Convert.ToBase64String(y.ImageData))

            }).ToArrayAsync();
            return result;
        }
        [HttpPost]
        public async Task<IActionResult> RemoveImage(RealEstateImageViewModel vm)
        {
            var dto = new FileToDatabaseDto()
            {
                Id = vm.ImageId
            };
            var image = await _fileServices.RemoveImageFromDatabase(dto);
            var realEstateId = image.RealEstateId;
            if (image == null)
            {
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Update), new { id = realEstateId });
           

        }
    }
}
