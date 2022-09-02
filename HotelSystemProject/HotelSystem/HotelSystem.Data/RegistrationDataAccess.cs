using HotelSystem.Data.DataModels;
using HotelSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Web.Mvc;

namespace HotelSystem.Data
{
    public class RegistrationDataAccess
    {
        public void CreateRegistration(RegistrationViewModel registration)
        {
            CustomersRegistration customersRecord = new CustomersRegistration();
                        customersRecord.Email = registration.Email;
            customersRecord.Password = registration.Password;
            customersRecord.IsEmailVerified = false;
            customersRecord.FirstName = registration.FirstName;
            customersRecord.ContactNum = registration.ContactNum;
            customersRecord.Address = registration.Address;
            customersRecord.ResetPasswordCode = registration.ResetPasswordCode;
            customersRecord.lastName = registration.lastName;
            customersRecord.DateOfBirth = registration.DateOfBirth;
            customersRecord.UserId = registration.UserId;
            customersRecord.ActivationCode = registration.ActivationCode;

            using (db_a784d6_hotelsystemEntities dc = new db_a784d6_hotelsystemEntities())
            {
                dc.CustomersRegistrations.Add(customersRecord);
                dc.SaveChanges();
            }
        }

        public CustomersRegistration GetCustomerByEmail(string email)
        {
            using (db_a784d6_hotelsystemEntities dc = new db_a784d6_hotelsystemEntities())
            {
                if (dc.CustomersRegistrations.Any(a => a.Email == email))
                {
                    return dc.CustomersRegistrations.First(a => a.Email == email);
                }
                return null;
            }
        }
        public bool UserVeify(string id)
        {

            using (db_a784d6_hotelsystemEntities dc = new db_a784d6_hotelsystemEntities())
            { 
                dc.Configuration.ValidateOnSaveEnabled = false; // This line I have added here to avoid 
                                                                //  Confirm password does not match issue on save changes
                var v = dc.CustomersRegistrations.Where(a => a.ActivationCode == new Guid(id)).FirstOrDefault();
                if (v != null)
                {
                    v.IsEmailVerified = true;
                    dc.SaveChanges();
                }
                return true;
            }

        }


        public LoginViewModel CusLogin(LoginViewModel login)
        {

            var customer = GetCustomerByEmail(login.Email);
            if (customer != null)
            {
                if (!customer.IsEmailVerified)
                {
                    login.LoginError = "Please verify your email first";
                } else if (login.Password == customer.Password)

                {
                    login.CusId = customer.UserId;
                    using (db_a784d6_hotelsystemEntities dc = new db_a784d6_hotelsystemEntities())
                    {  UserLogging loggedUser = new UserLogging();
                        loggedUser.LoggedUserId = customer.UserId;
                        loggedUser.FirstName = customer.FirstName;
                        loggedUser.LastName = customer.lastName;
                        dc.UserLoggings.Add(loggedUser);
                        dc.Configuration.ValidateOnSaveEnabled = false;
                        dc.SaveChanges();
                    }
                    return login;
                } 
                else
                {
                    login.LoginError = "Invalid credential provided";
                }
            }
            else
            {
                login.LoginError = "Invalid credential provided";
            }

            return login;
        }

 
        public CustomersRegistration ResetPassword(string id)
        {
            using (db_a784d6_hotelsystemEntities dc = new db_a784d6_hotelsystemEntities())
            {
                var user = dc.CustomersRegistrations.Where(a => a.ResetPasswordCode == id).FirstOrDefault();
                if (user != null)
                {
                    return user;
                }
                return null;

            }
        }
        public CustomersRegistration ResetPassword(ResetPasswordViewModel model)
        {
            using (db_a784d6_hotelsystemEntities dc = new db_a784d6_hotelsystemEntities())
            {
                var user = dc.CustomersRegistrations.Where(a => a.ResetPasswordCode == model.ResetCode).FirstOrDefault();
                if (user != null)
                {
                    user.Password = model.NewPassword;
                    user.ResetPasswordCode = "";
                    dc.Configuration.ValidateOnSaveEnabled = false;
                    dc.SaveChanges();
                    return user;
                }
                return null;
            }
        }
        public string GetForgotEmail(string emailId)
        {
            using (db_a784d6_hotelsystemEntities dc = new db_a784d6_hotelsystemEntities())
            {
                var account = dc.CustomersRegistrations.Where(a => a.Email == emailId).FirstOrDefault();
                if (account != null)
                {
                    string resetCode = Guid.NewGuid().ToString();
                    account.ResetPasswordCode = resetCode;
                    dc.Configuration.ValidateOnSaveEnabled = false;
                    dc.SaveChanges();
                    return resetCode;
                    
                }
                return null;
        }
        }

        public DataTable UserLIst()
        {
            String constring = "Data Source=SQL5109.site4now.net; Initial Catalog=db_a784d6_hotelsystem; Integrated Security=false;user id=db_a784d6_hotelsystem_admin; password=Password@123";
            SqlConnection sqlcon = new SqlConnection(constring);
            String pname = "GetUserList"; ;
            sqlcon.Open();
            SqlCommand com = new SqlCommand(pname, sqlcon);
            com.CommandType = CommandType.StoredProcedure;
            SqlDataReader dr = com.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            return dt;
        }
    }
}


