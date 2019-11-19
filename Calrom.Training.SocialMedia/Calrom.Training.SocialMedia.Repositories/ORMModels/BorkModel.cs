using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Calrom.Training.SocialMedia.Database.ORMModels
{
    public class BorkModel
    {
        public virtual DateTime DateBorked { get; set; }
        public virtual string BorkText { get; set; }
        public virtual int UserId { get; set; }

    }
}