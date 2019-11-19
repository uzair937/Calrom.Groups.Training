using System;
using System.Collections.Generic;
using System.Linq;
using Calrom.Training.SocialMedia.Database.Models;

namespace Calrom.Training.SocialMedia.Database.Repositories
{
    public class BorkRepository : IRepository<BorkModel>
    {
        private static BorkRepository borkRepository;

        private BorkRepository() { }

        public List<BorkModel> borks = new List<BorkModel>();

        public void Add(BorkModel entity)
        {
            borks.Add(entity);
        }

        public void Delete(BorkModel entity)
        {
            borks.Remove(entity);
        }

        public BorkModel FindById(int Id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<BorkModel> List()
        {
            borks = borks.OrderByDescending(a => a.DateBorked).ToList();
            return borks;
        }

        public IEnumerable<BorkModel> GetFollowedUsers(int userId)
        {
            var userRepo = UserRepository.GetRepository();
            var followedUsers = userRepo.GetFollowedUsers(userId);
            var followedBorks = new List<BorkModel>();
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
