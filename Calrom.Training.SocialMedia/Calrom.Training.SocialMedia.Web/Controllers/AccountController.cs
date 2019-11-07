using Calrom.Training.SocialMedia.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Calrom.Training.SocialMedia.Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        public ActionResult Account()
        {
            var userId = this.HttpContext.Session["UserId"] as int?;
            var timeLineViewModel = new TimeLineViewModel(userId ?? 0);
            return View(timeLineViewModel);
        }
    }
}