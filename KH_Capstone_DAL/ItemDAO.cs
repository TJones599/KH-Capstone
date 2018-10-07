using KH_Capstone_DAL.Mappers;
using KH_Capstone_DAL.Models;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace KH_Capstone_DAL
{
    public class ItemDAO
    {
        private readonly string connectionString;
        private readonly string logPath;

        public ItemDAO(string connectionString, string logPath)
        {
            this.connectionString = connectionString;
            this.logPath = logPath;
        }

        public ItemDO ViewItemSingle(int id)
        {
            ItemDO item = new ItemDO();

            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            using (SqlCommand sqlCMD = new SqlCommand("ITEM_PULL_SINGLE", sqlCon))
            {
                sqlCMD.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCMD.Parameters.AddWithValue("ItemID", id);

                sqlCon.Open();

                using (SqlDataReader reader = sqlCMD.ExecuteReader())
                {
                    Mapper.MapSingleItem(reader);
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
            return items;
        }

        public void UpdateItem(ItemDO item)
        {
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            using (SqlCommand sqlCMD = new SqlCommand("ITEM_UPDATE", sqlCon))
            {
                sqlCMD.CommandType = System.Data.CommandType.StoredProcedure;

                sqlCMD.Parameters.AddWithValue("ItemID", item.ItemID );
                sqlCMD.Parameters.AddWithValue("ItemName", item.Name );
                sqlCMD.Parameters.AddWithValue("Description", item.Description );
                sqlCMD.Parameters.AddWithValue("Image", item.ImagePath );
                sqlCMD.Parameters.AddWithValue("Purchaseable", item.Purchasable);
                sqlCMD.Parameters.AddWithValue("Validate", item.Validated );

                sqlCon.Open();
                sqlCMD.ExecuteNonQuery();
            }
        }

        public void DeleteItem(int id)
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

        public void CreateNewItem(ItemDO item)
        {
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            using (SqlCommand sqlCMD = new SqlCommand("ITEM_NEW", sqlCon))
            {
                sqlCMD.CommandType = System.Data.CommandType.StoredProcedure;

                sqlCMD.Parameters.AddWithValue("ItemName", item.Name );
                sqlCMD.Parameters.AddWithValue("Description", item.Description );
                sqlCMD.Parameters.AddWithValue("Image", item.ImagePath );
                sqlCMD.Parameters.AddWithValue("Purchasable", item.Purchasable );
                sqlCMD.Parameters.AddWithValue("Validate", item.Validated );

                sqlCon.Open();
                sqlCMD.ExecuteNonQuery();
            }
        }
    }
}
