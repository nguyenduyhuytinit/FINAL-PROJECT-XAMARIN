﻿using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using AndroidX.AppCompat.App;
using Device_Check_App.Resources.Database;
using Device_Check_App.Resources.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using static Android.App.DatePickerDialog;

namespace Device_Check_App
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme")]
    public class MainActivity : Activity, IOnDateSetListener
    {
        
        ListView listViewData;
        List<Device> listSource = new List<Device>();
        Database db;
        LoginActivity dbUser;
        MailMessage mail;
        MailMessage mail1;
        Device deivice;
        ISharedPreferencesEditor sessionedditor;
        ISharedPreferences session  = Application.Context.GetSharedPreferences("filename", FileCreationMode.Private);
        string SESSSION_EMAIL, SESSSION_ROLE, SESSION_BORRWEREMAIL;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
             
            Button btnAdd = FindViewById<Button>(Resource.Id.addpage);
            Button btnDelete = FindViewById<Button>(Resource.Id.remove);
            ImageView btnBorrow = FindViewById<ImageView>(Resource.Id.btnborrow);
            ImageView btnReturn = FindViewById<ImageView>(Resource.Id.returnDevice);
            var deviceName = FindViewById<TextView>(Resource.Id.deviceName);
            var status = FindViewById<TextView>(Resource.Id.deviceStatus);
            var borrower = FindViewById<EditText>(Resource.Id.borrower);
            var teamName = FindViewById<EditText>(Resource.Id.borrowerTeam);
            var returnDate = FindViewById<EditText>(Resource.Id.returnDate);
            var borrowDate = FindViewById<TextView>(Resource.Id.borrowedDate);
            var reason = FindViewById<EditText>(Resource.Id.reason);
            var userName = FindViewById<TextView>(Resource.Id.userName);
            ImageView btnLogout = FindViewById<ImageView>(Resource.Id.btnLogOut);

            borrower.Visibility = Android.Views.ViewStates.Invisible;
            //get Username
            SESSSION_EMAIL = session.GetString("EMAIL", "");
            SESSSION_ROLE = session.GetString("ROLE", "");
          
            userName.Text = SESSSION_EMAIL;

            //Button Logout Click
            btnLogout.SetImageResource(Resource.Drawable.SignOutIcon);
            btnLogout.Click += delegate { StartActivity(typeof(LoginActivity)); };

            //Load Database
            db = new Database();
            db.createDatabase();
            listViewData = FindViewById<ListView>(Resource.Id.listView);
            LoadData();

            //Click Return Date
            returnDate.Click += delegate
            {
                OnClickReturnDateTxt();
            };

            //Button Borrow
            btnBorrow.SetImageResource(Resource.Drawable.Borrow);
            btnBorrow.Click += delegate
            {
                if (status.Text.Equals("Available")) 
                {
                    Device device = new Device()
                    {
                        Id = int.Parse(deviceName.Tag.ToString()),
                        Device_Name = deviceName.Text,
                        Status = "Pending",
                        Borrower = session.GetString("EMAIL", ""),
                        Team_Borrower = teamName.Text,
                        Borrowed_Date = System.DateTime.Now.ToString("yyyy-MM-dd"),
                        Return_Date = returnDate.Text,
                        Reason_Borrow = reason.Text,
                        Uname = session.GetString("EMAIL", "")
  

                    };
                    db.updateTable(device);
                    //Send Mail
                    mail = new MailMessage("xamarinproject111@gmail.com", SESSSION_EMAIL, "System Notice", "Susscess borrow this device");
                    SmtpClient client = new SmtpClient();
                    client.Host = ("smtp.gmail.com");
                    client.Port = 587;
                    client.Credentials = new System.Net.NetworkCredential("xamarinproject111@gmail.com", "Hoanhdung");
                    client.EnableSsl = true;

                    client.Send(mail);
                    //Load Data
                    LoadData();
                    Toast.MakeText(this, "Susscess borrow this device", ToastLength.Long).Show();
                }else
                    Toast.MakeText(this, "Can't borrow because this Device is " + status.Text, ToastLength.Long).Show();

            };

            //Button Return
            btnReturn.SetImageResource(Resource.Drawable.ReturnIcon);
            btnReturn.Click += delegate
            {
                if (status.Text.Equals("Pending") || status.Text.Equals("Borrowed"))
                {
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
                    //Send Mail
                    mail1 = new MailMessage("xamarinproject111@gmail.com", borrower.Text, "System Notice", "Susscess return this device");
                    SmtpClient client = new SmtpClient();
                    client.Host = ("smtp.gmail.com");
                    client.Port = 587;
                    client.Credentials = new System.Net.NetworkCredential("xamarinproject111@gmail.com", "Hoanhdung");
                    client.EnableSsl = true;

                    client.Send(mail1);
                    

                    Toast.MakeText(this, "Susscess Return this device", ToastLength.Long).Show();
                }else
                    Toast.MakeText(this, "This device is still available", ToastLength.Long).Show();

            };

            //Item Click
            listViewData.ItemClick += (s, e) =>
            {

                //Set Backround for selected item  
                for (int i = 0; i < listViewData.Count; i++)
                {
                    if (e.Position == i)
                        listViewData.GetChildAt(i).SetBackgroundColor(Android.Graphics.Color.WhiteSmoke);
                    else
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
            string split = SESSSION_EMAIL;

           listSource = db.selectTableByUserName(split);

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