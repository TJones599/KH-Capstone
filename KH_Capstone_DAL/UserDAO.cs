using KH_Capstone_DAL.Mappers;
using KH_Capstone_DAL.Models;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace KH_Capstone_DAL
{
    public class UserDAO
    {
        private readonly string connectionString;
        private readonly string logPath;

        public UserDAO(string connectionString, string logPath)
        {
            this.connectionString = connectionString;
            this.logPath = logPath;
        }

        public UserDO ViewSingleUser(int id)
        {
            UserDO user = new UserDO();

            try
            {
                using (SqlConnection sqlCon = new SqlConnection(connectionString))
                using (SqlCommand sqlCMD = new SqlCommand("USER_PULL_SINGLE", sqlCon))
                {
                    using (SqlDataReader reader = sqlCMD.ExecuteReader())
                    {
                        user = Mapper.MapSingleUser(reader);
                    }
                }
            }
            catch(SqlException sqlEx)
            {

            }
            return user;
        }

        public List<UserDO> ViewAllUsers()
        {
            List<UserDO> users = new List<UserDO>();

            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            using (SqlCommand sqlCMD = new SqlCommand("USER_PULL_ALL", sqlCon))
            {
                sqlCMD.CommandType = System.Data.CommandType.StoredProcedure;

                using (SqlDataReader reader = sqlCMD.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        users.Add(Mapper.MapSingleUser(reader));
                    }
                }
            }
            return users;
        }

        public void UpdateUser(UserDO user)
        {
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            using (SqlCommand sqlCMD = new SqlCommand("USER_UPDATE", sqlCon))
            {
                sqlCMD.CommandType = System.Data.CommandType.StoredProcedure;

                sqlCMD.Parameters.AddWithValue("RoleID", user.UserId);
                sqlCMD.Parameters.AddWithValue("UserName", user.UserName);
                sqlCMD.Parameters.AddWithValue("Password", user.Password );
                sqlCMD.Parameters.AddWithValue("Role", user.Role);

                sqlCon.Open();
                sqlCMD.ExecuteNonQuery();
            }
        }

        public void DeleteUser(int id)
        {
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            using (SqlCommand sqlCMD = new SqlCommand("USER_DELETE", sqlCon))
            {
                sqlCMD.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCMD.Parameters.AddWithValue("UserID", id);

                sqlCon.Open();
                sqlCMD.ExecuteNonQuery();
            }
        }

        public void CreateNewUser(UserDO user)
        {
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            using (SqlCommand sqlCMD = new SqlCommand("USER_NEW", sqlCon))
            {
                sqlCMD.CommandType = System.Data.CommandType.StoredProcedure;

                sqlCMD.Parameters.AddWithValue("UserName", user.UserName );
                sqlCMD.Parameters.AddWithValue("Password", user.Password );
                sqlCMD.Parameters.AddWithValue("Role", user.Role );

                sqlCon.Open();
                sqlCMD.ExecuteNonQuery();
            }
        }
    }
}
