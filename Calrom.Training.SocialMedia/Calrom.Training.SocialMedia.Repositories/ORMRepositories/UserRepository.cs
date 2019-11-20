using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Calrom.Training.SocialMedia.Database.NHibernateTools;
using Calrom.Training.SocialMedia.Database.ORMModels;
using Microsoft.AspNetCore.Http;

namespace Calrom.Training.SocialMedia.Database.ORMRepositories
{
    public class UserRepository
    {
        private static UserRepository userRepository;

        private UserRepository() { }

        public void AddOrUpdate(UserModel userModel)
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                session.SaveOrUpdate(userModel);
                session.Flush();
            }
        }

        public void Delete(UserModel userModel)
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                session.Delete(userModel);
                session.Flush();
            }
        }

        public UserModel FindById(int Id)
        {
            var userModel = new UserModel();
            using (var session = NHibernateHelper.OpenSession())
            {
                userModel = session.Get<UserModel>(Id);
            }
            return userModel;
        }

        public IEnumerable<UserModel> List()
        {
            var userList = new List<UserModel>();
            using (var session = NHibernateHelper.OpenSession())
            {
                userList = session.Query<UserModel>().ToList();
            }
            return userList;
        }

        public void AddBork(string borkBoxString, int userId)
        {
            var userModel = new UserModel();
            using (var session = NHibernateHelper.OpenSession())
            {
                userModel = session.Get<UserModel>(userId);
                userModel.UserBorks.Add(new BorkModel
                {
                    BorkText = borkBoxString,
                    DateBorked = DateTime.Now
                });
                session.SaveOrUpdate(userModel);
                session.Flush();
            }
        }

        public IEnumerable<UserModel> GetFollowedUsers(int userId)
        {
            var userModel = new UserModel();
            using (var session = NHibernateHelper.OpenSession())
            {
                userModel = session.Get<UserModel>(userId);
            }
            return userModel.Followers;
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
            var currentUser = new UserModel();
            using (var session = NHibernateHelper.OpenSession())
            {
                currentUser = session.Get<UserModel>(currentUserId);
            }

            var targetUser = new UserModel();
            using (var session = NHibernateHelper.OpenSession())
            {
                targetUser = session.Get<UserModel>(targetUserId);
            }

            if (!currentUser.Following.Contains(targetUser))
            {
                currentUser.Following.Add(targetUser);
                targetUser.Followers.Add(currentUser);

                var type = (NotificationEnum)Enum.Parse(typeof(NotificationEnum), "Follow");
                targetUser.Notifications.Add(NewNotification(type, currentUserId, ""));
            }
            else
            {
                currentUser.Following.Remove(targetUser);
                targetUser.Followers.Remove(currentUser);

                var type = (NotificationEnum)Enum.Parse(typeof(NotificationEnum), "Unfollow");
                targetUser.Notifications.Add(NewNotification(type, currentUserId, ""));
            }

            using (var session = NHibernateHelper.OpenSession())
            {
                session.SaveOrUpdate(currentUser);
                session.SaveOrUpdate(targetUser);
                session.Flush();
            }
        }

        public List<BorkModel> GetSearchBorks(string searchText, int userId)
        {
            var searchedUsers = GetFollowedUsers(userId);
            var foundBorks = new List<BorkModel>();
            var borkList = new List<BorkModel>();
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

        public List<BorkModel> SearchUserBorks(string searchText, int userId)
        {
            var user = new UserModel();
            using (var session = NHibernateHelper.OpenSession())
            {
                user = session.Get<UserModel>(userId);
            }

            var foundBorks = new List<BorkModel>();
            foreach (var bork in user.UserBorks)
            {
                if (bork.BorkText.Contains(searchText))
                {
                    foundBorks.Add(bork);
                }
            }
            return foundBorks;
        }

        public NotificationModel NewNotification(NotificationEnum type, int userId, string likedBork)
        {
            var user = new UserModel();
            using (var session = NHibernateHelper.OpenSession())
            {
                user = session.Get<UserModel>(userId);
            }

            string Text = string.Empty;
            if (type == NotificationEnum.Like)
            {
                Text = user.UserName + " has liked your bork: \n" + likedBork;
            }
            else if (type == NotificationEnum.Follow)
            {
                Text = user.UserName + " has followed you!";
            }
            else if (type == NotificationEnum.Unfollow)
            {
                Text = user.UserName + " has unfollowed you!";
            }

            var notificationModel = new NotificationModel()
            {
                DateCreated = DateTime.Now,
                Type = type,
                Text = Text
            };

            return notificationModel;
        }
    }
}
