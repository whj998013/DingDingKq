using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttDbContext
{
    public class AttUserOper
    {
        AttContext ac = new AttContext();
        public USERINFO GetUserinfo(string userName)
        {
            return ac.USERINFO.FirstOrDefault(p => p.Name == userName && p.DEFAULTDEPTID != 70);
        }

        public int GetUserId(string userName)
        {
            var u = GetUserinfo(userName);
            if (u != null)
            {
                return u.USERID;
            }
            else
            {
                return 0;
            }
        }
    }
}
