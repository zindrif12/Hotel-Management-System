using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelSystem.ViewModels
{
    public class ReservationViewModel
    {
        public string roomId { get; set; }
        public int customerId { get; set; }
        public int numberOfDays { get; set; }
        public int numberOfPeople { get; set; }
        public string roomType { get; set; }
        public DateTime dateIn { get; set; }
        public DateTime dateOut { get; set; }
        public DateTime reservationDate { get; set; }
    }
}
