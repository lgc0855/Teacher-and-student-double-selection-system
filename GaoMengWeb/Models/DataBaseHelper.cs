using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Security;
using GaoMengWeb.Models;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Owin;
namespace GaoMengWeb.Models
{
    public class DataBaseHelper
    {
        public bool addProfessor(int UserID, int ProID, string ProName,int ProTitle, string ProInfoUrl, int ProNum ,int  ProMaxNum )
        {
            Professor p = new Professor();
            p.ProID = ProID;
            p.ProInfoUrl = ProInfoUrl;
            p.ProMaxNum = ProMaxNum;
            p.ProName = ProName;
            p.ProNum = ProNum;
            p.ProTitle = ProTitle;
            p.UserID = UserID;
            SsContext db = new SsContext();
            try
            {
                db.Professors.Add(p);
                db.SaveChanges();
            }catch(Exception e)
            {
                db = null;
                return false;
            }
            db = null;
            return true;
        }

        public bool addProfessor( int ProID, string ProName, string ProTitle, string ProInfoUrl, int ProNum, int ProMaxNum)
        {
            Professor p = new Professor();
            p.ProID = ProID;
            p.ProInfoUrl = ProInfoUrl;
            p.ProMaxNum = ProMaxNum;
            p.ProName = ProName;
            p.ProNum = ProNum;
            if (ProTitle.Equals("讲师"))
            {
                p.ProTitle = 0;
            }
            else if (ProTitle.Equals("副教授"))
            {
                p.ProTitle = 1;
            }
            else if (ProTitle.Equals("教授"))
            {
                p.ProTitle = 2;
            }
            SsContext db = new SsContext();
            try
            {
                p.UserID = addUser(p.ProName, 2, p.ProID.ToString());
                db.Professors.Add(p);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                db = null;
                return false;
            }
            db = null;
            return true;
        }
        internal int convertProTitleToNum(string title)
        {
            if(title == "讲师")
            {
                return 0;
            }else if(title== "副教授")
            {
                return 1;
            }else if(title == "教授")
            {
                return 2;
            }
            else
            {
                return -1;
            }
        }
        internal string creatSetting(DateTime InfoStart, DateTime InfoEnd, DateTime FirstStart, DateTime FirstEnd, DateTime SecondStart, DateTime SecondEnd)
        {
            try
            {
                SsContext db = new SsContext();
                List<Settings> list = db.Settingss.ToList();
                Settings s;
                if (list.Count==0)
                {
                    s = new Settings();
                    s.FirstEnd = FirstEnd;
                    s.FirstStart = FirstStart;
                    s.InfoEnd = InfoEnd;
                    s.InfoStart = InfoStart;
                    s.SecondEnd = SecondEnd;
                    s.SecondStart = SecondStart;
                    db.Settingss.Add(s);
                }
                else
                {
                    s = list[0];
                    s.FirstEnd = FirstEnd;
                    s.FirstStart = FirstStart;
                    s.InfoEnd = InfoEnd;
                    s.InfoStart = InfoStart;
                    s.SecondEnd = SecondEnd;
                    s.SecondStart = SecondStart;
                }
                db.SaveChanges();
            }catch(Exception e)
            {
                return e.Message;
            }
            return "success";
            
        }

        internal Settings getSetting()
        {
            SsContext db = new SsContext();
            List<Settings> list = db.Settingss.ToList();
            if (list.Count > 0)
            {
                return list[0];
            }
            else
            {
                return null;
            }
        }

        internal string batchAddJiaoWus(List<JiaoWu> listTeachers)
        {
            string rel = "";
            int num = 0;
            List<JiaoWu> list;
            int userId;
            try
            {
                SsContext db = new SsContext();
                foreach (JiaoWu p in listTeachers)
                {
                    list = db.JiaoWus.Where(s => s.JiaoWuID == p.JiaoWuID).ToList();
                    if (list.Count > 0)
                    {
                        num++;
                    }
                    else
                    {
                        userId = addUser(p.JiaoWuName, 1, p.JiaoWuID.ToString());
                        if (userId != -1)
                        {
                            p.UserID = userId;
                            db.JiaoWus.Add(p);
                        }
                    }

                }
                db.SaveChanges();
            }
            catch (Exception e)
            {
                return e.Message;
            }
            if (num > 0)
            {
                rel = "error:共操作" + listTeachers.Count + "个数据，其中" + num + "个失败,可能存在重复的教师。";
            }
            else
            {
                rel = "success:共操作" + listTeachers.Count + "个数据，其中" + num + "个失败。";
            }



            return rel;
        }

