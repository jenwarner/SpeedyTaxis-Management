using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Web;
using WebService.Models;

namespace WebService.Repositories
{
    public class DisciplinaryRecordRepository
    {
        private static OleDbConnection GetConnection()
        {
            string connString;
            //  change to your connection string in the following line
            // change provider for each computer
            connString = @"Provider=Microsoft.JET.OLEDB.4.0;Data Source=C:\Users\jenz_\Desktop\Computer Science\SpeedyTaxis/speedytaxis.mdb";
            return new OleDbConnection(connString);
        }

        public static bool AddDisciplinaryRecordToDB(DisciplinaryRecord disciplinaryRecord)
        {
            OleDbConnection myConnection = DBConnectivity.GetConnection();

            string myQuery = "INSERT INTO DisciplinaryRecord ([title],[description],[catecory],[dueDate]) VALUES (@title,@description,@category,@dueDate)";
            OleDbCommand myCommand = new OleDbCommand(myQuery, myConnection);
            myCommand.Parameters.AddWithValue("@title", disciplinaryRecord.Title);
            myCommand.Parameters.AddWithValue("@description", disciplinaryRecord.Description);
            myCommand.Parameters.AddWithValue("@category", disciplinaryRecord.Category);
            myCommand.Parameters.AddWithValue("@dueDate", disciplinaryRecord.DateAdded);

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
        public static bool DeleteDisciplinaryRecordFromDBWithID(int id)
        {
            OleDbConnection myConnection = DBConnectivity.GetConnection();

            string myQuery = "DELETE * FROM DisciplinaryRecord WHERE ID = @id";
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
        public static bool UpdateDisciplinaryRecordToDB(DisciplinaryRecord disciplinaryRecord)
        {
            OleDbConnection myConnection = DBConnectivity.GetConnection();

            string myQuery = "UPDATE DisciplinaryRecord SET title = @title, description = @description, category = @category, dateAdded = @dateAdded WHERE ID = @id";
            //

            // string myQuery = "UPDATE Driver SET firstName = @fName WHERE ID = @id";
            OleDbCommand myCommand = new OleDbCommand(myQuery, myConnection);
            //myCommand.Parameters.AddWithValue("@id", uN);
            myCommand.Parameters.AddWithValue("@title", disciplinaryRecord.Title);
            myCommand.Parameters.AddWithValue("@description", disciplinaryRecord.Description);
            myCommand.Parameters.AddWithValue("@category", disciplinaryRecord.Category);
            myCommand.Parameters.AddWithValue("@dateAdded", disciplinaryRecord.DateAdded);
            myCommand.Parameters.AddWithValue("@id", disciplinaryRecord.ID);

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