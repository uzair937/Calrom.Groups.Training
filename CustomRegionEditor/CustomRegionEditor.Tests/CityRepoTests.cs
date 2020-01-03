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
    public class CityRepoTests
    {
        [Test]
        public void Given_AnExistingCityName_Then_FindCityByName_Should_ReturnExistingCity() {
            // Arrange
            const string cityName = "Manchester";
            var cityModel = new CityModel { Name = cityName };
            var cityModels = new List<CityModel> { cityModel };
            
            var mockSession = new Mock<ISession>();
            mockSession.Setup(m => m.Query<CityModel>()).Returns(cityModels.AsQueryable());

            var mockSessionManager = new Mock<ISessionManager>();
            mockSessionManager.Setup(m => m.OpenSession()).Returns(mockSession.Object);


            var mockEagerLoader = new Mock<IEagerLoader>();
            mockEagerLoader.Setup(m => m.LoadEntities(cityModel)).Returns(cityModel);
            
            var cityRepo = new CityRepo(mockEagerLoader.Object, mockSessionManager.Object);

            // Act
            var cityFound = cityRepo.FindByName(cityName);

            // Assert
            Assert.IsNotNull(cityFound, "A city should have been received. Instead null was received.");
            Assert.AreEqual(cityName, cityFound.Name, "The found city name should match");
            mockSessionManager.Verify(m => m.OpenSession(), Times.Once, "We should only call OpenSession Once");
            mockSession.Verify(m => m.Query<CityModel>(), Times.Once, "Should have queried the cities once");
            mockEagerLoader.Verify(m => m.LoadEntities(cityModel), Times.Once, "Should have called load entities with the provided city");
        }

        [Test]
        public void Given_AnNonExistingCityName_Then_FindCityByName_Should_ReturnNoCity()
        {
            // Arrange
            const string cityName = "London";
            var cityModel = new CityModel { Name = cityName };
            var cityModels = new List<CityModel> { cityModel };

            var mockSession = new Mock<ISession>();
            mockSession.Setup(m => m.Query<CityModel>()).Returns(cityModels.AsQueryable());

            var mockSessionManager = new Mock<ISessionManager>();
            mockSessionManager.Setup(m => m.OpenSession()).Returns(mockSession.Object);


            var mockEagerLoader = new Mock<IEagerLoader>(MockBehavior.Strict);
            mockEagerLoader.Setup(m => m.LoadEntities((CityModel)null)).Returns((CityModel)null);

            var cityRepo = new CityRepo(mockEagerLoader.Object, mockSessionManager.Object);

            // Act
            var cityFound = cityRepo.FindByName("Null");

            // Assert
            Assert.IsNull(cityFound, "A city should have been received. Instead null was received.");
            mockSessionManager.Verify(m => m.OpenSession(), Times.Once, "We should only call OpenSession Once");
            mockSession.Verify(m => m.Query<CityModel>(), Times.Exactly(2), "Should have queried the cities twice");
            mockEagerLoader.Verify(m => m.LoadEntities((CityModel)null), Times.Once, "Should have called load entities with the provided city");
        }
    }
}
