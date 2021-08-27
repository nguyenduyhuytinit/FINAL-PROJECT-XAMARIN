using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Device_Check_App.Resources.Models
{
    public class Device
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Indexed]
        public string UserId { get; set; }

        public string Device_Name { get; set; }
        public string Status { get; set; }

        public string Borrower { get; set; }

        public string Team_Borrower { get; set; }

        public string Borrowed_Date { get; set; }

        public string Return_Date { get; set; }

        public string Reason_Borrow { get; set; }



    }
}