using CustomRegionEditor.Database.Interfaces;
using CustomRegionEditor.Database.Models;
using CustomRegionEditor.Database.Repositories;
using Moq;
using NHibernate;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace CustomRegionEditor.Test.Repositories
{
    [TestFixture]
    public class CountryRepoTests
    {
        [Test]
        public void Given_AnExistingCountryName_Then_FindCountryByName_Should_ReturnExistingCountry() {
            // Arrange
            const string countryName = "Africalifornia";
            var countryModel = new CountryModel { CountryName = countryName };
            var countryModels = new List<CountryModel> { countryModel };

            var mockCityRepo = new Mock<ISubRegionRepo<CityModel>>();
            var mockStateRepo = new Mock<ISubRegionRepo<StateModel>>();

            var mockSession = new Mock<ISession>();
            mockSession.Setup(m => m.Query<CountryModel>()).Returns(countryModels.AsQueryable());

            var mockSessionManager = new Mock<ISessionManager>();
            mockSessionManager.Setup(m => m.OpenSession()).Returns(mockSession.Object);

            var mockEagerLoader = new Mock<IEagerLoader>();
            mockEagerLoader.Setup(m => m.LoadEntities(countryModel)).Returns(countryModel);

            var countryRepo = new CountryRepo(mockEagerLoader.Object, mockSessionManager.Object, mockCityRepo.Object, mockStateRepo.Object);

            // Act
            var countryFound = countryRepo.FindByName(countryName);

            // Assert
            Assert.IsNotNull(countryFound, "We should have received an country, but instead received null.");
            Assert.AreEqual(countryName, countryFound.CountryName, "The found country name should match");
            mockSessionManager.Verify(m => m.OpenSession(), Times.Once, "We should only call OpenSession Once");
            mockSession.Verify(m => m.Query<CountryModel>(), Times.Once, "Should have queried the countrys once");
            mockEagerLoader.Verify(m => m.LoadEntities(countryModel), Times.Once, "Should have called load entities with the provided country");
        }

        [Test]
        public void Given_AnNonExistingCountryName_When_FindCountryByName_Should_ReturnNoCountry()
        {
            // Arrange
            const string countryName = "Africalifornia";
            var countryModel = new CountryModel { CountryName = countryName };
            var countryModels = new List<CountryModel> { countryModel };

            var mockCityRepo = new Mock<ISubRegionRepo<CityModel>>();
            var mockStateRepo = new Mock<ISubRegionRepo<StateModel>>();

            var mockSession = new Mock<ISession>();
            mockSession.Setup(m => m.Query<CountryModel>()).Returns(countryModels.AsQueryable());

            var mockSessionManager = new Mock<ISessionManager>();
            mockSessionManager.Setup(m => m.OpenSession()).Returns(mockSession.Object);

            var mockEagerLoader = new Mock<IEagerLoader>();
            mockEagerLoader.Setup(m => m.LoadEntities(countryModel)).Returns(countryModel);

            var countryRepo = new CountryRepo(mockEagerLoader.Object, mockSessionManager.Object, mockCityRepo.Object, mockStateRepo.Object);

            // Act
            var countryFound = countryRepo.FindByName("Penguin");

            // Assert
            Assert.IsNull(countryFound, "We should have received an null.");
            mockSessionManager.Verify(m => m.OpenSession(), Times.Once, "We should only call OpenSession Once");
            mockSession.Verify(m => m.Query<CountryModel>(), Times.Exactly(2), "Should have queried the countrys twice");
            mockEagerLoader.Verify(m => m.LoadEntities(countryModel), Times.Never, "Should return null");
        }
        
        [Test]
        public void Given_AnExistingCountryName_When_GetSubCountrys_Should_ReturnListOfEntries()
        {
            // Arrange
            const string countryName = "Africalifornia";
            const string stateName = "Toto";
            const string cityName = "Rains";

            var countryModel = new CountryModel { CountryName = countryName, CountryId = "AFR" };
            var countryModels = new List<CountryModel> { countryModel };
            
            var stateModel = new StateModel { StateName = stateName, Country = countryModel };
            var stateModels = new List<StateModel> { stateModel };
            var cityStateModel = new CityModel { CityName = cityName, State = stateModel };


            var cityModel = new CityModel { CityName = cityName, Country = countryModel };
            var cityModels = new List<CityModel> { cityModel };

            var entryModelCity = new CustomRegionEntryModel() { City = cityStateModel };
            var entryModelCities = new List<CustomRegionEntryModel>() { entryModelCity };
            
            var entryModelState = new CustomRegionEntryModel() { State = stateModel };
            var entryModelStates = new List<CustomRegionEntryModel>() { entryModelState };

            var mockCityRepo = new Mock<ISubRegionRepo<CityModel>>();
            var mockStateRepo = new Mock<ISubRegionRepo<StateModel>>();
            mockStateRepo.Setup(m => m.GetSubRegions(stateModel)).Returns(entryModelCities);
            mockCityRepo.Setup(m => m.GetSubRegions(cityModel)).Returns(new List<CustomRegionEntryModel>());

            var mockSession = new Mock<ISession>();
            mockSession.Setup(m => m.Query<CityModel>()).Returns(cityModels.AsQueryable());
            mockSession.Setup(m => m.Query<StateModel>()).Returns(stateModels.AsQueryable());
            
            var mockSessionManager = new Mock<ISessionManager>();
            mockSessionManager.Setup(m => m.OpenSession()).Returns(mockSession.Object);

            var mockEagerLoader = new Mock<IEagerLoader>();
            mockEagerLoader.Setup(m => m.LoadEntities(countryModel)).Returns(countryModel);

            var countryRepo = new CountryRepo(mockEagerLoader.Object, mockSessionManager.Object, mockCityRepo.Object, mockStateRepo.Object);

            // Act
            var entriesFound = countryRepo.GetSubRegions(countryModel);

            var idealResult = new List<CustomRegionEntryModel>()
            {
                new CustomRegionEntryModel() { City = cityModel},
                new CustomRegionEntryModel() { State = stateModel},
                new CustomRegionEntryModel() { City = cityStateModel},
            };
            // Assert
            Assert.IsNotNull(entriesFound, "We should have received a country and state.");
            Assert.AreEqual(entriesFound[2].City.CityName, idealResult[2].City.CityName);
            mockSessionManager.Verify(m => m.OpenSession(), Times.Once, "We should only call OpenSession once.");
            mockSession.Verify(m => m.Query<StateModel>(), Times.Once, "Should have queried the countries once.");
            mockEagerLoader.Verify(m => m.LoadEntities(countryModel), Times.Never, "Should return the country model.");
        }

        [Test]
        public void Given_ANoneExistingCountryName_When_GetSubCountrys_Should_ReturnEmptyListOfEntries()
        {
            // Arrange
            const string countryName = "Africa";
            const string stateName = "Toto";
            const string cityName = "Rains";

            var fakeCountry = new CountryModel { CountryName = "Dr Pepper", CountryId = "PEP" };


            var countryModel = new CountryModel { CountryName = countryName, CountryId = "AFR" };
            var countryModels = new List<CountryModel> { countryModel };

            var stateModel = new StateModel { StateName = stateName, Country = countryModel };
            var stateModels = new List<StateModel> { stateModel };

            var cityModel = new CityModel { CityName = cityName };

            var entryModel = new CustomRegionEntryModel() { City = cityModel };
            var entryModels = new List<CustomRegionEntryModel>() { entryModel };

            var mockCityRepo = new Mock<ISubRegionRepo<CityModel>>();
            var mockStateRepo = new Mock<ISubRegionRepo<StateModel>>();
            mockStateRepo.Setup(m => m.GetSubRegions(stateModel)).Returns(entryModels);

            var mockSession = new Mock<ISession>();
            mockSession.Setup(m => m.Query<StateModel>()).Returns(stateModels.AsQueryable());

            var mockSessionManager = new Mock<ISessionManager>();
            mockSessionManager.Setup(m => m.OpenSession()).Returns(mockSession.Object);

            var mockEagerLoader = new Mock<IEagerLoader>();
            mockEagerLoader.Setup(m => m.LoadEntities(countryModel)).Returns(countryModel);
            mockEagerLoader.Setup(m => m.LoadEntities(fakeCountry)).Returns(fakeCountry);

            var countryRepo = new CountryRepo(mockEagerLoader.Object, mockSessionManager.Object, mockCityRepo.Object, mockStateRepo.Object);

            // Act
            var entriesFound = countryRepo.GetSubRegions(fakeCountry);

            // Assert
            Assert.IsNotNull(entriesFound, "We should have received an empty list.");
            Assert.AreEqual(entriesFound, new List<CustomRegionEntryModel>());
            mockSessionManager.Verify(m => m.OpenSession(), Times.Once, "We should only call OpenSession once");
            mockSession.Verify(m => m.Query<StateModel>(), Times.Once, "Should have queried the countries once");
            mockEagerLoader.Verify(m => m.LoadEntities(countryModel), Times.Never, "Should return null");
        }
    }
}
