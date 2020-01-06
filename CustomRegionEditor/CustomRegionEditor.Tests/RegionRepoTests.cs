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
            var regionModel = new Region { Name = regionName };
            var regionModels = new List<Region> { regionModel };

            var mockCountryRepo = new Mock<ISubRegionRepo<Country>>();

            var mockSession = new Mock<ISession>();
            mockSession.Setup(m => m.Query<Region>()).Returns(regionModels.AsQueryable());

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
            mockSession.Verify(m => m.Query<Region>(), Times.Once, "Should have queried the regions once");
            mockEagerLoader.Verify(m => m.LoadEntities(regionModel), Times.Once, "Should have called load entities with the provided region");
        }

        [Test]
        public void Given_AnNonExistingRegionName_When_FindRegionByName_Should_ReturnNoRegion()
        {
            // Arrange
            const string regionName = "Africa";
            var regionModel = new Region { Name = regionName };
            var regionModels = new List<Region> { regionModel };

            var mockCountryRepo = new Mock<ISubRegionRepo<Country>>();

            var mockSession = new Mock<ISession>();
            mockSession.Setup(m => m.Query<Region>()).Returns(regionModels.AsQueryable());

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
            mockSession.Verify(m => m.Query<Region>(), Times.Exactly(2), "Should have queried the regions twice");
            mockEagerLoader.Verify(m => m.LoadEntities(regionModel), Times.Never, "Should return null");
        }
        
        [Test]
        public void Given_AnExistingRegionName_When_GetSubRegions_Should_ReturnListOfEntries()
        {
            // Arrange
            const string regionName = "Africa";
            const string countryName = "Toto";
            const string cityName = "Rains";

            var regionModel = new Region { Name = regionName, Id = "AFR" };
            var regionModels = new List<Region> { regionModel };
            
            var countryModel = new Country { Name = countryName, Region = regionModel };
            var countryModels = new List<Country> { countryModel };
            
            var cityModel = new City { Name = cityName };

            var entryModel = new CustomRegionEntry() { City = cityModel };
            var entryModels = new List<CustomRegionEntry>() { entryModel };

            var mockCountryRepo = new Mock<ISubRegionRepo<Country>>();
            mockCountryRepo.Setup(m => m.GetSubRegions(countryModel)).Returns(entryModels);

            var mockSession = new Mock<ISession>();
            mockSession.Setup(m => m.Query<Country>()).Returns(countryModels.AsQueryable());
            
            var mockSessionManager = new Mock<ISessionManager>();
            mockSessionManager.Setup(m => m.OpenSession()).Returns(mockSession.Object);

            var mockEagerLoader = new Mock<IEagerLoader>();
            mockEagerLoader.Setup(m => m.LoadEntities(regionModel)).Returns(regionModel);

            var regionRepo = new RegionRepo(mockEagerLoader.Object, mockSessionManager.Object, mockCountryRepo.Object);

            // Act
            var entriesFound = regionRepo.GetSubRegions(regionModel);

            var idealResult = new List<CustomRegionEntry>()
            {
                new CustomRegionEntry() { Country = countryModel},
                new CustomRegionEntry() { City = cityModel},
            };
            // Assert
            Assert.IsNotNull(entriesFound, "We should have received a country and city.");
            Assert.AreEqual(entriesFound[1].City.Name, idealResult[1].City.Name);
            mockSessionManager.Verify(m => m.OpenSession(), Times.Once, "We should only call OpenSession once.");
            mockSession.Verify(m => m.Query<Country>(), Times.Once, "Should have queried the countries once.");
            mockEagerLoader.Verify(m => m.LoadEntities(regionModel), Times.Never, "Should return the region model.");
        }

        [Test]
        public void Given_ANoneExistingRegionName_When_GetSubRegions_Should_ReturnEmptyListOfEntries()
        {
            // Arrange
            const string regionName = "Africa";
            const string countryName = "Toto";
            const string cityName = "Rains";

            var fakeRegion = new Region { Name = "Dr Pepper", Id = "PEP" };


            var regionModel = new Region { Name = regionName, Id = "AFR" };
            var regionModels = new List<Region> { regionModel };

            var countryModel = new Country { Name = countryName, Region = regionModel };
            var countryModels = new List<Country> { countryModel };

            var cityModel = new City { Name = cityName };

            var entryModel = new CustomRegionEntry() { City = cityModel };
            var entryModels = new List<CustomRegionEntry>() { entryModel };

            var mockCountryRepo = new Mock<ISubRegionRepo<Country>>();
            mockCountryRepo.Setup(m => m.GetSubRegions(countryModel)).Returns(entryModels);

            var mockSession = new Mock<ISession>();
            mockSession.Setup(m => m.Query<Country>()).Returns(countryModels.AsQueryable());

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
            Assert.AreEqual(entriesFound, new List<CustomRegionEntry>());
            mockSessionManager.Verify(m => m.OpenSession(), Times.Once, "We should only call OpenSession once");
            mockSession.Verify(m => m.Query<Country>(), Times.Once, "Should have queried the countries once");
            mockEagerLoader.Verify(m => m.LoadEntities(regionModel), Times.Never, "Should return null");
        }
    }
}
