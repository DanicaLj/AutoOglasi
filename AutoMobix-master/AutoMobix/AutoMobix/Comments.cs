using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace AutoMobix
{
    class Comments
    {
        public int IDUser { get; set; }
        public int IDPost { get; set; }
        public string Comment { get; set; }
    }
}