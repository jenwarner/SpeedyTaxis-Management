using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebService.Models
{
    public class Training
    {
        string title, description, dueDate, time, completed, expiryDate, type;
        int id, dId;
        // get and set variables
        public int ID
        {
            get { return id; }
            set { id = value; }
        }
        public int DID
        {
            get { return dId; }
            set { dId = value; }
        }
        public string Title
        {
            get { return title; }
            set { title = value; }
        }
        public string Description
        {
            get { return description; }
            set { description = value; }
        }
        public string DueDate
        {
            get { return dueDate; }
            set { dueDate = value; }
        }
        public string Time
        {
            get { return time; }
            set { time = value; }
        }
        public string Completed
        {
            get { return completed; }
            set { completed = value; }
        }
        public string ExpiryDate
        {
            get { return expiryDate; }
            set { expiryDate = value; }
        }
        public string Type
        {
            get { return type; }
            set { type = value; }
        }
        // following constructors intefere with the webservice, resulting in the driver object returning false when posting to database
        /* public Training(int id, string d, string s, string s, long pN, string eD, string lID, string a)
         {
             ID = id;
             FirstName = fN;
             Surname = sN;
             Sex = s;
             PhoneNumber = pN;
             EmploymentDate = eD;
             string title, description, dueDate, time, completed, expiryDate;
             int id, dId;
         }
         public Training(string fN, string sN, string s, long pN, string eD, string lID, string a)
         {
             FirstName = fN;
             Surname = sN;
             Sex = s;
             PhoneNumber = pN;
             EmploymentDate = eD;
             LicenceID = lID;
             Address = a;
         }*/
        public Training()
        {
        }
    }
}