using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineDatingSite.Models
{
    public class LoginModel
    {
        public string connstr = @"Data Source=.\SQLEXPRESS;Initial Catalog=TALK;Persist Security Info=True;User ID=jason;Password=123456";
        public SqlConnection connection;
        public SqlCommand command;
        public SqlDataReader reader;

        public LoginModel()
        {
            connection = new SqlConnection(connstr);

        }
        public bool Login(string username, string password)
        {
            connection.Open();
            command = new SqlCommand($"select*from [personal_information] where [username]='" + username + "'", connection );
            reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                if (reader.Read())
                {
                    if (password == Convert.ToString(reader["password"]))
                    {
                        connection.Close();
                        return true;
                    }
                    else
                    {
                        connection.Close();
                        return false;
                    }
                }
            }
            else
            {
                connection.Close();
                return false;
            }
            connection.Close();
            return false;


        }
    }
}
