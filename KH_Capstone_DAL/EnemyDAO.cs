using KH_Capstone_DAL.LoggerDAO;
using KH_Capstone_DAL.Mappers;
using KH_Capstone_DAL.Models;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace KH_Capstone_DAL
{
    public class EnemyDAO
    {
        //connection string for the server
        private readonly string connectionString;

        //constructor to establish connection string and logpath
        public EnemyDAO(string connectionString, string logPath)
        {
            this.connectionString = connectionString;
            Logger.logPath = logPath;
        }


        //View Single Enemy method taking in an interger id
        //connects to the sql server, providing the id to sort by and pulls back an enemies information
        public EnemyDO ViewSingleEnemy(int id)
        {
            EnemyDO enemy = new EnemyDO();

            //uses try catch to capture sql errors
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                using (SqlCommand pullSingleEnemy = new SqlCommand("ENEMY_PULL_SINGLE", sqlConnection))
                {
                    pullSingleEnemy.CommandType = System.Data.CommandType.StoredProcedure;
                    pullSingleEnemy.Parameters.AddWithValue("EnemyID", id);

                    sqlConnection.Open();

                    using (SqlDataReader reader = pullSingleEnemy.ExecuteReader())
                    {
                        if(reader.Read())
                        {
                            enemy = Mapper.MapSingleEnemy(reader);
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                sqlEx.Data["Logged"] = true;
                //logs any captured sql errors
                Logger.LogSqlException(sqlEx);
                throw sqlEx;
            }

            //returns enemy information
            return enemy;
        }

        //View all Enemies method
        //connects to the sql server and pulls back all enemies information
        public List<EnemyDO> ViewAllEnemies()
        {
            List<EnemyDO> enemy = new List<EnemyDO>();

            //uses a try catch to capture any sql errors
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                using (SqlCommand PullAllEnemies = new SqlCommand("ENEMY_PULL_ALL", sqlConnection))
                {
                    PullAllEnemies.CommandType = System.Data.CommandType.StoredProcedure;

                    sqlConnection.Open();

                    using (SqlDataReader reader = PullAllEnemies.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            EnemyDO tempEnemy = Mapper.MapSingleEnemy(reader);
                            enemy.Add(tempEnemy);
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                //logs the sql errors that occure
                Logger.LogSqlException(sqlEx);
            }

            return enemy;
        }

        //Update Enemy Method takes in an enemyDO object and provides that information to the server
        //filters by the enemy objects id, to prevent all enemies being updated with the same information
        public void UpdateEnemy(EnemyDO enemy)
        {
            //try catch to capture sql errors
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                using (SqlCommand UpdateEnemy = new SqlCommand("ENEMY_UPDATE", sqlConnection))
                {
                    UpdateEnemy.CommandType = System.Data.CommandType.StoredProcedure;

                    //creating parameters to pass information back to the stored procedure
                    UpdateEnemy.Parameters.AddWithValue("EnemyID", enemy.EnemyID);
                    UpdateEnemy.Parameters.AddWithValue("Name", enemy.Name);
                    UpdateEnemy.Parameters.AddWithValue("Location", enemy.Location);
                    UpdateEnemy.Parameters.AddWithValue("Description", enemy.Description);
                    UpdateEnemy.Parameters.AddWithValue("ImagePath", enemy.ImagePath);
                    UpdateEnemy.Parameters.AddWithValue("Validated", enemy.Validated);

                    sqlConnection.Open();
                    UpdateEnemy.ExecuteNonQuery();
                }
            }
            catch (SqlException sqlEx)
            {
                //logging any sql errors that occur
                Logger.LogSqlException(sqlEx);
            }
        }

        //Delete enemy method, takes in an id, and provides that id to the delete stored procedure in the database to delete an enemy entry
        public void DeleteEnemy(int id)
        {
            //try catch to capture any sql errors
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                using (SqlCommand DeleteEnemy = new SqlCommand("ENEMY_DELETE", sqlConnection))
                {
                    DeleteEnemy.CommandType = System.Data.CommandType.StoredProcedure;
                    //providing id to delete
                    DeleteEnemy.Parameters.AddWithValue("EnemyID", id);

                    sqlConnection.Open();
                    DeleteEnemy.ExecuteNonQuery();
                }
            }
            catch (SqlException sqlEx)
            {
                Logger.LogSqlException(sqlEx);
            }
        }

        /// <summary>
        /// New Enemy Method, takes in an enemydo object provides that information to the databases create new enemy procedure
        /// </summary>
        /// <param name="enemy">text</param>
        public void NewEnemy(EnemyDO enemy)
        {
            
            //try catch to capture sql errors
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                using (SqlCommand createEnemy = new SqlCommand("ENEMY_NEW", sqlConnection))//name change
                {
                    
                    //creating parameters to pass information back to the stored procedure
                    createEnemy.CommandType = System.Data.CommandType.StoredProcedure;

                    createEnemy.Parameters.AddWithValue("Name", enemy.Name);
                    createEnemy.Parameters.AddWithValue("Location", enemy.Location);
                    createEnemy.Parameters.AddWithValue("Description", enemy.Description);
                    createEnemy.Parameters.AddWithValue("ImagePath", enemy.ImagePath);
                    createEnemy.Parameters.AddWithValue("Validated", enemy.Validated);

                    sqlConnection.Open();
                    createEnemy.ExecuteNonQuery();
                }
            }
            catch (SqlException sqlEx)
            {
                //logging any sql errors that occure
                Logger.LogSqlException(sqlEx);
            }
        }
    }
}
