using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Pentagon.Models
{
    public class TutorialContext
    {
        public DbSet<Login> Login { get; set; }
        public DbSet<Tutorials> tutorials { get; set; }
        public DbSet<Chapters> Chapters { get; set; }
    }
}