using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GaoMengWeb.Models
{
    public class isAuthorizeAttribute:AuthorizeAttribute
    {
        DataBaseHelper dbhelper = new DataBaseHelper();

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            //根据需要添加
            filterContext.HttpContext.Response.Redirect("/Gao_Home/Error");

        }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            //根据需要添加，将自动根据返回值判断用户是否通过验证
            //true：通过
            //false:未通过

            try
            {
                HttpCookie accountCookie = httpContext.Request.Cookies["Account"];
                if (accountCookie == null)
                    return false;
                string id = accountCookie["userId"];
                int type = int.Parse(accountCookie["type"]);
                string passwd = accountCookie["password"];
                bool result = authorizedUser(id, passwd, type);

                return result;
            }
            catch (Exception e)
            {

                return false;
            }
        }


        public Boolean authorizedUser(string userId , string Passwd , int userType)
        {
                List<User> list = dbhelper.getUsers(userType);

                if (userType == 0)
                {
                    if (testAdmin(list, int.Parse(userId), Passwd))
                    {
                        return true;
                    }
                }
                else if (userType == 1)
                {
                    if (testJiaoWu(int.Parse(userId), Passwd))
                    {
                        return true;
                    }
                }
                else if (userType == 2)
                {
                    int st = dbhelper.testSettingTime();
                    if (st == 0 || st == 1 || st == 2)
                    {
                        return false;
                    }

                    if (testProfessor(int.Parse(userId), Passwd))
                    {
                        return true;
                    }
                }
                else if (userType == 3)
                {
                    int st = dbhelper.testSettingTime();
                    if (st == 0 || st == 3 || st == 4 || st == 5)
                    {
                        return false;
                    }
                    if (testStudent(userId, Passwd))
                    {
                        return true;
                    }
                }
                else
                {
                return false;
                }
            if (userId.Equals("-1"))
            {
                return false;
            }
            return false;
        }

        

        private bool testAdmin(List<User> list, int userId, string Passwd)
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
            if (ps.Count == 0)
            {
                return false;
            }
            else
            {
                Professor p = ps[0];
                User u = dbhelper.getUserByUserId(p.UserID);
                if (u.UserPassword == Passwd)
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
    }
}