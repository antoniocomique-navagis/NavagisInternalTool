using System;
using System.Linq;
using System.Web.Configuration;
using System.Web.Mvc;
using NavagisInternalTool.Models;

namespace NavagisInternalTool.Controllers
{
    public class AdminController : Controller
    {
        private ApplicationDBContext _applicationDBContext;
        private int _settingRecordID;
        private string _isLogedIn;

        public AdminController()
        {
            _applicationDBContext = new ApplicationDBContext();
            _settingRecordID = Convert.ToInt32(WebConfigurationManager.AppSettings["DBDefaultSettingID"]);
            _isLogedIn = "Yes";
        }

        protected override void Dispose(bool disposing)
        {
            _applicationDBContext.Dispose();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(User user)
        {
            if (!ModelState.IsValid)
                return View("Register", user);

            var _userInDb = _applicationDBContext.Users.SingleOrDefault(u => u.Username == user.Username && u.Password == user.Password);

            if (_userInDb != null)
            {
                Session["Username"] = user.Username;
                Session["isLogedIn"] = _isLogedIn;
                return RedirectToAction("Setting", "Admin");
            }

            ViewBag.Message = "Invalid username or password.";

            return View(user);
        }

        public ActionResult Users()
        {
            if (Session["isLogedIn"] != _isLogedIn)
                return RedirectToAction("Login", "Admin");

            return View(_applicationDBContext.Users.ToList());
        }

        [HttpGet]
        public ActionResult Register()
        {
            if (Session["isLogedIn"] != _isLogedIn)
                return RedirectToAction("Login", "Admin");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(User user)
        {
            var isSuccess = 0;
            if (!ModelState.IsValid)
                return View("Register", user);

            ModelState.Clear();
            _applicationDBContext.Users.Add(user);

            try
            {
                isSuccess = _applicationDBContext.SaveChanges();
                ViewBag.Message = user.Username + " successfully registered.";
            }
            catch (Exception)
            {
                ViewBag.Message = "The '" + user.Username + "' for username field is already used.";
            }

            if (isSuccess == 0)
                return View("Register", user);

            return RedirectToAction("Users", "Admin");
        }

        [HttpGet]
        public ActionResult Delete(int Id)
        {
            if (Session["isLogedIn"] != _isLogedIn)
                return RedirectToAction("Login", "Admin");
                

            var _userInDb = _applicationDBContext.Users.SingleOrDefault(u=>u.Id==Id);

            if (_userInDb != null)
            {
                _applicationDBContext.Users.Remove(_userInDb);
                _applicationDBContext.SaveChanges();
            }
            return RedirectToAction("Users", "Admin");
        }

        // GET: Admin
        public ActionResult Setting()
        {
            if (Session["isLogedIn"] != _isLogedIn)
                return RedirectToAction("Login", "Admin");

            var settingInDB = _applicationDBContext.Setting.SingleOrDefault(s => s.Id == _settingRecordID);
            return View(settingInDB);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Setting(Setting setting)
        {
            if (!ModelState.IsValid)
                return View("Setting",setting);

            var settingInDB = _applicationDBContext.Setting.SingleOrDefault(s => s.Id == _settingRecordID);

            settingInDB.ClientId = setting.ClientId;
            settingInDB.ClientSecret = setting.ClientSecret;
            settingInDB.BillingAccountName = setting.BillingAccountName;
            _applicationDBContext.SaveChanges();

            Session["Message"] = "Setting is successfully updated.";

            return RedirectToAction("Setting", "Admin");
        }
    }
}