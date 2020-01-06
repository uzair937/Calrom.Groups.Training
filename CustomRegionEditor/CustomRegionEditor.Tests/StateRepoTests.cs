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
    public class StateRepoTests
    {
        [Test]
        public void Given_AnExistingStateName_Then_FindStateByName_Should_ReturnExistingState() {
            // Arrange
            const string stateName = "Africalifornia";
            var stateModel = new State { Name = stateName };
            var stateModels = new List<State> { stateModel };

            var mockCityRepo = new Mock<ISubRegionRepo<City>>();

            var mockSession = new Mock<ISession>();
            mockSession.Setup(m => m.Query<State>()).Returns(stateModels.AsQueryable());

            var mockSessionManager = new Mock<ISessionManager>();
            mockSessionManager.Setup(m => m.OpenSession()).Returns(mockSession.Object);

            var mockEagerLoader = new Mock<IEagerLoader>();
            mockEagerLoader.Setup(m => m.LoadEntities(stateModel)).Returns(stateModel);
            
            var stateRepo = new StateRepo(mockEagerLoader.Object, mockSessionManager.Object, mockCityRepo.Object);

            // Act
            var stateFound = stateRepo.FindByName(stateName);

            // Assert
            Assert.IsNotNull(stateFound, "We should have received an state, but instead received null.");
            Assert.AreEqual(stateName, stateFound.Name, "The found state name should match");
            mockSessionManager.Verify(m => m.OpenSession(), Times.Once, "We should only call OpenSession Once");
            mockSession.Verify(m => m.Query<State>(), Times.Once, "Should have queried the states once");
            mockEagerLoader.Verify(m => m.LoadEntities(stateModel), Times.Once, "Should have called load entities with the provided state");
        }

        [Test]
        public void Given_AnNonExistingStateName_When_FindStateByName_Should_ReturnNoState()
        {
            // Arrange
            const string stateName = "Africalifornia";
            var stateModel = new State { Name = stateName };
            var stateModels = new List<State> { stateModel };

            var mockCityRepo = new Mock<ISubRegionRepo<City>>();

            var mockSession = new Mock<ISession>();
            mockSession.Setup(m => m.Query<State>()).Returns(stateModels.AsQueryable());

            var mockSessionManager = new Mock<ISessionManager>();
            mockSessionManager.Setup(m => m.OpenSession()).Returns(mockSession.Object);

            var mockEagerLoader = new Mock<IEagerLoader>();
            mockEagerLoader.Setup(m => m.LoadEntities(stateModel)).Returns(stateModel);

            var stateRepo = new StateRepo(mockEagerLoader.Object, mockSessionManager.Object, mockCityRepo.Object);

            // Act
            var stateFound = stateRepo.FindByName("Penguin");

            // Assert
            Assert.IsNull(stateFound, "We should have received an null.");
            mockSessionManager.Verify(m => m.OpenSession(), Times.Once, "We should only call OpenSession Once");
            mockSession.Verify(m => m.Query<State>(), Times.Exactly(2), "Should have queried the states twice");
            mockEagerLoader.Verify(m => m.LoadEntities(stateModel), Times.Never, "Should return null");
        }
        
        [Test]
        public void Given_AnExistingStateName_When_GetSubStates_Should_ReturnListOfEntries()
        {
            // Arrange
            const string stateName = "Africalifornia";
            const string cityName = "Toto";
            const string airportName = "Rains";

            var stateModel = new State { Name = stateName, Id = "AFR" };
            var stateModels = new List<State> { stateModel };
            
            var cityModel = new City { Name = cityName, State = stateModel };
            var cityModels = new List<City> { cityModel };
            
            var airportModel = new Airport { Name = airportName };

            var entryModel = new CustomRegionEntry() { Airport = airportModel };
            var entryModels = new List<CustomRegionEntry>() { entryModel };

            var mockCountryRepo = new Mock<ISubRegionRepo<City>>();
            mockCountryRepo.Setup(m => m.GetSubRegions(cityModel)).Returns(entryModels);

            var mockSession = new Mock<ISession>();
            mockSession.Setup(m => m.Query<City>()).Returns(cityModels.AsQueryable());
            
            var mockSessionManager = new Mock<ISessionManager>();
            mockSessionManager.Setup(m => m.OpenSession()).Returns(mockSession.Object);

            var mockEagerLoader = new Mock<IEagerLoader>();
            mockEagerLoader.Setup(m => m.LoadEntities(stateModel)).Returns(stateModel);

            var stateRepo = new StateRepo(mockEagerLoader.Object, mockSessionManager.Object, mockCountryRepo.Object);

            // Act
            var entriesFound = stateRepo.GetSubRegions(stateModel);

            var idealResult = new List<CustomRegionEntry>()
            {
                new CustomRegionEntry() { City = cityModel},
                new CustomRegionEntry() { Airport = airportModel},
            };
            // Assert
            Assert.IsNotNull(entriesFound, "We should have received a country and city.");
            Assert.AreEqual(entriesFound[1].Airport.Name, idealResult[1].Airport.Name);
            mockSessionManager.Verify(m => m.OpenSession(), Times.Once, "We should only call OpenSession once.");
            mockSession.Verify(m => m.Query<City>(), Times.Once, "Should have queried the countries once.");
            mockEagerLoader.Verify(m => m.LoadEntities(stateModel), Times.Never, "Should return the state model.");
        }

        [Test]
        public void Given_ANoneExistingStateName_When_GetSubStates_Should_ReturnEmptyListOfEntries()
        {
            // Arrange
            const string stateName = "Africa";
            const string cityName = "Toto";
            const string airportName = "Rains";

            var fakeState = new State { Name = "Dr Pepper", Id = "PEP" };


            var stateModel = new State { Name = stateName, Id = "AFR" };
            var stateModels = new List<State> { stateModel };

            var cityModel = new City { Name = cityName, State = stateModel };
            var cityModels = new List<City> { cityModel };

            var airportModel = new Airport { Name = airportName };

            var entryModel = new CustomRegionEntry() { Airport = airportModel };
            var entryModels = new List<CustomRegionEntry>() { entryModel };

            var mockCountryRepo = new Mock<ISubRegionRepo<City>>();
            mockCountryRepo.Setup(m => m.GetSubRegions(cityModel)).Returns(entryModels);

            var mockSession = new Mock<ISession>();
            mockSession.Setup(m => m.Query<City>()).Returns(cityModels.AsQueryable());

            var mockSessionManager = new Mock<ISessionManager>();
            mockSessionManager.Setup(m => m.OpenSession()).Returns(mockSession.Object);

            var mockEagerLoader = new Mock<IEagerLoader>();
            mockEagerLoader.Setup(m => m.LoadEntities(stateModel)).Returns(stateModel);
            mockEagerLoader.Setup(m => m.LoadEntities(fakeState)).Returns(fakeState);

            var stateRepo = new StateRepo(mockEagerLoader.Object, mockSessionManager.Object, mockCountryRepo.Object);

            // Act
            var entriesFound = stateRepo.GetSubRegions(fakeState);

            // Assert
            Assert.IsNotNull(entriesFound, "We should have received an empty list.");
            Assert.AreEqual(entriesFound, new List<CustomRegionEntry>());
            mockSessionManager.Verify(m => m.OpenSession(), Times.Once, "We should only call OpenSession once");
            mockSession.Verify(m => m.Query<City>(), Times.Once, "Should have queried the countries once");
            mockEagerLoader.Verify(m => m.LoadEntities(stateModel), Times.Never, "Should return null");
        }
    }
}
