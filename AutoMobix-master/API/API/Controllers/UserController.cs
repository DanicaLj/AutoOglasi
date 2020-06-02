using API.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace API.Controllers
{
    public class UserController : ApiController
    {
        private string str = "Data Source=.;Initial Catalog=Android;Integrated Security=True";
        private SqlConnection con;
        private SqlDataAdapter adapter;
        // GET api/<controller>
        public IEnumerable<User> Get()
        {
            con = new SqlConnection(str);
            DataTable dt = new DataTable();
            var query = "SELECT * FROM Users";
            adapter = new SqlDataAdapter { SelectCommand = new SqlCommand(query, con) };
            adapter.Fill(dt);
            List<User> users = new List<Models.User>(dt.Rows.Count);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow usersrecords in dt.Rows)
                {
                    users.Add(new ReadUser(usersrecords));
                }
            }

            return users;
        }

        // GET api/<controller>/5
        public IEnumerable<User> Get(int id)
        {
            con = new SqlConnection(str);
            DataTable dt = new DataTable();
            var query = "SELECT * FROM Users WHERE IDUser=" + id;
            adapter = new SqlDataAdapter { SelectCommand = new SqlCommand(query, con) };
            adapter.Fill(dt);
            List<User> users = new List<API.Models.User>(dt.Rows.Count);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow usersrecords in dt.Rows)
                {
                    users.Add(new ReadUser(usersrecords));
                }
            }

            return users;
        }

        // POST api/<controller>
        public string Post([FromBody]CreateUser value)
        {
            con = new SqlConnection(str);

            var query = "INSERT INTO Users(Email,Password) VALUES(@Email,@Password)";
            SqlCommand insertCommand = new SqlCommand(query, con);
            insertCommand.Parameters.AddWithValue("@Email", value.Email);
            insertCommand.Parameters.AddWithValue("@Password", value.Password);
            con.Open();
            int result = insertCommand.ExecuteNonQuery();
            if (result > 0)
            {
                return "true";
            }
            else
            {
                return "false";
            }
        }

        // PUT api/<controller>/5
        public string Put(int id, [FromBody]CreateUser value)
        {
            con = new SqlConnection(str);

            var query = "UPDATE Users SET Email=@Email,Password=@Password WHERE IDUser=" + id;
            SqlCommand insertCommand = new SqlCommand(query, con);
            insertCommand.Parameters.AddWithValue("@Email", value.Email);
            insertCommand.Parameters.AddWithValue("@Password", value.Password);
            con.Open();
            int result = insertCommand.ExecuteNonQuery();
            if (result > 0)
            {
                return "true";
            }
            else
            {
                return "false";
            }
            
        }

        // DELETE api/<controller>/5
        public string Delete(int id)
        {
            con = new SqlConnection(str);

            var query = "DELETE FROM Users WHERE IDUser=" + id;
            SqlCommand insertCommand = new SqlCommand(query, con);
            con.Open();
            int result = insertCommand.ExecuteNonQuery();
            if (result > 0)
            {
                return "true";
            }
            else
            {
                return "false";
            }
        }
    }
}