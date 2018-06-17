using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GaoMengWeb.Models;
using System.Web.Security;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using System.IO;
using System.Reflection;
using Newtonsoft.Json.Linq;
using Spire.Xls;
using System.Threading;

namespace GaoMengWeb.Controllers
{
    public class Gao_HomeController : Controller
    {
        DataBaseHelper dbhelper = new DataBaseHelper();
        
        // GET: Gao_Home
        public ActionResult Index()
        {
            
            return View();
        }

        public ActionResult Login(string error ="")
        {

               ViewData["ERROR"] = error;
            return View();
        }

        public ActionResult DoLogin(string userId = "-1", string Passwd="0000" , int userType=-1)
        {
            if (ModelState.IsValid)
            {
                List<User> list = dbhelper.getUsers(userType);

                if(userType == 0)
                {
                    if (testAdmin(list, int.Parse(userId), Passwd))
                    {
                        HttpCookie accountCookie = new HttpCookie("Account");
                        accountCookie["userId"] = userId.ToString();
                        accountCookie["password"] = Passwd;
                        accountCookie["type"] = userType.ToString();
                        accountCookie.Expires = DateTime.Now.AddMinutes(120);
                        Response.Cookies.Add(accountCookie);
                        return RedirectToAction("Index", "Gao_Admin");
                    }
                }else if (userType == 1)
                {
                    if (testJiaoWu(int.Parse(userId), Passwd))
                    {
                        HttpCookie accountCookie = new HttpCookie("Account");
                        accountCookie["userId"] = userId.ToString();
                        accountCookie["password"] = Passwd;
                        accountCookie["type"] = userType.ToString();
                        accountCookie.Expires = DateTime.Now.AddMinutes(120);
                        Response.Cookies.Add(accountCookie);
                        return RedirectToAction("Index", "Gao_Admin");
                    }
                }
                else if (userType == 2)
                {
                    int st = dbhelper.testSettingTime();
                    if (st == 0 || st==1 || st == 2)
                    {
                        return RedirectToAction("Login", "Gao_Home", new { error = "不在使用时间内" });
                    }

                    if (testProfessor(int.Parse(userId), Passwd))
                    {
                        HttpCookie accountCookie = new HttpCookie("Account");
                        accountCookie["userId"] = userId.ToString();
                        accountCookie["password"] = Passwd;
                        accountCookie["type"] = userType.ToString();
                        accountCookie.Expires = DateTime.Now.AddMinutes(120);
                        Response.Cookies.Add(accountCookie);
                        return RedirectToAction("Index", "GAO_Professor");
                    }
                }
                else if (userType == 3)
                {
                    int st = dbhelper.testSettingTime();
                    if (st == 0 || st == 3 || st == 4 || st == 5)
                    {
                        return RedirectToAction("Login", "Gao_Home", new { error = "不在使用时间内" });
                    }
                    if (testStudent(userId, Passwd))
                    {
                        HttpCookie accountCookie = new HttpCookie("Account");
                        accountCookie["userId"] = userId.ToString();
                        accountCookie["password"] = Passwd;
                        accountCookie["type"] = userType.ToString();
                        accountCookie.Expires = DateTime.Now.AddMinutes(120);
                        Response.Cookies.Add(accountCookie);
                        return RedirectToAction("Index", "GAO_Student");
                    }
                }
                else
                {
                    return RedirectToAction("Login", "Gao_Home", new { error = "请正确填写数据" });
                }

            }
            if(userId.Equals("-1"))
            {
                return RedirectToAction("Login", "Gao_Home", new { error = "请正确填写用户名和密码" });
            }
            return RedirectToAction("Login", "Gao_Home", new { error = "账号或密码不正确" });
        }

        public ActionResult ExitLogin()
        {
            if (Request.Cookies["Account"] != null)
            {
                HttpCookie myCookie = new HttpCookie("Account");
                myCookie.Expires = DateTime.Now.AddDays(-1d);
                Response.Cookies.Add(myCookie);
            }

            return View("Login");
        }

        private bool testAdmin(List<User> list , int userId, string Passwd)
        {
            foreach (User u in list)
            {
                if (u.UserID == userId && u.UserPassword == Passwd)
                {
                    return true;
                }
             }
            return false;
        }

        private bool testJiaoWu(int userId, string Passwd)
        {
            List<JiaoWu> ps = dbhelper.getJiaoWuByJId(userId);
            //dbhelper.getProfessorByProId(userId);
            if (ps.Count == 0)
            {
                return false;
            }
            else
            {
                JiaoWu p = ps[0];
                User u = dbhelper.getUserByUserId(p.UserID);
                if (u.UserPassword == Passwd)
                {
                    return true;
                }
            }
            return false;
        }

        private bool testProfessor(int userId, string Passwd)
        {
            List<Professor> ps = dbhelper.getProfessorByProId(userId);
            if(ps.Count== 0)
            {
                return false;
            }
            else
            {
                Professor p = ps[0];
                User u = dbhelper.getUserByUserId(p.UserID);
                if(u.UserPassword == Passwd)
                {
                    return true;
                }
            }
            return false;
        }

        private bool testStudent(string userId, string Passwd)
        {
            List<Student> ps = dbhelper.getStudentByStuID(userId);
            if (ps.Count == 0)
            {
                return false;
            }
            else
            {
                Student p = ps[0];
                User u = dbhelper.getUserByUserId(p.UserID);
                if (u.UserPassword == Passwd)
                {
                    return true;
                }
            }
            return false;
        }


        public string changePassword(string password)
        {
            string rel = "";
            HttpCookie accountCookie = Request.Cookies["Account"];
            int id = int.Parse(accountCookie["userId"]);
            int type = int.Parse(accountCookie["type"]);
            bool b = dbhelper.changePassword(type, id.ToString(), password);
            if (b)
            {
                rel = "修改成功";
            }
            else
            {
                rel = "修改失败";
            }
            return rel;
        }

        

    }
}