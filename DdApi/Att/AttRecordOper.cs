using DingTalk.Api;
using DingTalk.Api.Request;
using DingTalk.Api.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DdApi.Util;
using DdApi.Model;
namespace DdApi.Att
{
    public class AttRecordOper
    {
        IDdOper ddOper { get; set; }

        public DateTime CheckDateFrom { get; set; }
        public DateTime CheckDateTo { get; set; }
        public AttRecordOper(IDdOper _ddOper)
        {
            ddOper = _ddOper;
        }

        public List<AttRecord> GetCheckInOutRecord(List<string> emplist, DateTime DateFrom, DateTime DateTo)
        {
            List<AttRecord> attRecords = new List<AttRecord>();
            
            for (int i = 0; i <= emplist.Count / 50; i++)
            {
                var emps = emplist.Skip(i * 50).Take(50).ToList();
                bool hasMore = true;
                DateTime bdate = DateFrom;
                DateTime edate = bdate.AddDays(7);
                while (bdate <= DateTo && hasMore)
                {
                    if (edate > DateTo)
                    {
                        edate = DateTo;
                        hasMore = false;
                    }
                    attRecords.AddRange(GetCheckInOutRecordOnDingDingLimt(emps, bdate, edate));
                    bdate = bdate.AddDays(7);
                    edate = bdate.AddDays(7);
                }

            }

            return attRecords;
        }


        /// <summary>
        /// 从订订服务器取考勤数据，跨度不超7天，人员不超50
        /// </summary>
        /// <param name="emplist"></param>
        /// <param name="DateFrom"></param>
        /// <param name="DateTo"></param>
        /// <returns></returns>
        private List<AttRecord> GetCheckInOutRecordOnDingDingLimt(List<string> emplist, DateTime DateFrom, DateTime DateTo)
        {
            List<AttRecord> attRecords = new List<AttRecord>();
            IDingTalkClient client = new DefaultDingTalkClient("https://oapi.dingtalk.com/attendance/list");
            OapiAttendanceListRequest request = new OapiAttendanceListRequest
            {
                WorkDateFrom = DateTimeHelper.DatetimeToString(DateFrom),
                WorkDateTo = DateTimeHelper.DatetimeToString(DateTo),
                UserIdList = emplist,
                Offset = 0,
                Limit = 50
            };

            bool haveMore = false;
            do
            {
                OapiAttendanceListResponse response = client.Execute(request, ddOper.AccessToken);

                if (response.Errcode == 0)
                {
                    if (response.Recordresult != null)
                    {
                        response.Recordresult.ForEach(p =>
                           {
                               AttRecord ar = new AttRecord
                               {
                                   DdId = p.UserId,
                                   BaseCheckTime = DateTimeHelper.StampToDateTime(p.BaseCheckTime),
                                   WorkDate = DateTimeHelper.StampToDateTime(p.WorkDate),
                                   CheckType = p.CheckType,
                                   TimeResult = p.TimeResult,
                               };
                               if (p.TimeResult != "NotSigned") ar.CheckTime = DateTimeHelper.StampToDateTime(p.UserCheckTime);
                               attRecords.Add(ar);
                           });
                    }

                }

                if (response.HasMore)
                {
                    haveMore = true;
                    request.Offset += request.Limit;
                }
                else haveMore = false;


            } while (haveMore);

            return attRecords;

        }

    }


}
