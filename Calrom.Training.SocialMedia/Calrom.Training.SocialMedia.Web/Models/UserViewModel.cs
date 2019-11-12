using Calrom.Training.SocialMedia.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Calrom.Training.SocialMedia.Web.Models
{
    public class UserViewModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public List<BorkViewModel> UserBorks { get; set; }
        public string UserPP { get; set; }
        public List<int> FollowingId { get; set; }
        public List<int> FollowerId { get; set; }

        public UserDatabaseModel getDb()
        {
            var getDbBorks = new List<BorkDatabaseModel>();
            foreach (var bork in UserBorks)
            {
                getDbBorks.Add(bork.getDb());
            }
            var newDb = new UserDatabaseModel
            {
                UserId = UserId,
                UserName = UserName,
                Password = Password,
                UserBorks = getDbBorks,
                UserPP = UserPP,
                FollowingId = FollowingId,
                FollowerId = FollowerId
            };
            return newDb;
        }

        public UserViewModel getView(UserDatabaseModel getdb)
        {
            if (getdb == null) return null;
            var getViewBorks = new List<BorkViewModel>();
            var methodBork = new BorkViewModel();
            UserViewModel newDb = null;
            if (getdb.UserBorks != null)
            {
                foreach (var bork in getdb.UserBorks)
                {
                    getViewBorks.Add(methodBork.getView(bork));
                }
                getViewBorks = getViewBorks.OrderByDescending(a => a.DateBorked).ToList();
            }

            if (getdb.FollowingId == null) getdb.FollowingId = new List<int>();
            if (getdb.FollowerId == null) getdb.FollowerId = new List<int>();
            if (getdb.UserBorks == null) getViewBorks = new List<BorkViewModel>();

            newDb = new UserViewModel
            {
                UserId = getdb.UserId,
                UserName = getdb.UserName,
                Password = getdb.Password,
                UserBorks = getViewBorks,
                UserPP = getdb.UserPP,
                FollowingId = getdb.FollowingId,
                FollowerId = getdb.FollowerId
            };
            return newDb;
        }
    }
}
