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

        private UserModel cleanseReturn(UserModel user)
        {
            var x = 0;
            var newUser = user;
            if (user.Followers.Count == 0) newUser.Followers = new List<FollowerModel>();
            if (user.Following.Count == 0) newUser.Following = new List<FollowingModel>();
            if (user.Notifications.Count == 0) newUser.Notifications = new List<NotificationModel>();
            if (user.UserBorks.Count == 0) newUser.UserBorks = new List<BorkModel>();

            foreach (var bork in user.UserBorks)
            {
                newUser.UserBorks[x++].UserModel = newUser;
            }
            return newUser;
        }

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
            using (var session = NHibernateHelper.OpenSession())
            {
                var userModel = session.Get<UserModel>(Id);
                return cleanseReturn(userModel);
            }
        }

        public IEnumerable<UserModel> List()
        {
            var newList = new List<UserModel>();
            using (var session = NHibernateHelper.OpenSession())
            {
                var userList = session.Query<UserModel>().ToList();
                foreach (var user in userList)
                {
                    newList.Add(cleanseReturn(user));
                }
            }
            return newList;
        }

        public void AddBork(string borkBoxString, int userId)
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                var userModel = new UserModel();
                userModel = session.Get<UserModel>(userId);
                userModel.AddBorkToUser(new BorkModel
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
            var newUsers = new List<UserModel>();
            using (var session = NHibernateHelper.OpenSession())
            {
                var userModel = session.Get<UserModel>(userId);
                foreach (var following in userModel.Following)
                {
                    newUsers.Add(cleanseReturn(session.Get<UserModel>(following.FollowingId)));
                }
                newUsers.Add(cleanseReturn(userModel));
            }
            return newUsers;
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
            using (var session = NHibernateHelper.OpenSession())
            {
                var currentUser = session.Get<UserModel>(currentUserId);
                var targetUser = session.Get<UserModel>(targetUserId);


                if (!currentUser.Following.Select(a => a.FollowingId).ToList().Contains(targetUser.UserId))
                {
                    currentUser.AddFollowing(targetUser);
                    targetUser.AddFollower(currentUser);

                    var type = (NotificationEnum)Enum.Parse(typeof(NotificationEnum), "Follow");
                    targetUser.AddNotification(NewNotification(type, currentUserId, ""));
                }
                else
                {
                    currentUser.RemoveFollowing(targetUser);
                    targetUser.RemoveFollower(currentUser);

                    var type = (NotificationEnum)Enum.Parse(typeof(NotificationEnum), "Unfollow");
                    targetUser.AddNotification(NewNotification(type, currentUserId, ""));
                }
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
                user = cleanseReturn(session.Get<UserModel>(userId));
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
