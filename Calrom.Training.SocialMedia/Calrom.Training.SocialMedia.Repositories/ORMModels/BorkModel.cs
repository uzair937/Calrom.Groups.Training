using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Calrom.Training.SocialMedia.Database.ORMModels
{
    public class BorkModel
    {
        public virtual int BorkId { get; set; }
        public virtual DateTime DateBorked { get; set; }
        public virtual string BorkText { get; set; }
        public virtual UserModel UserModel { get; protected set; }

        public virtual void AddUser(UserModel user)
        {
            UserModel = user;
        }
    }
}