using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DdApi
{
    public interface IDdOper
    {
        string AccessToken { get; }
        string AgentID { get; set; }
        string CorpId { get; set; }
        string CorpSecret { get; set; }
        string JsApiTicket { get; }
        string AppKey { get; set; }
        string AppSecret { get; set; }
        string GetDeptName(long DeptId);
        string TimeStamp();
    }
}
