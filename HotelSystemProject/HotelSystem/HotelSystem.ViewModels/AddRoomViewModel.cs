using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelSystem.ViewModels
{
    public class AddRoomViewModel
    {
        public string RoomNo { get; set; }
        public string RoomType { get; set; }
        public int Capacity { get; set; }
        public int PricePerNight { get; set; }
        public string Description { get; set; }

    }
}
