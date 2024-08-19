using GroupMeetingASP.NETCoreWebApp.Filters;
using GroupMeetingASP.NETCoreWebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace GroupMeetingASP.NETCoreWebApp.Controllers
{
    [SessionAuthorize]
    public class UsersController : Controller
    {
        public IActionResult Index()
        {
            var users = Users.GetUsers();
            return View(users);
        }

        [HttpGet]
        public IActionResult AddUsers()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddUsers(Users user)
        {
            if (ModelState.IsValid)
            {
                Users.AddUser(user);
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        [HttpGet]
        public IActionResult EditUser(int id)
        {
            var user = Users.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost]
        public IActionResult EditUser(int id, Users user)
        {
            if (id != user.UserID)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                Users.UpdateUser(user);
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        [HttpGet]
        public IActionResult DeleteUser(int id)
        {
            var user = Users.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost, ActionName("DeleteUser")]
        public IActionResult DeleteUserConfirmed(int id)
        {
            Users.DeleteUser(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
