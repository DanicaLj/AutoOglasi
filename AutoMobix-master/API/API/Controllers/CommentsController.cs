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
    public class CommentsController : ApiController
    {
        private string str = "Data Source=.;Initial Catalog=Android;Integrated Security=True";
        private SqlConnection con;
        private SqlDataAdapter adapter;

        public IEnumerable<Comments> Get(int id)
        {
            con = new SqlConnection(str);
            DataTable dt = new DataTable();
            var query = "SELECT * FROM Comment WHERE IDPost=" + id;
            adapter = new SqlDataAdapter { SelectCommand = new SqlCommand(query, con) };
            adapter.Fill(dt);
            List<Comments> comment = new List<API.Models.Comments>(dt.Rows.Count);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow commentrecords in dt.Rows)
                {
                    comment.Add(new ReadComment(commentrecords));
                }
            }

            return comment;
        }

        public string Post([FromBody]CreateComment value)
        {
            con = new SqlConnection(str);

            var query = "INSERT INTO Comment(IDPost,IDUser, Comment) VALUES(@IDPost,@IDUser,@Comment)";
            SqlCommand insertCommand = new SqlCommand(query, con);
            insertCommand.Parameters.AddWithValue("@IDPost", value.IDPost);
            insertCommand.Parameters.AddWithValue("@IDUser", value.IDUser);
            insertCommand.Parameters.AddWithValue("@Comment", value.Comment);
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
