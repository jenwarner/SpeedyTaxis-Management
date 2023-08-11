using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebService.Models
{
    public class Driver
    {
        string firstName, surname, driverName, sex, employmentDate, licenceID, address;
        int id;
        long phoneNumber;
        // get and set variables
        public int ID
        {
            get { return id; }
            set { id = value; }
        }
        public long PhoneNumber
        {
            get { return phoneNumber; }
            set { phoneNumber = value; }
        }
        public string FirstName
        {
            get { return firstName; }
            set { firstName = value; }
        }
        public string Surname
        {
            get { return surname; }
            set { surname = value; }
        }
        public string Sex
        {
            get { return sex; }
            set { sex = value; }
        }
        public string EmploymentDate
        {
            get { return employmentDate; }
            set { employmentDate = value; }
        }
        public string LicenceID
        {
            get { return licenceID; }
            set { licenceID = value; }
        }
        public string Address
        {
            get { return address; }
            set { address = value; }
        }
        public string DriverName
        {
            get { return driverName; }
            set { driverName = value; }
        }
        // following constructors intefere with the webservice, resulting in the driver object returning false when posting to database
        public Driver(int id, string fN, string sN, string s, long pN, string eD, string lID, string a)
        {
            ID = id;
            FirstName = fN;
            Surname = sN;
            Sex = s;
            PhoneNumber = pN;
            EmploymentDate = eD;
            LicenceID = lID;
            Address = a;
        }
        public Driver(string fN, string sN, string s, long pN, string eD, string lID, string a)
        {
            FirstName = fN;
            Surname = sN;
            Sex = s;
            PhoneNumber = pN;
            EmploymentDate = eD;
            LicenceID = lID;
            Address = a;
        }

        public Driver(string dN)
        {
            DriverName = dN;
        }
        public Driver(int id, string dN)
        {
            ID = id;
            DriverName = dN;
        }
        public Driver()
        {
        }
    }
}