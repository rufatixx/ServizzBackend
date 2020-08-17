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

        //public List<UserStruct> LogIn(string phone, string key)
        //{


        //    List<UserStruct> user = new List<UserStruct>();
        //    using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        //    {

        //        connection.Open();


        //        using (MySqlCommand com = new MySqlCommand(@"select *,
        //             (select balanceValue from users_balance where userId=a.userId) as balance,
        //             (select earningValue from users_balance where userId=a.userId) as earning
        //             from user a where  email=@login and passwd=SHA2(@pass,512) and isActive=1", connection))
        //        {
        //            com.Parameters.AddWithValue("@login", mail);
        //            com.Parameters.AddWithValue("@pass", password);

        //            using (MySqlDataReader reader = com.ExecuteReader())
        //            {
        //                if (reader.HasRows)
        //                {


        //                    while (reader.Read())
        //                    {

        //                        UserStruct usr = new UserStruct();

                               
        //                        usr.name = reader["name"].ToString();
        //                        usr.surname = reader["surname"].ToString();
        //                        usr.email = reader["email"].ToString();
        //                        usr.phone = reader["mobile"].ToString();
        //                        //usr.birthDate = reader["birthdate"].ToString();
        //                        //usr.genderID = Convert.ToInt32(reader["genderID"]);
        //                        usr.cityId = Convert.ToInt32(reader["cityID"]);
        //                        //usr.professionID = Convert.ToInt32(reader["professionID"]);
        //                        //usr.regDate = DateTime.Parse(reader["cdate"].ToString());
        //                        //usr.balance = Convert.ToDecimal(reader["balance"]).ToString("0.000");
        //                        //usr.earning = Convert.ToDecimal(reader["earning"]).ToString("0.000");
        //                        user.Add(usr);


        //                    }
        //                    //  connection.Close();


        //                }
        //            }


        //            com.Dispose();


        //        }
        //        connection.Dispose();
        //        connection.Close();
        //    }


        //    return user;





        //}
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
        public long getUserID(long phone)
        {
            long userID = 0;

            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
               

                try
                {
                    



                    connection.Open();

                    using (MySqlCommand com = new MySqlCommand("select userID from user where mobile=@phone order by userID desc limit 1", connection)) {
                        com.Parameters.AddWithValue("@phone", phone);
                     
                        MySqlDataReader reader = com.ExecuteReader();
                        if (reader.HasRows)
                        {


                            while (reader.Read())
                            {
                                userID = Convert.ToInt32(reader[0]);
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
        public UserStruct getProfile(string userToken,string requestToken)
        {
            Security security = new Security(Configuration, _hostingEnvironment);
            int userID1 = security.selectUserToken(userToken);
            int userID2 = security.selectRequestToken(requestToken);
            UserStruct user = new UserStruct();
            if (userID1 == userID2)
            {
                using (MySqlConnection connection = new MySqlConnection(ConnectionString))
                {


                    try
                    {




                        connection.Open();

                        using (MySqlCommand com = new MySqlCommand("select * from user where userID=@userID order by userID desc limit 1", connection))
                        {
                            com.Parameters.AddWithValue("@userID", userID1);

                            MySqlDataReader reader = com.ExecuteReader();
                            if (reader.HasRows)
                            {
                                

                                while (reader.Read())
                                {
                                    user.name = reader["name"] == DBNull.Value ? "" : reader["name"].ToString();
                                    user.surname = reader["surname"] == DBNull.Value ? "" : reader["surname"].ToString();
                                    user.email = reader["email"] == DBNull.Value ? "" : reader["email"].ToString();
                                    user.phone = reader["mobile"] == DBNull.Value ? "" : reader["mobile"].ToString();
                                }
                                user.response = 0;
                                user.responseString = "OK";
                                user.requestToken = security.requestTokenGenerator(userToken, userID1);
                            }
                            else
                            {
                                user.response = 2;
                                user.responseString = "user not found";
                            }


                            connection.Close();


                        }



                    }
                    catch (Exception ex)
                    {
                        connection.Close();
                        user.response = 1;
                        user.responseString = $"Exception:{ex.Message}";


                    }

                }
            }
            else
            {
                user.response = 3;
                user.responseString = $"Access danied!";
            }
               

        
            return user;
        }
        public ResponseStruct<ServiceStruct> getServices(string userToken, string requestToken,string administrative_area_level_2)

        {
            Security security = new Security(Configuration, _hostingEnvironment);
            int userID1 = security.selectUserToken(userToken);
            int userID2 = security.selectRequestToken(requestToken);
            ResponseStruct<ServiceStruct> serviceResponse = new ResponseStruct<ServiceStruct>();
            serviceResponse.data = new List<ServiceStruct>();
            UserStruct user = new UserStruct();
            try
            {
                if (userID1 == userID2)
                {

                    serviceResponse.status = 1;//authorized

                    using (MySqlConnection connection = new MySqlConnection(ConnectionString))
                    {

                        connection.Open();

                        using (MySqlCommand com = new MySqlCommand(@"select *,
(select name from service where serviceId=a.serviceid)as serviceName,
(select serviceImgUrl from service where serviceId=a.serviceid)as serviceImgUrl from relservicecity a
where cityId = (select CityId from city where administrative_area_level_2 = @administrative_area_level_2)
", connection))
                        {
                            com.Parameters.AddWithValue("@administrative_area_level_2", administrative_area_level_2);



                            MySqlDataReader reader = com.ExecuteReader();
                            if (reader.HasRows)
                            {
                                serviceResponse.status = 1;//authorized and has rows

                                while (reader.Read())
                                {

                                    ServiceStruct service = new ServiceStruct();
                                    service.id = Convert.ToInt32(reader["serviceID"]);
                                    service.name = reader["serviceName"].ToString();
                                    service.serviceImgUrl = reader["serviceImgUrl"].ToString();
                                    //ads.countryID = Convert.ToInt32(reader["countryId"]);



                                    serviceResponse.data.Add(service);


                                }
                                connection.Close();
                               

                            }
                            else
                            {
                               
                                connection.Close();
                                serviceResponse.status = 2;//authorized and no rows

                            }
                            serviceResponse.requestToken = security.requestTokenGenerator(userToken, userID1);

                        }

                    }




                }
                else
                {
                    serviceResponse.status = 3;//access danied
                }
            }
            catch (Exception ex)
            {
                serviceResponse.status = 4; //error
            }
           
            return serviceResponse;


        }
    }
}
