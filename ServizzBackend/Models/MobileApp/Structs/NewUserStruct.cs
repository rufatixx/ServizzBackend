using System;
namespace ServizzBackend.Models.MobileApp.Structs
{
    public class NewUserStruct
    {
        public string userToken { get; set; }
        public string requestToken { get; set; }
        public int userType { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
    }
}
