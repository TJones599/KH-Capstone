using KH_Capstone_DAL.LoggerDAO;
using KH_Capstone_DAL.Mappers;
using KH_Capstone_DAL.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace KH_Capstone_DAL
{
    public class ItemDAO
    {
        private readonly string connectionString;

        public ItemDAO(string connectionString, string logPath)
        {
            this.connectionString = connectionString;
            Logger.logPath = logPath;
        }

        public ItemDO ViewItemSingle(int id)
        {
            ItemDO item = new ItemDO();

            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            using (SqlCommand sqlCMD = new SqlCommand("ITEM_PULL_SINGLE", sqlCon))
            {
                try
                {
                    sqlCMD.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCMD.Parameters.AddWithValue("ItemID", id);

                    sqlCon.Open();

                    using (SqlDataReader reader = sqlCMD.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            item = Mapper.MapSingleItem(reader);
                        }
                    }
                }
                catch (SqlException sqlEx)
                {
                    Logger.LogSqlException(sqlEx);
                    sqlEx.Data["Logged"] = true;
                    throw sqlEx;
                }
                catch (Exception ex)
                {
                    Logger.LogException(ex);
                    ex.Data["Logged"] = true;
                    throw ex;
                }
            }
            return item;
        }

        public List<ItemDO> ViewAllItems()
        {
            List<ItemDO> items = new List<ItemDO>();

            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            using (SqlCommand sqlCMD = new SqlCommand("ITEM_PULL_ALL", sqlCon))
            {
                try
                {
                    sqlCMD.CommandType = System.Data.CommandType.StoredProcedure;

                    sqlCon.Open();

                    using (SqlDataReader reader = sqlCMD.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            items.Add(Mapper.MapSingleItem(reader));
                        }
                    }
                }
                catch (SqlException sqlEx)
                {
                    Logger.LogSqlException(sqlEx);
                    sqlEx.Data["Logged"] = true;
                    throw sqlEx;
                }
                catch (Exception ex)
                {
                    Logger.LogException(ex);
                    ex.Data["Logged"] = true;
                    throw ex;
                }
            }
            return items;
        }

        public ItemDO ViewItemByName(string itemName)
        {
            ItemDO item = new ItemDO();

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                using (SqlCommand ViewByName = new SqlCommand("ITEM_PULL_BY_NAME", sqlConnection))
                {
                    ViewByName.CommandType = System.Data.CommandType.StoredProcedure;
                    ViewByName.Parameters.AddWithValue("Name", itemName);

                    sqlConnection.Open();
                    using (SqlDataReader reader = ViewByName.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            item = Mapper.MapSingleItem(reader);
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

            return item;
        }

        public void UpdateItem(ItemDO item)
        {
            try
            {

                using (SqlConnection sqlCon = new SqlConnection(connectionString))
                using (SqlCommand sqlCMD = new SqlCommand("ITEM_UPDATE", sqlCon))
                {
                    sqlCMD.CommandType = System.Data.CommandType.StoredProcedure;

                    sqlCMD.Parameters.AddWithValue("ItemID", item.ItemID);
                    sqlCMD.Parameters.AddWithValue("ItemName", item.Name);
                    sqlCMD.Parameters.AddWithValue("Description", item.Description);
                    sqlCMD.Parameters.AddWithValue("Image", item.ImagePath);
                    sqlCMD.Parameters.AddWithValue("Purchasable", item.Purchasable);
                    sqlCMD.Parameters.AddWithValue("Validate", item.Validated);

                    sqlCon.Open();
                    sqlCMD.ExecuteNonQuery();
                }
            }
            catch (SqlException sqlEx)
            {
                Logger.LogSqlException(sqlEx);
                sqlEx.Data["Logged"] = true;
                throw sqlEx;
            }
        }

        public void DeleteItem(int id)
        {
            try
            {
                using (SqlConnection sqlCon = new SqlConnection(connectionString))
                using (SqlCommand sqlCMD = new SqlCommand("ITEM_DELETE", sqlCon))
                {
                    sqlCMD.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCMD.Parameters.AddWithValue("ItemID", id);

                    sqlCon.Open();
                    sqlCMD.ExecuteNonQuery();
                }
            }
            catch (SqlException sqlEx)
            {
                Logger.LogSqlException(sqlEx);
                sqlEx.Data["Logged"] = true;
                throw sqlEx;
            }
        }

        public void CreateNewItem(ItemDO item)
        {
            try
            {

                using (SqlConnection sqlCon = new SqlConnection(connectionString))
                using (SqlCommand sqlCMD = new SqlCommand("ITEM_NEW", sqlCon))
                {
                    sqlCMD.CommandType = System.Data.CommandType.StoredProcedure;

                    sqlCMD.Parameters.AddWithValue("ItemName", item.Name);
                    sqlCMD.Parameters.AddWithValue("Description", item.Description);
                    sqlCMD.Parameters.AddWithValue("Image", item.ImagePath);
                    sqlCMD.Parameters.AddWithValue("Purchasable", item.Purchasable);
                    sqlCMD.Parameters.AddWithValue("Validate", item.Validated);

                    sqlCon.Open();
                    sqlCMD.ExecuteNonQuery();
                }
            }
            catch (SqlException sqlEx)
            {
                Logger.LogSqlException(sqlEx);
                sqlEx.Data["Logged"] = true;
                throw sqlEx;
            }
        }
    }
}
