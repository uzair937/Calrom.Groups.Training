using CustomRegionEditor.Database.Interfaces;
using CustomRegionEditor.Database.Models;
using CustomRegionEditor.Database.NHibLazyLoader;
using CustomRegionEditor.Database.Repositories;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomRegionEditor.Test.Repositories
{
    [TestFixture]
    public class AirportRepoTests
    {
        [Test]
        public void FindAirportByName() {
            // Arrange
            ISessionFactoryManager factoryManager = new NHibernateSessionFactoryManager();
            ISessionManager sessionManager = new NHibernateSessionManager(factoryManager);
            IEagerLoader eagerLoader = new EagerLoader();

            var airportRepo = new AirportRepo(eagerLoader, sessionManager);
            var entry = "ANAA";

            // Act
            var airportModel = airportRepo.FindByName(entry);

            // Assert
            Assert.IsNotNull(airportModel);
        }
    }
}
