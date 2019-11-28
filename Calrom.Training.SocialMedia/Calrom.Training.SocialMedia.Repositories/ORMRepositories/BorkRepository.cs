using System;
using System.Collections.Generic;
using System.Linq;
using Calrom.Training.SocialMedia.Database.NHibernateTools;
using Calrom.Training.SocialMedia.Database.ORMModels;

namespace Calrom.Training.SocialMedia.Database.ORMRepositories
{
    public class BorkRepository : IRepository<BorkModel>
    {
        private static BorkRepository borkRepository;
        private BorkRepository() { }

        private List<BorkModel> CleanseReturn(List<BorkModel> borks)
        {
            if (borks == null) return null;

            var x = 0;
            var repo = UserRepository.GetRepository();
            var newBorks = borks;
            foreach (var bork in borks)
            {
                newBorks[x++].UserModel = repo.CleanseReturn(bork.UserModel);
            }
            return newBorks;
        }

        public List<BorkModel> borks = new List<BorkModel>();

        public void AddOrUpdate(BorkModel borkModel)
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                session.SaveOrUpdate(borkModel);
                session.Flush();
            }
        }

        public void Delete(BorkModel borkModel)
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                session.Delete(borkModel);
                session.Flush();
            }
        }

        public BorkModel FindById(int Id)
        {
            var borkModel = new BorkModel();
            using (var session = NHibernateHelper.OpenSession())
            {
                borkModel = session.Get<BorkModel>(Id);
            }
            return borkModel;
        }

        public IEnumerable<BorkModel> List()
        {
            var borkList = new List<BorkModel>();
            using (var session = NHibernateHelper.OpenSession())
            {
                borkList = CleanseReturn(session.Query<BorkModel>().ToList());
            }
            return borkList;
        }

        public IEnumerable<BorkModel> GetFollowedBorks(int userId)
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
