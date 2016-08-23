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
        [Route("site")]

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
                    return View("Login", null);
                }
                else
                {
                    login.UserID = (int)reader["UserID"];
                    return View("Login", login);
                }
            }
        }

        [Route("site/listofcourses")]
        public ActionResult ListOfCourses(int userid)
        {
            List<Tutorials> tutorial = new List<Tutorials>();
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
                    return Json("null", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    while (reader.Read())
                    {

                        Tutorials t = new Tutorials()
                        {
                            UserID = (int)reader["UserID"],
                            TutorialID = (int)reader["TutorialID"],
                            TutorialTitle = (string)reader["TutorialTitle"]
                        };
                        tutorial.Add(t);
                    }
                    return Json(tutorial, JsonRequestBehavior.AllowGet);
                }
            }

        }

        [Route("site/listofchapters")]
        public ActionResult ListOfChapter(int tutorialid)
        {
            Dictionary<int, string> chapters = new Dictionary<int, string>();
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
                    return Json("null", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    while (reader.Read())
                    {
                        int chapterID = (int)reader["ChapterID"];
                        string chapterName = (string)reader["ChapterName"];
                        chapters.Add(chapterID, chapterName);

                    }
                    return Json(chapters, JsonRequestBehavior.AllowGet);
                }
            }
        }



        [Route("site/contentofchapter")]
        public ActionResult ContentOfChapter(int chapterid)
        {
            List<Chapters> chapters = new List<Chapters>();
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
                    return Json("null", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    while (reader.Read())
                    {

                        Chapters c = new Chapters()
                        {
                            ChapterID = (int)reader["ChapterID"],
                            HierarchyLevel = (int)reader["HierarchyLevel"],
                            ChapterName = (string)reader["ChapterName"],
                            Description = (string)reader["Description"],
                            TypeOfFile = (int)reader["TypeOfFile"],
                            FileContents = (string)reader["FileContents"],
                        };
                        chapters.Add(c);
                    }
                    return Json(chapters, JsonRequestBehavior.AllowGet);
                }
            }
        }


        [Route("site/edit")]
        public async Task<ActionResult> Edit(Chapters chapter)
        {
            string constring = ConfigurationManager.ConnectionStrings["TutorialsContext"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constring))
            {
                SqlCommand command = new SqlCommand();
                command.CommandText = "insert into Chapters values(@ti,@hl,@cn,@d,@tf,@fc);select CAST(scope_identity() as int)";
                command.Connection = con;
                command.Parameters.AddWithValue("@cn",chapter.ChapterName);
                command.Parameters.AddWithValue("@hl",chapter.HierarchyLevel);
                command.Parameters.AddWithValue("@ti",chapter.TutorialID);
                command.Parameters.AddWithValue("@d",chapter.Description);
                command.Parameters.AddWithValue("@tf",chapter.TypeOfFile);
                command.Parameters.AddWithValue("@fc",chapter.FileContents);
                con.Open();
                int id = (int)command.ExecuteScalar();
                return Json(id, JsonRequestBehavior.AllowGet);
            }

        }

        [Route("site/addnewcourse")]
        public async Task<ActionResult> AddNewCourse(Tutorials tutorial)
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
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
            }
        }
    }
}