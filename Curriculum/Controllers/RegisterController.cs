using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using Newtonsoft.Json;

namespace Curriculum.Controllers
{
    public class RegisterController : Controller
    {
        const string connectString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=\"C:\\Users\\USER\\documents\\visual studio 2015\\Projects\\Curriculum\\Curriculum\\App_Data\\Curriculum.mdf\";Integrated Security=True";
        // GET: Register
        public ActionResult Index()
        {
            return View();
        }

        public void Register(string account, string password) {
            SqlConnection conn = new SqlConnection(connectString);
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();

            cmd.CommandText = "INSERT INTO Account VALUES (@account, @password)";
            cmd.Parameters.Add("@account", SqlDbType.VarChar, 15).Value = account;
            cmd.Parameters.Add("@password", SqlDbType.VarChar, 15).Value = password;
            cmd.ExecuteNonQuery();
            conn.Close();
        }
    }
}