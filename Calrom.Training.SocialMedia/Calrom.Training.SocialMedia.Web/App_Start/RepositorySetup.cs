using Calrom.Training.SocialMedia.Database.NHibernateTools;
using Calrom.Training.SocialMedia.Database.ORMModels;
using Calrom.Training.SocialMedia.Database.ORMRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Calrom.Training.SocialMedia.Web
{
    public class RepositorySetup
    {
        public void Initialise()
        {
            IRepository<UserModel> repo = UserRepository.GetRepository();

            InitialiseBorkers(repo);
        }

        private void InitialiseBorkers(IRepository<UserModel> repo)
        {
            var userOne = new UserModel
            {
                UserName = ("test-user-" + 1),
                Password = ("test-pass-" + 1),
                UserPP = "../../images/doggo.jpg"
            };
            var userTwo = new UserModel
            {
                UserName = ("test-user-" + 2),
                Password = ("test-pass-" + 2),
                UserPP = "../../images/user-2.jpg"
            };

            repo.AddOrUpdate(userOne);
            repo.AddOrUpdate(userTwo);

            var followerModel = new FollowerModel
            {
                UserId = 1,
            };
            followerModel.SetFollower(userTwo);
            userOne.AddFollower(followerModel);

            followerModel = new FollowerModel
            {
                UserId = 2,
            };
            followerModel.SetFollower(userOne);
            userTwo.AddFollower(followerModel);
            
            var followingModel = new FollowingModel
            {
                UserId = 1,
            };
            followingModel.SetFollowing(userTwo);
            userOne.AddFollowing(followingModel);

            followingModel = new FollowingModel
            {
                UserId = 2,
            };
            followingModel.SetFollowing(userOne);
            userTwo.AddFollowing(followingModel);

            repo.AddOrUpdate(userOne);
            repo.AddOrUpdate(userTwo);

            for (int x = 0; x < 10; x++)
            {
                var y = x % 2;

                if (y == 0)
                {
                    var newBork = new BorkModel
                    {
                        BorkText = "Bork! This is an example bork",
                        DateBorked = DateTime.Now.AddYears(-x * 100),
                    };
                    newBork.AddUser(userOne);
                    userOne.AddBorkToUser(newBork);
                }
                else if (y == 1)
                {
                    var newBork = new BorkModel
                    {
                        BorkText = "Bork! This is an example bork",
                        DateBorked = DateTime.Now.AddYears(-x * 100),
                    };
                    newBork.AddUser(userTwo);
                    userTwo.AddBorkToUser(newBork);
                }
                repo.AddOrUpdate(userOne);
                repo.AddOrUpdate(userTwo);
            }
        }
    }
}
