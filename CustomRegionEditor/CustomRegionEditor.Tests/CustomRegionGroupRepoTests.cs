﻿using CustomRegionEditor.Database.Interfaces;
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
                CustomRegionEntries = new List<CustomRegionEntryModel>() { customRegionEntryModel }
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
                CustomRegionEntries = new List<CustomRegionEntryModel>() { customRegionEntryModel }
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

        [Test]
        public void Given_UnfilteredSearch_Then_GetSearchResults_Should_ReturnResults()
        {
            // Arrange
            const string searchTerm = "-All";
            var customRegionEntryModel = new CustomRegionEntryModel();
            var customRegionGroupModel = new CustomRegionGroupModel()
            {
                CustomRegionEntries = new List<CustomRegionEntryModel>() { customRegionEntryModel }
            };
            var models = new List<CustomRegionGroupModel> { customRegionGroupModel };

            var mockSession = new Mock<ISession>();
            mockSession.Setup(m => m.Query<CustomRegionGroupModel>()).Returns(models.AsQueryable());

            var mockSessionManager = new Mock<ISessionManager>();
            mockSessionManager.Setup(m => m.OpenSession()).Returns(mockSession.Object);

            var mockEagerLoader = new Mock<IEagerLoader>(MockBehavior.Strict);
            mockEagerLoader.Setup(m => m.LoadEntities(It.IsAny<List<CustomRegionGroupModel>>())).Returns(models);

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
            var resultsFound = customRegionGroupRepo.GetSearchResults(searchTerm, null);

            // Assert
            Assert.IsNotNull(resultsFound, "Results should have been received. Cannot be empty.");
            mockSessionManager.Verify(m => m.OpenSession(), Times.Once, "We should only call OpenSession once");
            mockSession.Verify(m => m.Query<CustomRegionGroupModel>(), Times.Once, "Cannot query more than once");
            mockEagerLoader.Verify(m => m.LoadEntities(resultsFound), Times.Once, "Should be populated");
        }


        [Test]
        public void Given_GroupEntity_Then_AddOrUpdate_Should_OpenASessionAndSave()
        {
            // Arrange
            var mockSessionManager = new Mock<ISessionManager>();
            var mockEagerLoader = new Mock<IEagerLoader>();
            var mockEntryRepository = new Mock<ICustomRegionEntryRepository>();
            var mockAirportRepo = new Mock<ISubRegionRepo<AirportModel>>();
            var mockCityRepo = new Mock<ISubRegionRepo<CityModel>>();
            var mockCountryRepo = new Mock<ISubRegionRepo<CountryModel>>();
            var mockRegionRepo = new Mock<ISubRegionRepo<RegionModel>>();
            var mockStateRepo = new Mock<ISubRegionRepo<StateModel>>();

            var customRegionGroupRepo = new CustomRegionGroupRepo(mockEagerLoader.Object, mockSessionManager.Object,
                mockEntryRepository.Object, mockAirportRepo.Object, mockCityRepo.Object, mockStateRepo.Object,
                mockCountryRepo.Object, mockRegionRepo.Object);

            var mockSession = new Mock<ISession>();
            mockSessionManager.Setup(m => m.OpenSession()).Returns(mockSession.Object);

            var customRegionEntryModel = new CustomRegionEntryModel();
            var customRegionGroupModel = new CustomRegionGroupModel()
            {
                CustomRegionEntries = new List<CustomRegionEntryModel>() { customRegionEntryModel }
            };

            //Act
            customRegionGroupRepo.AddOrUpdate(customRegionGroupModel);

            //Assert

            mockSessionManager.Verify(m => m.OpenSession(), Times.Once, "We should only call OpenSession once");
            mockSession.Verify(m => m.SaveOrUpdate(customRegionGroupModel), Times.Once, "Should only save or update once");
            mockSession.Verify(m => m.Flush(), Times.Once, "Should flush");
        }
        
        [Test]
        public void Given_GroupEntity_Then_Delete_Should_OpenASessionAndDelete()
        {
            // Arrange
            var mockSessionManager = new Mock<ISessionManager>();
            var mockEagerLoader = new Mock<IEagerLoader>();
            var mockEntryRepository = new Mock<ICustomRegionEntryRepository>();
            var mockAirportRepo = new Mock<ISubRegionRepo<AirportModel>>();
            var mockCityRepo = new Mock<ISubRegionRepo<CityModel>>();
            var mockCountryRepo = new Mock<ISubRegionRepo<CountryModel>>();
            var mockRegionRepo = new Mock<ISubRegionRepo<RegionModel>>();
            var mockStateRepo = new Mock<ISubRegionRepo<StateModel>>();

            var customRegionGroupRepo = new CustomRegionGroupRepo(mockEagerLoader.Object, mockSessionManager.Object,
                mockEntryRepository.Object, mockAirportRepo.Object, mockCityRepo.Object, mockStateRepo.Object,
                mockCountryRepo.Object, mockRegionRepo.Object);

            var mockSession = new Mock<ISession>();
            mockSessionManager.Setup(m => m.OpenSession()).Returns(mockSession.Object);

            var customRegionEntryModel = new CustomRegionEntryModel();
            var customRegionGroupModel = new CustomRegionGroupModel()
            {
                CustomRegionEntries = new List<CustomRegionEntryModel>() { customRegionEntryModel }
            };

            //Act
            customRegionGroupRepo.Delete(customRegionGroupModel);

            //Assert

            mockSessionManager.Verify(m => m.OpenSession(), Times.Once, "We should only call OpenSession once");
            mockSession.Verify(m => m.Delete(customRegionGroupModel), Times.Once, "Should only save or update once");
            mockSession.Verify(m => m.Flush(), Times.Once, "Should flush");
        }
        
        [Test]
        public void Given_ListCalled_Then_List_Should_ReturnAListOfGroupRegions()
        {
            // Arrange
            var mockSessionManager = new Mock<ISessionManager>();
            var mockEagerLoader = new Mock<IEagerLoader>();
            var mockEntryRepository = new Mock<ICustomRegionEntryRepository>();
            var mockAirportRepo = new Mock<ISubRegionRepo<AirportModel>>();
            var mockCityRepo = new Mock<ISubRegionRepo<CityModel>>();
            var mockCountryRepo = new Mock<ISubRegionRepo<CountryModel>>();
            var mockRegionRepo = new Mock<ISubRegionRepo<RegionModel>>();
            var mockStateRepo = new Mock<ISubRegionRepo<StateModel>>();

            var customRegionGroupRepo = new CustomRegionGroupRepo(mockEagerLoader.Object, mockSessionManager.Object,
                mockEntryRepository.Object, mockAirportRepo.Object, mockCityRepo.Object, mockStateRepo.Object,
                mockCountryRepo.Object, mockRegionRepo.Object);

            var mockSession = new Mock<ISession>();
            mockSessionManager.Setup(m => m.OpenSession()).Returns(mockSession.Object);

            var customRegionEntryModel = new CustomRegionEntryModel();
            var customRegionGroupModel = new CustomRegionGroupModel()
            {
                CustomRegionEntries = new List<CustomRegionEntryModel>() { customRegionEntryModel }
            };
            var regionList = new List<CustomRegionGroupModel>() { customRegionGroupModel };

            mockSession.Setup(m => m.Query<CustomRegionGroupModel>()).Returns(regionList.AsQueryable());
            //Act
            customRegionGroupRepo.Delete(customRegionGroupModel);

            //Assert

            mockSessionManager.Verify(m => m.OpenSession(), Times.Once, "We should only call OpenSession once");
            mockSession.Verify(m => m.Delete(customRegionGroupModel), Times.Once, "Should only save or update once");
            mockSession.Verify(m => m.Flush(), Times.Once, "Should flush");
        }
    }
}
