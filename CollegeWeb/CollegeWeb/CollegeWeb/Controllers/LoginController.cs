using CollegeWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CollegeWeb.Controllers
{
    public class LoginController : Controller
    {
        CollegeContext db=new CollegeContext();
        // GET: Login
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(ViewModel objViewModel)
        {
            var temp = db.Users.Where(x=>x.Email==objViewModel.Email && x.Password==objViewModel.Password).Select(x=> x.UserId).FirstOrDefault();
            if(temp!= null)
            {
                var getRole = db.UserInRoles.Where(x => x.UserId == temp).Select(x => x.RoleId).FirstOrDefault();
                if (getRole == 1)
                {
                    return RedirectToAction("HomePage", "SuperAdmin");
                }

                else if(getRole == 2)
                {
                    return RedirectToAction("HomePage", "Admin");
                }
                else if(getRole == 3)
                {
                    return RedirectToAction("HomePage", "Teacher");
                }
                else if(getRole == 4)
                {
                    return RedirectToAction("HomePage", "Student");
                }
            }
            else
            {
                ModelState.AddModelError("", "Email or password is wrong.");
            }
            return View(objViewModel);



        }
    }
}