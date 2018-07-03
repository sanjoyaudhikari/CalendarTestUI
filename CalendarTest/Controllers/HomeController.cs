using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CalendarTest.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


        // Connecting to database
        public JsonResult GetEvents()
        {
            using (calendarTestEntities dc = new calendarTestEntities())
            {
                var events = dc.Events.ToList();
                return new JsonResult { Data = events, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

        }

        ////Save Event
        [HttpPost]
        public JsonResult SaveEvent(Event e)
        {
            var status = false;
            using (calendarTestEntities dc = new calendarTestEntities())
            {
                if (e.EventId > 0)
                {
                    //Update the event
                    var v = dc.Events.Where(a => a.EventId == e.EventId).FirstOrDefault();
                    if (v != null)
                    {
                        v.Subject = e.Subject;
                        v.Start = e.Start;
                        v.End = e.End;
                        v.Description = e.Description;
                        v.IsFullDay = e.IsFullDay;
                        v.ThemeColor = e.ThemeColor;
                    }
                }
                else
                {
                    dc.Events.Add(e);
                }
                dc.SaveChanges();
                status = true;
            }
            return new JsonResult { Data = new { status = status } };
        }


        //Delete Event
        [HttpPost]
        public JsonResult DeleteEvent(int eventID)
        {
            var status = false;
            using (calendarTestEntities dc = new calendarTestEntities())
            {
                var v = dc.Events.Where(a => a.EventId == eventID).FirstOrDefault();
                if (v != null)
                {
                    dc.Events.Remove(v);
                    dc.SaveChanges();
                    status = true;
                }
            }
            return new JsonResult { Data = new { status = status } };
        }



    }
}