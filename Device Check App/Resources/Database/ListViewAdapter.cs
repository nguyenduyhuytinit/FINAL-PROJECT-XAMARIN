using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Device_Check_App.Resources.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Device_Check_App.Resources.Database
{
    public class ViewHolder : Java.Lang.Object
    {
        public TextView txtView_DeviceName { get; set; }
        public TextView txtView_Status { get; set; }
        public TextView txtView_Borrower { get; set; }
        public TextView txtView_BorrowerTeam { get; set; }
        public TextView txtView_BorrowerDate { get; set; }
        public TextView txtView_ReturnDate { get; set; }
        public TextView txtView_Reason { get; set; }

    }

    public class ListViewAdapter : BaseAdapter
    {
        private Activity activity;
        private List<Device> listDevice;
        public ListViewAdapter(Activity activity, List<Device> listDevice)
        {
            this.activity = activity;
            this.listDevice = listDevice;
        }

        public override int Count
        {
            get { return listDevice.Count; }
        }
        public override Java.Lang.Object GetItem(int position)
        {
            return null;
        }

        public override long GetItemId(int position)
        {
            return listDevice[position].Id;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView ?? activity.LayoutInflater.Inflate(Resource.Layout.ListView, parent, false);
            var txtDeviceName = view.FindViewById<TextView>(Resource.Id.txtView_DeviceName);
            var txtStatus = view.FindViewById<TextView>(Resource.Id.txtView_Status);
            var txtBorrower = view.FindViewById<TextView>(Resource.Id.txtView_Borrower);
            var txtBorrowerTeam = view.FindViewById<TextView>(Resource.Id.txtView_BorrowerTeam);
            var txtBorrowDate = view.FindViewById<TextView>(Resource.Id.txtView_BorrowerDate);
            var txtreturnDate = view.FindViewById<TextView>(Resource.Id.txtView_ReturnDate);
            var txtreason = view.FindViewById<TextView>(Resource.Id.txtView_Reason);
            txtDeviceName.Text = listDevice[position].Device_Name;
            txtStatus.Text = listDevice[position].Status;
            txtBorrower.Text = listDevice[position].Borrower;
            txtBorrowerTeam.Text = listDevice[position].Team_Borrower;
            txtBorrowDate.Text = listDevice[position].Borrowed_Date;
            txtreturnDate.Text = listDevice[position].Return_Date;
            txtreason.Text = listDevice[position].Reason_Borrow;

            return view;

        }
    }

}
