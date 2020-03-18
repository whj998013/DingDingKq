using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DdApi.Model;
using DingTalk.Api;
using DingTalk.Api.Request;
using DingTalk.Api.Response;

namespace DdApi.Sys
{
    public class DeptProvider
    {
        private IDdOper DdOper { get; set; }
        public DeptProvider(IDdOper _ddoper)
        {
            DdOper = _ddoper;
        }

        public List<Dept> GetDepts()
        {
            List<Dept> dlist = new List<Dept>();
            IDingTalkClient client = new DefaultDingTalkClient("https://oapi.dingtalk.com/department/list");
            OapiDepartmentListRequest request = new OapiDepartmentListRequest();
            request.SetHttpMethod("GET");
            OapiDepartmentListResponse response = client.Execute(request, DdOper.AccessToken);
            if (response.Errcode == 0)
            {
                foreach (var p in response.Department)
                {
                    Dept newDept = new Dept();
                    newDept.DeptID = p.Id;
                    newDept.DeptName = p.Name;
                    if (newDept.DeptID != 1)
                    {
                        newDept.ParentDeptId = p.Parentid;
                    }
                    dlist.Add(newDept);
                }
            }
            return dlist;
        }
        public List<User> GetAllUser()
        {
            return GetUserList(GetDepts());

        }
        public List<User> GetUserList(List<Dept> depts)
        {
            List<User> nlist = new List<User>();
            depts.ForEach(p =>
            {
                nlist.AddRange(GetDeptUserList(p));
            });
            
            return nlist;
        }

        public List<User> GetDeptUserList(Dept dept)
        {
            List<User> ulist = new List<User>();
            IDingTalkClient client = new DefaultDingTalkClient("https://oapi.dingtalk.com/user/listbypage");
            OapiUserListRequest request = new OapiUserListRequest();
            request.DepartmentId = dept.DeptID;
            request.SetHttpMethod("GET");
            request.Offset = 0;
            request.Size = 100;
            OapiUserListResponse execute = client.Execute(request, DdOper.AccessToken);
            if (execute.Userlist != null && execute.Userlist.Count > 0)
            {
                execute.Userlist.ForEach(p =>
                {
                    if (p.Active)
                    {
                        ulist.Add(new User
                        {
                            DdId = p.Userid,
                            UserName = p.Name,
                            DeptId = p.Department,
                            Avatar = p.Avatar,
                            IsLeader = p.IsLeader,
                            DepartName = dept.DeptName
                        }); ;
                    }
                });
            }

            return ulist;
        }

      
    }
}
