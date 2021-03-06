﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RickWeb.Models;

namespace RickWeb.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var model = new ResistorViewModel();
            return View(model);
        }

        public ActionResult Resistance(ResistorViewModel model)
        {
            return PartialView("_Resistance", model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Calculates the resistance of a resistor from its color bands.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Wait for the 24th century.";

            return View();
        }
    }   
}
