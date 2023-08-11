using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Web;
using WebService.Models;

namespace WebService.Repositories
{
    public class DriverRepository
    {
        private static OleDbConnection GetConnection()
        {
            string connString;
            //  change to your connection string in the following line
            // change provider for each computer
            connString = @"Provider=Microsoft.JET.OLEDB.4.0;Data Source=C:\Users\jenz_\Desktop\Computer Science\SpeedyTaxis/speedytaxis.mdb";
            return new OleDbConnection(connString);
        }
        public static bool AddDriverToDB(Driver driver)
        {
            OleDbConnection myConnection = DBConnectivity.GetConnection();

            string myQuery = "INSERT INTO Driver ([firstName],[surname],[sex],[phoneNumber],[employmentDate],[licenceID],[address]) VALUES (@fName,@lName,@sex,@phoneNumber,@employmentDate,@licenceID,@address)";
            OleDbCommand myCommand = new OleDbCommand(myQuery, myConnection);
            myCommand.Parameters.AddWithValue("@fName", driver.FirstName);
            myCommand.Parameters.AddWithValue("@lName", driver.Surname);
            myCommand.Parameters.AddWithValue("@sex", driver.Sex);
            myCommand.Parameters.AddWithValue("@phoneNumber", driver.PhoneNumber);
            myCommand.Parameters.AddWithValue("@employmentDate", driver.EmploymentDate);
            myCommand.Parameters.AddWithValue("@licenceID", driver.LicenceID);
            myCommand.Parameters.AddWithValue("@address", driver.Address);

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

        public static bool DeleteDriverFromDBWithID(int id)
        {
            OleDbConnection myConnection = DBConnectivity.GetConnection();

            string myQuery = "DELETE * FROM Driver WHERE ID = @id";
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

        public static bool UpdateDriverToDB(Driver driver)
        {
            OleDbConnection myConnection = DBConnectivity.GetConnection();

            string myQuery = "UPDATE Driver SET firstName = @firstName, surname = @surname, sex = @sex, phoneNumber = @phoneNumber, employmentDate = @employmentDate, licenceID = @licenceID, address = @address WHERE ID = @id";
            //

            // string myQuery = "UPDATE Driver SET firstName = @fName WHERE ID = @id";
            OleDbCommand myCommand = new OleDbCommand(myQuery, myConnection);
            //myCommand.Parameters.AddWithValue("@id", uN);
            myCommand.Parameters.AddWithValue("@firstName", driver.FirstName);
            myCommand.Parameters.AddWithValue("@surname", driver.Surname);
            myCommand.Parameters.AddWithValue("@sex", driver.Sex);
            myCommand.Parameters.AddWithValue("@phoneNumber", driver.PhoneNumber);
            myCommand.Parameters.AddWithValue("@employmentDate", driver.EmploymentDate);
            myCommand.Parameters.AddWithValue("@licenceID", driver.LicenceID);
            myCommand.Parameters.AddWithValue("@address", driver.Address);
            myCommand.Parameters.AddWithValue("@id", driver.ID);

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

        public static bool GetDriverIDFromDBWithName(Driver driver)
        {
            int id = 0;
            OleDbConnection myConnection = DBConnectivity.GetConnection();

            string myQuery = "SELECT ID FROM Driver WHERE firstName = @firstName AND surname = @surname";
            OleDbCommand myCommand = new OleDbCommand(myQuery, myConnection);
            myCommand.Parameters.AddWithValue("@firstName", driver.FirstName);
            myCommand.Parameters.AddWithValue("@surname", driver.Surname);
            try
            {
                myConnection.Open();
                id = Convert.ToInt32(myCommand.ExecuteScalar().ToString());
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
    }
}