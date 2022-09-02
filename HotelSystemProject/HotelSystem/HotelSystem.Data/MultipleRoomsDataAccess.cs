using HotelSystem.Data.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelSystem.Data
{
    public class MultipleRoomsDataAccess
    {
        public List<MultipleRoom> GetAllMultipleRooms()
        {
            db_a784d6_hotelsystemEntities db = new db_a784d6_hotelsystemEntities();
            var multipleRoomsList = db.MultipleRooms.ToList();

            return multipleRoomsList;
        }
    }
}
