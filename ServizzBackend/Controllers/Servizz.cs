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
        [Route("user/login")]
        
        public ActionResult<List<User>> log_in(string mail, string pass)
        {
            //var ipAddress = HttpContext.Connection.RemoteIpAddress;

            //communication.log($"mail -> {mail}\npass ->{security.sha256(pass)}", "log_in(string mail, string pass)", ipAddress.ToString());

            List<User> user = new List<User>();
            if (string.IsNullOrEmpty(mail) || string.IsNullOrEmpty(pass))
            {

                return user;
            }
            else
            {



                DbSelect select = new DbSelect(Configuration, _hostingEnvironment);
                user = select.LogIn(mail, pass);

                return user;
            }
        }
        [HttpPost]
        [Route("user/signUp")]
      
        public ActionResult<Status> signUp([FromBody] NewUser newUser)
        {
//            var ipAddress = HttpContext.Connection.RemoteIpAddress;

//            communications.log(@$"mail ->{mail}\n
//                                name -> {name}\n
//                                surname ->{surname}\n
//                                pass ->{security.sha256(pass)}\n
//                                phone ->{phone}\n
//                                bDate ->{bDate}\n
//                                gender ->{gender}\n
//                                country ->{country}\n
//                                city ->{city}\n
//                                profession ->{profession}
//", "signUp(string mail, string name, string surname, string pass, string phone, string bDate, string gender, string country, string city, string profession)", ipAddress.ToString());
            Status statusCode = new Status();
            if (!string.IsNullOrEmpty(newUser.email) &&
                !string.IsNullOrEmpty(newUser.name) &&
                !string.IsNullOrEmpty(newUser.surname) &&
                !string.IsNullOrEmpty(newUser.pass) &&
                !string.IsNullOrEmpty(newUser.phone) &&
                //!string.IsNullOrEmpty(bDate) &&
                //!string.IsNullOrEmpty(gender) &&
                //!string.IsNullOrEmpty(country) &&
                newUser.cityId > 0) //&&
                //!string.IsNullOrEmpty(profession))
            {


                DbInsert insert = new DbInsert(Configuration, _hostingEnvironment);
                statusCode = insert.SignUp(newUser);

                return statusCode;
            }
            statusCode.response = 3;//Ошибка параметров
            statusCode.responseString = "Parameters are wrong";
            return statusCode;

        }

        [HttpGet]
        [Route("get/cities")]
       
        public ActionResult<List<City>> getCities(int countryId)
        {
            //var ipAddress = HttpContext.Connection.RemoteIpAddress;

            //communication.log($"countryID -> {countryId}", "getCities(int countryId)", ipAddress.ToString());
            List<City> cities = new List<City>();
            DbSelect select = new DbSelect(Configuration, _hostingEnvironment);
            return select.getCities(countryId);

            
        }
        [HttpGet]
        [Route("get/services")]

        public ActionResult<List<City>> getServices(int countryId)
        {
            //var ipAddress = HttpContext.Connection.RemoteIpAddress;

            //communication.log($"countryID -> {countryId}", "getCities(int countryId)", ipAddress.ToString());
            List<City> cities = new List<City>();
            DbSelect select = new DbSelect(Configuration, _hostingEnvironment);
            return select.getCities(countryId);


        }
    }
}
