using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Device_Check_App
{
    [Activity(Label = "ForgotPass")]
    public class ActivityForgotPass : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.forggot_pass);
            // Create your application here
            //ActionBar.SetDisplayHomeAsUpEnabled(true);
        }
    }
}