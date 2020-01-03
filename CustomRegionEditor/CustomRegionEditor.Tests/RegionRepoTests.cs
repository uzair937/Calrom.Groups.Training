using CustomRegionEditor.Database.Interfaces;
using CustomRegionEditor.Database.Models;
using CustomRegionEditor.Database.NHibernate;
using CustomRegionEditor.Database.Repositories;
using Moq;
using NHibernate;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CustomRegionEditor.Test.Repositories
{
    [TestFixture]
    public class RegionRepoTests
    {
        [Test]
        public void Given_AnExistingRegionName_Then_FindRegionByName_Should_ReturnExistingRegion() {
            // Arrange
            const string regionName = "Africa";
            var regionModel = new RegionModel { Name = regionName };
            var regionModels = new List<RegionModel> { regionModel };

            var mockCountryRepo = new Mock<ISubRegionRepo<CountryModel>>();

            var mockSession = new Mock<ISession>();
            mockSession.Setup(m => m.Query<RegionModel>()).Returns(regionModels.AsQueryable());

            var mockSessionManager = new Mock<ISessionManager>();
            mockSessionManager.Setup(m => m.OpenSession()).Returns(mockSession.Object);

            var mockEagerLoader = new Mock<IEagerLoader>();
            mockEagerLoader.Setup(m => m.LoadEntities(regionModel)).Returns(regionModel);
            
            var regionRepo = new RegionRepo(mockEagerLoader.Object, mockSessionManager.Object, mockCountryRepo.Object);

            // Act
            var regionFound = regionRepo.FindByName(regionName);

            // Assert
            Assert.IsNotNull(regionFound, "We should have received an region, but instead received null.");
            Assert.AreEqual(regionName, regionFound.Name, "The found region name should match");
            mockSessionManager.Verify(m => m.OpenSession(), Times.Once, "We should only call OpenSession Once");
            mockSession.Verify(m => m.Query<RegionModel>(), Times.Once, "Should have queried the regions once");
            mockEagerLoader.Verify(m => m.LoadEntities(regionModel), Times.Once, "Should have called load entities with the provided region");
        }

        [Test]
        public void Given_AnNonExistingRegionName_When_FindRegionByName_Should_ReturnNoRegion()
        {
            // Arrange
            const string regionName = "Africa";
            var regionModel = new RegionModel { Name = regionName };
            var regionModels = new List<RegionModel> { regionModel };

            var mockCountryRepo = new Mock<ISubRegionRepo<CountryModel>>();

            var mockSession = new Mock<ISession>();
            mockSession.Setup(m => m.Query<RegionModel>()).Returns(regionModels.AsQueryable());

            var mockSessionManager = new Mock<ISessionManager>();
            mockSessionManager.Setup(m => m.OpenSession()).Returns(mockSession.Object);

            var mockEagerLoader = new Mock<IEagerLoader>();
            mockEagerLoader.Setup(m => m.LoadEntities(regionModel)).Returns(regionModel);

            var regionRepo = new RegionRepo(mockEagerLoader.Object, mockSessionManager.Object, mockCountryRepo.Object);

            // Act
            var regionFound = regionRepo.FindByName("Penguin");

            // Assert
            Assert.IsNull(regionFound, "We should have received an null.");
            mockSessionManager.Verify(m => m.OpenSession(), Times.Once, "We should only call OpenSession Once");
            mockSession.Verify(m => m.Query<RegionModel>(), Times.Exactly(2), "Should have queried the regions twice");
            mockEagerLoader.Verify(m => m.LoadEntities(regionModel), Times.Never, "Should return null");
        }
        
        [Test]
        public void Given_AnExistingRegionName_When_GetSubRegions_Should_ReturnListOfEntries()
        {
            // Arrange
            const string regionName = "Africa";
            const string countryName = "Toto";
            const string cityName = "Rains";

            var regionModel = new RegionModel { Name = regionName, Id = "AFR" };
            var regionModels = new List<RegionModel> { regionModel };
            
            var countryModel = new CountryModel { Name = countryName, Region = regionModel };
            var countryModels = new List<CountryModel> { countryModel };
            
            var cityModel = new CityModel { Name = cityName };

            var entryModel = new CustomRegionEntryModel() { City = cityModel };
            var entryModels = new List<CustomRegionEntryModel>() { entryModel };

            var mockCountryRepo = new Mock<ISubRegionRepo<CountryModel>>();
            mockCountryRepo.Setup(m => m.GetSubRegions(countryModel)).Returns(entryModels);

            var mockSession = new Mock<ISession>();
            mockSession.Setup(m => m.Query<CountryModel>()).Returns(countryModels.AsQueryable());
            
            var mockSessionManager = new Mock<ISessionManager>();
            mockSessionManager.Setup(m => m.OpenSession()).Returns(mockSession.Object);

            var mockEagerLoader = new Mock<IEagerLoader>();
            mockEagerLoader.Setup(m => m.LoadEntities(regionModel)).Returns(regionModel);

            var regionRepo = new RegionRepo(mockEagerLoader.Object, mockSessionManager.Object, mockCountryRepo.Object);

            // Act
            var entriesFound = regionRepo.GetSubRegions(regionModel);

            var idealResult = new List<CustomRegionEntryModel>()
            {
                new CustomRegionEntryModel() { Country = countryModel},
                new CustomRegionEntryModel() { City = cityModel},
            };
            // Assert
            Assert.IsNotNull(entriesFound, "We should have received a country and city.");
            Assert.AreEqual(entriesFound[1].City.Name, idealResult[1].City.Name);
            mockSessionManager.Verify(m => m.OpenSession(), Times.Once, "We should only call OpenSession once.");
            mockSession.Verify(m => m.Query<CountryModel>(), Times.Once, "Should have queried the countries once.");
            mockEagerLoader.Verify(m => m.LoadEntities(regionModel), Times.Never, "Should return the region model.");
        }

        [Test]
        public void Given_ANoneExistingRegionName_When_GetSubRegions_Should_ReturnEmptyListOfEntries()
        {
            // Arrange
            const string regionName = "Africa";
            const string countryName = "Toto";
            const string cityName = "Rains";

            var fakeRegion = new RegionModel { Name = "Dr Pepper", Id = "PEP" };


            var regionModel = new RegionModel { Name = regionName, Id = "AFR" };
            var regionModels = new List<RegionModel> { regionModel };

            var countryModel = new CountryModel { Name = countryName, Region = regionModel };
            var countryModels = new List<CountryModel> { countryModel };

            var cityModel = new CityModel { Name = cityName };

            var entryModel = new CustomRegionEntryModel() { City = cityModel };
            var entryModels = new List<CustomRegionEntryModel>() { entryModel };

            var mockCountryRepo = new Mock<ISubRegionRepo<CountryModel>>();
            mockCountryRepo.Setup(m => m.GetSubRegions(countryModel)).Returns(entryModels);

            var mockSession = new Mock<ISession>();
            mockSession.Setup(m => m.Query<CountryModel>()).Returns(countryModels.AsQueryable());

            var mockSessionManager = new Mock<ISessionManager>();
            mockSessionManager.Setup(m => m.OpenSession()).Returns(mockSession.Object);

            var mockEagerLoader = new Mock<IEagerLoader>();
            mockEagerLoader.Setup(m => m.LoadEntities(regionModel)).Returns(regionModel);
            mockEagerLoader.Setup(m => m.LoadEntities(fakeRegion)).Returns(fakeRegion);

            var regionRepo = new RegionRepo(mockEagerLoader.Object, mockSessionManager.Object, mockCountryRepo.Object);

            // Act
            var entriesFound = regionRepo.GetSubRegions(fakeRegion);

            // Assert
            Assert.IsNotNull(entriesFound, "We should have received an empty list.");
            Assert.AreEqual(entriesFound, new List<CustomRegionEntryModel>());
            mockSessionManager.Verify(m => m.OpenSession(), Times.Once, "We should only call OpenSession once");
            mockSession.Verify(m => m.Query<CountryModel>(), Times.Once, "Should have queried the countries once");
            mockEagerLoader.Verify(m => m.LoadEntities(regionModel), Times.Never, "Should return null");
        }
    }
}
