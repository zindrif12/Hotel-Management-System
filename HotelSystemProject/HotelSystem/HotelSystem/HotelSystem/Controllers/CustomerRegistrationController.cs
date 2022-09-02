using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using HotelSystem.Models;


namespace HotelSystem.Controllers
{
    public class CustomerRegistrationController : Controller
    {
        //Registration Action

        [HttpGet]
        public ActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registration([Bind(Exclude = "IsEmailVerified,ActivationCode")] CustomersRegistration registration)
        {
            bool Status = false;
            string message = "";
            //
            // Model Validation 
            if (ModelState.IsValid)
            {

                #region //Email is already Exist 
                var isExist = IsEmailExist(registration.Email);
                if (isExist)
                {
                    ModelState.AddModelError("EmailExist", "Email already exist");
                    return View(registration);
                }
                #endregion

                #region Generate Activation Code 
                registration.ActivationCode = Guid.NewGuid();
                #endregion

                #region  Password Hashing 
                registration.Password = Crypto.Hash(registration.Password);
              
                #endregion
                registration.IsEmailVerified = false;

                #region Save to Database
                using (db_a784d6_hotelsystemEntities3 dc = new db_a784d6_hotelsystemEntities3())
                {
                    dc.CustomersRegistrations.Add(registration);
                    dc.SaveChanges();

                    //Send Email to User
                    SendVerificationLinkEmail(registration.Email, registration.ActivationCode.ToString());
                    message = "Registration successfully done. Account activation link " +
                        " has been sent to your email id:" + registration.Email;
                    Status = true;
                }
                #endregion
            }
            else
            {
                message = "Invalid Request";
            }

            ViewBag.Message = message;
            ViewBag.Status = Status;
           
            //Send email to user
            return View(registration);

        }


        //Verify Email
        [HttpGet]
        public ActionResult VerifyAccount(string id)
        {
            bool Status = false;
            using (db_a784d6_hotelsystemEntities3 dc = new db_a784d6_hotelsystemEntities3())
            {
                dc.Configuration.ValidateOnSaveEnabled = false; // This line I have added here to avoid 
                                                                // Confirm password does not match issue on save changes
                var v = dc.CustomersRegistrations.Where(a => a.ActivationCode == new Guid(id)).FirstOrDefault();
                if (v != null)
                {
                    v.IsEmailVerified = true;
                    dc.SaveChanges();
                    Status = true;
                }
                else
                {
                    ViewBag.Message = "Invalid Request";
                }
            }
            ViewBag.Status = Status;
            return View();
        }

        //Verify Email LINK
        [NonAction]
        public bool IsEmailExist(string emailID)
        {
            using (db_a784d6_hotelsystemEntities3 dc = new db_a784d6_hotelsystemEntities3())
            {
                var v = dc.CustomersRegistrations.Where(a => a.Email == emailID).FirstOrDefault();
                return v != null;
            }
        }

        private void SendVerificationLinkEmail(string Email, string activationCode)
        {
            var verifyUrl = "/CustomerRegistration/VerifyAccount/" + activationCode;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);

            var fromEmail = new MailAddress("testone949@gmail.com", "Verification");
            var toEmail = new MailAddress(Email);
            var fromEmailPassword = "lnszrrxihtwiqjhl"; // Replace with actual password
            string subject = "Your account is successfully created!";

            string body = "<br/><br/>We are excited to tell you that your Dotnet Awesome account is" +
                " successfully created. Please click on the below link to verify your account" +
                " <br/><br/><a href='" + link + "'>" + link + "</a> ";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
                smtp.Send(message);
        }










    }
}