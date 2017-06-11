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
    public class MainController : Controller
    {
        const string connectString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=\"C:\\Users\\USER\\documents\\visual studio 2015\\Projects\\Curriculum\\Curriculum\\App_Data\\Curriculum.mdf\";Integrated Security=True";

        //---------------- Login's functions -----------------------------
        // GET: Main
        public ActionResult Login()
        {
            Session["account"] = null;
            return View();
        }

        public string LoginAccount(string account, string password)
        {
            SqlConnection conn = new SqlConnection(connectString);
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();

            cmd.CommandText = "SELECT account FROM Account WHERE account=@account AND password=@password";
            cmd.Parameters.Add("@account", SqlDbType.VarChar, 15).Value = account;
            cmd.Parameters.Add("@password", SqlDbType.VarChar, 15).Value = password;
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read() == true)
            {
                //帳號與密碼正確
                reader.Close();
                conn.Close();
                Session["account"] = account;
                return "login success";
            }
            else
            {
                //帳號或密碼不正確
                reader.Close();
                conn.Close();
                return "login failure";
            }
            
        }

        //---------------- Register's functions -----------------------------
        public ActionResult Register()
        {
            return View();
        }

        public string RegisterAccount(string account, string password)
        {
            SqlConnection conn = new SqlConnection(connectString);
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();

            cmd.CommandText = "SELECT account FROM Account WHERE account=@account";
            cmd.Parameters.Add("@account", SqlDbType.VarChar, 15).Value = account;
            SqlDataReader reader = cmd.ExecuteReader();
            if(reader.Read() == true)
            {
                //帳號已經存在
                reader.Close();
                conn.Close();
                return "account exist";
            }
            else
            {
                //帳號未存在，可以註冊此帳號
                reader.Close();
                cmd.CommandText = "INSERT INTO Account VALUES (@account, @password)";
                cmd.Parameters.Add("@password", SqlDbType.VarChar, 15).Value = password;
                cmd.ExecuteNonQuery();
                conn.Close();
                return "register success";
            }      
        }

        //---------------- Curriculum's functions -----------------------------
        public ActionResult Curriculum()
        {
            if(Session["account"] == null)
            { 
                return View("Login");
            }
            else return View();
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
            conn.Close();
            return JsonConvert.SerializeObject(dt);
        }

        public void AddClass(string row, string col, string teacher, string className, string classRoom)
        {
            SqlConnection conn = new SqlConnection(connectString);
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();

            cmd.CommandText = "INSERT INTO Curriculum VALUES (@row, @col, @account, @className, @teacher, @classRoom)";
            cmd.Parameters.Add("@row", SqlDbType.NVarChar, 15).Value = row;
            cmd.Parameters.Add("@col", SqlDbType.NVarChar, 15).Value = col;
            cmd.Parameters.Add("@account", SqlDbType.VarChar, 15).Value = (string)Session["account"];
            cmd.Parameters.Add("@className", SqlDbType.NVarChar, 10).Value = className;
            cmd.Parameters.Add("@teacher", SqlDbType.NVarChar, 10).Value = teacher;
            cmd.Parameters.Add("@classRoom", SqlDbType.NVarChar, 10).Value = classRoom;

            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public void DeleteClass(string row, string col)
        {
            SqlConnection conn = new SqlConnection(connectString);
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();

            cmd.CommandText = "DELETE FROM Curriculum WHERE row=@row AND col=@col";
            cmd.Parameters.Add("@row", SqlDbType.VarChar, 10).Value = row;
            cmd.Parameters.Add("@col", SqlDbType.VarChar, 10).Value = col;
            cmd.ExecuteNonQuery();
            conn.Close();
        }
    }
}