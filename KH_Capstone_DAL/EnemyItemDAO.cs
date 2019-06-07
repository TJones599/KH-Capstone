using KH_Capstone_DAL.LoggerDAO;
using KH_Capstone_DAL.Models;
using System;
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

        public void CreateEnemyDetails(EnemyItemDO enemyDetails)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                using (SqlCommand CreateEnemyDetails = new SqlCommand("ENEMYDROPS_CREATE", sqlConnection))
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
                sqlEx.Data["Logged"] = true;
                throw sqlEx;
            }
        }

        public void DeleteEnemyItems(int id)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                using (SqlCommand DeleteEnemyItem = new SqlCommand("ENEMYDROPS_DELETE", sqlConnection))
                {
                    DeleteEnemyItem.CommandType = CommandType.StoredProcedure;

                    DeleteEnemyItem.Parameters.AddWithValue("LinkID", id);

                    sqlConnection.Open();
                    DeleteEnemyItem.ExecuteNonQuery();
                }
            }
            catch (SqlException sqlEx)
            {
                Logger.LogSqlException(sqlEx);
                sqlEx.Data["Logged"] = true;
                throw sqlEx;
            }
        }

        public void UpdateEnemyLink(EnemyItemDO enemyLink)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                using (SqlCommand UpdateEnemyLink = new SqlCommand("ENEMYDROPS_UPDATE", sqlConnection))
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
                sqlEx.Data["Logged"] = true;
                throw sqlEx;
            }
        }

        public List<EnemyItemDO> ViewByEnemyID(int id)
        {
            List<EnemyItemDO> enemy = new List<EnemyItemDO>();
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                using (SqlCommand PullEnemyDrops = new SqlCommand("ENEMYDROPS_BY_ENEMYID", sqlConnection))
                {
                    PullEnemyDrops.CommandType = CommandType.StoredProcedure;

                    PullEnemyDrops.Parameters.AddWithValue("EnemyID", id);

                    sqlConnection.Open();
                    using (SqlDataReader reader = PullEnemyDrops.ExecuteReader())
                    {
                        while(reader.Read())
                        {
                            EnemyItemDO temp = Mappers.Mapper.MapSingleEnemyLink(reader);
                            enemy.Add(temp);
                        }
                    }
                }
            }
            catch(SqlException sqlEx)
            {
                Logger.LogSqlException(sqlEx);
                sqlEx.Data["Logged"] = true;
                throw sqlEx;
            }
            catch(Exception ex)
            {
                Logger.LogException(ex);
                ex.Data["Logged"] = true;
                throw ex;
            }
            return enemy;
        }

        public List<EnemyItemDO> ViewByItemID(int id)
        {
            List<EnemyItemDO> enemy = new List<EnemyItemDO>();
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                using (SqlCommand ViewByItem = new SqlCommand("ENEMYDROPS_BY_DROPID", sqlConnection))
                {
                    ViewByItem.CommandType = CommandType.StoredProcedure;

                    ViewByItem.Parameters.AddWithValue("ItemID", id);

                    sqlConnection.Open();
                    using (SqlDataReader reader = ViewByItem.ExecuteReader())
                    {
                        while(reader.Read())
                        {
                            EnemyItemDO temp = Mappers.Mapper.MapSingleEnemyLink(reader);
                            enemy.Add(temp);
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                Logger.LogSqlException(sqlEx);
                sqlEx.Data["Logged"] = true;
                throw sqlEx;
            }
            catch(Exception ex)
            {
                Logger.LogException(ex);
                ex.Data["Logged"] = true;
                throw ex;
            }
            return enemy;
        }

        public List<EnemyItemDO> ViewAllLinks()
        {
            List<EnemyItemDO> fullLinkList = new List<EnemyItemDO>();
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                using (SqlCommand pullAllLinks = new SqlCommand("ENEMYDROPS_VIEW_ALL", sqlConnection))
                {
                    pullAllLinks.CommandType = CommandType.StoredProcedure;

                    sqlConnection.Open();
                    using (SqlDataReader reader = pullAllLinks.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            EnemyItemDO temp = Mappers.Mapper.MapSingleEnemyLink(reader);
                            fullLinkList.Add(temp);
                        }
                    }
                }
            }
            catch(SqlException sqlEx)
            {
                Logger.LogSqlException(sqlEx);
                sqlEx.Data["Logged"] = true;
                throw sqlEx;
            }
            catch(Exception ex)
            {
                Logger.LogException(ex);
                ex.Data["Logged"] = true;
                throw ex;
            }
            return fullLinkList;
        }
    }
}
