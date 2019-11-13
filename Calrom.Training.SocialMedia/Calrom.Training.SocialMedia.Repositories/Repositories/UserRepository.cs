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
            var currentUser = userList.First(a => a.UserId == userId);
            foreach (var user in userList)    //move logic to .List() send current userId
            {
                if (currentUser.FollowingId.Contains(user.UserId) || userId == user.UserId)
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
            }

            return userRepository;
        }

        public void FollowUser(int currentUserId, int targetUserId)
        {
            if (currentUserId == 0) return;
            var currentUser = userList.First(a => a.UserId == currentUserId);
            var targetUser = userList.First(a => a.UserId == targetUserId);

            var currentUserIndex = userList.IndexOf(currentUser);
            var targetUserIndex = userList.IndexOf(targetUser);

            if (!currentUser.FollowingId.Contains(targetUserId))
            {
                userList.ElementAt(currentUserIndex).FollowingId.Add(targetUserId);
                userList.ElementAt(targetUserIndex).FollowerId.Add(currentUserId);
            }
            else
            {
                userList.ElementAt(currentUserIndex).FollowingId.Remove(targetUserId);
                userList.ElementAt(targetUserIndex).FollowerId.Remove(currentUserId);
            }
        }
    }
}
