using KH_Capstone_DAL.LoggerDAO;
using KH_Capstone_DAL.Mappers;
using KH_Capstone_DAL.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace KH_Capstone_DAL
{
    public class EnemyDAO
    {
        //connection string for the server
        private readonly string connectionString;

        //constructor to gather connection string and logpath and assign them approprietly
        public EnemyDAO(string connectionString, string logPath)
        {
            this.connectionString = connectionString;
            Logger.logPath = logPath;
        }

        /// <summary>
        /// connect to sql server, provides the id to sort by, and pull back a single enemies information
        /// </summary>
        /// <param name="id"></param>
        /// <returns>EnemyDO</returns>
        public EnemyDO ViewSingleEnemy(int id)
        {
            EnemyDO enemy = new EnemyDO();

            //uses try catch to capture sql errors
            try
            {
                //creating an sqlconnection and sqlcommand to access the database
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                using (SqlCommand pullSingleEnemy = new SqlCommand("ENEMY_PULL_SINGLE", sqlConnection))
                {
                    //setting the sqlcommands commandtype to storedprocedure to the server knows that its looking
                    //for a stored procedure and not running the text as a command
                    pullSingleEnemy.CommandType = System.Data.CommandType.StoredProcedure;

                    //providing the method parameter id to the sqlcommand parameter for the stored procedure
                    pullSingleEnemy.Parameters.AddWithValue("EnemyID", id);

                    //opening slqconnection
                    sqlConnection.Open();

                    //using sqldatareader to pull information from the server, row by row
                    using (SqlDataReader reader = pullSingleEnemy.ExecuteReader())
                    {

                        //check to see if the storedprocedure returned anything, and if so, passing that information to the mapper
                        if (reader.Read())
                        {
                            enemy = Mapper.MapSingleEnemy(reader);
                        }
                    }
                }
            }
            //any exceptions occured due to the server connection or command are caught here
            catch (SqlException sqlEx)
            {
                //logs any SqlExceptions and creates a Logged key in the exceptions data dictionary and sets it to true
                Logger.LogSqlException(sqlEx);
                sqlEx.Data["Logged"] = true;

                throw sqlEx;
            }
            //any exceptions occuring for any reason other then a sql exception are caught here, including mapping exceptions
            catch (Exception ex)
            {
                //logs any non-SqlExceptions, creates a Logged key in the exceptions data dictionary and sets it to true.
                Logger.LogException(ex);
                ex.Data["Logged"] = true;
                //then throws the exception
                throw ex;
            }

            //returns enemy information
            return enemy;
        }

        /// <summary>
        /// connect to sql server and pulls back a list of enemy information 
        /// </summary>
        /// <returns>List<EnemyDO></returns>
        public List<EnemyDO> ViewAllEnemies()
        {
            //creating a list of Type EnemyDO to store all the enemies in the database. this will be returned later.
            List<EnemyDO> enemy = new List<EnemyDO>();

            //uses a try catch to capture any sql errors
            try
            {
                //creates a sqlconnection and slqcommand to connect to the server
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                using (SqlCommand pullAllEnemies = new SqlCommand("ENEMY_PULL_ALL", sqlConnection))
                {
                    //sets SqlCommands CommandType to stored procedure so the database knows what its looking at
                    pullAllEnemies.CommandType = System.Data.CommandType.StoredProcedure;

                    //opens connection
                    sqlConnection.Open();

                    //uses SqlDataReader to pull information form the server, row by row
                    using (SqlDataReader reader = pullAllEnemies.ExecuteReader())
                    {
                        //so long as the server is sending information, we'll pass that information to the mapper
                        while (reader.Read())
                        {
                            //creating a temporary EnemyDO variable to store the mapped information to it
                            EnemyDO tempEnemy = Mapper.MapSingleEnemy(reader);
                            //and adding that enemy data to the list created above
                            enemy.Add(tempEnemy);
                        }
                    }
                }
            }
            //catching any exceptions thrown due to our server connection or command
            catch (SqlException sqlEx)
            {
                //logs the sql errors that occure and creates a key Logged in the exceptions data dictionary, setting it to true
                Logger.LogSqlException(sqlEx);
                sqlEx.Data["Logged"] = true;
                //throws the exception
                throw sqlEx;
            }
            //catching any exception that occure for reasons other than our server connection or command
            catch (Exception ex)
            {
                //logs the thrown exception, creates a key Logged in the exceptions data dictionary, and sets it to true
                Logger.LogException(ex);
                ex.Data["Logged"] = true;
                //throws the exception
                throw ex;
            }
            //returns our enemy information list
            return enemy;
        }

        //Update Enemy Method takes in an enemyDO object and provides that information to the server
        //filters by the enemy objects id, to prevent all enemies being updated with the same information

        /// <summary>
        /// takes in an EnemyDO and updates the the enemy in the database based on the EnemyID inside the EnemyDO
        /// </summary>
        /// <param name="enemy"></param>
        public void UpdateEnemy(EnemyDO enemy)
        {
            //try catch to capture sql errors
            try
            {
                //creating a connection and command for our server connection
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                using (SqlCommand UpdateEnemy = new SqlCommand("ENEMY_UPDATE", sqlConnection))
                {
                    //setting SqlCommands CommandType to stored procedure so the database knows what its looking for
                    UpdateEnemy.CommandType = System.Data.CommandType.StoredProcedure;

                    //creating parameters to pass information back to the stored procedure
                    UpdateEnemy.Parameters.AddWithValue("EnemyID", enemy.EnemyID);
                    UpdateEnemy.Parameters.AddWithValue("Name", enemy.Name);
                    UpdateEnemy.Parameters.AddWithValue("Location", enemy.Location);
                    UpdateEnemy.Parameters.AddWithValue("Description", enemy.Description);
                    UpdateEnemy.Parameters.AddWithValue("ImagePath", enemy.ImagePath);
                    UpdateEnemy.Parameters.AddWithValue("Validated", enemy.Validated);

                    //opening the connection
                    sqlConnection.Open();
                    //executes the stored procedure with the given paramters
                    UpdateEnemy.ExecuteNonQuery();
                }
            }
            //catchs any errors caused by the server connection or command
            catch (SqlException sqlEx)
            {
                //logging any sql errors that occur, and creates a key Logged in the exceptions data dictionary, marking it as true
                Logger.LogSqlException(sqlEx);
                sqlEx.Data["Logged"] = true;
                //throws the exception
                throw sqlEx;
            }
        }

        //Delete enemy method, takes in an id, and provides that id to the delete stored procedure in the database to delete an enemy entry

        /// <summary>
        /// Deletes an enemy from the database based on the integer id provided
        /// </summary>
        /// <param name="id"></param>
        public void DeleteEnemy(int id)
        {
            //try catch to capture any sql errors
            try
            {
                //creating an sqlconnection and sqlcommand for our server connection
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                using (SqlCommand DeleteEnemy = new SqlCommand("ENEMY_DELETE", sqlConnection))
                {
                    //setting sqlcommands command type to stored procedure to it knows what to look for
                    DeleteEnemy.CommandType = System.Data.CommandType.StoredProcedure;
                    //providing id to delete
                    DeleteEnemy.Parameters.AddWithValue("EnemyID", id);

                    //opening the connection to the server
                    sqlConnection.Open();
                    //executing the command, using the parameteres provided
                    DeleteEnemy.ExecuteNonQuery();
                }
            }
            //catches any SqlExceptions that occure due to our server connection or command
            catch (SqlException sqlEx)
            {
                //logs the exception and creates a Logged key in the exceptions data dictionary, marking it as true
                Logger.LogSqlException(sqlEx);
                sqlEx.Data["Logged"] = true;
                //throws the exception
                throw sqlEx;
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
                //creating an sqlConnection and SqlCommand for our server connection
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                using (SqlCommand createEnemy = new SqlCommand("ENEMY_NEW", sqlConnection))//name change
                {
                    //marking the SqlCommands command type to stored procedure so the database knows what to look for
                    createEnemy.CommandType = System.Data.CommandType.StoredProcedure;

                    //creating parameters to pass information back to the stored procedure
                    createEnemy.Parameters.AddWithValue("Name", enemy.Name);
                    createEnemy.Parameters.AddWithValue("Location", enemy.Location);
                    createEnemy.Parameters.AddWithValue("Description", enemy.Description);
                    createEnemy.Parameters.AddWithValue("ImagePath", enemy.ImagePath);
                    createEnemy.Parameters.AddWithValue("Validated", enemy.Validated);

                    //opening connecction to the server
                    sqlConnection.Open();
                    //executing the command with the given paramters
                    createEnemy.ExecuteNonQuery();
                }
            }
            //catches any sqlexceptions that occure due to our connection or command
            catch (SqlException sqlEx)
            {
                //logging any sql errors that occure and creates a Logged key in the exceptions data dictionary, marking it as true
                Logger.LogSqlException(sqlEx);
                sqlEx.Data["Logged"] = true;
                //throws the exception
                throw sqlEx;
            }
        }

        public EnemyDO ViewEnemyByName(string name)
        {
            EnemyDO enemyInfo = new EnemyDO();

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                using (SqlCommand pullEnemyByName = new SqlCommand("ENEMY_PULL_BY_NAME", sqlConnection))
                {
                    pullEnemyByName.CommandType = System.Data.CommandType.StoredProcedure;

                    pullEnemyByName.Parameters.AddWithValue("Name", name);

                    sqlConnection.Open();

                    using (SqlDataReader reader = pullEnemyByName.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            enemyInfo = Mapper.MapSingleEnemy(reader);
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                Logger.LogSqlException(sqlEx);
                sqlEx.Data["Logged"] = true;
                throw;
            }
            catch(Exception ex)
            {
                Logger.LogException(ex);
                ex.Data["Logged"] = true;
                throw ex;
            }

            return enemyInfo;
        }
    }
}
