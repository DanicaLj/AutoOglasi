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
    public class PostController : ApiController
    {
        // GET api/<controller>
        private string str = "Data Source=.;Initial Catalog=Android;Integrated Security=True";
        private SqlConnection con;
        private SqlDataAdapter adapter;
        // GET api/<controller>
        public IEnumerable<Post> Get()
        {
            con = new SqlConnection(str);
            DataTable dt = new DataTable();
            var query = "SELECT * FROM Posts";
            adapter = new SqlDataAdapter { SelectCommand = new SqlCommand(query, con) };
            adapter.Fill(dt);
            List<Post> Posts = new List<Models.Post>(dt.Rows.Count);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow Postsrecords in dt.Rows)
                {
                    Posts.Add(new ReadPost(Postsrecords));
                }
            }

            return Posts;
        }

        // GET api/<controller>/5
        public IEnumerable<Post> Get(int id)
        {
            con = new SqlConnection(str);
            DataTable dt = new DataTable();
            var query = "SELECT * FROM Posts WHERE IDPost=" + id;
            adapter = new SqlDataAdapter { SelectCommand = new SqlCommand(query, con) };
            adapter.Fill(dt);
            List<Post> Posts = new List<Models.Post>(dt.Rows.Count);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow Postsrecords in dt.Rows)
                {
                    Posts.Add(new ReadPost(Postsrecords));
                }
            }

            return Posts;
        }

        [Route("api/Post/GetPostFilter/{brend}/{year}")]
        public IEnumerable<Post> GetPostFilter(string brend, int year)
        {
            con = new SqlConnection(str);
            DataTable dt = new DataTable();
            var query = "SELECT * FROM Posts WHERE Brend='" + brend + "'AND Year='" + year + "'";
            adapter = new SqlDataAdapter { SelectCommand = new SqlCommand(query, con) };
            adapter.Fill(dt);
            List<Post> Posts = new List<Models.Post>(dt.Rows.Count);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow Postsrecords in dt.Rows)
                {
                    Posts.Add(new ReadPost(Postsrecords));
                }
            }

            return Posts;
        }

        // POST api/<controller>
        [HttpPost]
        [AllowAnonymous]
        public string Post([FromBody]CreatePost value)
        {
            con = new SqlConnection(str);

            var query = "INSERT INTO Posts(Image,Year,Brend,Comment,Description,IDUser,RealImage) VALUES(@Image,@Year,@Brend,@Comment,@Description,@IDUser,@RealImage)";
            SqlCommand insertCommand = new SqlCommand(query, con);
            insertCommand.Parameters.AddWithValue("@Image", value.Image);
            insertCommand.Parameters.AddWithValue("@Year", value.Year);
            insertCommand.Parameters.AddWithValue("@Brend", value.Brend);
            insertCommand.Parameters.AddWithValue("@Comment", value.Comment);
            insertCommand.Parameters.AddWithValue("@Description", value.Description);
            insertCommand.Parameters.AddWithValue("@IDUser", value.IDUser);
            insertCommand.Parameters.Add(new SqlParameter("@RealImage", SqlDbType.VarBinary)
            {
                Direction = ParameterDirection.Input,
                Value = value.RealImage,
                Size = value.RealImage.Length,
                
            });


            try
            {
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
            catch(Exception e)
            {
                
            }
            return "true";
        }

        // PUT api/<controller>/5
        public string Put(int id, [FromBody]CreatePost value)
        {
            con = new SqlConnection(str);

            var query = "UPDATE Posts SET Image=@Image,Year=@Year,Brend=@Brend,Comment=@Comment,Description=@Description,IDUser=@IDUser WHERE IDPost=" + id;
            SqlCommand insertCommand = new SqlCommand(query, con);
            insertCommand.Parameters.AddWithValue("@Image", value.Image);
            insertCommand.Parameters.AddWithValue("@Year", value.Year);
            insertCommand.Parameters.AddWithValue("@Brend", value.Brend);
            insertCommand.Parameters.AddWithValue("@Comment", value.Comment);
            insertCommand.Parameters.AddWithValue("@Description", value.Description);
            insertCommand.Parameters.AddWithValue("@IDUser", value.IDUser);
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

            var query = "DELETE FROM Posts WHERE IDPost=" + id;
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