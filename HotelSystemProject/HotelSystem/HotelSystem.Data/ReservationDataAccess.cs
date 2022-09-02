using HotelSystem.Data.DataModels;
using HotelSystem.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Xml.Schema;

namespace HotelSystem.Data
{
    public class ReservationDataAccess
    {
        public List<Reservation> GetAllReservations()
        {
            db_a784d6_hotelsystemEntities db = new db_a784d6_hotelsystemEntities();
            var reservationList = db.Reservations.ToList();
            return reservationList;
        }

        public List<CheckAvailabilityViewModel> GetCheckAvailability(DateTime startDate, DateTime endDate)
        {
            ReservationDataAccess reservationDataAccess = new ReservationDataAccess();
            RoomDataAccess roomDataAccess = new RoomDataAccess();
            MultipleRoomsDataAccess multipleRoomsDataAccess = new MultipleRoomsDataAccess();

            var reservationList = reservationDataAccess.GetAllReservations();
            var roomsList = roomDataAccess.GetAllRooms();
            var multipleRoomsList = multipleRoomsDataAccess.GetAllMultipleRooms();

            using (db_a784d6_hotelsystemEntities dc = new db_a784d6_hotelsystemEntities())
            {
                var reservationListFiltered = reservationList.Where(x => (Convert.ToDateTime(x.dateIn) >= startDate && Convert.ToDateTime(x.dateIn) <= endDate) &&
                                                                     (Convert.ToDateTime(x.dateOut) >= startDate && Convert.ToDateTime(x.dateOut) <= endDate));

                var roomReservationList = reservationListFiltered.Join(dc.MultipleRooms, o => o.Id,
                                                                                     i => i.reservationId,
                                                                                     (o, i) => i).Join(dc.Rooms, o => o.roomId,
                                                                                     i => i.Id,
                                                                                     (o, i) => i).ToList();

                var availableRooms = dc.Rooms.ToList().Except(roomReservationList).ToList();
                //var availableRooms = roomsList;


                List<CheckAvailabilityViewModel> checkAvailabilityList = new List<CheckAvailabilityViewModel>();

                foreach (var reservation in availableRooms)
                {
                    CheckAvailabilityViewModel checkAvailability = new CheckAvailabilityViewModel
                    {
                        RoomNo = reservation.RoomNo,
                        RoomType = reservation.RoomType,
                    };
                    checkAvailabilityList.Add(checkAvailability);
                }
                return checkAvailabilityList;
            }
        }


        public Reservation CreateReservation(ReservationViewModel reservation)
        {

            using (db_a784d6_hotelsystemEntities dc = new db_a784d6_hotelsystemEntities())
            {
                CustomersRegistration customersRegistration = new CustomersRegistration();
                Reservation reservationRecord = new Reservation();
                reservationRecord.reservationDate = DateTime.Now;
                reservationRecord.roomType = reservation.roomType;
                reservationRecord.dateIn = reservation.dateIn.Date;
                reservationRecord.dateOut = reservation.dateOut;
                reservationRecord.numberOfPeople = reservation.numberOfPeople;
                reservationRecord.RoomId = reservation.roomId;

                reservationRecord.customerId = GetLoggedUserId();

                dc.Reservations.Add(reservationRecord);
                dc.SaveChanges();

                return reservationRecord;
            }
        }

        public String GetCustomerById()
        {
            using (db_a784d6_hotelsystemEntities dc = new db_a784d6_hotelsystemEntities())
            {
                var loggedUserId = GetLoggedUserId();
                if (dc.CustomersRegistrations.Any(a => a.UserId == loggedUserId))
                {
                    var cusName = dc.CustomersRegistrations.Where(a => a.UserId == loggedUserId).FirstOrDefault();
                    return cusName.FirstName + " " + cusName.lastName;
                }
                return null;
            }
        }

        public List<Reservation> GetMyReservation(ReservationViewModel reservationVM)
        {
            using (db_a784d6_hotelsystemEntities dc = new db_a784d6_hotelsystemEntities())
            {
                CustomersRegistration customersRegistration = new CustomersRegistration();
                ReservationDataAccess reservationDataAccess = new ReservationDataAccess();
                Reservation reservation = new Reservation();

                var reservationList = reservationDataAccess.GetAllReservations();
                var loggedUserId = GetLoggedUserId();

                var myReservations = dc.Reservations.Where(a => a.customerId == loggedUserId).ToList();

                return myReservations;

            }

        }

    
        public void Edit([Bind(Include = "Id,customerId,reservationDate,dateIn,dateOut,cost,roomType,numberOfPeople,RoomId")] Reservation reservation)
        {
            using (db_a784d6_hotelsystemEntities db = new db_a784d6_hotelsystemEntities())
            {

                db.Entry(reservation).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        public Reservation FindId(int? id)
        {
            using (db_a784d6_hotelsystemEntities db = new db_a784d6_hotelsystemEntities())
            {
                Reservation reservation = db.Reservations.Find(id);
                return reservation;
            }

        }

        public void DeleteConfirm(Reservation reservation)
        {
            using (db_a784d6_hotelsystemEntities db = new db_a784d6_hotelsystemEntities())
            {
                db.Reservations.Attach(reservation);
                db.Entry(reservation).State = EntityState.Deleted;
                //db.Reservations.Remove(reservation);
                db.SaveChanges();
            }
        }
        public int GetLoggedUserId()
        {

            using (db_a784d6_hotelsystemEntities dc = new db_a784d6_hotelsystemEntities())
            {
                var loggedUserId = (from a in dc.UserLoggings orderby a.Id descending select a.LoggedUserId).First();
                return loggedUserId;
            }
        }

    }
}

        


        


    

