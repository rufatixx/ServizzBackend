using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ServizzBackend.Model.CommonScripts;
using ServizzBackend.Models.MobileApp.Database;
using ServizzBackend.Models.MobileApp.Structs;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ServizzBackend.Controllers
{
    [Route("api/[controller]")]
    public class Servizz : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        public IConfiguration Configuration;
        Communications communications;
        public Servizz(IConfiguration configuration, IWebHostEnvironment hostingEnvironment)
        {
            Configuration = configuration;

            _hostingEnvironment = hostingEnvironment;
           communications = new Communications(Configuration, _hostingEnvironment);
        }
        // GET: api/values
        /* [HttpPost]
         public IEnumerable<string> Get()
         {
             return new string[] { "value1", "value2" };
         }

         */
        [HttpPost]
        [Route("sendSMS")]
        
        public ActionResult<StatusStruct> sendSMS(long phone)
        {
            //var ipAddress = HttpContext.Connection.RemoteIpAddress;

            //communication.log($"mail -> {mail}\npass ->{security.sha256(pass)}", "log_in(string mail, string pass)", ipAddress.ToString());

            //List<UserStruct> user = new List<UserStruct>();
           
                DbInsert insert = new DbInsert(Configuration, _hostingEnvironment);
                return insert.sendSMS(phone);
           
        }
        [HttpPost]
        [Route("verify")]

        public ActionResult<VerifyStruct> verifySMS(long phone,string code)
        {
            //var ipAddress = HttpContext.Connection.RemoteIpAddress;

            //communication.log($"mail -> {mail}\npass ->{security.sha256(pass)}", "log_in(string mail, string pass)", ipAddress.ToString());

            //List<UserStruct> user = new List<UserStruct>();

            DbInsert insert = new DbInsert(Configuration, _hostingEnvironment);
            return insert.verifySMS(phone,code);

        }
        [HttpPost]
        [Route("user/signup")]

        public ActionResult<StatusStruct> signUp([FromBody] NewUserStruct newUser)
        {
            //var ipAddress = HttpContext.Connection.RemoteIpAddress;

            //communication.log($"mail -> {mail}\npass ->{security.sha256(pass)}", "log_in(string mail, string pass)", ipAddress.ToString());

            //List<UserStruct> user = new List<UserStruct>();

            DbInsert insert = new DbInsert(Configuration, _hostingEnvironment);
            return insert.signUp(newUser);

        }
        [HttpPost]
        [Route("user/get/profile")]

        public ActionResult<UserStruct> getProfile(string userToken,string requestToken)
        {
            //var ipAddress = HttpContext.Connection.RemoteIpAddress;

            //communication.log($"mail -> {mail}\npass ->{security.sha256(pass)}", "log_in(string mail, string pass)", ipAddress.ToString());

            //List<UserStruct> user = new List<UserStruct>();

            DbSelect select = new DbSelect(Configuration, _hostingEnvironment);
            return select.getProfile(userToken,requestToken);

        }
        //        [HttpPost]
        //        [Route("user/signUp")]

        //        public ActionResult<StatusStruct> signUp([FromBody] NewUserStruct newUser)
        //        {
        ////            var ipAddress = HttpContext.Connection.RemoteIpAddress;

        ////            communications.log(@$"mail ->{mail}\n
        ////                                name -> {name}\n
        ////                                surname ->{surname}\n
        ////                                pass ->{security.sha256(pass)}\n
        ////                                phone ->{phone}\n
        ////                                bDate ->{bDate}\n
        ////                                gender ->{gender}\n
        ////                                country ->{country}\n
        ////                                city ->{city}\n
        ////                                profession ->{profession}
        ////", "signUp(string mail, string name, string surname, string pass, string phone, string bDate, string gender, string country, string city, string profession)", ipAddress.ToString());
        //            StatusStruct statusCode = new StatusStruct();
        //            if (!string.IsNullOrEmpty(newUser.email) &&
        //                !string.IsNullOrEmpty(newUser.name) &&
        //                !string.IsNullOrEmpty(newUser.surname) &&
        //                !string.IsNullOrEmpty(newUser.pass) &&
        //                !string.IsNullOrEmpty(newUser.phone) &&
        //                //!string.IsNullOrEmpty(bDate) &&
        //                //!string.IsNullOrEmpty(gender) &&
        //                //!string.IsNullOrEmpty(country) &&
        //                newUser.cityId > 0) //&&
        //                //!string.IsNullOrEmpty(profession))
        //            {


        //                DbInsert insert = new DbInsert(Configuration, _hostingEnvironment);
        //                statusCode = insert.SignUp(newUser);

        //                return statusCode;
        //            }
        //            statusCode.response = 3;//Ошибка параметров
        //            statusCode.responseString = "Parameters are wrong";
        //            return statusCode;

        //        }

        [HttpGet]
        [Route("get/cities")]
       
        public ActionResult<List<CityStruct>> getCities(int countryId)
        {
            //var ipAddress = HttpContext.Connection.RemoteIpAddress;

            //communication.log($"countryID -> {countryId}", "getCities(int countryId)", ipAddress.ToString());
            List<CityStruct> cities = new List<CityStruct>();
            DbSelect select = new DbSelect(Configuration, _hostingEnvironment);
            return select.getCities(countryId);

            
        }
        [HttpGet]
        [Route("get/services")]

        public  ResponseStruct<ServiceStruct> getServices(string userToken, string requestToken,string administrative_area_level_2)
        {
            //var ipAddress = HttpContext.Connection.RemoteIpAddress;

            //communication.log($"countryID -> {countryId}", "getCities(int countryId)", ipAddress.ToString());
           
            DbSelect select = new DbSelect(Configuration, _hostingEnvironment);
            return select.getServices( userToken, requestToken, administrative_area_level_2);


        }
    }
}
