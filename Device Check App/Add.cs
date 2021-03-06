using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Device_Check_App.Resources.Database;
using Device_Check_App.Resources.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Device_Check_App
{
    [Activity(Label = "Add")]
    public class Add : Activity
    {
        ListView lstViewData;
        List<Device> listSource = new List<Device>();
        Database db;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.add);
            // Create your application here
            


            // Create your application here
            db = new Database();
            db.createDatabase();

            var btnAdd = FindViewById<ImageView>(Resource.Id.addnew);
            var deviceName = FindViewById<EditText>(Resource.Id.deviceName);
            //var deviceStatus = FindViewById<EditText>(Resource.Id.deviceStatus);
            btnAdd.SetImageResource(Resource.Drawable.AddNewDeviceIcon);
            //
            btnAdd.Click += delegate
            {
                Device device = new Device()
                {
                    Device_Name = deviceName.Text,
                    Status = "Available",
                    Borrower = string.Empty,
                    Team_Borrower = string.Empty,
                    Borrowed_Date = string.Empty,
                    Return_Date = System.DateTime.Now.ToString("yyyy-MM-dd"),
                    Reason_Borrow = string.Empty
                };
                db.insertIntoTable(device);
                StartActivity(typeof(Admin_Activity));
            };
        }
    }
}