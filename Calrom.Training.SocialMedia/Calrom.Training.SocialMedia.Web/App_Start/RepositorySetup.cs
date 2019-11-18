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

            InitialiseBorkers(userRepo);
            InitialiseBorkRepo(userRepo, borkRepo);
        }

        private void InitialiseBorkers(UserRepository userRepository)
        {
            userRepository.Add(new UserDatabaseModel
            {
                UserId = 1,
                UserName = ("test-user-" + 1),
                Password = ("test-pass-" + 1),
                UserBorks = new List<BorkDatabaseModel>(),
                UserPP = "../../images/doggo.jpg",
                FollowingId = new List<int> { 2 },
                FollowerId = new List<int> { 2 },
                Notifications = new List<NotificationDatabaseModel>()
            });
            userRepository.Add(new UserDatabaseModel
            {
                UserId = 2,
                UserName = ("test-user-" + 2),
                Password = ("test-pass-" + 2),
                UserBorks = new List<BorkDatabaseModel>(),
                UserPP = "../../images/user-2.jpg",
                FollowingId = new List<int> { 1 },
                FollowerId = new List<int> { 1 },
                Notifications = new List<NotificationDatabaseModel>()
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
