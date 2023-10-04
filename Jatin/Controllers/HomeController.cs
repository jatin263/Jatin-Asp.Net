using Jatin.Data;
using Jatin.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Diagnostics;

namespace Jatin.Controllers
{
    public class HomeController : Controller
    {
        private static string name = "Jatin";
        private static List<User> _users;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [Route("Register")]    
        public IActionResult Register(UserView v)
        {
            List<User> users = new List<User>();
           if(ModelState.IsValid)
            {
                Guid id = Guid.NewGuid();
                User user = new User();
                user.Id = id;
                user.Name= v.Name;
                user.Username = v.Username;
                user.Password = v.Password;
                string dbCS = ApplicationDB.getConnectionString();
                SqlConnection conn = null;
                try
                {
                    using (conn = new SqlConnection(dbCS))
                    {
                        conn.Open();
                        if (conn.State == System.Data.ConnectionState.Open)
                        {
                            string sql = "Insert into Users values (@id,@name,@username,@password)";
                            SqlCommand sqlCommand = new SqlCommand(sql,conn);
                            sqlCommand.Parameters.AddWithValue("@id", user.Id);
                            sqlCommand.Parameters.AddWithValue("@name",user.Name);
                            sqlCommand.Parameters.AddWithValue("@username", user.Username);
                            sqlCommand.Parameters.AddWithValue("@password", user.Password);
                            if(sqlCommand.ExecuteNonQuery() > 0)
                            {
                                ViewBag.UserMsg = "Saved";
                            }
                            else
                            {
                                ViewBag.UserMsg = "Not Saved";
                            }
                            sql = "Select * from Users";
                            sqlCommand = new SqlCommand(sql,conn);
                            SqlDataReader dr = sqlCommand.ExecuteReader();
                            while (dr.Read())
                            {
                               User u = new User();
                                u.Id = new Guid(dr["ID"].ToString());
                                u.Username = dr["Username"].ToString();
                                u.Password = dr["Password"].ToString();
                                u.Name = dr["Name"].ToString();
                                users.Add(u);
                            }
                            _users= users;
                            return RedirectToAction("Home");
                            
                        }
                        else
                        {
                            ViewBag.msg = "Close";
                        }
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.msg = ex.Message;
                }
                finally
                {
                    conn.Close();
                }
            }
           return View("Index",v);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Home()
        {
            List<User> users = _users;
            return View(users);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}