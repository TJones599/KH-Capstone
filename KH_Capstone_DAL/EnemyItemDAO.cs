using KH_Capstone_DAL.LoggerDAO;
using KH_Capstone_DAL.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace KH_Capstone_DAL
{
    public class EnemyItemDAO
    {
        private readonly string connectionString;

        public EnemyItemDAO(string connectionString, string logPath)
        {
            this.connectionString = connectionString;
            Logger.logPath = logPath;
        }

        public void CreateEnemyDetails(EnemyItemDetailsDO enemyDetails)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                using (SqlCommand CreateEnemyDetails = new SqlCommand("ENEMY_ITEM_CREATE", sqlConnection))
                {
                    CreateEnemyDetails.CommandType = CommandType.StoredProcedure;

                    CreateEnemyDetails.Parameters.AddWithValue("EnemyID", enemyDetails.EnemyID);
                    CreateEnemyDetails.Parameters.AddWithValue("ItemID", enemyDetails.ItemID);

                    sqlConnection.Open();
                    CreateEnemyDetails.ExecuteNonQuery();
                }
            }
            catch(SqlException sqlEx)
            {
                Logger.LogSqlException(sqlEx);
            }
        }

        public void DeleteEnemyItems(int id)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                using (SqlCommand DeleteEnemyItem = new SqlCommand("ENEMY_ITEM_DELETE", sqlConnection))
                {
                    DeleteEnemyItem.CommandType = CommandType.StoredProcedure;

                    DeleteEnemyItem.Parameters.AddWithValue("LinkID", id);

                    sqlConnection.Open();
                    DeleteEnemyItem.ExecuteNonQuery();
                }
            }
            catch(SqlException sqlEx)
            {
                Logger.LogSqlException(sqlEx);
            }
        }

        public void UpdateEnemyLink(EnemyItemDetailsDO enemyLink)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                using (SqlCommand UpdateEnemyLink = new SqlCommand("ENEMY_ITEM_UPDATE", sqlConnection))
                {
                    UpdateEnemyLink.CommandType = CommandType.StoredProcedure;

                    UpdateEnemyLink.Parameters.AddWithValue("LinkID",enemyLink.LinkID);
                    UpdateEnemyLink.Parameters.AddWithValue("EnemyID",enemyLink.EnemyID);
                    UpdateEnemyLink.Parameters.AddWithValue("ItemID",enemyLink.ItemID);

                    sqlConnection.Open();
                    UpdateEnemyLink.ExecuteNonQuery();
                }
            }
            catch(SqlException sqlEx)
            {
                Logger.LogSqlException(sqlEx);
            }
        }

        public List<EnemyItemDetailsDO> ViewByEnemyID(int id)
        {
            List<EnemyItemDetailsDO> enemy = new List<EnemyItemDetailsDO>();
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                using (SqlCommand PullEnemyDetails = new SqlCommand("ENEMY_ITEM_PULL_BY_ENEMYID", sqlConnection))
                {
                    PullEnemyDetails.CommandType = CommandType.StoredProcedure;

                    PullEnemyDetails.Parameters.AddWithValue("EnemyID", id);

                    sqlConnection.Open();
                    using (SqlDataReader reader = PullEnemyDetails.ExecuteReader())
                    {
                        while(reader.Read())
                        {
                            EnemyItemDetailsDO temp = Mappers.Mapper.MapSingleEnemyLink(reader);
                            enemy.Add(temp);
                        }
                    }
                }
            }
            catch(SqlException sqlEx)
            {
                Logger.LogSqlException(sqlEx);
            }
            return enemy;
        }

        public List<EnemyItemDetailsDO> ViewByItemID(int id)
        {
            List<EnemyItemDetailsDO> enemy = new List<EnemyItemDetailsDO>();
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                using (SqlCommand ViewByItem = new SqlCommand("ENEMY_ITEM_PULL_BY_ITEMID", sqlConnection))
                {
                    ViewByItem.CommandType = CommandType.StoredProcedure;

                    ViewByItem.Parameters.AddWithValue("ItemID", id);

                    sqlConnection.Open();
                    using (SqlDataReader reader = ViewByItem.ExecuteReader())
                    {
                        while(reader.Read())
                        {
                            EnemyItemDetailsDO temp = Mappers.Mapper.MapSingleEnemyLink(reader);
                            enemy.Add(temp);
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                Logger.LogSqlException(sqlEx);
            }
            return enemy;
        }
    }
}
