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



            // Direct to Forgotpass


            //Button login
            ImageView loginBtn = FindViewById<ImageView>(Resource.Id.buttonLogin1);
            loginBtn.SetImageResource(Resource.Drawable.LoginIcon);
            loginBtn.Click += LoginButton_Clicked;

            //Img icon
            ImageView imgIcon = FindViewById<ImageView>(Resource.Id.imgIcon);
            imgIcon.SetImageResource(Resource.Mipmap.ic_launcher_foreground);

            //Login with Facebook

            ImageView buttonFacebook = FindViewById<ImageView>(Resource.Id.buttonFacebook);
            buttonFacebook.SetImageResource(Resource.Drawable.GmailLoginIcon);
            buttonFacebook.Click += ButtonFacebook_Click;

            //BTN IMAGE SINGUP
            ImageView buttonImgSignUp = FindViewById<ImageView>(Resource.Id.btnImageSignup);
            buttonImgSignUp.SetImageResource(Resource.Drawable.add);
            buttonImgSignUp.Click  += delegate { StartActivity(typeof(ActivitySignup)); };


            //Add Addmin Account
            string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "Device.db");
            var db = new SQLiteConnection(dbPath);

                db.CreateTable<User>();
                User admin = new User();
                admin.Email = "admin";
                admin.Password = "admin";
                admin.Role = "ADMIN";
                db.Insert(admin);

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