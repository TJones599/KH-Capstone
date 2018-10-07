using KH_Capstone_DAL.Mappers;
using KH_Capstone_DAL.Models;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace KH_Capstone_DAL
{
    public class EnemyDAO
    {
        private readonly string connectionString;
        private readonly string logString;

        public EnemyDAO(string connectionString, string logString)
        {
            this.connectionString = connectionString;
            this.logString = logString;
        }

        public EnemyDO ViewSingleEnemy(int id)
        {
            EnemyDO enemy = new EnemyDO();

            try
            {
                using (SqlConnection sqlCon = new SqlConnection(connectionString))
                using (SqlCommand sqlCMD = new SqlCommand("ENEMY_PULL_SINGLE", sqlCon))
                {
                    sqlCMD.CommandType = System.Data.CommandType.StoredProcedure;

                    sqlCMD.Parameters.AddWithValue("EnemyID", id);

                    sqlCon.Open();

                    using (SqlDataReader reader = sqlCMD.ExecuteReader())
                    {
                        enemy = Mapper.MapSingleEnemy(reader);
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                //ToDo: create logger and log files
            }

            return enemy;
        }

        public List<EnemyDO> ViewAllEnemies()
        {
            List<EnemyDO> enemy = new List<EnemyDO>();

            try
            {
                using (SqlConnection sqlCon = new SqlConnection(connectionString))
                using (SqlCommand sqlCMD = new SqlCommand("ENEMY_PULL_ALL", sqlCon))
                {
                    sqlCMD.CommandType = System.Data.CommandType.StoredProcedure;

                    sqlCon.Open();

                    using (SqlDataReader reader = sqlCMD.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            enemy.Add(Mapper.MapSingleEnemy(reader));
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                //ToDo: create logger and log files
            }

            return enemy;
        }

        public void UpdateEnemy(EnemyDO enemy)
        {
            try
            {
                using (SqlConnection sqlCon = new SqlConnection(connectionString))
                using (SqlCommand sqlCMD = new SqlCommand("ENEMY_UPDATE", sqlCon))
                {
                    sqlCMD.CommandType = System.Data.CommandType.StoredProcedure;

                    sqlCMD.Parameters.AddWithValue("EnemyID", enemy.EnemyID);
                    sqlCMD.Parameters.AddWithValue("Name", enemy.Name);
                    sqlCMD.Parameters.AddWithValue("Location", enemy.Location);
                    sqlCMD.Parameters.AddWithValue("Description", enemy.Description);
                    sqlCMD.Parameters.AddWithValue("ImagePath", enemy.ImagePath);
                    sqlCMD.Parameters.AddWithValue("Validated", enemy.Validated);

                    sqlCon.Open();
                    sqlCMD.ExecuteNonQuery();
                }
                //ToDo: create sql connection and map to do object
            }
            catch (SqlException sqlEx)
            {
                //ToDo: create logger and log files
            }
        }

        public void DeleteEnemy(int id)
        {
            try
            {
                using (SqlConnection sqlCon = new SqlConnection(connectionString))
                using (SqlCommand sqlCMD = new SqlCommand("ENEMY_DELETE", sqlCon))
                {
                    sqlCMD.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCMD.Parameters.AddWithValue("EnemyID", id);

                    sqlCon.Open();
                    sqlCMD.ExecuteNonQuery();
                }
                //ToDo: create sql connection and map to do object
            }
            catch (SqlException sqlEx)
            {
                //ToDo: create logger and log files
            }
        }

        public void NewEnemy(EnemyDO enemy)
        {
            try
            {
                using (SqlConnection sqlCon = new SqlConnection(connectionString))
                using (SqlCommand sqlCMD = new SqlCommand("ENEMY_NEW", sqlCon))
                {
                    sqlCMD.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCMD.Parameters.AddWithValue("Name", enemy.Name);
                    sqlCMD.Parameters.AddWithValue("Location", enemy.Location);
                    sqlCMD.Parameters.AddWithValue("Description", enemy.Description);
                    sqlCMD.Parameters.AddWithValue("ImagePath", enemy.ImagePath);
                    sqlCMD.Parameters.AddWithValue("Validated", enemy.Validated);

                    sqlCon.Open();
                    sqlCMD.ExecuteNonQuery();
                }
                //ToDo: create sql connection and map to do object
            }
            catch (SqlException sqlEx)
            {
                //ToDo: create logger and log files
            }
        }
    }
}
