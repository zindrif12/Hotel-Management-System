using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelSystem.ViewModels
{
    public class CheckAvailabilityViewModel
    {
        public string RoomNo { get; set; }
        [DataType(DataType.Date)]
        public DateTime ArrivalDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime DepartureDate { get; set; }
        public string RoomType { get; set; }
    }
}
