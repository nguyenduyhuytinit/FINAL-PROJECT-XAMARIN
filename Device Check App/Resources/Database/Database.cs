using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Device_Check_App.Resources.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Device_Check_App.Resources.Database
{
    public class Database
    {
        string folder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
        public bool createDatabase()
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "Device.db")))
                {
                    connection.CreateTable<Device>();
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }
        //Add or Insert Operation  

        public bool insertIntoTable(Device device)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "Device.db")))
                {
                    connection.Insert(device);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }
        public List<Device> selectTable()
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "Device.db")))
                {
                    return connection.Table<Device>().ToList();
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return null;
            }
        }
        //SELECT TABLE WHERE STATUS AND USERNAME
        public List<Device> selectTableByUserName(string userName)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "Device.db")))
                {
                    return connection.Table<Device>().Where(x => x.Uname.Equals(userName) || x.Status.Equals("Available")).ToList();
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return null;
            }
        }


        //Edit Operation  

        public bool updateTable(Device device)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "Device.db")))
                {
                    connection.Query<Device>("UPDATE Device set Device_Name=?, Status=?, Borrower=?, Team_Borrower=?, Borrowed_Date=?, Return_Date=?, Reason_Borrow=?, Uname = ? Where Id=?", device.Device_Name, device.Status, device.Borrower, device.Team_Borrower,device.Borrowed_Date, device.Return_Date, device.Reason_Borrow,  device.Uname, device.Id);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }
        //Delete Data Operation  

        public bool removeTable(Device device)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "Device.db")))
                {
                    connection.Delete(device);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }
        //Select Operation  

        public bool selectTable(int Id)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "Device.db")))
                {
                    connection.Query<Device>("SELECT * FROM Device Where Id=?", Id);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }

        //SELECt Email(UserName)
        public string getEmail(int Id)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "Device.db")))
                {
                   return  connection.Query<User>("SELECT Email FROM User Where Id=?", Id).ToString();
                }
                    
            }
            catch (SQLiteException ex)
            {
                
                return Log.Info("SQLiteEx", ex.Message).ToString();
            }
        }
        // Find Status
        public List<Device> FindAllStatusPending()
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "Device.db")))
                {
                    return connection.Query<Device>("SELECT * FROM Device Where Status= ? ", "Pending");
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return null;
            }
        }

        //get Role
        public string getRole( string userName)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "Device.db")))
                {
                    return connection.Query<Device>("SELECT Role FROM users Where Email = ? ", userName).FirstOrDefault().ToString();
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return null;
            }
        }

    }
}