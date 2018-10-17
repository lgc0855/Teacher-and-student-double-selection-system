using GaoMengWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace GaoMengWeb.Controllers
{
    public class Gao_JiaoWuController : Controller
    {
        private DataBaseHelper dbhelper = new DataBaseHelper();
        // GET: Gao_JiaoWu
        public ActionResult Student()
        {
            return View();
        }


        public ActionResult Professor()
        {
            return View();
        }

        public ActionResult ProfessorInfo(int UserId)
        {
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
            }
            else
            {
                ViewData["userId"] = int.Parse(accountCookie["userId"]);
            }
            return View();
        }

        public ActionResult StudentInfo(string id)
        {
            HttpCookie accountCookie = Request.Cookies["Account"];
            int usertype = int.Parse(accountCookie["type"]);
            int userId = int.Parse(accountCookie["userId"]);
            List<JiaoWu> jiaoWus = dbhelper.getJiaoWuByJId(userId);


            List<Student> list = dbhelper.getStudentByStuID(id);
            if (list.Count > 0)
            {
                Student s = list[0];

                if (jiaoWus.Count <= 0 || jiaoWus[0].JiaoWuMajorID != s.StuMajorID)
                    return View();
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

                User u1 = dbhelper.getUserByUserId(s.StuFirstWill);
                User u2 = dbhelper.getUserByUserId(s.StuSecondWill);
                List<Professor> u3 = dbhelper.getProfessorByProId(s.StuFinalWill);
                if (u1 != null)
                {
                    ViewData["StuFirstWillName"] = u1.UserName;
                }
                else
                {
                    ViewData["StuFirstWillName"] = "";
                }

                if (u2 != null)
                {
                    ViewData["StuSecondWillName"] = u2.UserName;
                }
                else
                {
                    ViewData["StuSecondWillName"] = "";
                }

                if (u3.Count > 0)
                {
                    ViewData["StuFinalWillName"] = u3[0].ProName;
                }
                else
                {
                    ViewData["StuFinalWillName"] = "";
                }
                ViewData["StuFirstWill"] = s.StuFirstWill;
                ViewData["StuSecondWill"] = s.StuSecondWill;
                ViewData["StuFinalWill"] = s.StuFinalWill;
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
                ViewData["StuFirstWill"] = "";
                ViewData["StuSecondWill"] = "";
                ViewData["StuFinalWill"] = "";
                ViewData["StuFirstWillName"] = "";
                ViewData["StuSecondWillName"] = "";
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

        public string getStudents(int p)
        {
            HttpCookie accountCookie = Request.Cookies["Account"];
            int usertype = int.Parse(accountCookie["type"]);
            int userId = int.Parse(accountCookie["userId"]);


            string rel = "";
            List<Student> ps = null;
            int order = 1;
            JiaoWuStudent ap = null;
            List<JiaoWuStudent> list = new List<JiaoWuStudent>();
            List<JiaoWu> jiaoWus = dbhelper.getJiaoWuByJId(userId);
            JiaoWu jiaoWu = new JiaoWu();
            if (jiaoWus.Count <= 0)
                return rel;
            else
                jiaoWu = jiaoWus[0];
            if (p == 0)
            {
                ps = dbhelper.findAllStudents();
            }
            else if (p == 1)
            {
                ps = dbhelper.findNoInfoStudents();
            }
            else if (p == 2)
            {
                ps = dbhelper.findNoWillStudents();
            }
            else if (p == 3)
            {
                ps = dbhelper.findNoFinalWillStudents();
            }
            else
            {
                return rel;
            }

            ps = selectJiaoWuStudent(ps, jiaoWu);

            if (ps == null)
            {
                return rel;
            }
            else
            {
                foreach (Student t in ps)
                {
                    ap = new JiaoWuStudent();
                    ap.Order = order++;
                    ap.UserId = t.UserID;
                    if (t.StuFinalWill == 0)
                    {
                        ap.StuFinalWill = "无";
                    }
                    else
                    {
                        List<Professor> temlist = dbhelper.getProfessorByProId(t.StuFinalWill);
                        ap.StuFinalWill = temlist[0].ProName;
                    }
                    ap.StuID = t.StuID;
                    if (t.StuInfoChecked)
                    {
                        ap.StuInfoChecked = "是";
                    }
                    else
                    {
                        ap.StuInfoChecked = "否";
                    }
                    ap.StuName = t.StuName;
                    if (t.StuWillChecked)
                    {
                        ap.StuWillChecked = "是";
                    }
                    else
                    {
                        ap.StuWillChecked = "否";
                    }
                    if (usertype == 1)
                    {
                        List<JiaoWu> jlist = dbhelper.getJiaoWuByJId(userId);
                        JiaoWu j = jlist[0];
                        if (j.JiaoWuMajorID == t.StuMajorID)
                        {
                            list.Add(ap);
                        }
                    }
                    else
                    {
                        list.Add(ap);
                    }

                }
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                var json = serializer.Serialize(list);
                rel = json.ToString();
                serializer = null;
            }
            return rel;

        }

        public string searchStudents(string name, int p)
        {

            HttpCookie accountCookie = Request.Cookies["Account"];
            int usertype = int.Parse(accountCookie["type"]);
            int userId = int.Parse(accountCookie["userId"]);

            string rel = "";
            List<Student> ps = null;
            int order = 1;
            JiaoWuStudent ap = null;
            List<JiaoWuStudent> list = new List<JiaoWuStudent>();
            List<JiaoWu> jiaoWus = dbhelper.getJiaoWuByJId(userId);
            JiaoWu jiaoWu = new JiaoWu();
            if (jiaoWus.Count <= 0)
                return rel;
            else
                jiaoWu = jiaoWus[0];

            if (p == 0)
            {
                ps = dbhelper.findAllStudents();
            }
            else if (p == 1)
            {
                ps = dbhelper.findNoInfoStudents();
            }
            else if (p == 2)
            {
                ps = dbhelper.findNoWillStudents();
            }
            else if (p == 3)
            {
                ps = dbhelper.findNoFinalWillStudents();
            }
            else
            {
                return rel;
            }

            ps = selectJiaoWuStudent(ps, jiaoWu);

            if (ps == null)
            {
                return rel;
            }
            else
            {
                foreach (Student t in ps)
                {
                    ap = new JiaoWuStudent();
                    ap.Order = order++;
                    ap.UserId = t.UserID;
                    if (t.StuFinalWill == 0)
                    {
                        ap.StuFinalWill = "无";
                    }
                    else
                    {
                        List<Professor> temlist = dbhelper.getProfessorByProId(t.StuFinalWill);
                        ap.StuFinalWill = temlist[0].ProName;
                    }
                    ap.StuID = t.StuID;
                    if (t.StuInfoChecked)
                    {
                        ap.StuInfoChecked = "是";
                    }
                    else
                    {
                        ap.StuInfoChecked = "否";
                    }

                    ap.StuName = t.StuName;
                    if (t.StuWillChecked)
                    {
                        ap.StuWillChecked = "是";
                    }
                    else
                    {
                        ap.StuWillChecked = "否";
                    }
                    list.Add(ap);
                }



            }
            if (name == "")
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                var json = serializer.Serialize(list);
                rel = json.ToString();
                serializer = null;
            }
            else
            {
                foreach (JiaoWuStudent s in list)
                {
                    if (s.StuName.Equals(name))
                    {
                        List<JiaoWuStudent> tem = new List<JiaoWuStudent>();
                        tem.Add(s);
                        JavaScriptSerializer serializer = new JavaScriptSerializer();
                        var json = serializer.Serialize(tem);
                        rel = json.ToString();
                        serializer = null;
                    }
                }
            }
            return rel;
        }

        public List<Student> selectJiaoWuStudent(List<Student> ps, JiaoWu j)
        {
            List<Student> result = new List<Student>();
            foreach(Student t in ps)
            {
                if (t.StuMajorID == j.JiaoWuMajorID)
                    result.Add(t);
            }
            return result;
        }


        public class JiaoWuStudent
        {
            public int Order { set; get; }
            public int UserId { set; get; }
            public string StuID { set; get; }
            public string StuName { set; get; }
            public string StuInfoChecked { get; set; }//信息是否提交 1
            public string StuWillChecked { get; set; }//是否两个志愿提交 1
            public String StuFinalWill { get; set; }//最终确定导师 1
        }
    }
}