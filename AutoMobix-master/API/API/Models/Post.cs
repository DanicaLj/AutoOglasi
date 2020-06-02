using API.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Web;

namespace API.Models
{
    public class Post
    {
        public int IDPost { get; set; }
        public string Image { get; set; }
        public int Year { get; set; }
        public string Brend { get; set; }
        public string Comment { get; set; }
        public string Description { get; set; }
        public int IDUser { get; set; }
        public byte[] RealImage { get; set; }

        [ForeignKey("IDUser")]
        public User User { get; set; }
    }

    public class CreatePost : Post
    {
        public CreatePost()
        {
            Image = "";
            Year = 0;
            Brend = "";
            Comment = "";
        }
    }

    public class ReadPost : Post
    {
        public ReadPost(DataRow row)
        {
            IDPost = Convert.ToInt32(row["IDPost"]);
            Image = row["Image"].ToString();
            Year = Convert.ToInt32(row["Year"]);
            Brend = row["Brend"].ToString();
            Comment = row["Comment"].ToString();
            Description = row["Description"].ToString();
            IDUser = Convert.ToInt32(row["IDUser"]);
            //RealImage = row["RealImage"];
        }
        public int IDPost { get; set; }
        public string Image { get; set; }
        public int Year { get; set; }
        public string Brend { get; set; }
        public string Comment { get; set; }
        public string Description { get; set; }
        public int IDUser { get; set; }
        public byte[] RealImage { get; set; }

    }
}