using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Calrom.Training.SocialMedia.Database.Models;
using Microsoft.AspNetCore.Http;

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

        public void AddBork(string borkBoxString, int userId)
        {
            var borkRepository = BorkRepository.GetRepository();
            var userList = userRepository.List();
            var currentUserDb = userList.First(a => a.UserId == userId);
            currentUserDb.UserBorks.Add(new BorkDatabaseModel
            {
                BorkText = borkBoxString,
                DateBorked = DateTime.Now,
                UserId = userId
            });
            currentUserDb.UserBorks = currentUserDb.UserBorks.OrderByDescending(a => a.DateBorked).ToList();
            borkRepository.Add(currentUserDb.UserBorks.First());
        }

        public IEnumerable<UserDatabaseModel> GetFollowedUsers(int userId)
        {
            var followedUsers = new List<UserDatabaseModel>();
            var currentUser = userList.First(a => a.UserId == userId);
            foreach (var user in userList)
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
                var type = (NotificationType)Enum.Parse(typeof(NotificationType), "Follow");
                userList.ElementAt(targetUserIndex).Notifications.Add(new NotificationDatabaseModel(type, currentUserId, ""));
            }
            else
            {
                userList.ElementAt(currentUserIndex).FollowingId.Remove(targetUserId);
                userList.ElementAt(targetUserIndex).FollowerId.Remove(currentUserId);
                var type = (NotificationType)Enum.Parse(typeof(NotificationType), "Unfollow");
                userList.ElementAt(targetUserIndex).Notifications.Add(new NotificationDatabaseModel(type, currentUserId, ""));
            }
        }

        public List<BorkDatabaseModel> GetSearchBorks(string searchText, int userId)
        {
            var searchedUsers = GetFollowedUsers(userId);
            var foundBorks = new List<BorkDatabaseModel>();
            var borkList = new List<BorkDatabaseModel>();
            foreach (var user in searchedUsers)
            {
                borkList = borkList.Concat(user.UserBorks).ToList();
            }
            foreach (var bork in borkList)
            {
                if (bork.BorkText.Contains(searchText))
                {
                    foundBorks.Add(bork);
                }
            }
            return foundBorks;
        }

        public List<BorkDatabaseModel> SearchUserBorks(string searchText, int userId)
        {
            var user = userList.FirstOrDefault(a => a.UserId == userId);
            var foundBorks = new List<BorkDatabaseModel>();
            foreach (var bork in user.UserBorks)
            {
                if (bork.BorkText.Contains(searchText))
                {
                    foundBorks.Add(bork);
                }
            }
            return foundBorks;
        }
    }
}
