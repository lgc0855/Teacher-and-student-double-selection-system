using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GaoMengWeb.Models;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Reflection;
using Spire.Xls;
using System.Threading;
using System.Text;

namespace GaoMengWeb.Controllers
{
    public class GAO_StudentController : Controller
    {
        private DataBaseHelper dbhelper = new DataBaseHelper();
        // GET: GAO_Student
        public ActionResult Index()
        {
            HttpCookie accountCookie = Request.Cookies["Account"];
            string userId = accountCookie["userId"];
            List<Student> list = dbhelper.getStudentByStuID(userId);
            if(list.Count >0)
            {
                Student s = list[0];
                ViewData["StuName"] = s.StuName;
                ViewData["StuID"] = s.StuID;
                ViewData["Age"] = s.Age;
                ViewData["StuMajorID"] = s.StuMajorID;
                ViewData["StuTel"] = s.StuTel;
                if (s.StuIfWork)
                {
                    ViewData["StuIfWork"] = 1;
                }
                else
                {
                    ViewData["StuIfWork"] = 0;
                }
                ViewData["StuMail"] = s.StuMail;
                ViewData["StuResumeUrl"] = s.StuResumeUrl;

                ViewData["InfoEnd"] = dbhelper.getInfoEnd();
            }
            else
            {
                ViewData["StuName"] = "";
                ViewData["StuID"] = "";
                ViewData["Age"] = "";
                ViewData["StuMajorID"] = "";
                ViewData["StuTel"] = "";
                ViewData["StuIfWork"] = "";
                ViewData["StuMail"] = "";
            }
           
            if (accountCookie == null)
            {
                ViewData["userId"] = "";
            }
            else
            {
                ViewData["userId"] = int.Parse(accountCookie["userId"]);
            }
            int st = dbhelper.testSettingTime();
            ViewData["TestSettingTime"] = st;

            return View();
        }

        public ActionResult Professor()
        {
            int st = dbhelper.testSettingTime();
            ViewData["TestSettingTime"] = st;

            HttpCookie accountCookie = Request.Cookies["Account"];

            string userId = accountCookie["userId"];
            List<Student> list = dbhelper.getStudentByStuID(userId);
            if (list.Count > 0)
            {
                Student s = list[0];
                ViewData["StuName"] = s.StuName;
            }
            else
            {
                ViewData["StuName"] = "";
            }

            if (accountCookie == null)
            {
                ViewData["userId"] = "";
            }
            else
            {
                ViewData["userId"] = int.Parse(accountCookie["userId"]);
            }
            return View();
        }

        public ActionResult FinalWill()
        {
            int st = dbhelper.testSettingTime();
            ViewData["TestSettingTime"] = st;
            HttpCookie accountCookie = Request.Cookies["Account"];
            string userId =accountCookie["userId"];
            List<Student> list = dbhelper.getStudentByStuID(userId);
            if (list.Count > 0)
            {
                Student s = list[0];
                List<Professor> ps = dbhelper.getProfessorByProId(s.StuFinalWill);
                if(ps.Count <=0)
                {
                    ViewData["ProName"] = "";
                }else
                {
                    ViewData["ProName"] = ps[0].ProName;
                }
                ViewData["StuName"] = s.StuName;
            }
            else
            {
                ViewData["ProName"] = "";
                ViewData["StuName"] = "";
            }
            
            if (accountCookie == null)
            {
                ViewData["userId"] = "";
            }
            else
            {
                ViewData["userId"] = int.Parse(accountCookie["userId"]);
            }
            return View();
        }

        public ActionResult ProfessorInformation(int UserId)
        {
            int st = dbhelper.testSettingTime();
            ViewData["TestSettingTime"] = st;
            Professor p = dbhelper.findProfessor(UserId);
            if (p == null)
            {
                return View("Index");
            }
            ViewData["ProName"] = p.ProName;
            if (p.ProTitle == 0)
            {
                ViewData["ProTitle"] = "讲师";
            }
            else if (p.ProTitle == 1)
            {
                ViewData["ProTitle"] = "副教授";
            }
            else
            {
                ViewData["ProTitle"] = "教授";
            }
            ViewData["ProInfoUrl"] = p.ProInfoUrl;
            ViewData["ProMaxNum"] = p.ProMaxNum;
            ViewData["ProNum"] = p.ProNum;
            ViewData["ProID"] = p.ProID;
            HttpCookie accountCookie = Request.Cookies["Account"];
            if (accountCookie == null)
            {
                ViewData["userId"] = "";
                ViewData["StuName"] = "";
            }
            else
            {
                ViewData["userId"] = int.Parse(accountCookie["userId"]);
                string userId =accountCookie["userId"];
                List<Student> list = dbhelper.getStudentByStuID(userId);
                if (list.Count > 0)
                {
                    Student s = list[0];
                    ViewData["StuName"] = s.StuName;
                }
                else
                {
                    ViewData["StuName"] = "";
                }
            }
            return View();
        }

