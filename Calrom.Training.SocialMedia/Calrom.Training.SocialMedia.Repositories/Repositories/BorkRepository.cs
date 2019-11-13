using System;
using System.Collections.Generic;
using System.Linq;
using Calrom.Training.SocialMedia.Database.Models;

namespace Calrom.Training.SocialMedia.Database.Repositories
{
    public class BorkRepository : IRepository<BorkDatabaseModel>
    {
        private static BorkRepository borkRepository;

        private BorkRepository() { }

        public List<BorkDatabaseModel> borks = new List<BorkDatabaseModel>();

        public void Add(BorkDatabaseModel entity)
        {
            borks.Add(entity);
        }

        public void Delete(BorkDatabaseModel entity)
        {
            borks.Remove(entity);
        }

        public BorkDatabaseModel FindById(int Id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<BorkDatabaseModel> List()
        {
            borks = borks.OrderByDescending(a => a.DateBorked).ToList();
            return borks;
        }

        public IEnumerable<BorkDatabaseModel> List(int userId)
        {
            var userRepo = UserRepository.GetRepository();
            var followedUsers = userRepo.List(userId);
            var followedBorks = new List<BorkDatabaseModel>();
            if (followedUsers != null)
            {
                foreach (var user in followedUsers)
                {
                    if (user.UserBorks != null)
                    {
                        followedBorks = followedBorks.Concat(user.UserBorks).ToList();
                    }
                }
                followedBorks = followedBorks.OrderByDescending(a => a.DateBorked).ToList();
            }
            return followedBorks;
        }

        public static BorkRepository GetRepository()
        {
            if (borkRepository == null)
            {
                borkRepository = new BorkRepository();
            }

            return borkRepository;
        }
    }
}
