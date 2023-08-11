using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Web;
using WebService.Models;

namespace WebService.Repositories
{
    public class TrainingRepository
    {
        private static OleDbConnection GetConnection()
        {
            string connString;
            //  change to your connection string in the following line
            // change provider for each computer
            connString = @"Provider=Microsoft.JET.OLEDB.4.0;Data Source=C:\Users\jenz_\Desktop\Computer Science\SpeedyTaxis/speedytaxis.mdb";
            return new OleDbConnection(connString);
        }

        public static bool AddTrainingToDB(Training training)
        {
            OleDbConnection myConnection = DBConnectivity.GetConnection();

            string myQuery = "INSERT INTO Training ([title],[description],[dueDate],[time],[completed]) VALUES (@title,@description,@dueDate,@time,@completed)";
            OleDbCommand myCommand = new OleDbCommand(myQuery, myConnection);
            myCommand.Parameters.AddWithValue("@title", training.Title);
            myCommand.Parameters.AddWithValue("@description", training.Description);
            myCommand.Parameters.AddWithValue("@dueDate", training.DueDate);
            myCommand.Parameters.AddWithValue("@time", training.Time);
            myCommand.Parameters.AddWithValue("@completed", training.Completed);

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

        public static bool DeleteTrainingFromDBWithID(int id)
        {
            OleDbConnection myConnection = DBConnectivity.GetConnection();

            string myQuery = "DELETE * FROM Training WHERE ID = @id";
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

        public static bool UpdateTrainingToDB(Training training)
        {
            OleDbConnection myConnection = DBConnectivity.GetConnection();

            string myQuery = "UPDATE Training SET title = @title, description = @description, dueDate = @date, time = @time, completed = @completed WHERE ID = @id";
            //

            // string myQuery = "UPDATE Driver SET firstName = @fName WHERE ID = @id";
            OleDbCommand myCommand = new OleDbCommand(myQuery, myConnection);
            //myCommand.Parameters.AddWithValue("@id", uN);
            myCommand.Parameters.AddWithValue("@title", training.Title);
            myCommand.Parameters.AddWithValue("@description", training.Description);
            myCommand.Parameters.AddWithValue("@dueDate", training.DueDate);
            myCommand.Parameters.AddWithValue("@time", training.Time);
            myCommand.Parameters.AddWithValue("@completed", training.Completed);
            myCommand.Parameters.AddWithValue("@id", training.ID);

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
        public static bool AddTrainingToDBFromDriverLicenceID(Training training, Driver driver)
        {
            OleDbConnection myConnection = DBConnectivity.GetConnection();
            // get driver id from licenceID
            int id = 0;
            string myQuery = "SELECT ID FROM Driver WHERE licenceID = " + driver.LicenceID;
            OleDbCommand myCommand = new OleDbCommand(myQuery, myConnection);

         
            try
            {
                myConnection.Open();
                OleDbDataReader myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    id = int.Parse(myReader["ID"].ToString());
                }
                // add training
                if (training.Type == "advanced driving course")
                {
                    string myQuery0 = "INSERT INTO Training ([title],[dID],[expiryDate]) VALUES (@title,@dID,@expiryDate)";
                    OleDbCommand myCommand0 = new OleDbCommand(myQuery0, myConnection);
                    myCommand0.Parameters.AddWithValue("@title", "Advanced Driving Course");
                    myCommand0.Parameters.AddWithValue("@dID", id);
                    myCommand0.Parameters.AddWithValue("@expiryDate", training.ExpiryDate);

                    myCommand.ExecuteNonQuery();
                }
                else if (training.Type == "driving at night")
                {
                    string myQuery0 = "INSERT INTO Training ([title],[dID],[expiryDate]) VALUES (@title,@dID,@expiryDate)";
                    OleDbCommand myCommand0 = new OleDbCommand(myQuery0, myConnection);
                    myCommand0.Parameters.AddWithValue("@title", "Driving at Night");
                    myCommand0.Parameters.AddWithValue("@dID", id);
                    myCommand0.Parameters.AddWithValue("@expiryDate", training.ExpiryDate);

                    myCommand.ExecuteNonQuery();
                }
                else if (training.Type == "cyclist awareness")
                {
                    string myQuery0 = "INSERT INTO Training ([title],[dID],[expiryDate]) VALUES (@title,@dID,@expiryDate)";
                    OleDbCommand myCommand0 = new OleDbCommand(myQuery0, myConnection);
                    myCommand0.Parameters.AddWithValue("@title", "Cyclist Awareness");
                    myCommand0.Parameters.AddWithValue("@dID", id);
                    myCommand0.Parameters.AddWithValue("@expiryDate", training.ExpiryDate);

                    myCommand.ExecuteNonQuery();
                }
                else if (training.Type == "reduce fuel consumption")
                {
                    string myQuery0 = "INSERT INTO Training ([title],[dID],[expiryDate]) VALUES (@title,@dID,@expiryDate)";
                    OleDbCommand myCommand0 = new OleDbCommand(myQuery0, myConnection);
                    myCommand0.Parameters.AddWithValue("@title", "Reduce Fuel Consumption");
                    myCommand0.Parameters.AddWithValue("@dID", id);
                    myCommand0.Parameters.AddWithValue("@expiryDate", training.ExpiryDate);

                    myCommand.ExecuteNonQuery();
                }

                return true;
            }
            catch (Exception)
            {
                // throw;
                return false;
            }
            finally
            {
                myCommand.Dispose();
                myConnection.Close();
            }
        }
    }
}