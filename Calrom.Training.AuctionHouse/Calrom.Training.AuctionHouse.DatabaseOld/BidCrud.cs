using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;

namespace Calrom.Training.AuctionHouse.Database
{
    class BidCrud
    {
        private readonly string connectionString = string.Empty;


        public BidCrud()
        {
            connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=TestDB;Integrated Security=True";
        }

        public int InsertBid(BidDatabaseModel bidDatabaseModel)
        {
            string sqlQuery = string.Format($"Insert into bidDatabaseModel (bidId) values ('{bidDatabaseModel.ItemID}');" + "Select @@Identity", bidDatabaseModel.UserID, bidDatabaseModel.ItemName, bidDatabaseModel.ItemID);

            SqlConnection sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();

            SqlCommand sqlCommand = new SqlCommand(sqlQuery, sqlConnection);

            int newId = Convert.ToInt32(sqlCommand.ExecuteScalar());

            sqlCommand.Dispose();
            sqlConnection.Close();
            sqlConnection.Dispose();

            return newId;
        }

        public BidDatabaseModel GetBidById(int id)
        {
            var result = new BidDatabaseModel();

            string sqlQuery = $"select * from bidDatabaseModel where userId = {id}";

            SqlConnection sqlConnection = new SqlConnection(connectionString);

            SqlCommand sqlCommand = new SqlCommand(sqlQuery, sqlConnection);

            sqlCommand.Dispose();
            
            sqlConnection.Open();

            sqlConnection.Close();

            sqlConnection.Dispose();

            return result;
        }
    }
}
