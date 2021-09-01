using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Widget;
using Device_Check_App.Resources.Database;
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
     
        ISharedPreferencesEditor session;
        ISharedPreferences sp = Application.Context.GetSharedPreferences("filename", FileCreationMode.Private);
        string SESSSION_EMAIL;
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
            RadioButton radioBtnAdmin = FindViewById<RadioButton>(Resource.Id.radio_admin);
            RadioButton radioBtnUser = FindViewById<RadioButton>(Resource.Id.radio_user);
             Database _db = new Database();

        //Get user from table=[users] === Info entry
        string dpPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "Device.db");
            var db = new SQLiteConnection(dpPath);
            var data = db.Table<User>(); //Call table

            //LINQ querry
            var dataLoginUser = data.Where(x => x.Email == editTextLogin.Text && x.Password == editTextPass.Text && x.Role == "USER").FirstOrDefault();
            var dataLoginAdmin = data.Where(x => x.Email == editTextLogin.Text && x.Password == editTextPass.Text && x.Role == "ADMIN").FirstOrDefault();
            if (editTextLogin.Text != "" && editTextPass.Text != "")
            {
              
                if (dataLoginUser != null )
                {
                    Toast.MakeText(this, "Login successful", ToastLength.Short).Show();
                    //TODO
                    //Save SESSION
                    SESSSION_EMAIL = editTextLogin.Text;
                    //ROLES***************************************************

                    //Add roles
                    session = sp.Edit();
                    session.PutString("EMAIL", SESSSION_EMAIL);
                    session.Commit();
                    //Redicrect to list devices
                    string role = _db.getRole( editTextLogin.Text);
                        StartActivity(typeof(MainActivity));

                }else if(dataLoginAdmin != null)
                {
                    Toast.MakeText(this, "Login successful", ToastLength.Short).Show();
                    //TODO
                    //Save SESSION
                    SESSSION_EMAIL = editTextLogin.Text;
                    //ROLES***************************************************

                    //Add roles
                    session = sp.Edit();
                    session.PutString("EMAIL", SESSSION_EMAIL);
                    session.Commit();
                    //Redicrect to list devices
                    string role = _db.getRole(editTextLogin.Text);
                    StartActivity(typeof(Admin_Activity));
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


        //ROLES***************************************************

        public void RadioButton_Click(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            Toast.MakeText(this, "Your role is  " + rb.Text, ToastLength.Short).Show();
        }
    }

}