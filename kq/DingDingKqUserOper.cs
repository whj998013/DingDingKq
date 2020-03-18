using AttDbContext;
using DdApi.Sys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DdApi;
using SG.Utilities.FToS;
using DingDingKq.Model;
using DingDingKq;
using DdApi.Model;

namespace kq
{
    public class DingDingKqUserOper
    {
        DeptProvider dpr;
        DdOperator DdOper { get; set; }
        DingDingKqContext Dkc { get; set; }
        AttContext Ac { get; set; }
        public DingDingKqUserOper(DdOperator _ddoper,DingDingKqContext _dkc,AttContext _ac)
        {
            DdOper = _ddoper;
            dpr = new DeptProvider(DdOper);
            Dkc = _dkc;
            Ac = _ac;
        }
        public void EmpSync()
        {
            var ulist = GetDdUserList();
            AttUserOper auo = new AttUserOper();
            ulist.ForEach(p =>
            {
                var kqu = Dkc.KqUsers.FirstOrDefault(ku => ku.DdId == p.DdId);
                if (kqu == null)
                {
                    KqUser ku = p.ToSon<DdApi.Model.User, KqUser>();
                    var u = auo.GetUserinfo(ku.UserName);
                    if (u != null)
                    {
                        ku.AttDeptId = (int)u.DEFAULTDEPTID;
                        ku.AttUserId = u.USERID;
                    }
                    Dkc.KqUsers.Add(ku);
                }else if (kqu != null && kqu.AttUserId == 0)
                {
                    var u = auo.GetUserinfo(kqu.UserName);
                    if (u != null)
                    {
                        kqu.AttDeptId = (int)u.DEFAULTDEPTID;
                        kqu.AttUserId = u.USERID;
                    }
                }
            });
            Dkc.SaveChanges();

        }
        public List<User> GetDdUserList()
        {
            var ulist = dpr.GetUserList(DdOper.DeptList);
            return ulist;
        }
    }
}
