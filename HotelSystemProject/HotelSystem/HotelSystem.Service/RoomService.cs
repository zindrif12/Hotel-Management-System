using HotelSystem.Data;
using HotelSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelSystem.Service
{
    public class RoomService
    {
        public void CreateRoom(AddRoomViewModel addRoomViewModel)
        {
            RoomDataAccess roomDataAccess = new RoomDataAccess();

            roomDataAccess.CreateRoom(addRoomViewModel);
        }

        //public object GetRooms()
        //{
        //    RoomDataAccess roomDataAccess = new RoomDataAccess();
        //    object getRooms = roomDataAccess.GetRoom();
        //    return getRooms;
        //}

        public List<AddRoomViewModel> GetAllRooms()
        {
            RoomDataAccess roomDataAccess = new RoomDataAccess();
            var roomsList = roomDataAccess.GetAllRooms();
            List<AddRoomViewModel> getAllRoomsList = new List<AddRoomViewModel>();
            foreach (var reservation in roomsList)
            {
                AddRoomViewModel getRoomsList = new AddRoomViewModel
                {
                    RoomNo = reservation.RoomNo,
                    RoomType = reservation.RoomType
                };
                getAllRoomsList.Add(getRoomsList);
            }
            return getAllRoomsList;

        }
    }
}
