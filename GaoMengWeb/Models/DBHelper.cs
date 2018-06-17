using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Security;
using 高盟_web.Models;

namespace 高盟_web.Models
{
    public class DBHelper
    {
        public User findUserById(string id)
        {
            UserDBContext uDBC = new UserDBContext();
            User user;
            try
            {
                user = uDBC.users.Find(id);
            }
            catch (Exception e)
            {
                return null;
            }
            return user;
        }
        public User findUser(string name , string password)
        {
            UserDBContext uDBC = new UserDBContext();
            foreach(User u in uDBC.users)
            {
                if(u.userName == name && u.userPassword == password)
                {
                    return u;
                }
            }
            return null;
        }
    }
}