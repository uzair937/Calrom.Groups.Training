using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace Calrom.Training.AuctionHouse.Database
{
    public class UserRepo : IRepository<UserDatabaseModel>
    {
        private static DataConverter DataInstance { get { return DataConverter.GetInstance; } }
        private static UserRepo Instance = null;
        private static readonly object padlock = new object();
        public static UserRepo GetInstance
        {
            get
            {
                if (Instance == null)
                {
                    lock (padlock)
                    {
                        if (Instance == null)
                        {
                            Instance = new UserRepo();
                        }
                    }
                }
                return Instance;
            }
        }

        private static int GetRandom()
        {
            var rng = RandomNumberGenerator.Create();
            var salt = new byte[4];
            rng.GetBytes(salt);
            var result = BitConverter.ToInt32(salt, 0) & int.MaxValue;
            return result;
        }

        private List<UserDatabaseModel> _userContext;
        public UserRepo()
        {
            _userContext = new List<UserDatabaseModel>();
        }
        public void Add(UserDatabaseModel entity)
        {
            if (entity.UserID == 0)
            {
                entity.UserID = GetRandom();
            }
            DataInstance.ConvertUser(entity);
            _userContext.Add(entity);
        }

        public List<UserDatabaseModel> List()
        {
            return _userContext;
        }
    }
}
