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
    
    public partial class UserLogging
    {
        public int Id { get; set; }
        public int LoggedUserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
