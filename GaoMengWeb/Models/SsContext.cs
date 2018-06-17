using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data.Entity;

namespace GaoMengWeb.Models
{
  
    public class SsContext:DbContext
  {
        public SsContext() : base("DefaultConnection")
        {
        }
    public DbSet<User> Users { get; set; }
    public DbSet<Student> Students{ get; set; }
    public DbSet<Settings> Settingss { get; set; }
    public DbSet<ProfessorToStudent> ProfessorToStudents { get; set; }
    public DbSet<Professor> Professors { get; set; }
    public DbSet<JiaoWu> JiaoWus { get; set; }
    }
}
