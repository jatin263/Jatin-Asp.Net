using Jatin.Data;
using Jatin.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Jatin.Controllers
{
    public class HomeController : Controller
    {
        private static string uId = null;
        private static string uName = null;
        private static List<ReminderTaskView> _reminderTaskView = null;
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
        }


        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(UserView v)
        {
            if (ModelState.IsValid)
            {
                if(v.Profile_Path != null) {
                    string folder = "userProfile/";
                    folder += Guid.NewGuid().ToString()+v.Profile_Path.FileName;
                    string serverFolder = Path.Combine(_webHostEnvironment.WebRootPath, folder);
                    using (var stream = System.IO.File.Create(serverFolder))
                    {
                        await v.Profile_Path.CopyToAsync(stream);
                    }
                    if (ApplicationDB.registerUser(v,folder))
                    {
                        ViewBag.Msg = "Register Successfully";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.Msg = "Error while Registering";
                    }
                }
                else
                {
                    if (ApplicationDB.registerUser(v))
                    {
                        ViewBag.Msg = "Register Successfully";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.Msg = "Error while Register";
                    }
                }
            }
            return View(v);
        }


        [Route("Register")]
        public IActionResult Register()
        {
            return View();
        }

       

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [Route("Login")]    
        public IActionResult Login(LoginModel v)
        {
            if(ModelState.IsValid)
            {
                User u = ApplicationDB.getUser(v.Username.ToString());
                if (u == null || u.Password != v.Password)
                {
                    ViewBag.msg = "Wrong Username or Password";
                    return View("Index", v);
                }
                uId = u.Id.ToString();
                uName = u.Name.ToString();
                return RedirectToAction("Home");
            }
            return View("Index",v);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [Route("Home")]
        public IActionResult Home()
        {
            if (uId == null)
            {
                return RedirectToAction("Index");
            }
            ViewBag.UserId = uId;
            ViewBag.UserName = uName;
            return View();
        }

        [HttpGet]
        [Route("fetchAllData")]
        public IActionResult getAllReminders()
        {
            JsonResponse j = new JsonResponse();
            string userId = uId;
            if(uId == null)
            {
                j.msg = "Invalid User";
                j.Tasks = null;
            }
            
            Guid g;
            try
            {
                g = new Guid(userId);
            }
            catch (Exception ex)
            {
                j.msg = "Invalid UserId";
                return Json(j);
            }
            j.msg = "success";
            j.Tasks = ApplicationDB.getReminderTasks(g);
            return Json(j);
            
        }


        [HttpPost]
        [Route("AddReminder")]
        public IActionResult AddReminder(NewReminderTask nRT)
        {
            AddJsonResponse ajr = new AddJsonResponse();
            if (uId == null)
            {
                
                ajr.msg = "Invalid User";
                return Json(ajr);
            }
            if (!ModelState.IsValid)
            {
                ajr.msg = "Invalid Paramters";
                return Json(ajr);
            }
            Guid guid = new Guid(uId);
            if (ApplicationDB.setReminderTask(nRT, guid))
            {
                ajr.msg = "Success";
            }
            else
            {
                ajr.msg = "Failed";
            }
            
            return Json(ajr);

        }

        [HttpPost]
        [Route("DeleteReminder")]
        public IActionResult DeleteReminder(int remId)
        {
            AddJsonResponse ajr= new AddJsonResponse();
            if (uId == null)
            {
                ajr = new AddJsonResponse();
                ajr.msg = "Invalid User";
                return Json(ajr);
            }
            if (ApplicationDB.deleteReminder(remId,uId))
            {
                ajr.msg = "Success";
            }
            else
            {
                ajr.msg = "Failed";
            }
            return Json(ajr);
        }

        [HttpPost]
        [Route("DisableTask")]
        public IActionResult DisableTask(int remId)
        {
            AddJsonResponse ajr = new AddJsonResponse();
            if (uId == null)
            {
                ajr = new AddJsonResponse();
                ajr.msg = "Invalid User";
                return Json(ajr);
            }
            if (ApplicationDB.disableTask(remId, uId))
            {
                ajr.msg = "Success";
            }
            else
            {
                ajr.msg = "Failed";
            }
            return Json(ajr);
        }

        [HttpPost]
        [Route("EnableTask")]
        public IActionResult EnableTask(int remId)
        {
            AddJsonResponse ajr = new AddJsonResponse();
            if (uId == null)
            {
                ajr = new AddJsonResponse();
                ajr.msg = "Invalid User";
                return Json(ajr);
            }
            if (ApplicationDB.enableTask(remId, uId))
            {
                ajr.msg = "Success";
            }
            else
            {
                ajr.msg = "Failed";
            }
            return Json(ajr);
        }


        [HttpPost]
        [Route("UpdateReminder")]
        public IActionResult UpdateReminder(UpdateReminder nRT)
        {
            AddJsonResponse ajr = new AddJsonResponse();
            if (uId == null)
            {

                ajr.msg = "Invalid User";
                return Json(ajr);
            }
            if (!ModelState.IsValid)
            {
                ajr.msg = "Invalid Paramters";
                return Json(ajr);
            }
            Guid guid = new Guid(uId);
            if (ApplicationDB.updateReminderTask(nRT, guid))
            {
                ajr.msg = "Success";
            }
            else
            {
                ajr.msg = "Failed";
            }

            return Json(ajr);

        }




        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}