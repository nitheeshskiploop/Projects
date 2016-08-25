using Pentagon.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Pentagon.Controllers
{
    public class SiteController : Controller
    {
        [Route("")]
        public ActionResult TutorialApp()
        {
            return View("index");
        }

        [Route("site")]
        [Route("site/login")]
        [HttpGet]
        public ActionResult Login(string username, string password)
        {
            Login login = new Login();
            login.UserName = username;
            login.Password = password;
            string constring = ConfigurationManager.ConnectionStrings["TutorialsContext"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constring))
            {
                SqlCommand command = new SqlCommand();
                command.CommandText = "select UserID from Login where UserName=@UserName and Password=@Password";
                command.Parameters.AddWithValue("@UserName", login.UserName);
                command.Parameters.AddWithValue("@Password", login.Password);
                command.Connection = con;

                con.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read() == false)
                {
                    if (command != null)
                        command.Dispose();
                    if (command != null)
                        con.Dispose();
                    return Json(0,JsonRequestBehavior.AllowGet);
                }
                else
                {
                    login.UserID = (int)reader["UserID"];
                    if (command != null)
                        command.Dispose();
                    if (command != null)
                        con.Dispose();
                    return Json(login.UserID, JsonRequestBehavior.AllowGet);

                }
            }
        }

        [Route("site/listofcourses")]
        [HttpGet]

        public ActionResult ListOfCourses(int userid)
        {
            List<TutorialList> tutorial = new List<TutorialList>();
            string constring = ConfigurationManager.ConnectionStrings["TutorialsContext"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constring))
            {
                SqlCommand command = new SqlCommand();
                command.CommandText = "select * from Tutorials where UserID=@UserID";
                command.Parameters.AddWithValue("@UserID", userid);
                command.Connection = con;

                con.Open();

                SqlDataReader reader = command.ExecuteReader();
                if (reader == null)
                {
                    if (command != null)
                        command.Dispose();
                    if (command != null)
                        con.Dispose();
                    return Json("null", JsonRequestBehavior.AllowGet);
                }
                while (reader.Read())
                {

                    TutorialList t = new TutorialList()
                    {
                        TutorialID = (int)reader["TutorialID"],
                        TutorialTitle = (string)reader["TutorialTitle"],
                    };
                    tutorial.Add(t);
                }
                if (command != null)
                    command.Dispose();
                if (command != null)
                    con.Dispose();
                return Json(tutorial, JsonRequestBehavior.AllowGet);
            }
        }

        [Route("site/listofchapters")]
        [HttpGet]
        public ActionResult ListOfChapter(int tutorialid)
        {
            List<ChaptersList> chapters = new List<ChaptersList>();
            string constring = ConfigurationManager.ConnectionStrings["TutorialsContext"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constring))
            {
                SqlCommand command = new SqlCommand();
                command.CommandText = "select ChapterID,ChapterName from Chapters Where TutorialID=@TutorialID";
                command.Parameters.AddWithValue("@TutorialID", tutorialid);
                command.Connection = con;

                con.Open();

                SqlDataReader reader = command.ExecuteReader();
                if (reader == null)
                {
                    if (command != null)
                        command.Dispose();
                    if (command != null)
                        con.Dispose();
                    return Json("null", JsonRequestBehavior.AllowGet);
                }
                while (reader.Read())
                {
                    ChaptersList c = new ChaptersList()
                    {
                        ChapterID = (int)reader["ChapterID"],
                        ChapterName = (string)reader["ChapterName"],
                    };
                    chapters.Add(c);
                }
                if (command != null)
                    command.Dispose();
                if (command != null)
                    con.Dispose();
                return Json(chapters, JsonRequestBehavior.AllowGet);
            }
        }


        [Route("site/contentofchapter")]
        [HttpGet]
        public ActionResult ContentOfChapter(int chapterid)
        {
            List<ChapterContent> chapters = new List<ChapterContent>();
            string constring = ConfigurationManager.ConnectionStrings["TutorialsContext"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constring))
            {
                SqlCommand command = new SqlCommand();
                command.CommandText = "select * from Chapters Where ChapterID=@ChapterID";
                command.Parameters.AddWithValue("@ChapterID", chapterid);
                command.Connection = con;

                con.Open();

                SqlDataReader reader = command.ExecuteReader();
                if (reader == null)
                {
                    if (command != null)
                        command.Dispose();
                    if (command != null)
                        con.Dispose();
                    return Json("null", JsonRequestBehavior.AllowGet);
                }
                while (reader.Read())
                {

                    ChapterContent c = new ChapterContent()
                    {
                        ChapterName = (string)reader["ChapterName"],
                        Description = (string)reader["Description"],
                        TypeOfFile = (int)reader["TypeOfFile"],
                        FileContents = (string)reader["FileContents"],
                    };
                    chapters.Add(c);
                }
                if (command != null)
                    command.Dispose();
                if (command != null)
                    con.Dispose();
                return Json(chapters, JsonRequestBehavior.AllowGet);
            }
        }


        [Route("site/addnewchapter")]
        [HttpGet]
        public ActionResult AddNewChapter(Chapters chapter)
        {
            string constring = ConfigurationManager.ConnectionStrings["TutorialsContext"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constring))
            {
                SqlCommand command = new SqlCommand();
                command.CommandText = "insert into Chapters values(@ti,@hl,@cn,@d,@tf,@fc);select CAST(scope_identity() as int)";
                command.Connection = con;
                command.Parameters.AddWithValue("@cn", chapter.ChapterName);
                command.Parameters.AddWithValue("@hl", chapter.HierarchyLevel);
                command.Parameters.AddWithValue("@ti", chapter.TutorialID);
                command.Parameters.AddWithValue("@d", chapter.Description);
                command.Parameters.AddWithValue("@tf", chapter.TypeOfFile);
                command.Parameters.AddWithValue("@fc", chapter.FileContents);
                con.Open();
                int id = (int)command.ExecuteScalar();
                if (command != null)
                    command.Dispose();
                if (command != null)
                    con.Dispose();
                return Json(id, JsonRequestBehavior.AllowGet);
            }

        }

        [Route("site/addnewcourse")]
        [HttpGet]
        public ActionResult AddNewCourse(Tutorials tutorial)
        {
            string constring = ConfigurationManager.ConnectionStrings["TutorialsContext"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constring))
            {
                SqlCommand command = new SqlCommand();
                command.CommandText = "insert into Tutorials values();select CAST(scope_identity() as int)";
                command.Connection = con;
                command.Parameters.AddWithValue("@ui", tutorial.UserID);
                command.Parameters.AddWithValue("@tt", tutorial.TutorialTitle);
                con.Open();
                int id = (int)command.ExecuteScalar();
                if (id == 0)
                {
                    if (command != null)
                        command.Dispose();
                    if (command != null)
                        con.Dispose();
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
                if (command != null)
                    command.Dispose();
                if (command != null)
                    con.Dispose();
                return Json(id, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