        public string uploadFile(HttpPostedFileBase file)
        {
            HttpCookie accountCookie = Request.Cookies["Account"];
            var severPath = this.Server.MapPath("/resume/ "+ accountCookie["userId"]+"/");

            if (!Directory.Exists(severPath))
            {
                Directory.CreateDirectory(severPath);
            }

            System.IO.DirectoryInfo di = new DirectoryInfo(severPath);
            foreach (FileInfo f in di.GetFiles())
            {
                f.Delete();
            }
            foreach (DirectoryInfo dir in di.GetDirectories())
            {
                dir.Delete(true);
            }

            var savePath = Path.Combine(severPath, file.FileName);
            try
            {
                if (string.Empty.Equals(file.FileName) || (".doc" != Path.GetExtension(file.FileName) && ".docx" != Path.GetExtension(file.FileName) && ".pdf" != Path.GetExtension(file.FileName)))
                {
                    throw new Exception("文件格式不正确");
                }

                file.SaveAs(savePath);
                dbhelper.saveResumeUrl(accountCookie["userId"], "/resume/ " + accountCookie["userId"] + "/"+ file.FileName);
            }
            catch (Exception e)
            {
                return "fail" + e.Message;
            }
            return "{}";
        }


        public ActionResult saveInfo(string StuName, string StuID, int Age, int StuMajorID, string StuTel, string StuMail, bool StuIfWork)
        {
            bool b = dbhelper.updateStudent(StuName, StuID, Age, StuMajorID, StuTel, StuMail, StuIfWork);
            return RedirectToAction("Index", "GAO_Student");
        }

        public string deleteResume(string StuID,  string path)
        {
            try
            {
                if (System.IO.File.Exists(path))
                    System.IO.File.Delete(path);
            }
            catch(Exception e)
            {
                return "删除失败";
            }

            dbhelper.saveResumeUrl(StuID,"");

            return "删除成功";
        }


        public string confirmUpload()
        {
            HttpCookie accountCookie = Request.Cookies["Account"];
            bool b = dbhelper.studentConfirm(accountCookie["userId"]);
            if (b)
            {
                return "已确认";
            }
            else
            {
                return "确认失败";
            }
        }

        public string confirmWill(int first,int second)
        {
            string rel = "";
            HttpCookie accountCookie = Request.Cookies["Account"];
            string id =accountCookie["userId"];
            SsContext db = new SsContext();
            List<Student> list = db.Students.Where(s => s.StuID == id).ToList();
            if (list.Count <= 0)
            {
                rel = "出错";
            }
            else
            {
                Student s = list[0];
                s.StuFirstWill = first;
                s.StuSecondWill = second;
                s.StuFirstWillStates = 0;
                s.StuSecondWillStates = 0;
                s.StuWillChecked = true;
                db.SaveChanges();
                rel = "成功";
            }
            return rel;
        }

        public void FileEx()
        {
            HttpCookie accountCookie = Request.Cookies["Account"];
            string id = accountCookie["userId"];
            List<Student> list = dbhelper.getStudentByStuID(id);
            if (list.Count <= 0)
            {
                return;
            }
            string strWord = ExprotMissionToWord(this.Server.MapPath("/Content/模板.htm"), list[0]);
            Response.ContentEncoding = System.Text.Encoding.UTF7;
            Response.ClearContent();
            Response.ClearHeaders();
            Response.AddHeader("content-disposition", "attachment;filename=导师接收表.doc");
            Response.AddHeader("Content-type", "application");
            Response.ContentType = "application/ms-html";
            Response.ContentEncoding = System.Text.Encoding.Default; Response.Write(strWord);
            Response.Flush();
            Response.Close();
        }

        public string ExprotMissionToWord(string templatePath, Student u)
        {
            StringBuilder sb = new StringBuilder(1024);
            string strWord = this.Server.MapPath("/Content/模板.htm");
            StreamReader sr = new StreamReader(strWord, Encoding.Default);
            sb.Append(sr.ReadToEnd());
            sr.Close();

            sb.Replace("{id}", u.StuID.ToString());
            sb.Replace("{name}", u.StuName);
            if (u.Gender)
            {
                sb.Replace("{gender}", "男"); //u.Gender if判断下
            }
            else
            {
                sb.Replace("{gender}", "女"); //u.Gender if判断下
            }
            sb.Replace("{school}", u.StuGraSchool);
            sb.Replace("{major}", u.StuGraMajor);
            if (u.StuMajorID == 0)
            {
                sb.Replace("{majorwill}", "软件工程与管理");
            }
            else if (u.StuMajorID == 1)
            {
                sb.Replace("{majorwill}", "虚拟现实与应用");
            }
            else if (u.StuMajorID == 2)
            {
                sb.Replace("{majorwill}", "人工智能");
            }
            else if (u.StuMajorID == 3)
            {
                sb.Replace("{majorwill}", "大数据技术与应用");
            }
            else
            {
                sb.Replace("{majorwill}", "未知");
            }
            if (u.StuIfWork)
            {
                sb.Replace("{status}", "是");
            }
            else
            {
                sb.Replace("{status}", "否");
            }
            sb.Replace("{tel}", u.StuTel);

            return sb.ToString();
        }

    }


}