//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HotelSystem.Data.DataModels
{
    using System;
    using System.Collections.Generic;
    
    public partial class Room
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Room()
        {
            this.MultipleRooms = new HashSet<MultipleRoom>();
        }
    
        public int Id { get; set; }
        public string RoomNo { get; set; }
        public string Description { get; set; }
        public int PricePerNight { get; set; }
        public Nullable<int> Capacity { get; set; }
        public string RoomType { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MultipleRoom> MultipleRooms { get; set; }
    }
}
