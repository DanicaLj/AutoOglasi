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
    [Activity(Label = "EditPost")]
    public class EditPost : Activity
    {
        private EditText edtBrend, edtYear, edtDesc;
        private Button btnEdit;
        int UserId;
        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.EditPost);
            edtBrend = FindViewById<EditText>(Resource.Id.edtBrend);
            edtYear = FindViewById<EditText>(Resource.Id.edtYear);
            edtDesc = FindViewById<EditText>(Resource.Id.edtDesc);
            btnEdit = FindViewById<Button>(Resource.Id.btnEdit);
            var selectedItemId = Intent.Extras.GetInt("selectedItemIdToEdit");
            var localIDUser = Application.Context.GetSharedPreferences("Users", FileCreationMode.Private);
            int ID = localIDUser.GetInt("IDUser", 0);
            
            try
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.DefaultRequestHeaders.Add("Host", "localhost");
                var httpResponse = await client.GetStringAsync(@"http://10.0.2.2:44393/api/Post/Get" + "/" + selectedItemId.ToString());
                var posts = JsonConvert.DeserializeObject<List<Post>>(httpResponse);

                foreach (var post in posts)
                {
                    edtBrend.Text = post.Brend.ToString();
                    edtYear.Text = post.Year.ToString();
                    edtDesc.Text = post.Description.ToString();
                    UserId = post.IDUser;
                }

            }
            catch (Exception ex)
            {
                Toast.MakeText(this, ex.Message, ToastLength.Long).Show();
            }

            btnEdit.Click += async delegate
            {
                Post post = new Post();
                post.IDUser = ID;
                post.Description = edtDesc.Text;
                post.Image = "dddd";
                post.Year = Int32.Parse(edtYear.Text);
                post.Brend = edtBrend.Text;
                post.Comment = "dddd";
                
                HttpClient client = new HttpClient();
                HttpResponseMessage httpResponse = new HttpResponseMessage();
                var json = JsonConvert.SerializeObject(post);
                var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.DefaultRequestHeaders.Add("Host", "localhost");
                try
                {
                    httpResponse = await client.PutAsJsonAsync(@"http://10.0.2.2:44393/api/Post/Put" + "/" + selectedItemId.ToString(), post);
                    if (httpResponse.StatusCode == System.Net.HttpStatusCode.OK || httpResponse.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        Toast.MakeText(this, "Your post is edited!", ToastLength.Long).Show();
                        var intent = new Intent(this, typeof(MainActivity));
                        StartActivity(intent);
                    }

                }
                catch (Exception ex)
                {
                    Toast.MakeText(this, "Your post is not saved!", ToastLength.Long).Show();
                }
             
            };

        }
    }
}