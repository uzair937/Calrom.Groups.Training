using Calrom.Training.SocialMedia.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Calrom.Training.SocialMedia.Web.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult Account()
        {
            var timeLineViewModel = TimeLineViewModel.getTimeLineViewModel();
            return View(timeLineViewModel);
        }
    }
}