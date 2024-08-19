using GroupMeetingASP.NETCoreWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using GroupMeetingASP.NETCoreWebApp.Filters;

namespace GroupMeetingASP.NETCoreWebApp.Controllers
{

    [SessionAuthorize]
    public class GroupMeetingController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            var userJson = HttpContext.Session.GetString("User");
            RegisterUser currentUser = null;

            if (userJson != null)
            {
                currentUser = JsonConvert.DeserializeObject<RegisterUser>(userJson);
            }

            ViewBag.CurrentUser = currentUser;
            // Assuming you have a method to get the group meetings
            var groupMeetings = GroupMeeting.GetGroupMeetings();

            return View(groupMeetings);
        }


        [HttpGet]
        public IActionResult AddGroupMeeting()
        {
            ViewBag.GroupMeetingModes = GroupMeeting.GetGroupMeetingModes();
            return View();
        }

        [HttpPost]
        public IActionResult AddGroupMeeting(GroupMeeting groupMeeting)
        {
            if (ModelState.IsValid)
            {
                GroupMeeting.AddGroupMeeting(groupMeeting);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.GroupMeetingModes = GroupMeeting.GetGroupMeetingModes();
            return View(groupMeeting);
        }

        //[HttpPost]
        //public IActionResult AddGroupMeeting([Bind] GroupMeeting groupMeeting)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        if (GroupMeeting.AddGroupMeeting(groupMeeting) > 0)
        //        {
        //            return RedirectToAction("Index");
        //        }

        //    }
        //    return View(groupMeeting);
        //}


        [HttpGet]
        public IActionResult EditMeeting(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }
            GroupMeeting group = GroupMeeting.GetGroupMeetingById(id);
            if (group == null)
                return NotFound();

            return View(group);
        }

        [HttpPost]
        public IActionResult EditMeeting(int? id, [Bind] GroupMeeting groupMeeting)
        {
            if (id != groupMeeting.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                GroupMeeting.UpdateGroupMeeting(groupMeeting);
                return RedirectToAction("Index");
            }
            return View(groupMeeting);
        }

        [HttpGet]
        public IActionResult DeleteMeeting(int id)
        {
            GroupMeeting group = GroupMeeting.GetGroupMeetingById(id);
            if (group == null)
            {
                return NotFound();
            }

            return View(group);
        }

        [HttpPost]
        public IActionResult DeleteMeeting(int id, GroupMeeting groupMeeting)
        {
            if (GroupMeeting.DeleteGroupMeeting(id) > 0)
            {
                return RedirectToAction("Index");
            }
            return View(groupMeeting);
        }


        private const int PageSize = 10; // You can adjust this as per your requirement

        [HttpGet]
        public IActionResult InactiveGroupMeetings(int pageNumber = 1, int pageSize = 5)
        {
            if (pageNumber > 1)
            {
                pageSize = 10; // For subsequent pages, show 10 records
            }

            int totalGroupMeetings = GroupMeeting.GetInactiveTotalCount();
            var groupMeetings = GroupMeeting.GetInactiveGroupMeetings(pageNumber, pageSize);

            ViewBag.CurrentPage = pageNumber;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalPages = (int)Math.Ceiling(totalGroupMeetings / (double)pageSize);

            return View(groupMeetings);
        }

       /// <summary>
       /// ///[HttpGet]
       /// </summary>
       /// <returns></returns>
        // GET: Home
        public ActionResult ChartData()
        {
            List<DataPoint> dataPoints = new List<DataPoint>();

            dataPoints.Add(new DataPoint("NXP", 14));
            dataPoints.Add(new DataPoint("Infineon", 10));
            dataPoints.Add(new DataPoint("Renesas", 9));
            dataPoints.Add(new DataPoint("STMicroelectronics", 8));
            dataPoints.Add(new DataPoint("Texas Instruments", 7));
            dataPoints.Add(new DataPoint("Bosch", 5));
            dataPoints.Add(new DataPoint("ON Semiconductor", 4));
            dataPoints.Add(new DataPoint("Toshiba", 3));
            dataPoints.Add(new DataPoint("Micron", 3));
            dataPoints.Add(new DataPoint("Osram", 2));
            dataPoints.Add(new DataPoint("Others", 35));

            ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints);

            return View();
        }




        [HttpPost]
        public JsonResult AddMeetingBack(int id)
        {
            try
            {
                int rowsAffected = GroupMeeting.AddMeetingBack(id);
                if (rowsAffected > 0)
                {
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false, message = "Failed to add meeting back" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }


}
