using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace Calrom.Training.AuctionHouse.Database
{
    public class UserRepo : IRepository<UserDatabaseModel>
    {
        private static int ids = 0;
        private static UserRepo Instance = null;
        private static readonly object padlock = new object();
        public static UserRepo getInstance
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
            var result = BitConverter.ToInt32(salt,0) & int.MaxValue;
            return result;
        }

        private UserDatabaseModel _userContext;
        public UserRepo()
        {
            _userContext = new UserDatabaseModel();
            _userContext.UserList = new List<UserDatabaseModel>();
        }
        public void Add(UserDatabaseModel userDatabaseModel)
        {
            if (userDatabaseModel.UserID == 0)
            {
                userDatabaseModel.UserID = GetRandom();
            }
            _userContext.UserList.Add(userDatabaseModel);
        }

        public List<UserDatabaseModel> List()
        {
            return _userContext.UserList;
        }
    }
}
