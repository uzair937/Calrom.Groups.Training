using Calrom.Training.SocialMedia.Database.Models;
using Calrom.Training.SocialMedia.Database.Repositories;
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
            var userRepo = UserRepository.GetRepository();
            var borkRepo = BorkRepository.GetRepository();

            InitialiseBorkers(userRepo, borkRepo);
            InitialiseBorkRepo(userRepo, borkRepo);
        }

        private void InitialiseBorkers(UserRepository userRepository, BorkRepository borkRepository)
        {
            var assignBorks = new List<BorkDatabaseModel>();
            var followOne = new List<int> { 1 };
            var followTwo = new List<int> { 2 };
            var followedOne = new List<int> { 1 };
            var followedTwo = new List<int> { 2 };
            var notifOne = new List<NotificationDatabaseModel>();
            var notifTwo = new List<NotificationDatabaseModel>();
            userRepository.Add(new UserDatabaseModel
            {
                UserId = 1,
                UserName = ("test-user-" + 1),
                Password = ("test-pass-" + 1),
                UserBorks = assignBorks,
                UserPP = "../../images/doggo.jpg",
                FollowingId = followTwo,
                FollowerId = followedTwo,
                Notifications = notifOne
            });
            var assignTwoBorks = new List<BorkDatabaseModel>();
            userRepository.Add(new UserDatabaseModel
            {
                UserId = 2,
                UserName = ("test-user-" + 2),
                Password = ("test-pass-" + 2),
                UserBorks = assignTwoBorks,
                UserPP = "../../images/user-2.jpg",
                FollowingId = followOne,
                FollowerId = followedOne,
                Notifications = notifTwo
            });

            var userList = userRepository.List();
            for (int x = 0; x < 10; x++)
            {
                var y = x % 2;
                userList.ElementAt(y).UserBorks.Add(new BorkDatabaseModel
                {
                    BorkText = "Bork! This is an example bork",
                    DateBorked = DateTime.Now.AddYears(-x * 100),
                    UserId = userList.ElementAt(y).UserId
                });
            }
        }

        private void InitialiseBorkRepo(UserRepository userRepository, BorkRepository borkRepository)
        {
            var userList = userRepository.List();
            for (int x = 0; x < 5; x++) borkRepository.Add(userList.ElementAt(0).UserBorks[x]);
            for (int x = 0; x < 5; x++) borkRepository.Add(userList.ElementAt(1).UserBorks[x]);
        }
    }
}
