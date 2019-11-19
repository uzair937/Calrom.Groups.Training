using System;
using System.Collections.Generic;
using System.Text;
using Calrom.Training.SocialMedia.Database.Models;
using System.Data.SqlClient;
using NHibernate;

namespace Calrom.Training.SocialMedia.Database.NHibernateTools
{
    public class BorkCrud
    {
        private string connectionString = string.Empty;

        public BorkCrud()
        {
            connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=TestDB;Integrated Security=True";
        }

        public int InsertBork(BorkDatabaseModel borkDatabaseModel)
        {
            string sqlQuery = string.Format("Insert into borkDatabaseModel (userId) values ('{0}');" + "Select @@Identity", borkDatabaseModel.UserId, borkDatabaseModel.BorkText);

            SqlConnection sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();

            SqlCommand command = new SqlCommand(sqlQuery, sqlConnection);

            int newId = Convert.ToInt32(command.ExecuteScalar());

            command.Dispose();
            sqlConnection.Close();
            sqlConnection.Dispose();

            return newId;
        }

        public BorkDatabaseModel GetBorkById(int id)
        {
            var result = new BorkDatabaseModel();

            string sqlQuery = $"select * from borkDatabaseModel where userId = {id}";

            SqlConnection sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();

            SqlCommand command = new SqlCommand(sqlQuery, sqlConnection);

            var queryReturn = command.ExecuteScalar();
            if (typeof(queryReturn)) == BorkDatabaseModel)
            {
                result = queryReturn;
            }

            command.Dispose();
            sqlConnection.Close();
            sqlConnection.Dispose();
            return result;
        }
    }
}
