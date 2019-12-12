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
    public class CustomRegionGroupRepoTests
    {
        [Test]
        public void Given_AValidSearch_Then_GetSearchResults_Should_ReturnCustomRegionGroupList() 
        {
            // Arrange
            const string stateName = "Texas";
            const string filter = "state";
            var stateModel = new StateModel { StateName = stateName };
            var customRegionEntryModel = new CustomRegionEntryModel()
            {
                State = stateModel
            };
            var customRegionGroupModel = new CustomRegionGroupModel()
            {
                CustomRegionEntries = new List<CustomRegionEntryModel>() {customRegionEntryModel}
            };
            var models = new List<CustomRegionGroupModel> { customRegionGroupModel };
            
            var mockSession = new Mock<ISession>();
            mockSession.Setup(m => m.Query<CustomRegionGroupModel>()).Returns(models.AsQueryable());

            var mockSessionManager = new Mock<ISessionManager>();
            mockSessionManager.Setup(m => m.OpenSession()).Returns(mockSession.Object);

            var mockEagerLoader = new Mock<IEagerLoader>(MockBehavior.Strict);
            mockEagerLoader.SetupSequence(m => m.LoadEntities(It.IsAny<List<CustomRegionGroupModel>>()))
                .Returns(models)
                .Returns(new List<CustomRegionGroupModel>());

            var mockEntryRepository = new Mock<ICustomRegionEntryRepository>();
            var mockAirportRepo = new Mock<ISubRegionRepo<AirportModel>>();
            var mockCityRepo = new Mock<ISubRegionRepo<CityModel>>();
            var mockCountryRepo = new Mock<ISubRegionRepo<CountryModel>>();
            var mockRegionRepo = new Mock<ISubRegionRepo<RegionModel>>();
            var mockStateRepo = new Mock<ISubRegionRepo<StateModel>>();
            mockStateRepo.Setup(m => m.FindByName(stateModel.StateName));

            var customRegionGroupRepo = new CustomRegionGroupRepo(mockEagerLoader.Object, mockSessionManager.Object,
                mockEntryRepository.Object, mockAirportRepo.Object, mockCityRepo.Object, mockStateRepo.Object,
                mockCountryRepo.Object, mockRegionRepo.Object);
            // Act
            var resultsFound = customRegionGroupRepo.GetSearchResults(stateName, filter);

            // Assert
            Assert.IsNotNull(resultsFound, "Results should have been received. Instead null was received.");
            //Assert.AreEqual(cityName, cityFound.CityName, "The found city name should match");
            mockSessionManager.Verify(m => m.OpenSession(), Times.Once, "We should only call OpenSession Once");
            mockSession.Verify(m => m.Query<CustomRegionGroupModel>(), Times.Exactly(2), "Should have queried the models twice");
            mockEagerLoader.Verify(m => m.LoadEntities(models), Times.Once, "Should have called load entities with the provided city");
            mockEagerLoader.Verify(m => m.LoadEntities(new List<CustomRegionGroupModel>()), Times.Once, "Should have called load entities with the provided city");
        }

        [Test]
        public void Given_InvalidSearch_Then_GetSearchResults_Should_ReturnEmptyLists() 
        {
            // Arrange
            const string stateName = "Blank";
            const string filter = "state";
            var stateModel = new StateModel { StateName = stateName };
            var customRegionEntryModel = new CustomRegionEntryModel()
            {
                State = stateModel
            };
            var customRegionGroupModel = new CustomRegionGroupModel()
            {
                CustomRegionEntries = new List<CustomRegionEntryModel>() {customRegionEntryModel}
            };
            var models = new List<CustomRegionGroupModel> { customRegionGroupModel };
            
            var mockSession = new Mock<ISession>();
            mockSession.Setup(m => m.Query<CustomRegionGroupModel>()).Returns(models.AsQueryable());

            var mockSessionManager = new Mock<ISessionManager>();
            mockSessionManager.Setup(m => m.OpenSession()).Returns(mockSession.Object);

            var mockEagerLoader = new Mock<IEagerLoader>(MockBehavior.Strict);
            mockEagerLoader.SetupSequence(m => m.LoadEntities(It.IsAny<List<CustomRegionGroupModel>>()))
                .Returns(models)
                .Returns(new List<CustomRegionGroupModel>());

            var mockEntryRepository = new Mock<ICustomRegionEntryRepository>();
            var mockAirportRepo = new Mock<ISubRegionRepo<AirportModel>>();
            var mockCityRepo = new Mock<ISubRegionRepo<CityModel>>();
            var mockCountryRepo = new Mock<ISubRegionRepo<CountryModel>>();
            var mockRegionRepo = new Mock<ISubRegionRepo<RegionModel>>();
            var mockStateRepo = new Mock<ISubRegionRepo<StateModel>>();

            var customRegionGroupRepo = new CustomRegionGroupRepo(mockEagerLoader.Object, mockSessionManager.Object,
                mockEntryRepository.Object, mockAirportRepo.Object, mockCityRepo.Object, mockStateRepo.Object,
                mockCountryRepo.Object, mockRegionRepo.Object);
            // Act
            var resultsFound = customRegionGroupRepo.GetSearchResults(stateName, filter);

            // Assert
            Assert.IsNotNull(resultsFound, "Results should not have been received. Needs to be null.");
            mockSessionManager.Verify(m => m.OpenSession(), Times.Once, "We should only call OpenSession Once");
            mockSession.Verify(m => m.Query<CustomRegionGroupModel>(), Times.Exactly(2), "Should have queried the models twice");
            mockEagerLoader.Verify(m => m.LoadEntities(resultsFound), Times.Once, "Should be empty");
            mockEagerLoader.Verify(m => m.LoadEntities(new List<CustomRegionGroupModel>()), Times.Once, "Should be empty");
        }
    }
}
