using System;
namespace ServizzBackend.Models.MobileApp.Structs
{
    public class UserStruct:StatusStruct
    {
      
        public string email { get; set; }
        public string phone { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
     
        //public string email { get; set; }
    }
}
