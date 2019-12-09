using ExamPracticeProblem2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ExamPracticeProblem2.Controllers
{
    public class ScheduleController : Controller
    {
        private DB_128040_practiceEntities db = new DB_128040_practiceEntities();
        // GET: Schedule
        public ActionResult Index(int? year)
        {
            if (year == null)
            {
                year = DateTime.Now.Year;
            }
            var games = db.FootballSchedules.Where (s => s.Season == year.Value);

            //var g = new List<FootballSchedule>();

            //foreach (var s in db.FootballSchedules)
            //{
            //    g.Add(s);
            //}
            return View(games);
        }
    }
}