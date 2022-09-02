using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using HotelSystem.Data;
using HotelSystem.Data.DataModels;
using HotelSystem.ViewModels;
using HotelSystem.Service;
using System.Data;

namespace HotelSystem.Controllers
{
    public class CustomerController : Controller
    {
        RegistrationService registrationService = new RegistrationService();
        ReservationService reservationService = new ReservationService();

        [HttpGet]
        public ActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registration([Bind(Exclude = "IsEmailVerified,ActivationCode")] RegistrationViewModel registration)
        {
            bool Status = false;
            string message = "";

            // Model Validation 
            if (ModelState.IsValid)
            {
                //Email is already Exist 
                var isExist = registrationService.IsEmailExist(registration.Email);
                if (isExist)
                {
                    ModelState.AddModelError("EmailExist", "Email already exist");
                    return View(registration);
                }
                // Generate Activation Code 
                registration.ActivationCode = Guid.NewGuid();
                //  Password Hashing 
                registration.Password = Crypto.Hash(registration.Password);
                registration.IsEmailVerified = false;
                registrationService.CreateRegistration(registration, Request);
                message = "Registration successfully done. Account activation link " +
                        " has been sent to your email id:" + registration.Email;
                Status = true;
            }
            else
            {
                message = "Invalid Request";
            }
            ViewBag.Message = message;
            ViewBag.Status = Status;

            return View(registration);
        }


        // Verify Email
        [HttpGet]
        public ActionResult VerifyAccount(string id)
        {
            bool Status = false;
            var isVerified = registrationService.UserVerify(id);
          
               if(isVerified)
               {
                    Status= true;
               }
                else
                {
                    ViewBag.Message = "Invalid Request";
                }
            
            ViewBag.Status = Status;
            return View();
        }


        //Login
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        //Login POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel login, string ReturnUrl = "")
        {
            login.Password = Crypto.Hash(login.Password);

            LoginViewModel user = registrationService.UserLogin(login);

            if (user.CusId > 0)  
            {
                var cookie = registrationService.HttpCookie(login);
                Response.Cookies.Add(cookie);
                if (Url.IsLocalUrl(ReturnUrl))
                {
                    return Redirect(ReturnUrl);
                }
                else
                {
                    return RedirectToAction("CheckAvailability", "Reservation");
                }
            }
            else
            {
                ViewBag.message = user.LoginError;
                return View("Login");
            }
        }

        //Logout

        [Authorize]

        public ActionResult Logout()
        {
            Session.Clear();
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Customer");
        }

        //Forget Password
        [HttpGet]
        public ActionResult ForgotPassword()
        {
            return View();
        }


        [HttpPost]
        public ActionResult ForgotPassword(string EmailID)
        {
            string message = "";
            bool status = false;
            var forgotpassword = registrationService.ForgotPassword(EmailID, Request);

                if (forgotpassword != false)
                {
                    message = "Reset password link has been sent to your email id.";
                }
                else
                {
                    message = "Account not found";
                }
            ViewBag.Message = message;
            return View();
        }

        public ActionResult ResetPassword(string id)
        {         
            var resetPassword = registrationService.ResetPassword(id);
            if (string.IsNullOrWhiteSpace(id))
            {
                return HttpNotFound();
            }
                if (resetPassword != null)
                {
                    
                    return View(resetPassword);
                }
                else
                {
                    return HttpNotFound();
                }
            }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPasswordViewModel model)
        {
            var message = "";
            model.NewPassword = Crypto.Hash(model.NewPassword);
            var resetPassword = registrationService.ResetPassword(model);
            if (ModelState.IsValid)
            {
                    if (resetPassword == true)
                    {
                        message = "New password updated successfully";
                    }
                }
            else
            {
                message = "Something invalid";
            }
            ViewBag.Message = message;
            return View(model);
        }

        [HttpGet]
        public ActionResult UserList()
        {
            DataTable dt = registrationService.UserList();
            return View(dt);
        }



    }
}