        internal string batchAddTeachers( List<Professor> listTeachers)
        {
            string rel = "";
            int num = 0;
            List<Professor> list;
            int userId;
            try
            {
                SsContext db = new SsContext();
                foreach (Professor p in listTeachers)
                {
                    list = db.Professors.Where(s => s.ProID == p.ProID).ToList();
                    if (list.Count > 0)
                    {
                        num++;
                    }
                    else
                    {
                        userId = addUser(p.ProName, 2, p.ProID.ToString());
                        if (userId != -1)
                        {
                            p.UserID = userId;
                            db.Professors.Add(p);
                        }
                    }
                    
                }
                db.SaveChanges();
            }catch(Exception e)
            {
                return e.Message;
            }
            if (num > 0)
            {
                rel = "error:共操作" + listTeachers.Count + "个数据，其中" + num + "个失败,可能存在重复的教师。";
            }
            else
            {
                rel = "success:共操作" + listTeachers.Count + "个数据，其中" + num + "个失败。";
            }
            


            return rel;
        }

        internal string batchAddStudents(List<Student> listStudents)
        {
            string rel = "";
            int num = 0;
            List<Student> list;
            int userId;
            try
            {
                SsContext db = new SsContext();
                foreach ( Student p in listStudents)
                {
                    list = db.Students.Where(s => s.StuID == p.StuID).ToList();
                    if (list.Count > 0)
                    {
                        num++;
                    }
                    else
                    {
                        userId = addUser(p.StuName, 3, p.StuID.ToString());
                        if (userId != -1)
                        {
                            p.UserID = userId;
                            db.Students.Add(p);
                        }
                    }
                    
                }
                db.SaveChanges();
            }
            catch (Exception e)
            {
                return e.Message;
            }

            if (num > 0)
            {
                rel = "error:共操作" + listStudents.Count + "个数据，其中" + num + "个失败,可能存在重复的数据。";
            }
            else
            {
                rel = "success:共操作" + listStudents.Count + "个数据，其中" + num + "个失败。";
            }



            return rel;
        }
        internal int addUser(string UserName , int RoleID , string password)
        {
            User t;
            try
            {
                SsContext db = new SsContext();
                User u = new User();
                u.UserName = UserName;
                u.UserPassword = password;
                u.RoleID = RoleID;
                t = db.Users.Add(u);
                db.SaveChanges();
                t = db.Users.ToArray()[db.Users.Count() - 1];
            }
            catch (Exception e)
            {
                return -1;
            }

            return t.UserID;
        }

        internal Professor findProfessor(int UserId)
        {
            SsContext db = new SsContext();
            List<Professor> ps = db.Professors.Where(s => s.UserID == UserId).ToList();
            Professor p;
            if (ps.Count <= 0)
            {
                p = null;
            }
            else
            {
                p = ps[0];
            }
            db = null;
            return p;
        }

        public bool deleteProfesById(int id)
        {
            try
            {
                SsContext db = new SsContext();
                List<Professor> ps = db.Professors.Where(s => s.UserID == id).ToList();
                Professor p = null;
                for (int i = 0; i < ps.Count; i++)
                {
                    p = ps[i];
                    db.Professors.Remove(p);
                }

                deleteUserById(id);
                db.SaveChanges();
            }
            catch(Exception e)
            {
                return false;
            }
            return true;
        }

