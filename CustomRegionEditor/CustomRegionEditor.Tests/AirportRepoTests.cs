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
    public class AirportRepoTests
    {
        [Test]
        public void Given_AnExistingAirportName_Then_FindAirportByName_Should_ReturnExistingAirport() {
            // Arrange
            const string airportName = "Lahore";
            var airportModel = new AirportModel { Name = airportName };
            var airportModels = new List<AirportModel> { airportModel };
            
            var mockSession = new Mock<ISession>();
            mockSession.Setup(m => m.Query<AirportModel>()).Returns(airportModels.AsQueryable());

            var mockSessionManager = new Mock<ISessionManager>();
            mockSessionManager.Setup(m => m.OpenSession()).Returns(mockSession.Object);


            var mockEagerLoader = new Mock<IEagerLoader>();
            mockEagerLoader.Setup(m => m.LoadEntities(airportModel)).Returns(airportModel);
            
            var airportRepo = new AirportRepo(mockEagerLoader.Object, mockSessionManager.Object);

            // Act
            var airportFound = airportRepo.FindByName(airportName);

            // Assert
            Assert.IsNotNull(airportFound, "We should have received an airport, but instead received null.");
            Assert.AreEqual(airportName, airportFound.Name, "The found airport name should match");
            mockSessionManager.Verify(m => m.OpenSession(), Times.Once, "We should only call OpenSession Once");
            mockSession.Verify(m => m.Query<AirportModel>(), Times.Once, "Should have queried the airports twice");
            mockEagerLoader.Verify(m => m.LoadEntities(airportModel), Times.Once, "Should have called load entities with the provided airport");
        }

        [Test]
        public void Given_AnNonExistingAirportName_When_FindAirportByName_Should_ReturnNoAirport()
        {
            // Arrange
            const string airportName = "William Booth";
            var airportModel = new AirportModel { Name = airportName };
            var airportModels = new List<AirportModel> { airportModel };

            var mockSession = new Mock<ISession>();
            mockSession.Setup(m => m.Query<AirportModel>()).Returns(airportModels.AsQueryable());

            var mockSessionManager = new Mock<ISessionManager>();
            mockSessionManager.Setup(m => m.OpenSession()).Returns(mockSession.Object);

            var mockEagerLoader = new Mock<IEagerLoader>(MockBehavior.Strict);
            mockEagerLoader.Setup(m => m.LoadEntities((AirportModel)null)).Returns((AirportModel)null);

            var airportRepo = new AirportRepo(mockEagerLoader.Object, mockSessionManager.Object);

            // Act
            var airportFound = airportRepo.FindByName("Uzair");

            // Assert
            Assert.IsNull(airportFound, "We should not have found an airport.");
            mockSessionManager.Verify(m => m.OpenSession(), Times.Once, "We should only call OpenSession Once");
            mockSession.Verify(m => m.Query<AirportModel>(), Times.Exactly(2), "Should have queried the airports twice");
            mockEagerLoader.Verify(m => m.LoadEntities((AirportModel)null), Times.Once, "Should have called load entities with null");
        }
        
        [Test]
        public void Given_AnAirportModel_When_GettingSubRegions_Should_ThrowNotImplemented()
        {
            // Arrange
            const string airportName = "William Booth";
            var airportModel = new AirportModel { Name = airportName };

            var mockSessionManager = new Mock<ISessionManager>();
            var mockEagerLoader = new Mock<IEagerLoader>(MockBehavior.Strict);

            var airportRepo = new AirportRepo(mockEagerLoader.Object, mockSessionManager.Object);

            // Act
            //var subregionsFound = airportRepo.GetSubRegions(airportModel);

            // Assert
            Assert.Throws<NotImplementedException>(() => airportRepo.GetSubRegions(airportModel), "Should throw an exception");
        }
    }
}
