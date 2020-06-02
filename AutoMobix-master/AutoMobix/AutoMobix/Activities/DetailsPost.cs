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
    [Activity(Label = "DetailsPost")]
    public class DetailsPost : Activity
    {
        private ListView lstDetailsPost, lstComment;
        private Button btnDeletePost, btnEditPost, btnOpenFilter,btnComment;
        private EditText edtComment;
        private List<string> post_details, post_comments;
        ArrayAdapter<string> arrayAdapter, arrayAdapterComments;
        int UserId;
        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.DetailsPost);

            edtComment = FindViewById<EditText>(Resource.Id.edtComment);
            lstDetailsPost = FindViewById<ListView>(Resource.Id.lstDetailsPost);
            lstComment = FindViewById<ListView>(Resource.Id.lstComment);
            btnDeletePost = FindViewById<Button>(Resource.Id.btnDeletePost);
            btnEditPost = FindViewById<Button>(Resource.Id.btnEditPost);
            btnOpenFilter = FindViewById<Button>(Resource.Id.btnOpenFilter);
            btnComment = FindViewById<Button>(Resource.Id.btnComment);

            var selectedItemId = Intent.Extras.GetInt("selectedItemId");
            var localIDUser = Application.Context.GetSharedPreferences("Users", FileCreationMode.Private);
            int ID = localIDUser.GetInt("IDUser", 0);

            try
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.DefaultRequestHeaders.Add("Host", "localhost");
                var httpResponse = await client.GetStringAsync(@"http://10.0.2.2:44393/api/Post/Get" + "/" + selectedItemId.ToString());
                var httpResponseComment = await client.GetStringAsync(@"http://10.0.2.2:44393/api/Comments/Get" + "/" + selectedItemId.ToString());
                var posts = JsonConvert.DeserializeObject<List<Post>>(httpResponse);
                var comments = JsonConvert.DeserializeObject<List<Comments>>(httpResponseComment);
                post_details = new List<string>();
                post_comments = new List<string>();

                foreach (var post in posts)
                {
                    post_details.Add(post.getDetails());
                    UserId = post.IDUser;
                }
                foreach(var comment in comments)
                {
                    post_comments.Add(comment.Comment);
                }
                arrayAdapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, post_details);
                arrayAdapterComments = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, post_comments);
                lstDetailsPost.Adapter = arrayAdapter;
                lstComment.Adapter = arrayAdapterComments;

            }
            catch (Exception ex)
            {
                Toast.MakeText(this, ex.Message, ToastLength.Long).Show();
            }

            btnDeletePost.Click += async delegate
            {
                if(UserId == ID)
                {
                    try
                    {
                        HttpClient client = new HttpClient();
                        client.DefaultRequestHeaders.Add("Accept", "application/json");
                        client.DefaultRequestHeaders.Add("Host", "localhost");
                        var httpResponse = await client.DeleteAsync(@"http://10.0.2.2:44393/api/Post/Delete" + "/" + selectedItemId.ToString());
                        Toast.MakeText(this, "Your post is deleted!", ToastLength.Long).Show();
                        var intent = new Intent(this, typeof(MainActivity));
                        StartActivity(intent);
                    }
                    catch (Exception ex)
                    {
                        Toast.MakeText(this, ex.Message, ToastLength.Long).Show();
                    }
                }
                else
                {
                    Toast.MakeText(this, "You can't delete this post it isn't yours", ToastLength.Long).Show();
                }

            };

            btnEditPost.Click += delegate
            {
                if(UserId == ID)
                {
                    var intent = new Intent(this, typeof(EditPost));
                    intent.PutExtra("selectedItemIdToEdit", selectedItemId);
                    StartActivity(intent);
                }
                else
                {
                    Toast.MakeText(this, "You can't edit this post it isn't yours", ToastLength.Long).Show();
                }
            };

            btnOpenFilter.Click += delegate
            {
                var intent = new Intent(this, typeof(FilterPost));
                StartActivity(intent);
            };

            btnComment.Click += async delegate
            {
                Comments comment = new Comments();
                comment.IDPost = selectedItemId;
                comment.IDUser = ID;
                comment.Comment = edtComment.Text;

                try
                {
                    HttpClient client = new HttpClient();
                    client.DefaultRequestHeaders.Add("Accept", "application/json");
                    client.DefaultRequestHeaders.Add("Host", "localhost");
                    var httpResponse = await client.PostAsJsonAsync(@"http://10.0.2.2:44393/api/Comments/Post", comment);
                    var intent = new Intent(this, typeof(MainActivity));
                    StartActivity(intent);
                }
                catch (Exception ex)
                {
                   Toast.MakeText(this, ex.Message, ToastLength.Long).Show();
                }

            };

        }
    }
}