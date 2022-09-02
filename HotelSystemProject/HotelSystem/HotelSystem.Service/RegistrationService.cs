using HotelSystem.Data;
using HotelSystem.Data.DataModels;
using HotelSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;

namespace HotelSystem.Service
{
    public class RegistrationService
    {
        RegistrationDataAccess registrationDataAccess = new RegistrationDataAccess();

        public void CreateRegistration(RegistrationViewModel registration, HttpRequestBase request)
        {
            registrationDataAccess.CreateRegistration(registration);
            // Send Email to User
            SendVerificationLinkEmail(registration.Email, registration.ActivationCode.ToString(), request);
        }

        public bool IsEmailExist(string emailID)
        {
            var customer = registrationDataAccess.GetCustomerByEmail(emailID);

            if (customer == null)
            {
                return false;
            }
            return true;
        }


        public void SendVerificationLinkEmail(string Email, string activationCode, HttpRequestBase request, string emailFor = "VerifyAccount")
        {
            var verifyUrl = "/Customer/" + emailFor + "/" + activationCode;
            var link = request.Url.AbsoluteUri.Replace(request.Url.PathAndQuery, verifyUrl);
            var fromEmail = new MailAddress("testone949@gmail.com", "Verification");
            var toEmail = new MailAddress(Email);
            var fromEmailPassword = "lnszrrxihtwiqjhl"; // Replace with actual password
            string subject = "";
            string body = "";
            if (emailFor == "VerifyAccount")
            {
                subject = "Your account is successfully created!";
                body = "<br/><br/>We are excited to tell you that your Dotnet Awesome account is" +
                    " successfully created. Please click on the below link to verify your account" +
                    " <br/><br/><a href='" + link + "'>" + link + "</a> ";
            }
            else if (emailFor == "ResetPassword")
            {
                subject = "Reset Password";
                body = "Hi,<br/>br/>We got request for reset your account password. Please click on the below link to reset your password" +
                    "<br/><br/><a href=" + link + ">Reset Password link</a>";
            }

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

        public bool UserVerify(string id)
        {
            var verify = registrationDataAccess.UserVeify(id);

            return verify;
        }

        //public bool UserLogin(LoginViewModel login)
        //{
        //    var userLogin = registrationDataAccess.UserLogin(login);
        //    return userLogin;
        //}

        public LoginViewModel UserLogin(LoginViewModel login)
        {
            return registrationDataAccess.CusLogin(login);
        }

        public HttpCookie HttpCookie(LoginViewModel login)
        {
          //  LoginViewModel userId = registrationDataAccess.UserLogin(login).UserId;
            int timeout = (bool)login.RememberMe ? 525600 : 20; // 525600 min = 1 year
            var ticket = new FormsAuthenticationTicket(login.Email, (bool)login.RememberMe, timeout);
            string encrypted = FormsAuthentication.Encrypt(ticket);
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted);
            cookie.Expires = DateTime.Now.AddMinutes(timeout);
            cookie.HttpOnly = true;
            return cookie;
        }

        public bool ForgotPassword(string emailId, HttpRequestBase request)
        {
           var getForgotEmail = registrationDataAccess.GetForgotEmail(emailId);
            var customer = registrationDataAccess.GetCustomerByEmail(emailId);


            if (getForgotEmail != null)
            {
                SendVerificationLinkEmail(customer.Email, getForgotEmail, request, "ResetPassword");
                return true;
            }
            return false;        
        }


        public ResetPasswordViewModel ResetPassword(string id)
        {
            var resetPasswrod = registrationDataAccess.ResetPassword(id);
            if(resetPasswrod != null)
            {
                ResetPasswordViewModel model = new ResetPasswordViewModel();
                model.ResetCode = id;
                return model;
            }
            return null;
        }

        public bool ResetPassword(ResetPasswordViewModel model)
        {
            var resetPassword = registrationDataAccess.ResetPassword(model);

            if(resetPassword != null) 
            { 
                return true;           
            }
            return false;
        }

        public DataTable UserList()
        {
            DataTable dt = registrationDataAccess.UserLIst();

            return dt;
        }
    }
}
