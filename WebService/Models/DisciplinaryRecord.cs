using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebService.Models
{
    public class DisciplinaryRecord
    {
        string title, description, dateAdded, category;
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
        public string DateAdded
        {
            get { return dateAdded; }
            set { dateAdded = value; }
        }
        public string Category
        {
            get { return category; }
            set { category = value; }
        }
    }
}