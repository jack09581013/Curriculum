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
    public class CurriculumController : Controller
    {
        const string connectString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=\"C:\\Users\\USER\\documents\\visual studio 2015\\Projects\\Curriculum\\Curriculum\\App_Data\\Curriculum.mdf\";Integrated Security=True";
        //const string connectString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=\"C:\\users\\user\\documents\\visual studio 2015\\Projects\\AIDC_Web\\AIDC_Web\\App_Data\\InvalidWorkingHoursDB.mdf\";Integrated Security=True";

        // GET: Curriculum
        public ActionResult Index()
        {
            return View();
        }

        public string CurData()
        {
            SqlConnection conn = new SqlConnection(connectString);
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();

            cmd.CommandText = "SELECT row, col, class_name, teacher, class_room FROM Curriculum WHERE account = @account ORDER BY row, col ASC";
            cmd.Parameters.Add("@account", SqlDbType.VarChar, 15).Value = (string)Session["account"];
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            ad.Fill(dt);
            return JsonConvert.SerializeObject(dt);
        }
    }
}