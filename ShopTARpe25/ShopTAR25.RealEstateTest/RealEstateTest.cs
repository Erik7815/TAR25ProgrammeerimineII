using ShopTARpe25.Core.Domain;
using ShopTARpe25.Core.Dto;
using ShopTARpe25.Core.ServiceInterface;
using ShopTARpe25.Data.Migrations;
using System.Runtime.CompilerServices;

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

        [Fact]
        //mul on vaja testida DetailsAsync meetodit
        public async Task TestDetailsAsync()
        {
            //arrange
            //see guid on juba andmebaasis olemas ankeedi juures 
            Guid databaseGuid = Guid.Parse("988ed4e7-4d02-47cd-8ffc-3ce75f241d4b");
            //kui vajutadnupule siis see guid saadetakse andmebaasi mida võrreldakse
            Guid guid = Guid.Parse("988ed4e7-4d02-47cd-8ffc-3ce75f241d4b");

            await Svc<IRealEstateServices>().DetailsAsync(databaseGuid);

            Assert.Equal(databaseGuid, guid);
        }

        //kustutamine meetodit on vaja testida. Andmebaasis on ankeet mida tahan ära kustuda
        [Fact]
             public async Task Should_DeleteByIdRealestate_WhenRealEstate()
        {
            //arrange
            RealEstateDto dto = MockRealEstateData();

            //act
            var result = await Svc<IRealEstateServices>().Create(dto);
            var deleteRealestate = await Svc<IRealEstateServices>().Delete((Guid)result.Id);


            //assert
            //võrdlen id-sid kas kustutasin selle ära mis oli andmebaas
            Assert.Equal(result.Id, deleteRealestate.Id);

        }

        
        private RealEstateDto MockRealEstateData()
        {
           return new RealEstateDto 
            {
                Area = 120.5,
                Address = "Test location",
                RoomNumber = 3,
                BuildingType = "house",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
             
        }
    }
}
