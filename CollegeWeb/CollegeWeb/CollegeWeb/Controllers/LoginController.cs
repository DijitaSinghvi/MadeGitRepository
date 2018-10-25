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
     /// <summary>
     /// Generate login form.
     /// </summary>
     /// <returns></returns>
        public ActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// Post login form.
        /// </summary>
        /// <param name="objViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        
        public ActionResult Login(ViewModel objViewModel)
        {
            //to authenticate email and password from database.
            var temp = db.Users.Where(x=>x.Email==objViewModel.Email && x.Password==objViewModel.Password).Select(x=> x.UserId).FirstOrDefault();
            if(temp!= null)
            {   //Redirect to homepage according to role of the user.
                var getRole = db.UserInRoles.Where(x => x.UserId == temp).Select(x => x.RoleId).FirstOrDefault();
                if (getRole == 1)
                {
                    return RedirectToAction("HomePage", "SuperAdmin", new { id = temp });
                }

                else if(getRole == 2)
                {
                    return RedirectToAction("HomePage", "Admin", new { id = temp });
                }
                else if(getRole == 3)
                {
                    return RedirectToAction("TeacherHomePage", "Teacher", new { id = temp });
                }
                else if(getRole == 4)
                {
                    return RedirectToAction("StudentHomePage", "Student", new { id = temp });
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