using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace Calrom.Training.AuctionHouse.Database
{
    public class UserRepo : IRepository<UserModel>
    {
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

        //private static int GetRandom()
        //{
        //    var rng = RandomNumberGenerator.Create();
        //    var salt = new byte[4];
        //    rng.GetBytes(salt);
        //    var result = BitConverter.ToInt32(salt, 0) & int.MaxValue;
        //    return result;
        //}

        private List<UserModel> _userContext;
        public UserRepo()
        {
            _userContext = new List<UserModel>();
        }
        public void Add(UserModel entity)
        {
            using (var dbSession = NHibernateHelper.OpenSession()) //single responsibilty
            {
                dbSession.SaveOrUpdate(entity);
                dbSession.Flush();
            }
        }

        public List<UserModel> List()
        {
            using (var dbSession = NHibernateHelper.OpenSession())
            {
                _userContext = dbSession.Query<UserModel>().ToList();
            }
            return _userContext;
        }
    }
}
