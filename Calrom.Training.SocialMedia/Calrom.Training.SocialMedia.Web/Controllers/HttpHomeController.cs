using Calrom.Training.SocialMedia.Database.Repositories;
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
    public class HttpHomeController : ApiController
    {
        [HttpPost]
        public IHttpActionResult AddBork(string borkText)
        {
            var repo = UserRepository.GetRepository();
            var currentUser = repo.List().FirstOrDefault(u => u.UserName == this.RequestContext.Principal.Identity.Name);
            if (currentUser == null)
            {
                return NotFound();
            }

            var timeLineViewModel = new TimeLineViewModel(currentUser.UserId);
            timeLineViewModel.AddBork(borkText);

            return Ok();
        }

        [HttpPost]
        public IHttpActionResult ChangePage(int pageNum)
        {
            return Ok();
        }
    }
}