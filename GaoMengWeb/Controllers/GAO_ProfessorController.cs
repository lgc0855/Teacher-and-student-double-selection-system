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

namespace GaoMengWeb.Controllers
{
    public class GAO_ProfessorController : Controller
    {
        private DataBaseHelper dbhelper = new DataBaseHelper();
        // GET: GAO_Professor
        public ActionResult Index()
        {
            int st = dbhelper.testSettingTime();
            ViewData["TestSettingTime"] = st;
            HttpCookie accountCookie = Request.Cookies["Account"];
            int id = int.Parse(accountCookie["userId"]);
            List<Professor> plist = dbhelper.getProfessorByProId(id);
            if(plist.Count <=0)
            {
                ViewData["SelectNum"] = "0";
            }
            else
            {
                ViewData["SelectNum"] = plist[0].ProMaxNum - plist[0].ProNum;
                ViewData["ProName"] = plist[0].ProName;
            }

            Settings s = dbhelper.getSetting();
            if(s== null)
            {
                ViewData["EndTime"] = "管理员未设置";
            }
            else
            {
                ViewData["EndTime"] = s.FirstEnd;
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

        public ActionResult SecondSelect()
        {
            int st = dbhelper.testSettingTime();
            ViewData["TestSettingTime"] = st;
            HttpCookie accountCookie = Request.Cookies["Account"];
            int id = int.Parse(accountCookie["userId"]);
            List<Professor> plist = dbhelper.getProfessorByProId(id);
            if (plist.Count <= 0)
            {
                ViewData["SelectNum"] = "0";
            }
            else
            {
                ViewData["SelectNum"] = plist[0].ProMaxNum - plist[0].ProNum;
                ViewData["ProName"] = plist[0].ProName;
            }

            Settings s = dbhelper.getSetting();
            if (s == null)
            {
                ViewData["EndTime"] = "管理员未设置";
            }
            else
            {
                ViewData["EndTime"] = s.SecondEnd;
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

        public ActionResult FinalStudents()
        {
            int st = dbhelper.testSettingTime();
            ViewData["TestSettingTime"] = st;
            HttpCookie accountCookie = Request.Cookies["Account"];
            List<Professor> plist = dbhelper.getProfessorByProId(int.Parse(accountCookie["userId"]));
            if (plist.Count <= 0)
            {
                ViewData["SelectNum"] = "0";
            }
            else
            {
                ViewData["ProName"] = plist[0].ProName;
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

        public string getStudents()
        {
            string rel = "";
            HttpCookie accountCookie = Request.Cookies["Account"];
            int id = int.Parse(accountCookie["userId"]);
            List<Student> list = dbhelper.getFirstWillStudents(id);
            if(list == null)
            {
                return rel;
            }
            List<ProfessorStudent> retunList = new List<ProfessorStudent>();
            ProfessorStudent ps = null;
            foreach(Student s in list)
            {
                ps = new ProfessorStudent();
                if (s.Gender)
                {
                    ps.Gender = "男";
                }
                else
                {
                    ps.Gender = "女";
                }
                ps.StuGraSchool = s.StuGraSchool;
                ps.StuID = s.StuID;
                ps.StuName = s.StuName;
                // 0软件工程与管理 1虚拟现实与应用 2人工智能 3大数据技术与应用
                if (s.StuMajorID == 0)
                {
                    ps.StuMajor = "软件工程与管理";
                }else if(s.StuMajorID == 1)
                {
                    ps.StuMajor = "虚拟现实与应用";
                }
                else if (s.StuMajorID == 2)
                {
                    ps.StuMajor = "人工智能";
                }
                else if (s.StuMajorID == 3)
                {
                    ps.StuMajor = "大数据技术与应用";
                }
                retunList.Add(ps);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            var json = serializer.Serialize(retunList);
            rel = json.ToString();
            return rel;
        }

        public string getStudentsSecond()
        {
            string rel = "";
            HttpCookie accountCookie = Request.Cookies["Account"];
            int id = int.Parse(accountCookie["userId"]);
            List<Student> list = dbhelper.getSecondWillStudents(id);
            if (list == null)
            {
                return rel;
            }
            List<ProfessorStudent> retunList = new List<ProfessorStudent>();
            ProfessorStudent ps = null;
            foreach (Student s in list)
            {
                ps = new ProfessorStudent();
                if (s.Gender)
                {
                    ps.Gender = "男";
                }
                else
                {
                    ps.Gender = "女";
                }
                ps.StuGraSchool = s.StuGraSchool;
                ps.StuID = s.StuID;
                ps.StuName = s.StuName;
                // 0软件工程与管理 1虚拟现实与应用 2人工智能 3大数据技术与应用
                if (s.StuMajorID == 0)
                {
                    ps.StuMajor = "软件工程与管理";
                }
                else if (s.StuMajorID == 1)
                {
                    ps.StuMajor = "虚拟现实与应用";
                }
                else if (s.StuMajorID == 2)
                {
                    ps.StuMajor = "人工智能";
                }
                else if (s.StuMajorID == 3)
                {
                    ps.StuMajor = "大数据技术与应用";
                }
                retunList.Add(ps);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            var json = serializer.Serialize(retunList);
            rel = json.ToString();
            return rel;
        }

        public ActionResult Student(int id)
        {
            int st = dbhelper.testSettingTime();
            ViewData["TestSettingTime"] = st;
            List<Student> list = dbhelper.getStudentByStuID(id);
            if (list.Count > 0)
            {
                Student s = list[0];
                ViewData["StuName"] = s.StuName;
                ViewData["StuID"] = s.StuID;
                ViewData["Age"] = s.Age;
                ViewData["StuMajorID"] = s.StuMajorID;
                ViewData["StuTel"] = s.StuTel;
                ViewData["StuResumeUrl"] = s.StuResumeUrl;
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
            HttpCookie accountCookie = Request.Cookies["Account"];
            if (accountCookie == null)
            {
                ViewData["userId"] = "";
            }
            else
            {
                ViewData["userId"] = int.Parse(accountCookie["userId"]);
            }
            List<Professor> plist = dbhelper.getProfessorByProId(int.Parse(accountCookie["userId"]));
            if (plist.Count <= 0)
            {
              
            }
            else
            {
                ViewData["ProName"] = plist[0].ProName;
            }


            return View();
        }

        public string addFirstSelect(int StuID)
        {
            string rel = "";
            HttpCookie accountCookie = Request.Cookies["Account"];
            int pid = int.Parse(accountCookie["userId"]);
            dbhelper.changeStudentWilleState(pid,StuID, 1, 1);
            rel = dbhelper.addProfessorToStudent(pid, StuID);
            return rel;
        }

        public string addSecondSelect(int StuID)
        {
            string rel = "";
            HttpCookie accountCookie = Request.Cookies["Account"];
            int pid = int.Parse(accountCookie["userId"]);
            dbhelper.changeStudentWilleState(pid, StuID, 2, 1);
            rel = dbhelper.addProfessorToStudent(pid, StuID);
            return rel;
        }

        public string getStudentsOfProfessorToStudent()
        {
            string rel = "";
            HttpCookie accountCookie = Request.Cookies["Account"];
            int id = int.Parse(accountCookie["userId"]);
            List<Student> list = dbhelper.getStudentsOfProfessorToStudent(id);
            if (list == null)
            {
                return rel;
            }
            List<ProfessorStudent> retunList = new List<ProfessorStudent>();
            ProfessorStudent ps = null;
            foreach (Student s in list)
            {
                ps = new ProfessorStudent();
                if (s.Gender)
                {
                    ps.Gender = "男";
                }
                else
                {
                    ps.Gender = "女";
                }
                ps.StuGraSchool = s.StuGraSchool;
                ps.StuID = s.StuID;
                ps.StuName = s.StuName;
                // 0软件工程与管理 1虚拟现实与应用 2人工智能 3大数据技术与应用
                if (s.StuMajorID == 0)
                {
                    ps.StuMajor = "软件工程与管理";
                }
                else if (s.StuMajorID == 1)
                {
                    ps.StuMajor = "虚拟现实与应用";
                }
                else if (s.StuMajorID == 2)
                {
                    ps.StuMajor = "人工智能";
                }
                else if (s.StuMajorID == 3)
                {
                    ps.StuMajor = "大数据技术与应用";
                }
                retunList.Add(ps);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            var json = serializer.Serialize(retunList);
            rel = json.ToString();
            return rel;
        }

        public string deleteFirstSelect(int StuID)
        {
            string rel = "";
            HttpCookie accountCookie = Request.Cookies["Account"];
            int pid = int.Parse(accountCookie["userId"]);
            rel = dbhelper.deleteProfessorToStudent(pid, StuID);
            dbhelper.changeStudentWilleState(pid,StuID, 1, 0);
            return rel;
        }

        public string deleteSecondSelect(int StuID)
        {
            string rel = "";
            HttpCookie accountCookie = Request.Cookies["Account"];
            int pid = int.Parse(accountCookie["userId"]);
            List<Student> list = dbhelper.getStudentByStuID(StuID);
            if(list.Count <=0)
            {
                rel = "未知错误";
                return rel;
            }
            if (list[0].StuFirstWillStates == 1)
            {
                rel = "此为第一轮选择的学生不可删除";
                return rel;
            }

            rel = dbhelper.deleteProfessorToStudent(pid, StuID);
            dbhelper.changeStudentWilleState(pid, StuID, 2, 0);
            return rel;
        }


        private class ProfessorStudent
        {
            public int StuID { get; set; }
            public string StuName { get; set; } //姓名1
            public string Gender { get; set; } //性别
            public string StuGraSchool { get; set; }//毕业学校
            public string StuMajor { get; set; }//方向 暂定 0软件工程与管理 1虚拟现实与应用 2人工智能 3大数据技术与应用
        }
    }
}