using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;

namespace AutoMobix.Activities
{
    [Activity(Label = "AddPost")]
    public class AddPost : Activity
    {
        private EditText edtBrnd, edtDescription, edtYr;
        private Button btnSave, btnChoseImage;
        private ImageView ImageView;
        private Android.Net.Uri SelectedImagePath;

        const int Image_Picker_Request = 71;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);


            SetContentView(Resource.Layout.AddPost);
            edtBrnd = FindViewById<EditText>(Resource.Id.edtBrnd);
            edtDescription = FindViewById<EditText>(Resource.Id.edtDescription);
            edtYr = FindViewById<EditText>(Resource.Id.edtYr);
            btnSave = FindViewById<Button>(Resource.Id.btnSave);
            btnChoseImage = FindViewById<Button>(Resource.Id.btnGetImage);
            ImageView = FindViewById<ImageView>(Resource.Id.imgView);

            var localIDUser = Application.Context.GetSharedPreferences("Users", FileCreationMode.Private);
            int ID = localIDUser.GetInt("IDUser", 0);
            btnSave.Click += async delegate
            {
                byte[] image = GetByteArrImageFromUri(SelectedImagePath);
                Post post = new Post();
                post.IDUser = ID;
                post.Description = edtDescription.Text;
                post.Image = "eeee";
                post.Year = Int32.Parse(edtYr.Text);
                post.Brend = edtBrnd.Text;
                post.Comment = "eee";
                post.RealImage = image;

                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/bson"));
                client.DefaultRequestHeaders.Add("Host", "localhost");
                MediaTypeFormatter bsonFormatter = new BsonMediaTypeFormatter();
                try
                {
                    var result = client.PostAsync("http://10.0.2.2:44393/api/Post/Post", post, bsonFormatter).Result;
                    if (result.StatusCode == System.Net.HttpStatusCode.OK || result.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        Clear();
                        Toast.MakeText(this, "Your post is saved!", ToastLength.Long).Show();
                        var intent = new Intent(this, typeof(MainActivity));
                        StartActivity(intent);
                    }
                }
                catch (Exception ex)
                {
                    Toast.MakeText(this, "Your post is not saved!", ToastLength.Long).Show();
                }
               



                //HttpResponseMessage httpResponse = new HttpResponseMessage();
                //var json = JsonConvert.SerializeObject(post);
                //var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
                //client.DefaultRequestHeaders.Add("Accept", "application/json");
                //client.DefaultRequestHeaders.Add("Host", "localhost");
                //try
                //{
                //    httpResponse = await client.PostAsJsonAsync(@"http://10.0.2.2:44393/api/Post/Post", post);
                //    if (httpResponse.StatusCode == System.Net.HttpStatusCode.OK || httpResponse.StatusCode == System.Net.HttpStatusCode.NoContent)
                //    {
                //        Clear();
                //        Toast.MakeText(this, "Your post is saved!", ToastLength.Long).Show();
                //        var intent = new Intent(this, typeof(MainActivity));
                //        StartActivity(intent);
                //    }

                //}
                //catch (Exception ex)
                //{
                //    Toast.MakeText(this, "Your post is not saved!", ToastLength.Long).Show();
                //}
            };

            btnChoseImage.Click += async delegate
            {
                ChoseImage();
            };
        }
        private void ChoseImage()
        {
            Intent choseImageIntent = new Intent();
            choseImageIntent.SetType("image/*");
            choseImageIntent.SetAction(Intent.ActionGetContent);
            StartActivityForResult(Intent.CreateChooser(choseImageIntent, "Select image !"), Image_Picker_Request);
        }
        void Clear()
        {
            edtYr.Text = "";
            edtDescription.Text = "";
            edtBrnd.Text = "";
        }
        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            if (requestCode == Image_Picker_Request && resultCode == Result.Ok
                && data != null && data.Data != null)
            {
                var filePath = data.Data;
                try
                {
                    SelectedImagePath = filePath;
                    Bitmap bitmap = MediaStore.Images.Media.GetBitmap(ContentResolver, filePath);
                    ImageView.SetImageBitmap(bitmap);
                }
                catch (Exception ex) { }
            }
            base.OnActivityResult(requestCode, resultCode, data);
        }
        private byte[] GetByteArrImageFromUri(Android.Net.Uri uri)
        {
            Bitmap bitmap = null;
            byte[] array = null;
            if (uri != null)
            {
                try
                {
                    bitmap = MediaStore.Images.Media.GetBitmap(ContentResolver, uri);
                    MemoryStream stream = new MemoryStream();
                    bitmap.Compress(Bitmap.CompressFormat.Jpeg, 50, stream);
                     array = stream.ToArray();
                }
                catch(Exception ex) { } 
            }
            return array;
        }

    }
}