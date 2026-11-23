using ShopTARpe25.Core.Domain;
using ShopTARpe25.Core.Dto;
using ShopTARpe25.Core.ServiceInterface;
using ShopTARpe25.Data.Migrations;

namespace ShopTARpe25.RealEstateTest
{
    public class RealEstateTest : TestBase
    {
        [Fact]
        public async Task ShouldNot_AddEmptyRealEstate_WhenReturnResult()
        {
            // Arrange
            RealEstateDto dto = new()
            {
                Area = 120.5,
                Address = "Test location",
                RoomNumber = 3,
                BuildingType = "house",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            //act siin tuleb kutsuda RealEstate create meetod esile
            var result = await Svc<IRealEstateServices>().Create(dto);

            //Assert
            Assert.NotNull(result);

            
        }
        [Fact]
        public async Task Should_GetByIdRealestate_WhenReturnsEqual()
        {
            //arrange
            Guid databaseGuid = Guid.Parse("988ed4e7-4d02-47cd-8ffc-3ce75f241d4b");
            Guid wrongGuid = Guid.NewGuid();

            //Act
            await Svc<IRealEstateServices>().DetailsAsync(databaseGuid);

            //Assert
            Assert.NotEqual(wrongGuid,databaseGuid);
        }

    }
}
