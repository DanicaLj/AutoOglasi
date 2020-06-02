using System;
using System.Collections.Generic;
using System.Net.Http;
using Android;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using AutoMobix.Activities;
using Newtonsoft.Json;

namespace AutoMobix
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar")]
    public class MainActivity : AppCompatActivity, NavigationView.IOnNavigationItemSelectedListener
    {
        private Button btnAdd, btnGetPost,btnExitApp;
        private ListView lstAllPost;
        private List<string> post_details;
        ArrayAdapter<string> arrayAdapter;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            btnAdd = FindViewById<Button>(Resource.Id.btnAdd);
            btnAdd.Click += delegate { StartActivity(typeof(AddPost)); };

            btnGetPost = FindViewById<Button>(Resource.Id.btnGetPost);
            lstAllPost = FindViewById<ListView>(Resource.Id.lstAllPost);
            btnExitApp = FindViewById<Button>(Resource.Id.btnExitApp);

            btnGetPost.Click += async delegate
            {
                try
                {
                    HttpClient client = new HttpClient();
                    client.DefaultRequestHeaders.Add("Accept", "application/json");
                    client.DefaultRequestHeaders.Add("Host", "localhost");
                    var httpResponse = await client.GetStringAsync(@"http://10.0.2.2:44393/api/Post/Get");
                    var posts = JsonConvert.DeserializeObject<List<Post>>(httpResponse);
                    post_details = new List<string>();
                    foreach (var post in posts)
                    {
                        post_details.Add(post.ToString());
                    }
                    arrayAdapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, post_details);
                    lstAllPost.Adapter = arrayAdapter;

                }
                catch (Exception ex)
                {
                    Toast.MakeText(this, ex.Message, ToastLength.Long).Show();
                }
            };

            btnExitApp.Click += delegate {
                this.FinishAffinity();
            };

            lstAllPost.ItemClick += (sender, e) => {
                var intent = new Intent(this, typeof(DetailsPost));
                string data = arrayAdapter.GetItem(e.Position);
                data = data.Replace("\n", string.Empty);
                string[] splitData = data.Split(" ");
                int result = Int32.Parse(splitData[0]);
                intent.PutExtra("selectedItemId", result);
                StartActivity(intent);
            };
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            FloatingActionButton fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
            fab.Click += FabOnClick;

            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            ActionBarDrawerToggle toggle = new ActionBarDrawerToggle(this, drawer, toolbar, Resource.String.navigation_drawer_open, Resource.String.navigation_drawer_close);
            drawer.AddDrawerListener(toggle);
            toggle.SyncState();

            NavigationView navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
            navigationView.SetNavigationItemSelectedListener(this);
        }

        public override void OnBackPressed()
        {
            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            if(drawer.IsDrawerOpen(GravityCompat.Start))
            {
                drawer.CloseDrawer(GravityCompat.Start);
            }
            else
            {
                base.OnBackPressed();
            }
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.action_settings)
            {
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        private void FabOnClick(object sender, EventArgs eventArgs)
        {
            View view = (View) sender;
            Snackbar.Make(view, "Replace with your own action", Snackbar.LengthLong)
                .SetAction("Action", (Android.Views.View.IOnClickListener)null).Show();
        }

        public bool OnNavigationItemSelected(IMenuItem item)
        {
            int id = item.ItemId;

            if (id == Resource.Id.nav_camera)
            {
                // Handle the camera action
            }
            else if (id == Resource.Id.nav_gallery)
            {

            }
            else if (id == Resource.Id.nav_slideshow)
            {

            }
            else if (id == Resource.Id.nav_manage)
            {

            }
            else if (id == Resource.Id.nav_share)
            {

            }
            else if (id == Resource.Id.nav_send)
            {

            }

            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            drawer.CloseDrawer(GravityCompat.Start);
            return true;
        }
    }
}

