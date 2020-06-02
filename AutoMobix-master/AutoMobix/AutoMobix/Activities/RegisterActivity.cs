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
    [Activity(Label = "RegisterActivity")]
    public class RegisterActivity : Activity
    {
        private EditText txtEmailreg, txtPasswordreg;
        private Button btnRegisternew;
        private bool userExist;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Register);
            txtEmailreg = FindViewById<EditText>(Resource.Id.txtEmailreg);
            txtPasswordreg = FindViewById<EditText>(Resource.Id.txtPasswordreg);
            btnRegisternew = FindViewById<Button>(Resource.Id.btnRegisternew);

            btnRegisternew.Click += DoRegistryAsync;

        }

        public async void DoRegistryAsync(object sender, EventArgs e)
        {
            User registerUser = new User();
            
            try
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.DefaultRequestHeaders.Add("Host", "localhost");
                var httpResponse = await client.GetStringAsync(@"http://10.0.2.2:44393/api/User/Get");
                var users = JsonConvert.DeserializeObject<List<User>>(httpResponse);

                foreach (var user in users)
                {
                    if (txtEmailreg.Text == user.Email.ToString())
                    {
                        userExist = true;
                    }

                }

            }
            catch (Exception ex)
            {
                Toast.MakeText(this, ex.Message, ToastLength.Long).Show();
            }

            try
            {
                if (!userExist)
                {
                    HttpClient client = new HttpClient();
                    HttpResponseMessage httpResponseRegister = new HttpResponseMessage();
                    client.DefaultRequestHeaders.Add("Accept", "application/json");
                    client.DefaultRequestHeaders.Add("Host", "localhost");
                    registerUser.Email = txtEmailreg.Text;
                    registerUser.Password = txtPasswordreg.Text;
                    httpResponseRegister = await client.PostAsJsonAsync(@"http://10.0.2.2:44393/api/User/Post", registerUser);
                    StartActivity(typeof(LoginActivity));
                }
                else
                {
                    Toast.MakeText(this, "Email already exists!", ToastLength.Long).Show();
                }
            }
            catch(Exception ex)
            {

            }


        }
    }
}