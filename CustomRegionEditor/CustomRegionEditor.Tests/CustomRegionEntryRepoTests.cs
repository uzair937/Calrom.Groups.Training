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
    public class CustomRegionEntryRepoTests
    {
        [Test]
        public void Given_CustomRegionEntry_Then_AddOrUpdate_Should_OpenASessionAndSave()
        {
            // Arrange
            var mockSessionManager = new Mock<ISessionManager>();
            var mockEagerLoader = new Mock<IEagerLoader>();
            var mockEntryRepository = new Mock<ICustomRegionEntryRepository>();

            var mockSession = new Mock<ISession>();
            mockSessionManager.Setup(m => m.OpenSession()).Returns(mockSession.Object);

            var customRegionEntryRepo = new CustomRegionEntryRepo(mockEagerLoader.Object, mockSessionManager.Object);

            var customRegionEntryModel = new CustomRegionEntry();

            //Act
            customRegionEntryRepo.AddOrUpdate(customRegionEntryModel);

            //Assert

            mockSessionManager.Verify(m => m.OpenSession(), Times.Once, "We should only call OpenSession once");
            mockSession.Verify(m => m.SaveOrUpdate(customRegionEntryModel), Times.Once, "Should only save or update once");
            mockSession.Verify(m => m.Flush(), Times.Once, "Should flush");
        }
        
        [Test]
        public void Given_CustomRegionEntry_Then_Delete_Should_OpenASessionAndRemove()
        {
            // Arrange
            var mockSessionManager = new Mock<ISessionManager>();
            var mockEagerLoader = new Mock<IEagerLoader>();
            var mockEntryRepository = new Mock<ICustomRegionEntryRepository>();

            var id = new Guid();
            var customRegionEntryModel = new CustomRegionEntry()
            {
                Id = id
            };
            var mockSession = new Mock<ISession>();
            mockSessionManager.Setup(m => m.OpenSession()).Returns(mockSession.Object);
            mockSession.Setup(m => m.Get<CustomRegionEntry>(id)).Returns(customRegionEntryModel);

            var customRegionEntryRepo = new CustomRegionEntryRepo(mockEagerLoader.Object, mockSessionManager.Object);

            //Act
            customRegionEntryRepo.Delete(customRegionEntryModel);

            //Assert
            mockSessionManager.Verify(m => m.OpenSession(), Times.Once, "We should only call OpenSession once");

            mockSession.Verify(m => m.Delete(customRegionEntryModel), Times.Once, "Should only save or update once");
            mockSession.Verify(m => m.Flush(), Times.Once, "Should flush");
        }
        
        [Test]
        public void Given_CustomRegionEntryList_Then_Delete_Should_OpenASessionAndRemoveAllObjectsFromList()
        {
            // Arrange
            var mockSessionManager = new Mock<ISessionManager>();
            var mockEagerLoader = new Mock<IEagerLoader>();
            var mockEntryRepository = new Mock<ICustomRegionEntryRepository>();

            var id = new Guid();
            var customRegionEntryModel = new CustomRegionEntry()
            {
                Id = id
            };
            var mockSession = new Mock<ISession>();
            mockSessionManager.Setup(m => m.OpenSession()).Returns(mockSession.Object);
            mockSession.Setup(m => m.Get<CustomRegionEntry>(id)).Returns(customRegionEntryModel);
            mockEntryRepository.Setup(m => m.Delete(customRegionEntryModel));

            var customRegionEntryRepo = new CustomRegionEntryRepo(mockEagerLoader.Object, mockSessionManager.Object);
            var customRegionEntryList = new List<CustomRegionEntry>() {customRegionEntryModel};

            //Act
            customRegionEntryRepo.Delete(customRegionEntryList);

            //Assert
            mockSessionManager.Verify(m => m.OpenSession(), Times.Once, "We should only call OpenSession once");
            mockSession.Verify(m => m.Delete(customRegionEntryModel), Times.Once, "Should only save or update once");
            //mockEntryRepository.Verify(m => m.Delete(customRegionEntryList), "Should delete items from list");
            mockSession.Verify(m => m.Flush(), Times.Once, "Should flush");
        }
    }
}
