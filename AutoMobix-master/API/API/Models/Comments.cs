using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace API.Models
{
    public class Comments
    {
        public int IDPost { get; set; }
        public int IDUser { get; set; }
        public string Comment { get; set; }
    }

    public class CreateComment : Comments
    {

    }

    public class ReadComment : Comments
    {
        public ReadComment(DataRow row)
        {
            IDUser = Convert.ToInt32(row["IDUser"]);
            IDPost = Convert.ToInt32(row["IDPost"]);
            Comment = row["Comment"].ToString();
        }
        public int IDPost { get; set; }
        public int IDUser { get; set; }
        public string Comment { get; set; }

    }
}