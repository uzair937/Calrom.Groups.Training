using AutoMapper;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Calrom.Training.AuctionHouse.EntityMapper
{
    /// <summary>
    /// AutoMapper Configuration
    /// </summary>
    public class AutoMapperConfiguration
    {
        private static MapperConfiguration mapperConfiguration;

        /// <summary>
        /// Configures this instance.
        /// </summary>
        public static void Configure()
        {
            foreach (var item in GetProfiles())
            {
                mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfiles(GetProfiles()));
            }
        }

        public static T GetInstance<T>(object entity) where T : class
        {
            var mapper = new Mapper(mapperConfiguration);
            return mapper.Map<T>(entity) as T;
        }

        /// <summary>
        /// Gets the profiles.
        /// </summary>
        /// <returns>return profiles</returns>
        private static IEnumerable<Profile> GetProfiles()
        {
            foreach (Type type in Assembly.GetExecutingAssembly().GetTypes())
            {
                if (!type.IsAbstract && typeof(Profile).IsAssignableFrom(type) && !type.IsGenericType)
                {
                    yield return (Profile)Activator.CreateInstance(type);
                }
            }
        }
    }
}
