using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace OnlineDatingSite.Models
{
    public class Data//存放使用者資訊的類別
    {
        public string name { get; set; }
        public string password { get; set; }
        public string nickname { get; set; }
        public string sex { get; set; }
        public string address { get; set; }
        public string birth { get; set; }
        public string email { get; set; }
        public string job { get; set; }
        public string interest { get; set; }
        public string resume { get; set; }
        public string personalphoto { get; set; }
        public string album1 { get; set; }
        public string album2 { get; set; }
        public string album3 { get; set; }
        public string album4 { get; set; }
        public string album5 { get; set; }
        public string album6 { get; set; }
        public string album7 { get; set; }
        public string album8 { get; set; }

    }
    public class SInformationModel
    {
        public string connstr = @"Data Source=.\SQLEXPRESS;Initial Catalog=TALK;Persist Security Info=True;User ID=jason;Password=123456";//宣告連接字串
        public SqlConnection connection;//宣告連接
        public SqlCommand command;//宣告

        public SqlDataReader reader;
        public List<Data> DataList = new List<Data>();
        public List<string> InterestList = new List<string>();

        public SInformationModel()
        {
            connection = new SqlConnection(connstr);
        }
        
        public void Select(string username)
        {
            
            connection.Open();
            command = new SqlCommand($"select*from [personal_information] where [username]='" + username + "'", connection);
            reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                if (reader.Read())
                {
                    Data data = new Data
                    {
                        name = reader["name"].ToString(),
                        password = reader["password"].ToString(),
                        nickname = reader["nickname"].ToString(),
                        sex = reader["sex"].ToString(),
                        address = reader["address"].ToString(),
                        birth = reader["birth"].ToString(),
                        email = reader["email"].ToString(),
                        job = reader["job"].ToString(),
                        interest = reader["interest"].ToString(),
                        resume = reader["resume"].ToString(),
                        personalphoto = reader["personalphoto"].ToString()
                    };
                    DataList.Add(data);
                }
            }
            connection.Close();
        }
        public void Alter(string username, string nickname, string address, string job, string interest, string resume, string personalphoto)
        {
            
            connection.Open();
            command = new SqlCommand($"UPDATE [personal_information] SET [nickname]=@nickname,[address]=@address,[job]=@job,[interest]=@interest,[resume]=@resume,[personalphoto]=@personalphoto WHERE username = '" + username + "'", connection);
            command.Parameters.Add(new SqlParameter("@nickname", nickname));
            command.Parameters.Add(new SqlParameter("@address", address));
            command.Parameters.Add(new SqlParameter("@job", job));
            command.Parameters.Add(new SqlParameter("@interest", interest));
            command.Parameters.Add(new SqlParameter("@resume", resume));
            command.Parameters.Add(new SqlParameter("@personalphoto", personalphoto));
            command.ExecuteNonQuery();
            connection.Close();
        }
        public void SelectList()
        {
            connection.Open();
            command = new SqlCommand($"select [interest] from [interest_list]", connection);
            reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    InterestList.Add(reader["interest"].ToString());
                }
            }
            connection.Close();
        }
        public void AlterPassword(string username, string password)
        {
            connection.Open();
            command = new SqlCommand($"UPDATE [personal_information] SET [password]=@password WHERE username = '" + username + "'", connection);
            command.Parameters.Add(new SqlParameter("@password", password));
            command.ExecuteNonQuery();
            connection.Close();
        }
        public void SelectAlbum(string username)
        {
            connection.Open();
            command = new SqlCommand($"select [album1],[album2],[album3],[album4],[album5],[album6],[album7],[album8] from [personal_information] where [username]='" + username + "'", connection);
            reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                if (reader.Read())
                {
                    Data data = new Data
                    {
                        album1 = reader["album1"].ToString(),
                        album2 = reader["album2"].ToString(),
                        album3 = reader["album3"].ToString(),
                        album4 = reader["album4"].ToString(),
                        album5 = reader["album5"].ToString(),
                        album6 = reader["album6"].ToString(),
                        album7 = reader["album7"].ToString(),
                        album8 = reader["album8"].ToString()
                    };
                    DataList.Add(data);
                }
            }
            connection.Close();
        }
        public void AlterAlbum(string username,string album1, string album2, string album3, string album4, string album5, string album6, string album7,string album8)
        {

            connection.Open();
            command = new SqlCommand($"UPDATE [personal_information] SET [album1]=@album1,[album2]=@album2,[album3]=@album3,[album4]=@album4,[album5]=@album5,[album6]=@album6,[album7]=@album7,[album8]=@album8 WHERE username = '" + username + "'", connection);
            command.Parameters.Add(new SqlParameter("@album1", album1));
            command.Parameters.Add(new SqlParameter("@album2", album2));
            command.Parameters.Add(new SqlParameter("@album3", album3));
            command.Parameters.Add(new SqlParameter("@album4", album4));
            command.Parameters.Add(new SqlParameter("@album5", album5));
            command.Parameters.Add(new SqlParameter("@album6", album6));
            command.Parameters.Add(new SqlParameter("@album7", album7));
            command.Parameters.Add(new SqlParameter("@album8", album8));
            command.ExecuteNonQuery();
            connection.Close();
        }
    }
}
