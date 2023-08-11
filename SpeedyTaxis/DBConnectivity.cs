using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using System.Data;
using System.Data.OleDb;
using Newtonsoft.Json;
using WebService.Models;

namespace SpeedyTaxis
{
    class DBConnectivity
    {
        /*private static OleDbConnection GetConnection()
        {
            
        }*/

        public static OleDbConnection GetConn()
        {
            string connString;
            //  change to your connection string in the following line
            // change provider for each computer
            connString = @"Provider=Microsoft.JET.OLEDB.4.0;Data Source=C:\Users\jenz_\Desktop\Computer Science\SpeedyTaxis/speedytaxis.mdb";
            return new OleDbConnection(connString);
        }
        public static string ActiveConnection()
        {
            string conn = "";
            OleDbConnection myConnection = GetConn();

            try
            {
                myConnection.Open();
                conn = "Connection to database successful";
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception in DBHandler", ex);
                conn = ex.ToString();
            }
            finally
            {
                myConnection.Close();
            }
            return conn;
        }

        // login admin
        public static string AdminLogin(string un, string pass)
        {
            string status = "";
            int x = 0;
            // Check username against database and login
            OleDbConnection myConnection = GetConn();
            string adminUNQuery = "SELECT count(*) FROM Admin WHERE userName = '" + un + "'";
            OleDbCommand adminUNCommand = new OleDbCommand(adminUNQuery, myConnection);

            try
            {
                myConnection.Open();
                x = Convert.ToInt32(adminUNCommand.ExecuteScalar().ToString());
                if (x > 0)
                {
                    string adminPWQuery = "SELECT password FROM Admin WHERE userName = '" + un + "'";
                    OleDbCommand adminPWCommand = new OleDbCommand(adminPWQuery, myConnection);
                    string adminPassword = adminPWCommand.ExecuteScalar().ToString().Replace(" ", "");

                    if (adminPassword == pass)
                    {
                        Admin admin = new Admin();
                        admin.username = un; // stores username written 
                        status = "Login successful!";
                        //Response.Redirect("AdminManage.aspx"); // redirects to page if password is correct
                    }
                    else
                    {
                        status = "Password is not correct!";
                    }

                }
                else if (x == 0)
                {
                    status = "Username is not correct!";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception in DBHandler", ex);
                status = ex.ToString(); ;
            }
            finally
            {
                myConnection.Close();
            }
            return status;
        }
        
        //public static 
        // driver personal details
        // ADD NEW DRIVER DETAILS
        
        public static void AddDriver(string fN, string sN, string s, string pN, string eD, string lID, string a)
        {
            OleDbConnection myConnection = GetConn();

                string myQuery = "INSERT INTO Driver ([firstName],[surname],[sex],[phoneNumber],[employmentDate],[licenceID],[address]) VALUES (@fName,@lName,@sex,@phoneNumber,@employmentDate,@licenceID,@address)";
                OleDbCommand myCommand = new OleDbCommand(myQuery, myConnection);
                myCommand.Parameters.AddWithValue("@fName", fN);
                myCommand.Parameters.AddWithValue("@lName", sN);
                myCommand.Parameters.AddWithValue("@sex", s);
                myCommand.Parameters.AddWithValue("@phoneNumber", pN);
                myCommand.Parameters.AddWithValue("@employmentDate", eD);
                myCommand.Parameters.AddWithValue("@licenceID", lID);
                myCommand.Parameters.AddWithValue("@address", a);

                try
                {
                    myConnection.Open();
                    myCommand.ExecuteNonQuery();
                    Console.WriteLine("Reg successful");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception in DBHandler", ex);
                }
                finally
                {
                    myConnection.Close();
                }

           // return driver;
            
        }
        public static void AddTraining(string t, string d, string date, string time, string c)
        {
            OleDbConnection myConnection = GetConn();

            string myQuery = "INSERT INTO Training ([title],[description],[dueDate],[time],[completed]) VALUES (@title,@description,@dueDate,@time,@completed)";
            OleDbCommand myCommand = new OleDbCommand(myQuery, myConnection);
            myCommand.Parameters.AddWithValue("@title", t);
            myCommand.Parameters.AddWithValue("@description", d);
            myCommand.Parameters.AddWithValue("@dueDate", date);
            myCommand.Parameters.AddWithValue("@time", time);
            myCommand.Parameters.AddWithValue("@completed", c);

            try
            {
                myConnection.Open();
                myCommand.ExecuteNonQuery();
                Console.WriteLine("Reg successful");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception in DBHandler", ex);
            }
            finally
            {
                myConnection.Close();
            }
        }
        public static void AddQualification(string t, string d, string date)
        {
            OleDbConnection myConnection = GetConn();

            string myQuery = "INSERT INTO Qualification ([title],[description],[dueDate]) VALUES (@title,@description,@dueDate)";
            OleDbCommand myCommand = new OleDbCommand(myQuery, myConnection);
            myCommand.Parameters.AddWithValue("@title", t);
            myCommand.Parameters.AddWithValue("@description", d);
            myCommand.Parameters.AddWithValue("@dueDate", date);
            try
            {
                myConnection.Open();
                myCommand.ExecuteNonQuery();
                Console.WriteLine("Reg successful");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception in DBHandler", ex);
            }
            finally
            {
                myConnection.Close();
            }
        }
        
        public static void DeleteDriverFromID(int dID)
        {
            OleDbConnection myConnection = GetConn();

            string myQuery = "DELETE * FROM Driver WHERE dID = @dID";
                OleDbCommand myCommand = new OleDbCommand(myQuery, myConnection);
                myCommand.Parameters.AddWithValue("@dID", dID);
                try
                {
                    myConnection.Open();
                    myCommand.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception in DBHandler", ex);
                }
                finally
                {
                    myConnection.Close();
                }
            
        }
        public static void DeleteQualificationFromID(int ID)
        {
            OleDbConnection myConnection = GetConn();
            // gt: central london

                string myQuery = "DELETE * FROM Qualification WHERE ID = @ID";
                OleDbCommand myCommand = new OleDbCommand(myQuery, myConnection);
                myCommand.Parameters.AddWithValue("@ID", ID);
                try
                {
                    myConnection.Open();
                    myCommand.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception in DBHandler", ex);
                }
                finally
                {
                    myConnection.Close();
                }

        }
        public static void DeleteTrainingFromID(int ID)
        {
            OleDbConnection myConnection = GetConn();

            string myQuery = "DELETE * FROM Training WHERE ID = @ID";
            OleDbCommand myCommand = new OleDbCommand(myQuery, myConnection);
            myCommand.Parameters.AddWithValue("@ID", ID);
            try
            {
                myConnection.Open();
                myCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception in DBHandler", ex);
            }
            finally
            {
                myConnection.Close();
            }

        }

        //
        
        
    }
}
