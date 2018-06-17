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
    public class Gao_AdminController : Controller
    {

        private DataBaseHelper dbhelper = new DataBaseHelper();
        // GET: Gao_Admin
        public ActionResult Index()
        {
            // string s = getProfessors(0);
            //  dbhelper.addProfessor(2, 22, "22", 1, "ii", 222, 222);
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

        public ActionResult JiaoWu()
        {
            // string s = getProfessors(0);
            //  dbhelper.addProfessor(2, 22, "22", 1, "ii", 222, 222);
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

        public ActionResult Setting()
        {
            Settings s = dbhelper.getSetting();
            if(s!= null)
            {
                ViewData["InfoStart"] = s.InfoStart;
                ViewData["InfoEnd"] = s.InfoEnd;
                ViewData["FirstStart"] = s.FirstStart;
                ViewData["FirstEnd"] = s.FirstEnd;
                ViewData["SecondStart"] = s.SecondStart;
                ViewData["SecondEnd"] = s.SecondEnd;
                HttpCookie accountCookie = Request.Cookies["Account"];
                if(accountCookie == null)
                {
                    ViewData["userId"] = "";
                }
                else
                {
                    ViewData["userId"] = int.Parse(accountCookie["userId"]);
                }
            }
            else
            {
                ViewData["InfoStart"] = "";
                ViewData["InfoEnd"] = "";
                ViewData["FirstStart"] = "";
                ViewData["FirstEnd"] = "";
                ViewData["SecondStart"] = "";
                ViewData["SecondEnd"] = "";
                HttpCookie accountCookie = Request.Cookies["Account"];
                if (accountCookie == null)
                {
                    ViewData["userId"] = "";
                }
                else
                {
                    ViewData["userId"] = int.Parse(accountCookie["userId"]);
                }
            }
            return View();
        }


        public ActionResult Student()
        {
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

        public ActionResult ProfessorInformation(int UserId)
        {
            Professor p = dbhelper.findProfessor(UserId);
            if(p== null)
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

        public ActionResult saveProfessorInfo(string ProName, int ProID, string ProTitle, int ProMaxNum, string ProInfoUrl, int ProNum)
        {
            string rel = dbhelper.updateProfessor(ProName, ProID, ProTitle, ProMaxNum, ProInfoUrl, ProNum);
            return View("Index");
        }

        /**
         * 
         * 
         * 
         * 
         * */
        public ActionResult creatSetting(DateTime InfoStart, DateTime InfoEnd, DateTime FirstStart, DateTime FirstEnd, DateTime SecondStart, DateTime SecondEnd)
        {
            string rel= dbhelper.creatSetting(InfoStart, InfoEnd, FirstStart, FirstEnd, SecondStart, SecondEnd);

            return  RedirectToAction("Setting", "Gao_Admin");
        }

        public ActionResult AddProfessorInfo()
        {
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

        public ActionResult AddJiaoWuInfo()
        {
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

        public ActionResult AddStudentInfo()
        {
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

        public ActionResult addProfessor(string ProName, int ProID, string ProTitle, int ProMaxNum, string ProInfoUrl, int ProNum)
        {
            dbhelper.addProfessor(ProID, ProName, ProTitle, ProInfoUrl, ProNum, ProMaxNum);
            return View("Index");
        }

        public ActionResult addStudent(string StuName, string StuID, int Age, int StuMajorID, string StuTel, string StuMail, bool StuIfWork)
        {
            bool b = dbhelper.addStudent(StuName, StuID, Age, StuMajorID, StuTel, StuMail, StuIfWork);
            return RedirectToAction("Student", "Gao_Admin");
        }

        public ActionResult StudentInfo(string id)
        {
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

                if (u3.Count>0)
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

        public ActionResult addJiaoWu(string JiaoWuName, int JiaoWuID, int JiaoWuMajorID)
        {
            bool b = dbhelper.addJiaoWu(JiaoWuName,JiaoWuID,JiaoWuMajorID);
            return RedirectToAction("JiaoWu", "Gao_Admin");
        }



        /*
         * @param file: excel文件
         * @功能：excel批量导入导师
         * */
        public string batchCreateTeachers(HttpPostedFileBase file)
        {
            var severPath = this.Server.MapPath("/ExcelFiles/");
            if (!Directory.Exists(severPath))
            {
                Directory.CreateDirectory(severPath);
            }
            var savePath = Path.Combine(severPath, file.FileName);
            Professor teacher = null;
            string result = "{}";
            List<Professor> listTeachers = new List<Professor>();
            Workbook workbook = new Workbook();
            Worksheet sheet = null;
            string error = "";
            try
            {
                if (string.Empty.Equals(file.FileName) || (".xls" != Path.GetExtension(file.FileName) && ".xlsx" != Path.GetExtension(file.FileName)))
                {
                    throw new Exception("文件格式不正确");
                }

                file.SaveAs(savePath);
                workbook.LoadFromFile(savePath);
                sheet = workbook.Worksheets[0];
                int row = sheet.Rows.Length;//获取不为空的行数
                int col = sheet.Columns.Length;//获取不为空的列数
                string tempId;
                string tempName;
                string tempTitle;
                string tempUrl;
                string tempMaxNum;
                int idcol = -11;
                int namecol = -11;
                int titlecol = -11;
                int idrow = -11;
                int urlcol = -11;
                int maxnumcol = -11;
                CellRange[] cellrange = sheet.Cells;
                int rangelength = cellrange.Length;
                for (int i = 0; i < row; i++)
                {
                    for (int j = 0; j < col; j++)
                    {
                        tempId = cellrange[i * col + j].Value;
                        if (tempId.Equals("工号"))
                        {
                            idcol = j;
                            idrow = i + 1;
                        }
                        if (tempId.Equals("姓名"))
                        {
                            namecol = j;
                        }
                        if (tempId.Equals("职称"))
                        {
                            titlecol = j;
                        }
                        if (tempId.Equals("介绍页面url"))
                        {
                            urlcol = j;
                        }
                        if (tempId.Equals("最大招收学生数"))
                        {
                            maxnumcol = j;
                        }
                    }
                    if (idcol >= 0 && namecol >= 0)
                    {
                        break;
                    }
                }

                if (idcol < 0 || namecol < 0)
                {
                    throw new Exception("不是教师表");
                }
                    for (int i = idrow; i < row; i++)
                    {
                        tempId = cellrange[i * col + idcol].Value;
                        tempName = cellrange[i * col + namecol].Value;
                        tempTitle = cellrange[i * col + titlecol].Value;
                    tempUrl = cellrange[i * col + urlcol].Value;
                    tempMaxNum = cellrange[i * col + maxnumcol].Value;
                    if (tempName != "")
                        {
                            teacher = new Professor();
                            teacher.ProID = int.Parse(tempId);
                            teacher.ProName = tempName;
                        int num = dbhelper.convertProTitleToNum(tempTitle);
                        if (num == -1)
                        {
                            throw new Exception("表中数据格式错误");
                        }
                        teacher.ProTitle = num;
                        teacher.ProInfoUrl=tempUrl;
                        teacher.ProMaxNum = int.Parse(tempMaxNum);
                            listTeachers.Add(teacher);
                        }

                    }
                    error = dbhelper.batchAddTeachers(listTeachers);
                    if (error.StartsWith("error"))
                    {
                        throw new Exception("数据库更新出错");
                    }
            }
            catch (Exception e)
            {
                if (e.Message.Equals("数据库更新出错"))
                {
                    result = "{\"error\":\""+error+"\"}";
                }
                else if (e.Message.Equals("文件格式不正确"))
                {
                    result = "{\"error\":\"文件格式不正确\"}";
                }
                else
                {
                    result = "{\"error\":\"在服务器端发生错误请联系管理员\"}";
                }
            }
            finally
            {
                workbook.Dispose();
                sheet = null;
                workbook = null;
            }
            return result;
        }


        /*
 * @param file: excel文件
 * @功能：excel批量导入j教务
 * */
        public string batchCreateJiaoWus(HttpPostedFileBase file)
        {
            var severPath = this.Server.MapPath("/ExcelFiles/");
            if (!Directory.Exists(severPath))
            {
                Directory.CreateDirectory(severPath);
            }
            var savePath = Path.Combine(severPath, file.FileName);
            JiaoWu teacher = null;
            string result = "{}";
            List<JiaoWu> listTeachers = new List<JiaoWu>();
            Workbook workbook = new Workbook();
            Worksheet sheet = null;
            string error = "";
            try
            {
                if (string.Empty.Equals(file.FileName) || (".xls" != Path.GetExtension(file.FileName) && ".xlsx" != Path.GetExtension(file.FileName)))
                {
                    throw new Exception("文件格式不正确");
                }

                file.SaveAs(savePath);
                workbook.LoadFromFile(savePath);
                sheet = workbook.Worksheets[0];
                int row = sheet.Rows.Length;//获取不为空的行数
                int col = sheet.Columns.Length;//获取不为空的列数
                string tempId;
                string tempName;
                string tempTitle;

                int idcol = -11;
                int namecol = -11;
                int titlecol = -11;
                int idrow = -11;
                CellRange[] cellrange = sheet.Cells;
                int rangelength = cellrange.Length;
                for (int i = 0; i < row; i++)
                {
                    for (int j = 0; j < col; j++)
                    {
                        tempId = cellrange[i * col + j].Value;
                        if (tempId.Equals("教务教师编号"))
                        {
                            idcol = j;
                            idrow = i + 1;
                        }
                        if (tempId.Equals("姓名"))
                        {
                            namecol = j;
                        }
                        if (tempId.Equals("负责专业编号"))
                        {
                            titlecol = j;
                        }
                    }
                    if (idcol >= 0 && namecol >= 0)
                    {
                        break;
                    }
                }

                if (idcol < 0 || namecol < 0)
                {
                    throw new Exception("不是教师表");
                }
                for (int i = idrow; i < row; i++)
                {
                    tempId = cellrange[i * col + idcol].Value;
                    tempName = cellrange[i * col + namecol].Value;
                    tempTitle = cellrange[i * col + titlecol].Value;
                    if (tempName != "")
                    {
                        teacher = new JiaoWu();
                        teacher.JiaoWuID = int.Parse(tempId);
                        teacher.JiaoWuName = tempName;
                        teacher.JiaoWuMajorID = int.Parse(tempTitle);
                        listTeachers.Add(teacher);
                    }

                }
                error = dbhelper.batchAddJiaoWus(listTeachers);
               // error = dbhelper.batchAddTeachers(listTeachers);
                if (error.StartsWith("error"))
                {
                    throw new Exception("数据库更新出错");
                }
            }
            catch (Exception e)
            {
                if (e.Message.Equals("数据库更新出错"))
                {
                    result = "{\"error\":\"" + error + "\"}";
                }
                else if (e.Message.Equals("文件格式不正确"))
                {
                    result = "{\"error\":\"文件格式不正确\"}";
                }
                else
                {
                    result = "{\"error\":\"在服务器端发生错误请联系管理员\"}";
                }
            }
            finally
            {
                workbook.Dispose();
                sheet = null;
                workbook = null;
            }
            return result;
        }

        /*
         * @param file: excel文件
         * @功能：excel批量导入学生
         * */
        public string batchCreateStudents(HttpPostedFileBase file)
        {
            var severPath = this.Server.MapPath("/ExcelFiles/");
            if (!Directory.Exists(severPath))
            {
                Directory.CreateDirectory(severPath);
            }
            var savePath = Path.Combine(severPath, file.FileName);
            Student student = null;
            string result = "{}";
            List<Student> listStudents = new List<Student>();
            Workbook workbook = new Workbook();
            Worksheet sheet = null;
            string error = "";
            try
            {
                if (string.Empty.Equals(file.FileName) || (".xls" != Path.GetExtension(file.FileName) && ".xlsx" != Path.GetExtension(file.FileName)))
                {
                    throw new Exception("文件格式不正确");
                }

                file.SaveAs(savePath);
                workbook.LoadFromFile(savePath);
                sheet = workbook.Worksheets[0];
                int row = sheet.Rows.Length;//获取不为空的行数
                int col = sheet.Columns.Length;//获取不为空的列数
                string tempId;
                string tempName;
                string tempGender;
                string tempAge;
                string tempStuGraSchool;
                string tempStuGraMajor;
                string tempStuTel;
                string tempStuMail;
                string tempStuIfWork;

                int idcol = -11;
                int namecol = -11;
                int Gendercol = -11;
             //   int Agecol = -11;
                int StuGraSchoolcol = -11;
                int StuGraMajorcol = -11;
             //   int StuTelcol = -11;
               // int StuMailcol = -11;
               // int StuIfWorkcol = -11;
                int idrow = -11;
                //int maxnumcol = -11;
                CellRange[] cellrange = sheet.Cells;
                int rangelength = cellrange.Length;
                for (int i = 0; i < row; i++)
                {
                    for (int j = 0; j < col; j++)
                    {
                        tempId = cellrange[i * col + j].Value;
                        if (tempId.Equals("学号"))
                        {
                            idcol = j;
                            idrow = i + 1;
                        }
                        else if (tempId.Equals("姓名"))
                        {
                            namecol = j;
                        }
                        else if (tempId.Equals("性别"))
                        {
                            Gendercol = j;
                        }
                   /*     else if (tempId.Equals("年龄"))
                        {
                            Agecol = j;
                        }
                        */
                        else if (tempId.Equals("毕业院校"))
                        {
                            StuGraSchoolcol = j;
                        }
                        else if (tempId.Equals("毕业专业"))
                        {
                            StuGraMajorcol = j;
                        }
                        /*      else if (tempId.Equals("电话"))
                             {
                                 StuTelcol = j;
                             }
                             else if (tempId.Equals("邮箱"))
                             {
                                 StuMailcol = j;
                             }
                             else if (tempId.Equals("是否在职工作"))
                             {
                                 StuIfWorkcol = j;
                             }
                             */
                    }
                    if (idcol >= 0 && namecol >= 0)
                    {
                        break;
                    }
                }

                if (idcol < 0 || namecol < 0)
                {
                    throw new Exception("不是学生表");
                }
                for (int i = idrow; i < row; i++)
                {
                    tempId = cellrange[i * col + idcol].Value;
                    tempName = cellrange[i * col + namecol].Value;
                    tempGender = cellrange[i * col + Gendercol].Value;
                //    tempAge = cellrange[i * col + Agecol].Value;
                    tempStuGraSchool = cellrange[i * col + StuGraSchoolcol].Value;
                    tempStuGraMajor = cellrange[i * col + StuGraMajorcol].Value;
                //    tempStuTel = cellrange[i * col + StuTelcol].Value;
                //    tempStuMail = cellrange[i * col + StuMailcol].Value;
                //    tempStuIfWork = cellrange[i * col + StuIfWorkcol].Value;
                    if (tempName != "")
                    {
                        student = new Student();
                        student.StuID = tempId;
                        student.StuName = tempName;
                        if (tempGender.Equals("男"))
                        {
                            student.Gender = true;
                        }
                        else
                        {
                            student.Gender = false;
                        }
                      //  student.Age = int.Parse(tempAge);
                        student.StuGraSchool = tempStuGraSchool;
                        student.StuGraMajor = tempStuGraMajor;
                    //    student.StuTel = tempStuTel;
                  //      student.StuMail = tempStuMail;
                        if (tempGender.Equals("是"))
                        {
                            student.StuIfWork = true;
                        }
                        else
                        {
                            student.StuIfWork = false;
                        }
                        listStudents.Add(student);
                    }

                }
                error = dbhelper.batchAddStudents(listStudents);
                if (error.StartsWith("error"))
                {
                    throw new Exception("数据库更新出错");
                }
            }
            catch (Exception e)
            {
                if (e.Message.Equals("不是学生表"))
                {
                    result = "{\"error\":\"不是学生表\"}";
                }
                else if (e.Message.Equals("文件格式不正确"))
                {
                    result = "{\"error\":\"文件格式不正确\"}";
                }
                else
                {
                    result = "{\"error\":\"在服务器端发生错误请联系管理员\"}";
                }
            }
            finally
            {
                workbook.Dispose();
                sheet = null;
                workbook = null;
            }
            return result;
        }

        /*
         * @param 0 表示全部老师
         * @param 1 表示名额未满老师
         * */
        public string getProfessors(int p )
        {
            string rel = "";
            List<Professor> ps = null;
            int order = 1;
            AdminProfessor ap = null;
            List<AdminProfessor> list = new List<AdminProfessor>();
            if (p == 0)
            {
                ps = dbhelper.findAllProfessors();
            }else if (p == 1)
            {
                ps = dbhelper.findNotFullProfessors();
            }
            else
            {
                return rel;
            }

            if(ps == null)
            {
                return rel;
            }
            else
            {
                foreach(Professor t in ps)
                {
                    ap = new AdminProfessor();
                    ap.Order = order++;
                    ap.UserId = t.UserID;
                    ap.ProMaxNum = t.ProMaxNum;
                    ap.ProName = t.ProName;
                    ap.ProNum = t.ProNum;
                    if(t.ProTitle == 0)
                    {
                        ap.ProTitle = "讲师";
                    }else if (t.ProTitle == 1)
                    {
                        ap.ProTitle = "副教授";
                    }else
                    {
                        ap.ProTitle = "教授";
                    }
                    list.Add(ap);
                }
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                var json = serializer.Serialize(list);
                rel = json.ToString();
                serializer = null;
            }
            return rel;

        }
        public string searchProfessors( string professorName,int p)
        {
            string rel = "";
            List<Professor> ps = null;
            int order = 1;
            AdminProfessor ap = null;
            List<AdminProfessor> list = new List<AdminProfessor>();
            if (p == 0)
            {
                ps = dbhelper.findAllProfessors();
            }
            else if (p == 1)
            {
                ps = dbhelper.findNotFullProfessors();
            }
            else
            {
                return rel;
            }

            if (ps == null)
            {
                return rel;
            }
            else
            {
                foreach (Professor t in ps)
                {
                    ap = new AdminProfessor();
                    ap.Order = order++;
                    ap.UserId = t.UserID;
                    ap.ProMaxNum = t.ProMaxNum;
                    ap.ProName = t.ProName;
                    ap.ProNum = t.ProNum;
                    if (t.ProTitle == 0)
                    {
                        ap.ProTitle = "讲师";
                    }
                    else if (t.ProTitle == 1)
                    {
                        ap.ProTitle = "副教授";
                    }
                    else
                    {
                        ap.ProTitle = "教授";
                    }
                    list.Add(ap);
                }
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                if (professorName == "")
                {
                    var json = serializer.Serialize(list);
                    rel = json.ToString();
                    serializer = null;
                }
                else
                {
                    List<AdminProfessor> tempList = new List<AdminProfessor>();
                    AdminProfessor temp = null;
                    foreach(AdminProfessor t in list)
                    {
                        if(professorName == t.ProName)
                        {
                            temp = t;
                            break;
                        }
                    }
                    tempList.Add(temp);
                    var json = serializer.Serialize(tempList);
                    rel = json.ToString();
                    serializer = null;
                }
            }
            return rel;

        }


        public string deleteProfess(int UserId)
        {
            bool b = dbhelper.deleteProfesById(UserId);
            if (b)
            {
                return "删除成功";
            }
            return "删除失败";
        }

        public string deleteJiaoWu(int UserId)
        {
            bool b = dbhelper.deleteJiaoWuById(UserId);
            if (b)
            {
                return "删除成功";
            }
            return "删除失败";
        }

        public string deleteStudent(int UserId)
        {
            bool b = dbhelper.deleteStudentById(UserId);
            if (b)
            {
                return "删除成功";
            }
            return "删除失败";
        }


        public string changeFinalWill(string ProName, string StuID)
        {
            Professor p = dbhelper.getProfessorByProName(ProName);
            if (p == null)
            {
                return "失败";
            }

            if (dbhelper.changeStudentWilleState(p.ProID, StuID, 3, 1))
                return "成功";
            else
                return "失败";
        }


        public string getStudents(int p)
        {
            HttpCookie accountCookie = Request.Cookies["Account"];
            int usertype = int.Parse(accountCookie["type"]);
            int userId = int.Parse(accountCookie["userId"]);
            

            string rel = "";
            List<Student> ps = null;
            int order = 1;
            AdminStudent ap = null;
            List<AdminStudent> list = new List<AdminStudent>();
            if (p == 0)
            {
                ps = dbhelper.findAllStudents();
            }
            else if (p == 1)
            {
                ps = dbhelper.findNoInfoStudents();
            }
            else if(p==2)
            {
                ps = dbhelper.findNoWillStudents();
            }else if (p == 3)
            {
                ps = dbhelper.findNoFinalWillStudents();
            }
            else
            {
                return rel;
            }

            if (ps == null)
            {
                return rel;
            }
            else
            {
                foreach (Student t in ps)
                {
                    ap = new AdminStudent();
                    ap.Order = order++;
                    ap.UserId = t.UserID;
                    if(t.StuFinalWill == 0)
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
                        ap.StuWillChecked = "是" ;
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
            string rel = "";
            List<Student> ps = null;
            int order = 1;
            AdminStudent ap = null;
            List<AdminStudent> list = new List<AdminStudent>();
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

            if (ps == null)
            {
                return rel;
            }
            else
            {
                foreach (Student t in ps)
                {
                    ap = new AdminStudent();
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
                        ap.StuInfoChecked ="是";
                    }
                    else
                    {
                        ap.StuInfoChecked ="否";
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
                foreach(AdminStudent s in list)
                {
                    if (s.StuName.Equals(name))
                    {
                        List<AdminStudent> tem = new List<AdminStudent>();
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


        public string deleteSelect(string StuID , int ProID)
        {
            string rel = "";
            rel = dbhelper.deleteProfessorToStudent(ProID, StuID);
            dbhelper.changeStudentWilleState(ProID, StuID, 1, 0);
            dbhelper.changeStudentWilleState(ProID, StuID, 2, 0);
            return rel;
        }

        public string selectStudentToProfessor(string StuID, int ProID)
        {
            string rel = "";
            List<Student> list = dbhelper.getStudentByStuID(StuID);
            if(list.Count < 1)
            {
                rel = "此学生不存在";
            }
            else
            {
                dbhelper.changeStudentWilleState(ProID, StuID, 3, 1);
                rel = dbhelper.addProfessorToStudent(ProID, StuID);
            }            
            return rel;
        }

        public string getStudentsOfProfessorToStudent( int id)
        {
            string rel = "";
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
                ps.Gender = s.Gender;
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


        public void FileEx(string id)
        {
            List<Student> list = dbhelper.getStudentByStuID(id);
            if (list.Count <= 0)
            {
                return;
            }
            string strWord = ExprotMissionToWord(this.Server.MapPath("/Content/模板.htm"),list[0]);
            HttpCookie accountCookie = Request.Cookies["Account"];
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

        public string ExprotMissionToWord(string templatePath ,Student u)
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
            else if(u.StuMajorID==1)
            {
                sb.Replace("{majorwill}", "虚拟现实与应用");
            }else if(u.StuMajorID == 2)
            {
                sb.Replace("{majorwill}", "人工智能");
            }else if (u.StuMajorID == 3)
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

        public string getJiaoWus()
        {
            string rel = "";
            List<JiaoWu> list = new List<JiaoWu>();
            list = dbhelper.getJiaoWus();

            List<AdminJiaoWu> aws = new List<AdminJiaoWu>();
            AdminJiaoWu aw;
            foreach(JiaoWu j in list)
            {
                aw = new AdminJiaoWu();
                aw.JiaoWuID = j.JiaoWuID;
                aw.JiaoWuName = j.JiaoWuName;
                aw.UserID = j.UserID;
                aw.setJiaoWuMajor(j.JiaoWuMajorID);
                aws.Add(aw);
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            var json = serializer.Serialize(aws);
            rel = json.ToString();
            serializer = null;
            
            return rel;

        }
        public string changeStudentInfo(string StuName , string StuID, int Age , int StuMajorID, string StuTel ,bool StuIfWork, string StuMail)
        {
            string rel = dbhelper.changeStudentInfo(StuName, StuID, Age, StuMajorID, StuTel, StuIfWork, StuMail);
            return rel;
        }

        public string findStudent(string StuID)
        {
            List<Student> list = dbhelper.getStudentByStuID(StuID);
            if(list.Count > 0)
            {
                return "{ \"isFound\":\"true\" , \"StuName\":"+ "\"" + list[0].StuName+"\" }";
            }
            else
            {
               return "{ \"isFound\":\"false\" , \"StuName\":" + "\"" + "该学生不存在" + "\" }";
            }
        }


        public class AdminProfessor
        {
            public int Order { set; get; }
            public int UserId { set; get; }
            public string ProName { set; get; }
            public string ProTitle { set; get; }
            public int ProMaxNum { set; get; }
            public int ProNum { set; get; }
        }

        public class AdminStudent
        {
            public int Order { set; get; }
            public int UserId { set; get; }
            public string StuID { set; get; }
            public string StuName { set; get; }
            public string StuInfoChecked { get; set; }//信息是否提交 1
            public string StuWillChecked { get; set; }//是否两个志愿提交 1
            public String StuFinalWill { get; set; }//最终确定导师 1
        }

        private class ProfessorStudent
        {
            public string StuID { get; set; }
            public string StuName { get; set; } //姓名1
            public bool Gender { get; set; } //性别
            public string StuGraSchool { get; set; }//毕业学校
            public string StuMajor { get; set; }//方向 暂定 0软件工程与管理 1虚拟现实与应用 2人工智能 3大数据技术与应用
        }

        private class AdminJiaoWu
        {
            public int UserID { get; set; }
            public int JiaoWuID { get; set; }
            public string JiaoWuName { get; set; }
            public string JiaoWuMajor;

            public void setJiaoWuMajor(int p)
            {
                switch (p)
                {
                    case 0:
                        JiaoWuMajor = "软件工程与管理";
                        break;
                    case 1:
                        JiaoWuMajor = "虚拟现实与应用";
                        break;
                    case 2:
                        JiaoWuMajor = "人工智能";
                        break;
                    case 3:
                        JiaoWuMajor = "大数据技术与应用";
                        break;
                }
            }

            public string getJiaoWuMajor()
            {
                return JiaoWuMajor;
            }
        }

    }
}