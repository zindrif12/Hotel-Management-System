using HotelSystem.Data;
using HotelSystem.Data.DataModels;
using HotelSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace HotelSystem.Service
{
    public class ReservationService
    {
        ReservationDataAccess reservationDataAccess = new ReservationDataAccess();

        public List<CheckAvailabilityViewModel> GetCheckAvailability(DateTime startDate, DateTime endDate)
        {
            var checkAvailability = reservationDataAccess.GetCheckAvailability(startDate,endDate);
            return checkAvailability;
        }

        public Reservation CreateReservation(ReservationViewModel reservation)
        {
            var reservationRecord = reservationDataAccess.CreateReservation(reservation);
            return reservationRecord;
        }



        public List<Reservation> GetMyReservations(ReservationViewModel reservationVM)
        {
            var myReservationsList = reservationDataAccess.GetMyReservation(reservationVM);
            return myReservationsList;
        }

        public String GetCustomerByID()
        {
            return reservationDataAccess.GetCustomerById();
        }


        public int LoggedUser()
        {
            return reservationDataAccess.GetLoggedUserId();
        }

        public void Edit(Reservation reservation)
        {
           reservationDataAccess.Edit(reservation);            
        }
        
        public Reservation FindId(int? id)
        {
            Reservation reservation = reservationDataAccess.FindId(id);
            return reservation;
        }

        public void DeleteConfirm(Reservation reservation)
        {
            reservationDataAccess.DeleteConfirm(reservation);
        }
    }

}
