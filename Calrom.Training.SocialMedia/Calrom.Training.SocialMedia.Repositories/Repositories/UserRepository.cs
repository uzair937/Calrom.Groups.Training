using System;
using System.Collections.Generic;
using System.Linq;
using Calrom.Training.SocialMedia.Database.Models;

namespace Calrom.Training.SocialMedia.Database.Repositories
{
    public class UserRepository : IRepository<UserDatabaseModel>
    {
        private static UserRepository userRepository;

        private UserRepository() { }

        private void initialiseBorkers()
        {
            var borkRepository = BorkRepository.GetRepository();
            var userRepository = GetRepository();
            var assignBorks = new List<BorkDatabaseModel>();
            var borkList = borkRepository.List();
            for (int i = 0; i < 5; i++)
            {
                if (i * 2 < 10)
                {
                    borkList.ElementAt(i * 2).UserId = 1;
                    assignBorks.Add(borkList.ElementAt(i * 2));
                }
            }
            var followOne = new List<int> { 1 };
            var followTwo = new List<int> { 2 };
            userRepository.Add(new UserDatabaseModel
            {
                UserId = 1,
                UserName = ("test-user-" + 1),
                Password = ("test-pass-" + 1),
                UserBorks = assignBorks,
                UserPP = "../../images/doggo.jpg",
                FollowingId = followTwo,
                FollowerId = followTwo
            });
            assignBorks = new List<BorkDatabaseModel>();
            for (int i = 0; i < 5; i++)
            {
                if (i * 2 + 1 < 10)
                {
                    borkList.ElementAt(i * 2 + 1).UserId = 2;
                    assignBorks.Add(borkRepository.List().ElementAt(i * 2 + 1));
                }
            }
            userRepository.Add(new UserDatabaseModel
            {
                UserId = 2,
                UserName = ("test-user-" + 2),
                Password = ("test-pass-" + 2),
                UserBorks = assignBorks,
                UserPP = "../../images/doggo.jpg",
                FollowingId = followOne,
                FollowerId = followOne
            });
        }

        private List<UserDatabaseModel> userList = new List<UserDatabaseModel>();

        public void Add(UserDatabaseModel entity)
        {
            userList.Add(entity);
        }

        public void Delete(UserDatabaseModel entity)
        {
            userList.Remove(entity);
        }

        public UserDatabaseModel FindById(int Id)
        {
            var userFound = userList.First(a => a.UserId == Id);
            return userFound;
        }

        public IEnumerable<UserDatabaseModel> List()
        {
            return userList;
        }

        public IEnumerable<UserDatabaseModel> List(int userId)
        {
            var followedUsers = new List<UserDatabaseModel>();
            foreach (var user in userList)    //move logic to .List() send current userId
            {
                if (user.FollowerId.Contains(userId) || userId == user.UserId)
                {
                    followedUsers.Add(user);
                }
            }
            return followedUsers;
        }

        public static UserRepository GetRepository()
        {
            if (userRepository == null)
            {
                userRepository = new UserRepository();
                userRepository.initialiseBorkers();
            }
            return userRepository;
        }


    }
}
