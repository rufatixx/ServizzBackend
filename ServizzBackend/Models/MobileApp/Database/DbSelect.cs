using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using ServizzBackend.Model.CommonScripts;
using ServizzBackend.Models.MobileApp.Structs;


namespace ServizzBackend.Models.MobileApp.Database
{
    public class DbSelect
    {
        private readonly string ConnectionString;
        public IConfiguration Configuration;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public DbSelect(IConfiguration configuration, IWebHostEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            ConnectionString = Configuration.GetSection("ConnectionStrings").GetSection("DefaultConnectionString").Value;
            _hostingEnvironment = hostingEnvironment;

        }

        public List<UserStruct> LogIn(string phone, string key)
        {


            List<UserStruct> user = new List<UserStruct>();
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {

                connection.Open();


                using (MySqlCommand com = new MySqlCommand(@"select *,
                     (select balanceValue from users_balance where userId=a.userId) as balance,
                     (select earningValue from users_balance where userId=a.userId) as earning
                     from user a where  email=@login and passwd=SHA2(@pass,512) and isActive=1", connection))
                {
                    com.Parameters.AddWithValue("@login", mail);
                    com.Parameters.AddWithValue("@pass", password);

                    using (MySqlDataReader reader = com.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {


                            while (reader.Read())
                            {

                                UserStruct usr = new UserStruct();

                               
                                usr.name = reader["name"].ToString();
                                usr.surname = reader["surname"].ToString();
                                usr.email = reader["email"].ToString();
                                usr.phone = reader["mobile"].ToString();
                                //usr.birthDate = reader["birthdate"].ToString();
                                //usr.genderID = Convert.ToInt32(reader["genderID"]);
                                usr.cityId = Convert.ToInt32(reader["cityID"]);
                                //usr.professionID = Convert.ToInt32(reader["professionID"]);
                                //usr.regDate = DateTime.Parse(reader["cdate"].ToString());
                                //usr.balance = Convert.ToDecimal(reader["balance"]).ToString("0.000");
                                //usr.earning = Convert.ToDecimal(reader["earning"]).ToString("0.000");
                                user.Add(usr);


                            }
                            //  connection.Close();


                        }
                    }


                    com.Dispose();


                }
                connection.Dispose();
                connection.Close();
            }


            return user;





        }
        public List<CityStruct> getCities(int countryId)

        {
            List<CityStruct> cityList = new List<CityStruct>();


            MySqlConnection connection = new MySqlConnection(ConnectionString);


            connection.Open();

            MySqlCommand com = new MySqlCommand("Select * from city where countryId=@countryId", connection);
            com.Parameters.AddWithValue("@countryId", countryId);



            MySqlDataReader reader = com.ExecuteReader();
            if (reader.HasRows)
            {


                while (reader.Read())
                {

                    CityStruct ads = new CityStruct();
                    ads.ID = Convert.ToInt32(reader["cityId"]);
                    ads.name = reader["name"].ToString();
                    ads.countryID = Convert.ToInt32(reader["countryId"]);



                    cityList.Add(ads);


                }
                connection.Close();
                return cityList;

            }
            else
            {
                connection.Close();
                return cityList;
            }


        }
        public long getUserID(string phone, string key)
        {
            long userID = 0;

            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
               

                try
                {
                    



                    connection.Open();

                    using (MySqlCommand com = new MySqlCommand("select userID from user where email=@mail and passwd=SHA2(@pass,512) limit 1", connection)) {
                        com.Parameters.AddWithValue("@mail", phone);
                        com.Parameters.AddWithValue("@pass", key);
                        MySqlDataReader reader = com.ExecuteReader();
                        if (reader.HasRows)
                        {


                            while (reader.Read())
                            {
                                userID = Convert.ToInt64(reader["userID"]);
                            }
                        }


                        connection.Close();
                        

                    }


                       
                }
                catch
                {
                    connection.Close();
                  

                }
               
            }
            return userID;
        }
        public List<ServiceStruct> getServices(string login,string pass)

        {
            List<ServiceStruct> cityList = new List<ServiceStruct>();
            long userID = getUserID(login, pass);
            if (userID>0)
            {



                using (MySqlConnection connection = new MySqlConnection(ConnectionString)) {

                    connection.Open();

                    using (MySqlCommand com = new MySqlCommand("Select serviceID,(select name from service where serviceid=a.serviceID)as serviceName from relservicecity a where cityId=(select cityId from user where userId = @userID)", connection)) {
                        com.Parameters.AddWithValue("@userID",userID);



                        MySqlDataReader reader = com.ExecuteReader();
                        if (reader.HasRows)
                        {


                            while (reader.Read())
                            {

                                ServiceStruct ads = new ServiceStruct();
                                ads.id = Convert.ToInt32(reader["serviceID"]);
                                ads.name = reader["serviceName"].ToString();
                                //ads.countryID = Convert.ToInt32(reader["countryId"]);



                                cityList.Add(ads);


                            }
                            connection.Close();


                        }
                        else
                        {
                            connection.Close();

                        }


                    }
                    
                }


                

            }
            return cityList;


        }
    }
}