        public bool deleteJiaoWuById(int id)
        {
            try
            {
                SsContext db = new SsContext();
                List<JiaoWu> ps = db.JiaoWus.Where(s => s.UserID == id).ToList();
                JiaoWu p = null;
                for (int i = 0; i < ps.Count; i++)
                {
                    p = ps[i];
                    db.JiaoWus.Remove(p);
                }

                deleteUserById(id);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }
        public bool deleteStudentById(int id)
        {
            try
            {
                SsContext db = new SsContext();
                List<Student> ss = db.Students.Where(s => s.UserID == id).ToList();
                Student student = null;
                for(int i=0;i< ss.Count; i++)
                {
                    student = ss[i];
                    db.Students.Remove(student);
                }
                
                deleteUserById(id);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        public bool deleteUserById(int id)
        {
            try
            {
                SsContext db = new SsContext();
                User p = db.Users.Find(id);

                db.Users.Remove(p);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }


        public List<Professor> findAllProfessors()
        {
            SsContext db = new SsContext();
            List<Professor> ps = db.Professors.ToList();
            db = null;
            return ps;
        }

        public List<Student> findAllStudents()
        {
            SsContext db = new SsContext();
            List<Student> ps = db.Students.ToList();
            db = null;
            return ps;
        }

        public List<User> getUsers( int userType)
        {
            SsContext db = new SsContext();
            List<User> list = db.Users.Where(s => s.RoleID == userType).ToList();
            return list;
        }

        public List<Professor> getProfessorByProId(int id)
        {
            SsContext db = new SsContext();
            List<Professor> list = db.Professors.Where(s => s.ProID == id).ToList();
            return list;
        }
        public List<JiaoWu> getJiaoWuByJId(int id)
        {
            SsContext db = new SsContext();
            List<JiaoWu> list = db.JiaoWus.Where(s => s.JiaoWuID == id).ToList();
            return list;
        }


        public User getUserByUserId(int Id)
        {
            SsContext db = new SsContext();
            User u = db.Users.Find(Id);
            return u;
        }

        public List<Student> getStudentByStuID(string id)
        {
            SsContext db = new SsContext();
            List<Student> list = db.Students.Where(s => s.StuID.Equals(id)).ToList();
            return list;
        }

        public List<Student> getStudentByUserID(int id)
        {
            SsContext db = new SsContext();
            List<Student> list = db.Students.Where(s => s.UserID == id).ToList();
            return list;
        }

        public List<Professor> findNotFullProfessors()
        {
            SsContext db = new SsContext();
            List<Professor> ps = db.Professors.Where(s => s.ProNum < s.ProMaxNum).ToList();
            return ps;
        }

        public List<Student> findNoInfoStudents()
        {
            SsContext db = new SsContext();
            List<Student> ps = db.Students.Where(s => s.StuInfoChecked == false).ToList();
            return ps;
        }
        /**
         * index 1 为StuFirstWillStates  ， 2为StuSecondWillStates
         * 
         * 
         * 
         * */
        public bool changeStudentWilleState(int pid, string StuID, int index ,  int value)
        {
            SsContext db = new SsContext();
            List<Student> list = db.Students.Where(s => s.StuID == StuID).ToList();
            if(list.Count <= 0)
            {
                return false;
            }
            Student student = list[0];
            if(index ==1)
            {
                student.StuFirstWillStates = value;
            }
            else if(index == 2)
            {
                student.StuSecondWillStates = value;
            }



            if(value == 0)
            {
                student.StuFinalWill = 0;
            }
            else
            {
                deleteProfessorToStudent(student.StuFinalWill, StuID);
                addProfessorToStudent(pid, StuID);
                student.StuFinalWill = pid;
            }
            db.SaveChanges();

            return true;
        }

        public List<Student> findNoWillStudents()
        {
            SsContext db = new SsContext();
            List<Student> ps = db.Students.Where(s => s.StuWillChecked == false).ToList();
            return ps;
        }
        public List<Student> findNoFinalWillStudents()
        {
            SsContext db = new SsContext();
            List<Student> ps = db.Students.Where(s => s.StuFinalWill == 0 ).ToList();
            return ps;
        }

        public bool updateStudent(string StuName, string StuID, int Age, int StuMajorID, string StuTel, string StuMail, bool StuIfWork)
        {
            SsContext db = new SsContext();
            List<Student> list = db.Students.Where(s => s.StuID == StuID).ToList();
            if (list.Count <= 0)
            {
                return false;
            }
            else
            {
                Student s = list[0];
                s.Age = Age;
                s.StuMajorID = StuMajorID;
                s.StuTel = StuTel;
                s.StuMail = StuMail;
                s.StuIfWork = StuIfWork;
                db.SaveChanges();
                return true;
            }
        }

        public bool addStudent( string StuName, string StuID, int Age, int StuMajorID, string StuTel, string StuMail, bool StuIfWork)
        {
            SsContext db = new SsContext();
            Student s = new Student();
            s.UserID = addUser(StuName, 3, StuID.ToString());
                s.StuID = StuID;
                s.StuName = StuName;
                s.Age = Age;
                s.StuMajorID = StuMajorID;
                s.StuTel = StuTel;
                s.StuMail = StuMail;
                s.StuIfWork = StuIfWork;
            db.Students.Add(s);
                db.SaveChanges();
                return true;
        }

        public bool addJiaoWu(string JiaoWuName, int JiaoWuID, int JiaoWuMajorID)
        {
            SsContext db = new SsContext();
            JiaoWu j = new JiaoWu();
            j.UserID = addUser(JiaoWuName, 1, JiaoWuID.ToString());
            j.JiaoWuID = JiaoWuID;
            j.JiaoWuMajorID = JiaoWuMajorID;
            j.JiaoWuName = JiaoWuName;
            db.JiaoWus.Add(j);
            db.SaveChanges();
            return true;
        }


        public string updateProfessor(string ProName, int ProID, string ProTitle, int ProMaxNum, string ProInfoUrl, int ProNum)
        {
            SsContext ss = new SsContext();
            List<Professor> list = ss.Professors.Where(s => s.ProID == ProID).ToList();
            if(list.Count <= 0)
            {
                return "未知错误";
            }
            Professor p = list[0];
            p.ProID = ProID;
            p.ProInfoUrl = ProInfoUrl;
            p.ProMaxNum = ProMaxNum;
            p.ProName = ProName;
            p.ProNum = ProNum;
            if (ProTitle.Equals("讲师"))
            {
                p.ProTitle = 0;
            }else if (ProTitle.Equals("副教授"))
            {
                p.ProTitle = 1;
            }
            else if (ProTitle.Equals("教授"))
            {
                p.ProTitle = 2;
            }
            ss.SaveChanges();
            return "成功";
        }
        public bool saveResumeUrl(string id ,string path)
        {
            SsContext db = new SsContext();
            List<Student> list = db.Students.Where(s => s.StuID == id).ToList();
            if (list.Count <= 0)
            {
                return false;
            }
            else
            {
                Student s = list[0];
                s.StuResumeUrl = path;
                db.SaveChanges();
                return true;
            }
        }

        public bool studentConfirm(string id)
        {
            SsContext db = new SsContext();
            List<Student> list = db.Students.Where(s => s.StuID == id).ToList();
            if (list.Count <= 0)
            {
                return false;
            }
            else
            {
                Student s = list[0];
                s.StuInfoChecked = true;
                db.SaveChanges();
                return true;
            }
        }

        public string getInfoEnd()
        {
            SsContext db = new SsContext();
            List<Settings> ss = db.Settingss.ToList();
            if (ss.Count > 0)
            {
                Settings s = ss[ss.Count - 1];
                return s.InfoEnd.ToString();
            }
            return "未设定";
        }

        public bool studentConfirmWill(string id, int first, int second)
        {
            SsContext db = new SsContext();
            List<Student> list = db.Students.Where(s => s.StuID == id).ToList();
            if (list.Count <= 0)
            {
                return false;
            }
            else
            {
                Student s = list[0];
                s.StuFirstWill = first;
                s.StuSecondWill = second;
                db.SaveChanges();
                return true;
            }
        }

        public List<Student> getFirstWillStudents(int id)
        {
            SsContext db = new SsContext();
            List<Professor> plist = db.Professors.Where(s => s.ProID == id).ToList();
            if(plist.Count <= 0)
            {
                return null;
            }
            Professor p = plist[0];
            List<Student> list = db.Students.Where(s => s.StuFirstWill == p.UserID && s.StuFirstWillStates == 0).ToList();
            if(list.Count > 0)
            {
                return list;
            }
            return null;
        }

        public List<Student> getSecondWillStudents(int id)
        {
            SsContext db = new SsContext();
            List<Professor> plist = db.Professors.Where(s => s.ProID == id ).ToList();
            if (plist.Count <= 0)
            {
                return null;
            }
            Professor p = plist[0];
            List<Student> list = db.Students.Where(s => s.StuSecondWill == p.UserID && s.StuSecondWillStates == 0 ).ToList();
            if (list.Count > 0)
            {
                return list;
            }
            return null;
        }

        public string addProfessorToStudent( int pid, string StuID)
        {
            SsContext db = new SsContext();
            try
            {
                
                ProfessorToStudent pts = new ProfessorToStudent();
                List<Professor> plist = db.Professors.Where(s => s.ProID == pid).ToList();
                if(plist.Count <= 0)
                {
                    return "未知错误";
                }
                Professor p = plist[0];
                int num = db.ProfessorToStudents.Where(s => s.ProID == pid).ToList().Count;
                if(num >= p.ProMaxNum || p.ProNum >= p.ProMaxNum)
                {
                    return "选择学生数已达最大值";
                }
                if(p.ProNum< p.ProMaxNum)
                p.ProNum++;


                pts.ProID = pid;
                pts.StuID = StuID;
                db.ProfessorToStudents.Add(pts);
                db.SaveChanges();
            }catch(Exception e)
            {
                ProfessorToStudent p =  db.ProfessorToStudents.Find(pid, StuID);
                if(p== null)
                {
                    return "操作失败";
                }
                else
                {
                    return "操作成功";
                }
            }
            return "操作成功";
        }

        public string deleteProfessorToStudent(int pid, string StuID)
        {
            try
            {
                SsContext db = new SsContext();
                ProfessorToStudent ps = db.ProfessorToStudents.Find(pid, StuID);
                ProfessorToStudent pts = new ProfessorToStudent();
                List<Professor> plist = db.Professors.Where(s => s.ProID == pid).ToList();
                if (plist[0].ProNum>0)
                plist[0].ProNum--;
                if (plist.Count <= 0)
                {
                    return "未知错误";
                }
                db.ProfessorToStudents.Remove(ps);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                return "操作失败";
            }
            return "操作成功";
        }


        public List<Student> getStudentsOfProfessorToStudent(int id)
        {
            List<Student> returnList = new List<Student>();
            SsContext db = new SsContext();
            List<ProfessorToStudent> pslist = db.ProfessorToStudents.Where(s => s.ProID == id).ToList();
            if(pslist.Count <= 0)
            {
                return null;
            }
            foreach(ProfessorToStudent ps in pslist)
            {
                Student s = getStudentBySid(ps.StuID);
                if(s != null)
                {
                    returnList.Add(s);
                }
            }
            return returnList;
        }

        private Student getStudentBySid(string id)
        {
            SsContext db = new SsContext();
            List<Student> plist = db.Students.Where(s => s.StuID == id).ToList();
            if (plist.Count <= 0)
            {
                return null;
            }
            Student p = plist[0];
            return p;
        }

         Professor getProfessorByPid( int id)
        {
            SsContext db = new SsContext();
            List<Professor> plist = db.Professors.Where(s => s.ProID == id).ToList();
            if (plist.Count <= 0)
            {
                return null;
            }
            Professor p = plist[0];
            return p;
        }

        public List<JiaoWu> getJiaoWus()
        {
            SsContext db = new SsContext();
            List<JiaoWu> list = db.JiaoWus.ToList();
            return list;
        }


        public Professor getProfessorByProName(string name)
        {
            SsContext db = new SsContext();
            List<Professor> list = db.Professors.Where(s => s.ProName == name).ToList();
            if(list.Count > 0)
            {
                return list[0];
            }
            else
            {
                return null;
            }
        }

        public bool changePassword(int type,string id,string password)
        {
            SsContext db = new SsContext();
            if (type == 2)
            {
                Professor p = getProfessorByPid(int.Parse(id));
                id = p.UserID.ToString();

            }else if (type == 3)
            {
                Student s = getStudentBySid(id);
                id = s.UserID.ToString();
            }

            User u = db.Users.Find(id);
            if (u != null)
            {
                try
                {
                    u.UserPassword = password;
                    db.SaveChanges();
                    return true;
                }catch(Exception e)
                {
                    return false;
                }
                
                
            }
            return false;
        }

        public int testSettingTime()
        {
            Settings s = getSetting();
            if (s == null)
            {
                return 0;
            }
            DateTime now = DateTime.Now;
            if (now < s.InfoStart)
            {
                return 0;
            }
            else if (now < s.InfoEnd)
            {
                return 1;
            }
            else if (now < s.FirstStart)
            {
                return 2;
            }
            else if (now < s.FirstEnd)
            {
                return 3;
            }
            else if (now < s.SecondStart)
            {
                return 4;
            }
            else if (now < s.SecondEnd)
            {
                return 5;
            }
            else
            {
                return 6;
            }

        }

        public string changeStudentInfo(string StuName, string StuID, int Age, int StuMajorID, string StuTel, bool StuIfWork, string StuMail)
        {
            SsContext db = new SsContext();
            string rel = "";
            List<Student> list = db.Students.Where(s=>s.StuID == StuID).ToList();
            if (list.Count < 1)
            {
                rel = "不存在此学生";
            }
            else
            {
                Student s = list[0];
                s.Age = Age;
                s.StuMajorID = StuMajorID;
                s.StuTel = StuTel;
                s.StuIfWork = StuIfWork;
                s.StuMail = StuMail;
                db.SaveChanges();
                rel = "成功";
            }
            return rel;
        }
    }
}