﻿namespace MaintenanceBookingService.UI.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ApprovedServices()
        {
            return View();
        }

        public ActionResult DeliveredServices()
        {
            return View();
        }
    }
}