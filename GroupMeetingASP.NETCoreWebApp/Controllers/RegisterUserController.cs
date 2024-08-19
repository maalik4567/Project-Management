using Microsoft.AspNetCore.Mvc;
using GroupMeetingASP.NETCoreWebApp.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using AspNetCoreHero.ToastNotification.Abstractions;
using Serilog;

namespace GroupMeetingASP.NETCoreWebApp.Controllers
{
	public class RegisterUserController : Controller
	{
		private readonly INotyfService _notyf;

		public RegisterUserController(INotyfService notyf)
		{
			_notyf = notyf ?? throw new ArgumentNullException(nameof(notyf));
		}

		[HttpGet]
		public IActionResult Signup()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Signup(RegisterUser user)
		{
			if (ModelState.IsValid)
			{
				// Check if the email already exists
				if (RegisterUser.EmailExists(user.Email))
				{
					ModelState.AddModelError("", "A user with this email already exists.");
					return View(user);
				}

				RegisterUser.AddUser(user);
				_notyf.Success("Successfully created account", 5);
				Log.Information("User signed up: {User}", user);
				return RedirectToAction("Login");
			}

			_notyf.Error("Failed to create account. Please check the details and try again.");
			Log.Warning("Failed signup attempt: {User}", user);
			return View(user);
		}

		[HttpGet]
		public JsonResult IsEmailExist(string email)
		{
			bool emailExists = RegisterUser.EmailExists(email);
			return Json(!emailExists); // returns true if email does not exist, false otherwise
		}

		[HttpGet]
		public IActionResult Login()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Login(string email, string password)
		{
			var user = RegisterUser.Login(email, password);

			if (user != null)
			{
				// Store user information in session as JSON
				HttpContext.Session.SetString("User", JsonConvert.SerializeObject(user));
				_notyf.Success("Successfully Loggedin!!", 5);
				Log.Information("User logged in: {Email}", email);
				return RedirectToAction("Index", "Home");
			}

			// If login fails
			ModelState.AddModelError("", "Invalid credentials. Please check your email and password.");
			_notyf.Error("Error in login to your account", 5);
			Log.Warning("Failed login attempt: {Email}", email);
			return View();
		}

		[HttpPost]
		public IActionResult Logout()
		{
			var userJson = HttpContext.Session.GetString("User");
			var user = userJson != null ? JsonConvert.DeserializeObject<RegisterUser>(userJson) : null;

			HttpContext.Session.Remove("User");
			Log.Information("User logged out: {User}", user);
			return RedirectToAction("Login", "RegisterUser");
		}

		// Method to retrieve object from session
		private T GetObjectFromSession<T>(string key)
		{
			var value = HttpContext.Session.GetString(key);
			return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
		}
	}
}
