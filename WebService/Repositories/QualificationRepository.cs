using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Web;
using WebService.Models;

namespace WebService.Repositories
{
    public class QualificationRepository
    {
        private static OleDbConnection GetConnection()
        {
            string connString;
            //  change to your connection string in the following line
            // change provider for each computer
            connString = @"Provider=Microsoft.JET.OLEDB.4.0;Data Source=C:\Users\jenz_\Desktop\Computer Science\SpeedyTaxis/speedytaxis.mdb";
            return new OleDbConnection(connString);
        }
        public static bool AddQualificationToDB(Qualification qualification)
        {
            OleDbConnection myConnection = DBConnectivity.GetConnection();

            string myQuery = "INSERT INTO Qualification ([title],[description],[dueDate]) VALUES (@title,@description,@dueDate)";
            OleDbCommand myCommand = new OleDbCommand(myQuery, myConnection);
            myCommand.Parameters.AddWithValue("@title", qualification.Title);
            myCommand.Parameters.AddWithValue("@description", qualification.Description);
            myCommand.Parameters.AddWithValue("@dueDate", qualification.DueDate);

            try
            {
                myConnection.Open();


                myCommand.ExecuteNonQuery();
                myCommand.Dispose();
                myConnection.Close();
                return true;
            }
            catch (Exception)
            {
                // throw;
                return false;
            }
        }

        public static bool DeleteQualificationFromDBWithID(int id)
        {
            OleDbConnection myConnection = DBConnectivity.GetConnection();

            string myQuery = "DELETE * FROM Qualification WHERE ID = @id";
            OleDbCommand myCommand = new OleDbCommand(myQuery, myConnection);
            myCommand.Parameters.AddWithValue("@id", id);
            try
            {
                myConnection.Open();
                myCommand.ExecuteNonQuery();
                return true;

            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception in DBHandler", ex);
                return false;
            }
            finally
            {
                myCommand.Dispose();
                myConnection.Close();
            }
        }

        public static bool UpdateQualificationToDB(Qualification qualification)
        {
            OleDbConnection myConnection = DBConnectivity.GetConnection();

            string myQuery = "UPDATE Qualification SET title = @title, description = @description, dueDate = @date WHERE ID = @id";
            //

            // string myQuery = "UPDATE Driver SET firstName = @fName WHERE ID = @id";
            OleDbCommand myCommand = new OleDbCommand(myQuery, myConnection);
            //myCommand.Parameters.AddWithValue("@id", uN);
            myCommand.Parameters.AddWithValue("@title", qualification.Title);
            myCommand.Parameters.AddWithValue("@description", qualification.Description);
            myCommand.Parameters.AddWithValue("@dueDate", qualification.DueDate);
            myCommand.Parameters.AddWithValue("@id", qualification.ID);

            try
            {
                myConnection.Open();


                myCommand.ExecuteNonQuery();
                myCommand.Dispose();
                myConnection.Close();
                return true;
            }
            catch (Exception)
            {
                // throw;
                return false;
            }
        }
    }
}