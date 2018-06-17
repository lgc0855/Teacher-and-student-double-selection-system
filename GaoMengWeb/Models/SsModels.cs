using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace GaoMengWeb.Models
{
    public class User
    {
        [Key]
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string UserPassword { get; set; }
        public int RoleID { get; set; }//账号身份 RoleID 0管理员 1教务老师 2 老师 3学生
    }

    public class JiaoWu
    {
        [Key]
        public int id { get; set; }
        public int UserID { get; set; }
        public int JiaoWuID { get; set; }
        public string JiaoWuName { get; set; }
        public int JiaoWuMajorID { get; set; }//方向代码 暂定 0软件工程与管理 1虚拟现实与应用 2人工智能 3大数据技术与应用

    }

    public class Student
    {
        [Key]
        public int id { get; set; }
        public int UserID { get; set; }

        public string StuID { get; set; } //学号 1
        public string StuName { get; set; } //姓名1
        public bool Gender { get; set; } //性别
        public int Age { get; set; }
        public string StuGraSchool { get; set; }//毕业学校
        public string StuGraMajor { get; set; }//毕业专业
        public int StuMajorID { get; set; }//方向代码 暂定 0软件工程与管理 1虚拟现实与应用 2人工智能 3大数据技术与应用

        public string StuTel { get; set; }//电话
        public string StuMail { get; set; }//邮箱

        public bool StuIfWork { get; set; }//是否在职工作
        public bool StuInfoChecked { get; set; }//信息是否提交 1

        public string StuResumeUrl { get; set; }//简历文件Url

        public int StuFirstWill { get; set; }//第一志愿导师ID
        public int StuSecondWill { get; set; }//第二志愿导师ID

        public int StuFirstWillStates { get; set; }//导师审核状态 默认0待审批 1通过 2不通过
        public int StuSecondWillStates { get; set; }

        public bool StuWillChecked { get; set; }//是否两个志愿提交 1
        public int StuFinalWill { get; set; }//最终确定导师 1

        //WillList实现学生的备选菜单 一个学生储存几个备选教授
    }

    public class Professor //导师实体类
    {
        [Key]
        public int id { get; set; }
        public int UserID { get; set; }

        public int ProID { get; set; }
        public string ProName { get; set; }
        public int ProTitle { get; set; }//教授职称 0讲师1副教授2教授
        public string ProInfoUrl { get; set; }//教师学院介绍页面url 例如http://soft.buaa.edu.cn/info/1060/1302.htm

        public int ProMaxNum { get; set; }//教师最大招收学生数
        public int ProNum { get; set; }//教师现在招收学生数


        //public int ProStuList { get; set; }//一个联合主键维护当前教授已经确定的学生
    }

    public class ProfessorToStudent
    {
        [Key, Column(Order = 0)]
        public int ProID { get; set; }
        [Key, Column(Order = 1)]
        public string StuID { get; set; } //学号
    }


    public class Settings //系统设置
    {
        [Key]
        public int SettingsID { get; set; }
        public DateTime InfoStart { get; set; } //资料准备时间
        public DateTime InfoEnd { get; set; }

        public DateTime FirstStart { get; set; } //第一轮导师选择
        public DateTime FirstEnd { get; set; }

        public DateTime SecondStart { get; set; } //第二轮导师选择
        public DateTime SecondEnd { get; set; }
    }

}
