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

        private void Initialise()
        {
            var borkRepository = GetRepository();
            for (int x = 0; x < 10; x++)
            {
                var y = x % 2;
                borkRepository.Add(new BorkDatabaseModel
                {
                    BorkText = "Bork! This is an example bork",
                    DateBorked = DateTime.Now.AddYears(-x * 100),
                    UserId = y + 1
                });
            }
        }

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
            foreach (var user in followedUsers) followedBorks = followedBorks.Concat(user.UserBorks).ToList();
            followedBorks = followedBorks.OrderByDescending(a => a.DateBorked).ToList();
            return followedBorks;
        }

        public static BorkRepository GetRepository()
        {
            if (borkRepository == null)
            {
                borkRepository = new BorkRepository();
                borkRepository.Initialise();
            }
            return borkRepository;
        }


    }
}
