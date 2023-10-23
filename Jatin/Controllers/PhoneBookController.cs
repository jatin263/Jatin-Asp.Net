using Jatin.Data;
using Jatin.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Jatin.Controllers
{
    [Route("api/")]
    [ApiController]
    public class PhoneBookController : ControllerBase
    {
        [Route("GetUser")]
        [HttpGet]
        public IActionResult GetUser(string email)
        {
            PhoneBookUser user = ApplicationDB.getPhoneBookUser(email);
            return new JsonResult(user);
        }

        [HttpGet]
        [Route("checkEmail")]
        public IActionResult CheckEmail(string email)
        {
            PhoneBookJson pbj = new PhoneBookJson();
            if(string.IsNullOrEmpty(email))
            {
                pbj.status = "error";
                pbj.msg = "Invalid Parameter";
                return new JsonResult(pbj);
            }
            else
            {
                int c = ApplicationDB.checkEmail(email);
                if(c == 0)
                {
                    pbj.msg = "All Ok";
                    pbj.status = "success";
                    
                }
                else
                {
                    pbj.msg = "Email Already Exists";
                    pbj.status = "error";
                }
            }
            return new JsonResult(pbj);
        }

        [HttpPost]
        public IActionResult AddContact(PhoneBookContact contact)
        {
            return new JsonResult(contact);
        }

        [Route("Registeration")]
        [HttpPost]
        public IActionResult AddUser(PhoneBookGetUser user)
        {
            PhoneBookJson pbj = new PhoneBookJson();
            if (user == null)
            {
                pbj.status="error";
                pbj.msg = "Invalid Parameters";
                return new JsonResult(pbj);
            }
            else if (!ModelState.IsValid)
            {
                pbj.status = "error";
                pbj.msg = "Invalid Parameters";
                return new JsonResult(pbj);
            }
            if (ApplicationDB.addUser(user))
            {
                pbj.status = "success";
                pbj.msg = "Registration Successful";
            }
            else
            {
                pbj.status = "error";
                pbj.msg = "Invalid Parameters";
            }
            return new JsonResult(pbj);
        }
    }
}
