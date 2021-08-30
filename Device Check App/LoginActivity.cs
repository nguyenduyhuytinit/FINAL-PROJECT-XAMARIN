using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using Device_Check_App.Resources.Models;
using SQLite;
using System;
using System.IO;

namespace Device_Check_App
{
    [Activity(Label = "LoginActivity", MainLauncher = true)]
    public class LoginActivity : Activity
    {
        private TextView textViewSignup;
        private TextView textViewForgotPass;
        private EditText editTextLogin;
        private EditText editTextPass;
        private Button loginBtn;
        private Button buttonFacebook;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            // SetContentView(Resource.Layout.forggot_pass);
            SetContentView(Resource.Layout.Login);

            //direct to Sign up
            textViewSignup = FindViewById<TextView>(Resource.Id.textViewSignup);
            textViewSignup.Click += delegate { StartActivity(typeof(ActivitySignup)); };

            // Direct to Forgotpass
            textViewForgotPass = FindViewById<TextView>(Resource.Id.textViewForgotPass);
            textViewForgotPass.Click += delegate { StartActivity(typeof(ActivityForgotPass)); };

            //Button login
            loginBtn = FindViewById<Button>(Resource.Id.buttonLogin1);
            loginBtn.Click += LoginButton_Clicked;



            //Login with Facebook

            buttonFacebook = FindViewById<Button>(Resource.Id.buttonFacebook);
            buttonFacebook.Click += ButtonFacebook_Click;


        }

        private void ButtonFacebook_Click(object sender, EventArgs e)
        {
            Toast.MakeText(this, "Sorry, this function is not work now", ToastLength.Short).Show();
        }



        //Handling Login button->TODO
        private void LoginButton_Clicked(object sender, EventArgs e)
        {

            editTextLogin = FindViewById<EditText>(Resource.Id.editTextLogin);
            editTextPass = FindViewById<EditText>(Resource.Id.editTextPass);


            //Get user from table=[users] === Info entry
            string dpPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "user.db3");
            var db = new SQLiteConnection(dpPath);
            var data = db.Table<User>(); //Call table

            //LINQ querry
            var dataLogin = data.Where(x => x.Email == editTextLogin.Text && x.Password == editTextPass.Text).FirstOrDefault();
            if (editTextLogin.Text != "" && editTextPass.Text != "")
            {
                if (dataLogin != null)
                {
                    Toast.MakeText(this, "Login successful", ToastLength.Short).Show();
                    //TODO            
                    //Redicrect to list devices
                    StartActivity(typeof(MainActivity));
                }
                else
                {
                    Toast.MakeText(this, "Email or Password invalid!", ToastLength.Short).Show();
                }

            }
        }


        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}