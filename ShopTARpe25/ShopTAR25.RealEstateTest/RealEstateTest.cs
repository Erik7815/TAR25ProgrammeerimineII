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
        //mul on kaks ankeeti loodud ja ühe kustutan ära
        //assertis võrdlen, kas need kaks on ühesugused
        [Fact]
        public async Task ShouldNot_DeleteByIdRealEstate_WhenDidNotDeleteRealEstate()
        {
        RealEstateDto dto = MockRealEstateData();


         var realestate = await Svc<IRealEstateServices>().Create(dto);
         var realestate2 = await Svc<IRealEstateServices>().Create(dto);
         var deleteRealestate = await Svc<IRealEstateServices>().Delete((Guid)realestate2.Id);


        Assert.NotEqual(realestate.Id, deleteRealestate.Id);
        }
        //uuendab andmeid
        [Fact]
        public async Task Should_UpdateByIdRealestate_WhenRealEstate()
        {
            var guid = new Guid("988ed4e7-4d02-47cd-8ffc-3ce75f241d4b");
            RealEstateDto dto = MockRealEstateData();

            RealEstateDto domain = new();

            domain.Id = guid;
            domain.Area = 200.0;
            domain.Address = "Update location";
            domain.RoomNumber = 5;
            domain.BuildingType = "Villa";
            domain.CreatedAt = DateTime.UtcNow;
            domain.UpdatedAt = DateTime.UtcNow;

            //act
             await Svc<IRealEstateServices>().Update(dto);

            //assert, 5tk teha
            Assert.Equal(domain.Id, guid);
            Assert.NotEqual(dto.Area, domain.Area);
            Assert.NotEqual(dto.RoomNumber, domain.RoomNumber);
            Assert.DoesNotMatch(dto.RoomNumber.ToString(), domain.RoomNumber.ToString());
            Assert.DoesNotMatch(dto.Address, domain.Address);
        }
        [Fact]
        public async Task Should_UpdateRealEstate_WhenUpdateDataVersion2()
        {

            RealEstateDto dto = MockRealEstateData2();
            RealEstateDto updatedto = MockRealEstateDataUpdate();

            var createreealestate = await Svc<IRealEstateServices>().Create(dto);
            var result = await Svc<IRealEstateServices>().Update(updatedto);

            Assert.DoesNotMatch(createreealestate.Address, result.Address);
            Assert.NotEqual(createreealestate.UpdatedAt, result.UpdatedAt);


        }

        //kasutame ankeeti kus kõik andmed on null
        //tuleb kasutada create ja update
        [Fact]
        public async Task Should_UpdateRealEstate_WhenUpdateDataVersion3()
        {
            RealEstateDto dto = MockRealEstateData();
            RealEstateDto updatedto = MockRealEstateData3();

            var realEstate = await Svc<IRealEstateServices>().Create(dto);
            var result = await Svc<IRealEstateServices>().Update(updatedto);

            Assert.NotEqual(realEstate.Id, result.Id);
            Assert.NotEqual(realEstate.Address, result.Address);

        }



        private RealEstateDto MockRealEstateData()
        {
           return new RealEstateDto 
            {
                Area = 120.5,
                Address = "Testaddress",
                RoomNumber = 8,
                BuildingType = "house",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
             
        }
        private RealEstateDto MockRealEstateData2()
        {
           return new RealEstateDto 
            {
                Area = 88.5,
                Address = "Sample address",
                RoomNumber = 3,
                BuildingType = "house",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
             
        }

        private RealEstateDto MockRealEstateDataUpdate()
        {
           return new RealEstateDto 
            {
                Area = 100,
                Address = "Woodland",
                RoomNumber = 2,
                BuildingType = "hidout",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
             
        }
        private RealEstateDto MockRealEstateData3()
        {
           return new RealEstateDto 
            {
                Area = null,
                Address = null,
                RoomNumber = 0,
                BuildingType = null,
                CreatedAt = null,
                UpdatedAt = null
            };
             
        }
    }
}
