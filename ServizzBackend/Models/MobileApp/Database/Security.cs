using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using ServizzBackend.Models.MobileApp.Structs;

namespace ServizzBackend.Models.MobileApp.Database
{
    public class Security
    {
        private readonly string ConnectionString;
        public IConfiguration Configuration;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public Security(IConfiguration configuration, IWebHostEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            ConnectionString = Configuration.GetSection("ConnectionStrings").GetSection("DefaultConnectionString").Value;
            _hostingEnvironment = hostingEnvironment;

        }
        private String Encypherion1Generator(string phone,string key)
        {
            var SecretKeyword1 = "myNewSecretKeywordForESMobileApplication1";
            var Chyper = "";
            var DT = (DateTime.Now).ToString("yyyy-MM-dd hh:mm:ss");
            DbSelect select = new DbSelect(Configuration,_hostingEnvironment);
          List<UserStruct> userData = select.LogIn(phone,key);
           
            

            if (UserId > 0)
            {
                Chyper = GetMd5Hash(SecretKeyword1 + FIN + DT + UserId.ToString());

                string query = "insert into esmobileapp.encryption_1 (USER_ID,USER_NAME,PASSWORD,DT,CHYPER) values (@userid,@username,@password,@dt,@chyper)";

                MySqlConnection con = new MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString());

                con.Open();
                MySqlCommand ishqebul_insertcmd = new MySqlCommand(query, con);
                ishqebul_insertcmd.Parameters.AddWithValue("userid", UserId);
                ishqebul_insertcmd.Parameters.AddWithValue("username", UserData.EMAIL);
                ishqebul_insertcmd.Parameters.AddWithValue("password", UserData.PASS);
                ishqebul_insertcmd.Parameters.AddWithValue("dt", DT);
                ishqebul_insertcmd.Parameters.AddWithValue("chyper", Chyper);
                ishqebul_insertcmd.ExecuteNonQuery();
            }

            return Chyper;
        }
    }
}
