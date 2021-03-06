﻿/* 
 * Site Controller     *
 * Created By Nitheesh *
 *                     * 
 */



using Pentagon.Models;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
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


        /* Function to Login */
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



        /* Function to return list of courses/tutorials as json object*/
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


        /*Function to return list of chapters as json object*/
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

        ///*Function to return content of chapters as json object*/
        //[Route("site/chapterscontent")]
        //[HttpGet]
        //public ActionResult ContentOfChapter(int chapterid)
        //{
        //    List<ChapterContent> chapters = new List<ChapterContent>();
        //    string constring = ConfigurationManager.ConnectionStrings["TutorialsContext"].ConnectionString;
        //    using (SqlConnection con = new SqlConnection(constring))
        //    {
        //        SqlCommand command = new SqlCommand();
        //        command.CommandText = "select * from Chapters Where ChapterID=@ChapterID";
        //        command.Parameters.AddWithValue("@ChapterID", chapterid);
        //        //command.CommandText = "select * from Chapters Where TutorialID=@TutorialID";
        //        //command.Parameters.AddWithValue("@TutorialID", tutorialid);
        //        command.Connection = con;

        //        con.Open();

        //        SqlDataReader reader = command.ExecuteReader();
        //        if (reader == null)
        //        {
        //            if (command != null)
        //                command.Dispose();
        //            if (command != null)
        //                con.Dispose();
        //            return Json("null", JsonRequestBehavior.AllowGet);
        //        }
        //        while (reader.Read())
        //        {

        //            ChapterContent c = new ChapterContent()
        //            {
        //                ChapterName = (string)reader["ChapterName"],
        //                Description = (string)reader["Description"],
        //                TypeOfFile = (int)reader["TypeOfFile"],
        //                FileContents = (string)reader["FileContents"],
        //            };
        //            chapters.Add(c);
        //        }
        //        if (command != null)
        //            command.Dispose();
        //        if (command != null)
        //            con.Dispose();
        //        return Json(chapters, JsonRequestBehavior.AllowGet);
        //    }
        //}

        ///*Function to insert new chapters as json object*/
        //[Route("site/addnewchapter")]
        //[HttpGet]
        //public ActionResult AddNewChapter(Chapters chapter)
        //{
        //    string constring = ConfigurationManager.ConnectionStrings["TutorialsContext"].ConnectionString;
        //    using (SqlConnection con = new SqlConnection(constring))
        //    {
        //        SqlCommand command = new SqlCommand();
        //        command.CommandText = "insert into Chapters values(@ti,@hl,@cn,@d,@tf,@fc);select CAST(scope_identity() as int)";
        //        command.Connection = con;
        //        command.Parameters.AddWithValue("@cn", chapter.ChapterName);
        //        command.Parameters.AddWithValue("@hl", chapter.HierarchyLevel);
        //        command.Parameters.AddWithValue("@ti", chapter.TutorialID);
        //        command.Parameters.AddWithValue("@d", chapter.Description);
        //        command.Parameters.AddWithValue("@tf", chapter.TypeOfFile);
        //        command.Parameters.AddWithValue("@fc", chapter.FileContents);
        //        con.Open();
        //        int id = (int)command.ExecuteScalar();
        //        if (command != null)
        //            command.Dispose();
        //        if (command != null)
        //            con.Dispose();
        //        return Json(id, JsonRequestBehavior.AllowGet);
        //    }

        //}

        ///*Function to insert new Course/tutorial as json object*/
        //[Route("site/addnewcourse")]
        //[HttpGet]
        //public ActionResult AddNewCourse(Tutorials tutorial)
        //{
        //    string constring = ConfigurationManager.ConnectionStrings["TutorialsContext"].ConnectionString;
        //    using (SqlConnection con = new SqlConnection(constring))
        //    {
        //        SqlCommand command = new SqlCommand();
        //        command.CommandText = "insert into Tutorials values();select CAST(scope_identity() as int)";
        //        command.Connection = con;
        //        command.Parameters.AddWithValue("@ui", tutorial.UserID);
        //        command.Parameters.AddWithValue("@tt", tutorial.TutorialTitle);
        //        con.Open();
        //        int id = (int)command.ExecuteScalar();
        //        if (id == 0)
        //        {
        //            if (command != null)
        //                command.Dispose();
        //            if (command != null)
        //                con.Dispose();
        //            return Json(false, JsonRequestBehavior.AllowGet);
        //        }
        //        if (command != null)
        //            command.Dispose();
        //        if (command != null)
        //            con.Dispose();
        //        return Json(id, JsonRequestBehavior.AllowGet);
        //    }
        //}






        /*temp function to return chapter object from database Delete this function later*/

        [Route("site/getchapters")]
        [HttpGet]
        public ActionResult GetChapter(int tutorialid)
        {
            List<Chapters> chapters = new List<Chapters>();
            string constring = ConfigurationManager.ConnectionStrings["TutorialsContext"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constring))
            {
                SqlCommand command = new SqlCommand();
                command.CommandText = "select * from Chapters Where TutorialID=@TutorialID";
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
                    Chapters c = new Chapters()
                    {
                        ChapterID = (int)reader["ChapterID"],
                        TutorialID = (int)reader["TutorialID"],
                        HierarchyLevel = (int)reader["HierarchyLevel"],
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

        /*Function to Add new chapters*/
        [Route("site/addchapter")]
        [HttpGet]
        public ActionResult AddNewChapter(int tid, string chaptername)
        {
            Chapters c = new Chapters()
            {
                TutorialID = tid,
                ChapterName = chaptername,
                Description = "Description",
                TypeOfFile = 1,
                FileContents = "url",
                HierarchyLevel = 1,

            };
            string constring = ConfigurationManager.ConnectionStrings["TutorialsContext"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constring))
            {
                SqlCommand command = new SqlCommand();
                command.CommandText = "insert into Chapters values(@ti,@hl,@cn,@d,@tf,@fc);select CAST(scope_identity() as int)";
                command.Connection = con;
                command.Parameters.AddWithValue("@cn", c.ChapterName);
                command.Parameters.AddWithValue("@hl", c.HierarchyLevel);
                command.Parameters.AddWithValue("@ti", c.TutorialID);
                command.Parameters.AddWithValue("@d", c.Description);
                command.Parameters.AddWithValue("@tf", c.TypeOfFile);
                command.Parameters.AddWithValue("@fc", c.FileContents);
                con.Open();
                int id = (int)command.ExecuteScalar();
                if (command != null)
                    command.Dispose();
                if (command != null)
                    con.Dispose();
                c.ChapterID = id;
                return Json(c, JsonRequestBehavior.AllowGet);
            }
        }

        /*function to add new tutorial */
        [Route("site/addtutorial")]
        [HttpPost]
        public ActionResult AddNewCourses(int uid,string tutorialname)
        {
            string constring = ConfigurationManager.ConnectionStrings["TutorialsContext"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constring))
            {
                SqlCommand command = new SqlCommand();
                command.CommandText = "insert into Tutorials values(@ui,@tt);select CAST(scope_identity() as int)";
                command.Connection = con;
                command.Parameters.AddWithValue("@ui", uid);
                command.Parameters.AddWithValue("@tt", tutorialname);
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


        /*Function to update chapter */
        [Route("site/updatechapter")]
        [HttpGet]
        public ActionResult UpdateChapter(Chapters c)
        {
            string constring = ConfigurationManager.ConnectionStrings["TutorialsContext"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constring))
            {
                SqlCommand command = new SqlCommand();
                command.CommandText = "UPDATE Chapters SET ChapterName = @cn,HierarchyLevel = @hl,Description = @d ,TypeOfFile = tf,FileContents = @fc where ChapterID = @ci and TutorialID = @ti)";
                command.Connection = con;
                command.Parameters.AddWithValue("@cn", c.ChapterName);
                command.Parameters.AddWithValue("@hl", c.HierarchyLevel);
                command.Parameters.AddWithValue("@ti", c.TutorialID);
                command.Parameters.AddWithValue("@d", c.Description);
                command.Parameters.AddWithValue("@tf", c.TypeOfFile);
                command.Parameters.AddWithValue("@fc", c.FileContents);
                command.Parameters.AddWithValue("@ci", c.ChapterID);
                con.Open();
                int id = (int)command.ExecuteScalar();
                if (command != null)
                    command.Dispose();
                if (command != null)
                    con.Dispose();
                c.ChapterID = id;
            }
            return Json(c.ChapterID, JsonRequestBehavior.AllowGet);
        }
    }
}
