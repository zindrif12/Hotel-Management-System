using HotelSystem.Data.DataModels;
using HotelSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelSystem.Data
{
    public class RoomDataAccess
    {

        public void CreateRoom(AddRoomViewModel room)
        {
            Room roomRecord = new Room();
            roomRecord.Description = room.Description;
            roomRecord.RoomNo = room.RoomNo.ToString();
            roomRecord.PricePerNight = room.PricePerNight;
            roomRecord.RoomType = room.RoomType;
            roomRecord.Capacity = room.Capacity;

            using (db_a784d6_hotelsystemEntities dc = new db_a784d6_hotelsystemEntities())
            {
                dc.Rooms.Add(roomRecord);
                dc.SaveChanges();
            }
        }
        public List<Room> GetAllRooms()
        {
            db_a784d6_hotelsystemEntities db = new db_a784d6_hotelsystemEntities();
            var roomsList = db.Rooms.ToList();

            return roomsList;
        }

        //public object GetRoom()
        //{
        //    db_a784d6_hotelsystemEntities dc = new db_a784d6_hotelsystemEntities();

        //   return dc.displayroom();
        //}
    }
}
