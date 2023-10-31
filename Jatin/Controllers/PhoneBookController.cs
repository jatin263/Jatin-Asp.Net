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

        [Route("Login")]
        [HttpPost]
        public IActionResult Login(PhoneBookLogin pbl)
        {
            if(!ModelState.IsValid)
            {
                PhoneBookJson pbj1 = new PhoneBookJson();
                pbj1.status = "Error";
                pbj1.msg = "All Parameter Requried";
                return new JsonResult(pbj1);
            }
            PhoneBookUser user = ApplicationDB.getPhoneBookUser(pbl.Email);
            if(user.Email == null)
            {
                PhoneBookJson pbj2 = new PhoneBookJson();
                pbj2.status = "error";
                pbj2.msg = "No User Found";
                return new JsonResult(pbj2);
            }
            if(pbl.Password == user.Password)
            {
                PhoneBookUserJson pbjj = new PhoneBookUserJson();
                pbjj.Email=user.Email;
                pbjj.PhoneNumber=user.PhoneNumber;
                pbjj.Id = user.Id;
                pbjj.Name = user.Name;
                return new JsonResult(pbjj);
            }
            PhoneBookJson pbj = new PhoneBookJson();
            pbj.status = "error";
            pbj.msg = "Wrong Password";
            return new JsonResult(pbj);

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
                pbj.msg = "Email or Phone Already Register";
            }
            return new JsonResult(pbj);
        }

        [Route("AddContact")]
        [HttpPost]
        public IActionResult AddContact(PhoneBookContactAdd p)
        {
            PhoneBookJson pbj = new PhoneBookJson();
            if(ApplicationDB.addContact(p)>0)
            {
                pbj.status="success";
                pbj.msg = "Add Successfully";
            }
            else { 
                pbj.status = "error";
                pbj.msg = "Error While Adding";
            }
            return new JsonResult(pbj); 
        }

        [HttpGet]
        [Route("GetContact/{uId}")]
        public IActionResult GetContact(string uId)
        {
            List<PhoneBookContact> f = null;
            f = ApplicationDB.GetContacts(uId);
            if (f!=null && f.Count>0)
            {
                PhoneBookContactJson phoneBookJson = new PhoneBookContactJson();
                phoneBookJson.status = "success";
                phoneBookJson.contacts = f;
                return new JsonResult(phoneBookJson);
            }
            PhoneBookJson pbj = new PhoneBookJson();
            pbj.status= "error";
            pbj.msg = uId;
            return new JsonResult(pbj);
        }

    }

}
