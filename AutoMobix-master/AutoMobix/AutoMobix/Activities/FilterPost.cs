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
    [Activity(Label = "FilterPost")]
    public class FilterPost : Activity
    {
        private EditText edtBrndFilter, edtYrFilter;
        private Button btnFilter;
        private ListView lstFilter;
        private List<string> post_details;
        ArrayAdapter<string> arrayAdapter;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.FilterPost);
            lstFilter = FindViewById<ListView>(Resource.Id.lstFilter);
            btnFilter = FindViewById<Button>(Resource.Id.btnFilter);
            edtYrFilter = FindViewById<EditText>(Resource.Id.edtYrFilter);
            edtBrndFilter = FindViewById<EditText>(Resource.Id.edtBrndFilter);

            btnFilter.Click += async delegate
            {
                try
                {
                    HttpClient client = new HttpClient();
                    client.DefaultRequestHeaders.Add("Accept", "application/json");
                    client.DefaultRequestHeaders.Add("Host", "localhost");
                    var httpResponse = await client.GetStringAsync(@"http://10.0.2.2:44393/api/Post/GetPostFilter" + "/" + edtBrndFilter.Text.ToString() + "/" + edtYrFilter.Text.ToString());
                    var posts = JsonConvert.DeserializeObject<List<Post>>(httpResponse);
                    post_details = new List<string>();
                    foreach (var post in posts)
                    {
                        post_details.Add(post.ToString());
                    }
                    arrayAdapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, post_details);
                    lstFilter.Adapter = arrayAdapter;

                }
                catch (Exception ex)
                {
                    Toast.MakeText(this, ex.Message, ToastLength.Long).Show();
                }
            };

            lstFilter.ItemClick += (sender, e) => {
                var intent = new Intent(this, typeof(DetailsPost));
                string data = arrayAdapter.GetItem(e.Position);
                data = data.Replace("\n", string.Empty);
                string[] splitData = data.Split(" ");
                int result = Int32.Parse(splitData[0]);
                intent.PutExtra("selectedItemId", result);
                StartActivity(intent);
            };


        }
    }
}