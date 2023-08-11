using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Web;

namespace WebService
{
    public class DBConnectivity
    {
        public static OleDbConnection GetConnection()
        {
            string connString;
            //  change to your connection string in the following line
            // change provider for each computer
            connString = @"Provider=Microsoft.JET.OLEDB.4.0;Data Source=C:\Users\jenz_\Desktop\Computer Science\SpeedyTaxis/speedytaxis.mdb";
            return new OleDbConnection(connString);
        }
    }
}