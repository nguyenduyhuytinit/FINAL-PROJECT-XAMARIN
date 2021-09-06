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
using System.Net.Mail;
using System.Text;
using static Android.App.DatePickerDialog;

namespace Device_Check_App
{
    [Activity(Label = "Admin_Activity")]
    public class Admin_Activity : Activity, IOnDateSetListener
    {
        ListView listViewData;
        List<Device> listSource = new List<Device>();
        Database db;
        MailMessage mail;
        MailMessage mail1;
        ISharedPreferences session = Application.Context.GetSharedPreferences("filename", FileCreationMode.Private);
        string SESSSION_EMAIL, SESSSION_ROLE;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Create your application here
            SetContentView(Resource.Layout.admin_activity);

            Button btnAdd = FindViewById<Button>(Resource.Id.addpage);
            Button btnDelete = FindViewById<Button>(Resource.Id.remove);
            Button btnAppove = FindViewById<Button>(Resource.Id.btnApproved);
            Button btnDisapproved = FindViewById<Button>(Resource.Id.btnDisApproved);
            var deviceName = FindViewById<TextView>(Resource.Id.deviceName);
            var status = FindViewById<TextView>(Resource.Id.deviceStatus);
            var borrower = FindViewById<EditText>(Resource.Id.borrower);
            var teamName = FindViewById<EditText>(Resource.Id.borrowerTeam);
            var returnDate = FindViewById<EditText>(Resource.Id.returnDate);
            var borrowDate = FindViewById<TextView>(Resource.Id.borrowedDate);
            var reason = FindViewById<EditText>(Resource.Id.reason);
            var userName = FindViewById<TextView>(Resource.Id.userName);
            Button btnLogout = FindViewById<Button>(Resource.Id.btnlogout);


            //get Username
            SESSSION_EMAIL = session.GetString("EMAIL", "");
            userName.Text = SESSSION_EMAIL;
            //Load Database
            db = new Database();
            db.createDatabase();
            listViewData = FindViewById<ListView>(Resource.Id.listView);
            LoadData();
            //Page Naviagtion (Add new Page)
            btnAdd.Click += delegate
            {
                StartActivity(typeof(Add));
            };


            //Button Logout Clicked
            btnLogout.Click += delegate { StartActivity(typeof(LoginActivity)); };

            //Button Delete
            btnDelete.Click += delegate
            {
                Device device = new Device()
                {
                    Id = int.Parse(deviceName.Tag.ToString()),
                    Device_Name = deviceName.Text,
                    Status = status.Text
                };
                db.removeTable(device);
                LoadData();
            };

            //Click Return Date
            returnDate.Click += delegate
            {
                OnClickReturnDateTxt();
            };

            //Button Approved
            btnAppove.Click += delegate
            {
                if (status.Text.Equals("Pending"))
                {
                    Device device = new Device()
                    {
                        Id = int.Parse(deviceName.Tag.ToString()),
                        Device_Name = deviceName.Text,
                        Status = "Borrowed",
                        Borrower = borrower.Text,
                        Team_Borrower = teamName.Text,
                        Borrowed_Date = System.DateTime.Now.ToString("yyyy-MM-dd"),
                        Return_Date = returnDate.Text,
                        Reason_Borrow = reason.Text,
                        Uname = borrower.Text
                    };
                    db.updateTable(device);
                    //Send Mail
                    mail = new MailMessage("xamarinproject111@gmail.com", borrower.Text, "System Notice", "Your Request Was Approved");
                    SmtpClient client = new SmtpClient();
                    client.Host = ("smtp.gmail.com");
                    client.Port = 587;
                    client.Credentials = new System.Net.NetworkCredential("xamarinproject111@gmail.com", "Hoanhdung");
                    client.EnableSsl = true;

                    client.Send(mail);
                    //Load Data
                    LoadData();
                    Toast.MakeText(this, "Susscess borrow this device", ToastLength.Long).Show();
                }
                else
                    Toast.MakeText(this, "Can't borrow because this Device is " + status.Text, ToastLength.Long).Show();

            };

            //Button DisApproved
            btnDisapproved.Click += delegate
            {
                if (status.Text.Equals("Pending"))
                {
                    //Send Mail
                    mail = new MailMessage("xamarinproject111@gmail.com", borrower.Text, "System Notice", "Your Request Was Not Approved");
                    SmtpClient client = new SmtpClient();
                    client.Host = ("smtp.gmail.com");
                    client.Port = 587;
                    client.Credentials = new System.Net.NetworkCredential("xamarinproject111@gmail.com", "Hoanhdung");
                    client.EnableSsl = true;

                    client.Send(mail);

                    Device device = new Device()
                    {

                        Id = int.Parse(deviceName.Tag.ToString()),
                        Device_Name = deviceName.Text,
                        Status = "Available",
                        Borrower = string.Empty,
                        Team_Borrower = string.Empty,
                        Borrowed_Date = string.Empty,
                        Return_Date = System.DateTime.Now.ToString("yyyy-MM-dd"),
                        Reason_Borrow = string.Empty,
                        Uname = string.Empty
                    };
                    db.updateTable(device);
                    
                    //Load Data
                    LoadData();
                    Toast.MakeText(this, "Susscess borrow this device", ToastLength.Long).Show();
                }
                else
                    Toast.MakeText(this, "Can't borrow because this Device is " + status.Text, ToastLength.Long).Show();

            };



            //Item Click
            listViewData.ItemClick += (s, e) =>
            {

                //Set Backround for selected item  
                for (int i = 0; i < listViewData.Count; i++)
                {
                    if (e.Position == i)
                        listViewData.GetChildAt(i).SetBackgroundColor(Android.Graphics.Color.Transparent);

                }
                //Binding Data  
                var txtDeviceName = e.View.FindViewById<TextView>(Resource.Id.txtView_DeviceName);
                var txtstatus = e.View.FindViewById<TextView>(Resource.Id.txtView_Status);
                var txtBorrower = e.View.FindViewById<TextView>(Resource.Id.txtView_Borrower);
                var txtTeam = e.View.FindViewById<TextView>(Resource.Id.txtView_BorrowerTeam);
                var txtReturnDate = e.View.FindViewById<TextView>(Resource.Id.txtView_ReturnDate);
                var txtBorrowDate = e.View.FindViewById<TextView>(Resource.Id.txtView_BorrowerDate);
                var txtReason = e.View.FindViewById<TextView>(Resource.Id.txtView_Reason);
                deviceName.Tag = e.Id;
                deviceName.Text = txtDeviceName.Text;
                status.Text = txtstatus.Text;
                borrower.Text = txtBorrower.Text;
                teamName.Text = txtTeam.Text;
                returnDate.Text = txtReturnDate.Text;
                txtBorrowDate.Text = txtBorrowDate.Text;
                reason.Text = txtReason.Text;
            };
        }



        private void OnClickReturnDateTxt()
        {
            var dateTime = DateTime.Now;
            DatePickerDialog datePicker = new DatePickerDialog(this, this, dateTime.Year, dateTime.Month, dateTime.Day);
            datePicker.Show();
        }

        public void OnDateSet(DatePicker view, int year, int month, int dayOfMonth)
        {
            FindViewById<EditText>(Resource.Id.returnDate).Text = new DateTime(year, month, dayOfMonth).ToShortDateString();
        }

        private void LoadData()
        {

            listSource = db.FindAllStatusPending();
            var adapter = new ListViewAdapter(this, listSource);
            listViewData.Adapter = adapter;
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}