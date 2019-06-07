using KH_Capstone_DAL.LoggerDAO;
using KH_Capstone_DAL.Mappers;
using KH_Capstone_DAL.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace KH_Capstone_DAL
{
    public class UserDAO
    {
        private readonly string connectionString;

        public UserDAO(string connectionString, string logPath)
        {
            this.connectionString = connectionString;
            Logger.logPath = logPath;
        }

        public UserDO ViewByUserName(string UserName)
        {
            UserDO user = new UserDO();

            try
            {
                using (SqlConnection sqlCon = new SqlConnection(connectionString))
                using (SqlCommand sqlCMD = new SqlCommand("USERS_VIEW_BY_USERNAME", sqlCon))
                {
                    sqlCMD.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCMD.Parameters.AddWithValue("name", UserName);

                    sqlCon.Open();

                    using (SqlDataReader reader = sqlCMD.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            user = Mapper.MapSingleUser(reader);
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                Logger.LogSqlException(sqlEx);
            }
            return user;
        }

        public UserDO ViewSingleUser(int id)
        {
            UserDO user = new UserDO();

            try
            {
                using (SqlConnection sqlCon = new SqlConnection(connectionString))
                using (SqlCommand sqlCMD = new SqlCommand("USERS_VIEW_SINGLE", sqlCon))
                {
                    sqlCMD.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCMD.Parameters.AddWithValue("UserID", id);

                    sqlCon.Open();

                    using (SqlDataReader reader = sqlCMD.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            user = Mapper.MapSingleUser(reader);
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                Logger.LogSqlException(sqlEx);
            }
            return user;
        }

        public List<UserDO> ViewAllUsers()
        {
            List<UserDO> users = new List<UserDO>();
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                using (SqlCommand sqlCMD = new SqlCommand("USERS_VIEW_ALL", sqlConnection))
                {
                    sqlCMD.CommandType = System.Data.CommandType.StoredProcedure;

                    sqlConnection.Open();

                    using (SqlDataReader reader = sqlCMD.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            users.Add(Mapper.MapSingleUser(reader));
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                Logger.LogSqlException(sqlEx);
            }
            return users;
        }

        public void UpdateUser(UserDO user)
        {
            try
            {
                using (SqlConnection sqlCon = new SqlConnection(connectionString))
                using (SqlCommand sqlCMD = new SqlCommand("USERS_UPDATE", sqlCon))
                {
                    sqlCMD.CommandType = System.Data.CommandType.StoredProcedure;

                    sqlCMD.Parameters.AddWithValue("UserID", user.UserId);
                    sqlCMD.Parameters.AddWithValue("UserName", user.UserName);
                    sqlCMD.Parameters.AddWithValue("Password", user.Password);
                    sqlCMD.Parameters.AddWithValue("Role", user.Role);
                    sqlCMD.Parameters.AddWithValue("FirstName", user.FirstName);
                    sqlCMD.Parameters.AddWithValue("LastName", user.LastName);
                    sqlCMD.Parameters.AddWithValue("Banned", user.Banned);
                    sqlCMD.Parameters.AddWithValue("Salt", user.Salt);

                    sqlCon.Open();
                    sqlCMD.ExecuteNonQuery();
                }
            }
            catch (SqlException sqlEx)
            {
                Logger.LogSqlException(sqlEx);
            }
        }

        public void DeleteUser(int id)
        {
            try
            {
                using (SqlConnection sqlCon = new SqlConnection(connectionString))
                using (SqlCommand sqlCMD = new SqlCommand("USERS_DELETE", sqlCon))
                {
                    sqlCMD.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCMD.Parameters.AddWithValue("UserID", id);

                    sqlCon.Open();
                    sqlCMD.ExecuteNonQuery();
                }
            }
            catch (SqlException sqlEx)
            {
                Logger.LogSqlException(sqlEx);
            }
        }

        public void AccountStatus(int id, bool accountStatus)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            using (SqlCommand UpdateAccountStatus = new SqlCommand("USERS_ACC_STATUS", sqlConnection))
            {
                try
                {
                    UpdateAccountStatus.CommandType = System.Data.CommandType.StoredProcedure;
                    UpdateAccountStatus.Parameters.AddWithValue("UserID", id);
                    UpdateAccountStatus.Parameters.AddWithValue("AccountStatus", accountStatus);

                    sqlConnection.Open();
                    UpdateAccountStatus.ExecuteNonQuery();
                }
                catch (SqlException sqlEx)
                {
                    Logger.LogSqlException(sqlEx);
                    sqlEx.Data["Logged"] = true;
                    throw sqlEx;
                }
            }
        }

        public void CreateNewUser(UserDO user)
        {
            try
            {
                using (SqlConnection sqlCon = new SqlConnection(connectionString))
                using (SqlCommand sqlCMD = new SqlCommand("USERS_CREATE", sqlCon))
                {
                    sqlCMD.CommandType = System.Data.CommandType.StoredProcedure;

                    sqlCMD.Parameters.AddWithValue("UserName", user.UserName);
                    sqlCMD.Parameters.AddWithValue("UserPass", user.Password);
                    sqlCMD.Parameters.AddWithValue("Role", user.Role);
                    sqlCMD.Parameters.AddWithValue("FirstName", user.FirstName);
                    sqlCMD.Parameters.AddWithValue("LastName", user.LastName);
                    sqlCMD.Parameters.AddWithValue("Banned", user.Banned);
                    sqlCMD.Parameters.AddWithValue("Inactive", user.Inactive);
                    sqlCMD.Parameters.AddWithValue("Salt", user.Salt);

                    sqlCon.Open();
                    sqlCMD.ExecuteNonQuery();
                }
            }
            catch (SqlException sqlEx)
            {
                Logger.LogSqlException(sqlEx);
            }
        }

        public List<RoleDO> GetRoleList()
        {
            List<RoleDO> Roles = new List<RoleDO>();
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                using (SqlCommand PullAllRoles = new SqlCommand("ROLES_VIEW_ALL", sqlConnection))
                {
                    PullAllRoles.CommandType = System.Data.CommandType.StoredProcedure;


                    sqlConnection.Open();

                    using (SqlDataReader reader = PullAllRoles.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            RoleDO temp = new RoleDO();
                            temp = Mapper.MapSingleRole(reader);
                            Roles.Add(temp);
                        }
                    }

                }
            }
            catch (SqlException sqlEx)
            {
                Logger.LogSqlException(sqlEx);
            }
            return Roles;
        }
    }
}
