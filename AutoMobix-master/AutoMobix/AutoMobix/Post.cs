using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace AutoMobix
{
    class Post
    {
        public int IDPost { get; set; }
        public string Image { get; set; }
        public int Year { get; set; }
        public string Brend { get; set; }
        public string Comment { get; set; }
        public string Description { get; set; }
        public int IDUser { get; set; }
        public byte[] RealImage { get; set; }

        public override string ToString()
        {
            return IDPost + " \n" + Brend + " \n" + Year;
        }

        public string getDetails()
        {
            return IDPost + " \n" + Brend + " \n" + Year + " \n" + Description;
        }

        
    }
}