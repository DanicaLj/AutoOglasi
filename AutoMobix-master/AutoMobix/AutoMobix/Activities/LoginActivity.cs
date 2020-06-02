using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;

namespace AutoMobix.Activities
{
    [Activity(Label = "LoginActivity", MainLauncher = true)]
    public class LoginActivity : Activity
    {
        EditText email;
        EditText password;
        private Button loginbtn, registerbtn;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Login);

            loginbtn = FindViewById<Button>(Resource.Id.loginbtn);
            email = FindViewById<EditText>(Resource.Id.txtEmail);
            password = FindViewById<EditText>(Resource.Id.txtPassword);

            loginbtn.Click += DoLoginAsync;
            registerbtn = FindViewById<Button>(Resource.Id.register);
            registerbtn.Click += DoRegister;
        }

        private void DoRegister(object sender, EventArgs e)
        {
            StartActivity(typeof(RegisterActivity));
        }

        public async void DoLoginAsync(object sender, EventArgs e)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.DefaultRequestHeaders.Add("Host", "localhost");
                var httpResponse = await client.GetStringAsync(@"http://10.0.2.2:44393/api/User/Get");
                var users = JsonConvert.DeserializeObject<List<User>>(httpResponse);

                foreach (var user in users)
                {
                   if(email.Text == user.Email.ToString() && password.Text == user.Password.ToString())
                   {
                        var localIDUser = Application.Context.GetSharedPreferences("Users", FileCreationMode.Private);
                        var UserEdit = localIDUser.Edit();
                        UserEdit.PutInt("IDUser", user.IDUser);
                        UserEdit.Commit();
                        Toast.MakeText(this, "Login successfully done!", ToastLength.Long).Show();
                        StartActivity(typeof(MainActivity));
                        return;
                   }
                   else
                   {
                        Toast.MakeText(this, "Wrong credentials found!", ToastLength.Long).Show();
                   }

                }

            }
            catch (Exception ex)
            {
                Toast.MakeText(this, ex.Message, ToastLength.Long).Show();
            }
        }

    }
}