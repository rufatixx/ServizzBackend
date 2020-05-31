using System;
using System.Net.Mail;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using ServizzBackend.Model.CommonScripts;
using ServizzBackend.Models.MobileApp.Structs;

namespace ServizzBackend.Models.MobileApp.Database
{
    public class DbInsert
    {
        Communications communications;
        private readonly string ConnectionString;
        public IConfiguration Configuration;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public DbInsert(IConfiguration configuration, IWebHostEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            ConnectionString = Configuration.GetSection("ConnectionStrings").GetSection("DefaultConnectionString").Value;
            _hostingEnvironment = hostingEnvironment;
            communications = new Communications(Configuration, _hostingEnvironment);

        }
        static string sha256(string randomString)
        {
            var crypt = new System.Security.Cryptography.SHA256Managed();
            var hash = new System.Text.StringBuilder();
            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(randomString));
            foreach (byte theByte in crypto)
            {
                hash.Append(theByte.ToString("x2"));
            }
            return hash.ToString();
        }
        public bool IsValidEmail(string emailaddress)
        {
            MySqlConnection connection = new MySqlConnection(ConnectionString);
            try
            {
                MailAddress m = new MailAddress(emailaddress);
                connection.Open();

                MySqlCommand com = new MySqlCommand("select * from user where email=@mail and isActive=1", connection);
                com.Parameters.AddWithValue("@mail", emailaddress);
                MySqlDataReader reader = com.ExecuteReader();

                bool except = reader.HasRows;
                connection.Close();
                if (except)
                {

                    return true;
                }

                return false;

            }
            catch (FormatException)
            {
                connection.Close();
                return false;
            }
        }
        public string createCode(int length)
        {
            // const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            const string valid = "1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }
        public StatusStruct SignUp(NewUserStruct newUser)

        {
            StatusStruct statusCode = new StatusStruct();

            if (!IsValidEmail(newUser.email)) // Проверка существования юзера
            {


                try
                {
                    string userToken = createCode(4);


                    DateTime now = DateTime.Now;

                    MySqlConnection connection = new MySqlConnection(ConnectionString);


                    connection.Open();

                    MySqlCommand com = new MySqlCommand("INSERT INTO user (name,surname,email,mobile,passwd,isactive,isblocked,resetrequested,cdate, countryId,cityId,userToken)" +
                        " Values (@name,@surname,@mail,@mobile,SHA2(@pass,512)" +
                        ",0,0,0,@dateTimeNow,1," +
                        "@cityId,SHA2(@userToken,512))", connection);


                    com.Parameters.AddWithValue("@name", newUser.name);
                    com.Parameters.AddWithValue("@surname", newUser.surname);

                    com.Parameters.AddWithValue("@mail", newUser.email);
                    com.Parameters.AddWithValue("@mobile", newUser.phone);
                    com.Parameters.AddWithValue("@pass", newUser.pass);
                    //  com.Parameters.AddWithValue("@birthDate", DateTime.Parse(bDate));
                    //com.Parameters.AddWithValue("@genderName", gender);
                    com.Parameters.AddWithValue("@dateTimeNow", now);
                    //  com.Parameters.AddWithValue("@countryName", country);
                    com.Parameters.AddWithValue("@cityId", newUser.cityId);
                    //com.Parameters.AddWithValue("@professionName", profession);
                    com.Parameters.AddWithValue("@userToken", userToken);
                    com.ExecuteNonQuery();

                    long lastId = com.LastInsertedId;
                    connection.Close();

                    connection.Open();
                    com.CommandText = "insert into users_balance (userId,cdate) values (@userId,@cDate)";
                    com.Parameters.AddWithValue("@userId", lastId);
                    com.Parameters.AddWithValue("@cDate", now);


                    com.ExecuteNonQuery();
                    connection.Close();


                    //communication.sendMail($"Qeydiyyatı tamamlamaq üçün, zəhmət olmasa <a href=\'https://pullu.az/api/androidmobileapp/verify?code={userToken}'>linkə</a> daxil olun", mail);
                    //Communication.sendMail($"Qeydiyyati tamamlamaq ucun shifre: {userToken}", newUser.email);
                    communications.sendSmsAsync($"Qeydiyyati tamamlamaq ucun shifre: {userToken}", newUser.phone);


                    //MailMessage mailMsg = new MailMessage();
                    //SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                    //mailMsg.IsBodyHtml = true;
                    //mailMsg.From = new MailAddress("asadzade99@gmail.com");
                    //mailMsg.To.Add($"{mail}");
                    //mailMsg.Subject = "Pullu (Dəstək)";
                    //mailMsg.Body = $"Qeydiyyatı tamamlamaq üçün, zəhmət olmasa <a href=\'https://pullu.az/api/androidmobileapp/verify?code={userToken}'>linkə</a> daxil olun";

                    //SmtpServer.Port = 587;
                    //SmtpServer.Credentials = new System.Net.NetworkCredential("asadzade99@gmail.com", "odqnmjiogipltmwi");
                    //SmtpServer.EnableSsl = true;

                    //SmtpServer.Send(mailMsg);



                    //string json = "{ \"email\" : \"" + mail + "\", \"password\" : \"" + pass + "\", \" returnSecureToken\" : true }";

                    //var content = new StringContent(json, Encoding.UTF8, "application/json");
                    //string url = "https://www.googleapis.com/identitytoolkit/v3/relyingparty/signupNewUser?key=AIzaSyCwEuju_UmuNNPrYtxEhsuddOfCzqZQ8nI";
                    //HttpClient client = new HttpClient();
                    //var rslt = client.PostAsync(url, content);
                    //var resp = rslt.Result.RequestMessage;

                    statusCode.response = 0; // Все ок
                    statusCode.responseString = "user inserted and code sent"; // Все ок
                    return statusCode;


                }
                catch (Exception ex)
                {
                    statusCode.response = 1;//Ошибка сервера
                    statusCode.responseString = $"Exception message: {ex.Message}";
                    return statusCode;
                }

                //http://127.0.0.1:44301/api/androidmobileapp/user/signUp?mail=asadzade99@gmail.com&username=asa&name=asa&surname=asa&pass=1&phone=1&bdate=1987-08-23&gender=Ki%C5%9Fi&country=Az%C9%99rbaycan&city=Bak%C4%B1&profession=Texnologiya%20sektoru

            }
            else
            {

                statusCode.response = 2;//Юзер существует
                statusCode.responseString = "user is already exist";
                return statusCode;
            }


        }
        public StatusStruct activateUser(int code)
        {
            StatusStruct status = new StatusStruct();
            if (code > 0)
            {


                try
                {


                    using (MySqlConnection connection = new MySqlConnection(ConnectionString))
                    {




                        connection.Open();

                        using (MySqlCommand com = new MySqlCommand("update user set isActive=1,userToken=null where userToken=SHA2(@userToken,512)", connection))
                        {

                            com.Parameters.AddWithValue("@userToken", code);
                            int exist = com.ExecuteNonQuery();
                            if (exist > 0)
                            {
                                status.response = 0;
                                status.responseString = "user activated";
                            }
                            else
                            {
                                status.response = 2;
                                status.responseString = "code is wrong";

                            }

                        }
                        connection.Close();


                    }
                }
                catch (Exception ex)
                {
                    status.response = 1;
                    status.responseString = $"Server error. Exception Message: {ex.Message}";

                }
            }
            else
            {
                status.response = 4;
                status.responseString = "code is empty";
            }
            return status;
        }
    }
}
