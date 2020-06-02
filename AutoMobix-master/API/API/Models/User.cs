using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace API.Models
{
    public class User
    {
        public int IDUser { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

    }

    public class CreateUser : User
    {

    }

    public class ReadUser : User
    {
        public ReadUser(DataRow row)
        {
            IDUser = Convert.ToInt32(row["IDUser"]);
            Email = row["Email"].ToString();
            Password = row["Password"].ToString();
        }
        public int IDUser { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

    }
}