using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Newtonsoft.Json;
using HotelSystem.ViewModels;
using HotelSystem.Service;
using HotelSystem.Data.DataModels;
using System.Web.Security;
using System.Data.Entity;
using System.Net;


//testcomment
namespace HotelSystem.Controllers
{
    public class ReservationController : Controller
    {
        ReservationService reservationService = new ReservationService();
        RoomService roomService = new RoomService();
        public ActionResult CheckAvailability()
        {
            return View();
        }
        [HttpPost]
        public ActionResult GetCheckAvailability(DateTime startDate, DateTime endDate)
        {                                       
            var checkAvailabilityList = reservationService.GetCheckAvailability(startDate, endDate);
      
            return Json(checkAvailabilityList, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult SaveDetails(string roomNo,DateTime start, DateTime end, string roomType)
        {
            var formattedStartDate = start.ToString("yyyy-MM-dd");
            var formattedEndDate = end.ToString("yyyy-MM-dd");

            CheckAvailabilityViewModel checkAvailabilityViewModel = new CheckAvailabilityViewModel();
            checkAvailabilityViewModel.RoomNo = roomNo;
            checkAvailabilityViewModel.ArrivalDate = Convert.ToDateTime(formattedStartDate);
            checkAvailabilityViewModel.DepartureDate = Convert.ToDateTime(formattedEndDate);
            checkAvailabilityViewModel.RoomType = roomType;
            return View(checkAvailabilityViewModel);  
        }
        [HttpPost]
        public ActionResult SaveDetails(ReservationViewModel reservation)
        {
            if (Request.Cookies.AllKeys.Contains(FormsAuthentication.FormsCookieName))
            {
                Reservation reservationRecord = reservationService.CreateReservation(reservation);
                return RedirectToAction("Receipt", "Reservation", reservationRecord);
            }
            else
            {
                return RedirectToAction("Login", "Customer");
            }
        }

        [HttpGet]
        public ActionResult AddRoom()
        {
           // object getRooms  = roomService.GetRooms();
            return View();
        }
        [HttpPost]
        public ActionResult AddRoom(AddRoomViewModel room) 
        {
            roomService.CreateRoom(room);
            return View();
        }

        [HttpPost]
        public ActionResult GetAllRooms()
        {
            var getAllRooms = roomService.GetAllRooms();

            return Json(getAllRooms, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Receipt(Reservation reservation,ReservationViewModel reservationView)
        {
            var loggedUserId = reservationService.LoggedUser();
            var loggedUsername = reservationService.GetCustomerByID();
            if (loggedUserId > 0)
            {
                ViewBag.FirstName = loggedUsername;

            }
            return View(reservation);
        }
        
        public ActionResult GetMyReservations(ReservationViewModel reservationVM)
        {
            if (Request.Cookies.AllKeys.Contains(FormsAuthentication.FormsCookieName))
            {
                var myReservationsList = reservationService.GetMyReservations(reservationVM);
                //return Json(myReservationsList, JsonRequestBehavior.AllowGet);
                return View(myReservationsList);
            }
            else
            {
                return RedirectToAction("Login", "Customer");
            }

        }

        // GET: ReservationCRUD/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservation reservation = reservationService.FindId(id);
            if (reservation == null)
            {
                return HttpNotFound();
            }
            return View(reservation);
        }

        private db_a784d6_hotelsystemEntities db = new db_a784d6_hotelsystemEntities();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Reservation reservation)
        {
            if (ModelState.IsValid)
            { 
                reservationService.Edit(reservation);
                return RedirectToAction("GetMyReservations");
            }
            return View(reservation);
        }

        // GET: ReservationCRUD/Delete/5
        public ActionResult Delete(int id)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            Reservation reservation = reservationService.FindId(id);
            if (reservation == null)
            {
                return HttpNotFound();
            }
            return View(reservation);
        }

        // POST: ReservationCRUD/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirm(int id)
        {
            var reservation = reservationService.FindId(id);

            reservationService.DeleteConfirm(reservation);

            return RedirectToAction("GetMyReservations");
        }

    }
}