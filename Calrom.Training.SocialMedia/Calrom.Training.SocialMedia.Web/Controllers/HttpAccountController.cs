using Calrom.Training.SocialMedia.Database.ORMRepositories;
using Calrom.Training.SocialMedia.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Calrom.Training.SocialMedia.Web.Controllers
{
    [Authorize]
    public class HttpAccountController : ApiController
    {
        [HttpPost]
        public IHttpActionResult FollowUser(int userId)
        {
            var repo = UserRepository.GetRepository();
            var targetUser = repo.List().FirstOrDefault(u => u.UserId == userId);
            if (targetUser == null)
            { 
                return NotFound();
            }

            var currentUser = repo.List().FirstOrDefault(u => u.UserName == this.RequestContext.Principal.Identity.Name);
            repo.FollowUser(currentUser.UserId, userId);

            return Ok(userId);
        }

        [HttpPost]
        public IHttpActionResult GetId(string userName)
        {
            var repo = UserRepository.GetRepository();
            var targetUser = repo.List().FirstOrDefault(u => u.UserName == userName);
            if (targetUser == null)
            {
                return NotFound();
            }

            var targetUserId = targetUser.UserId;

            return Ok(targetUserId);
        }
    }
}