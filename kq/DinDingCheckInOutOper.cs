using AttDbContext;
using DdApi;
using DingDingKq;
using DingTalk.Api;
using DingTalk.Api.Request;
using DingTalk.Api.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SG.Utilities;
using DdApi.Model;
using DingDingKq.Model;
namespace kq
{
    public class DinDingCheckInOutOper
    {
        DdOperator DdOper { get; set; }
        DingDingKqContext Dkc { get; set; }
        AttContext Ac { get; set; }

        public Func<List<string>, DateTime, DateTime, List<AttRecord>> GetCheckInOutRecord { get; set; }
        public DinDingCheckInOutOper(DdOperator _ddoper, DingDingKqContext _dkc, AttContext _ac)
        {
            DdOper = _ddoper;
            Dkc = _dkc;
            Ac = _ac;
        }
        public void SyncByFailRecord(List<FailRecord> failRecords)
        {
            var kuList = Dkc.KqUsers.ToList();
            failRecords.ForEach(p =>
            {
                if (!p.IsSucess)
                {
                    var u = kuList.FirstOrDefault(k => k.DdId == p.DDid);
                    if (u == null)
                    {
                        p.Note += "|" + DateTime.Now + "_未同步到考勤系统";
                    }
                    else if (u.AttUserId == 0)
                    {
                        p.Note += "|" + DateTime.Now + "_考勤系统中未找到该员工";
                    }
                    else
                    {
                        var ar = GetCheckInOutRecord(new List<string>() { p.DDid }, p.BegingDate, p.EndDate);
                        List<CHECKINOUT> cios = new List<CHECKINOUT>();
                        ar.ForEach(aritem =>
                        {
                            if (aritem.TimeResult != "NotSigned")
                            {
                                int c = Ac.CHECKINOUT.Count(ci => ci.USERID == u.AttUserId && ci.CHECKTIME == aritem.CheckTime);
                                if (c == 0)
                                {
                                    cios.Add(new CHECKINOUT
                                    {
                                        USERID = u.AttUserId,
                                        CHECKTYPE = "I",
                                        CHECKTIME = aritem.CheckTime,
                                        VERIFYCODE = 1,
                                        SENSORID = "3",
                                        sn = "0246361160468",
                                    });
                                }
                            }
                        });
                        var list = cios.Distinct(new CheckInoutComparer()).ToList();
                        Ac.CHECKINOUT.AddRange(list);
                        p.IsSucess = true;
                        p.Note += "|" + DateTime.Now + "_同步成功";
                    }

                    Dkc.SaveChanges();
                    Ac.SaveChanges();

                }


            });

        }
        public void WriteToAttSystem(List<AttRecord> attRecords, DateTime datetimeBegin, DateTime datetimeEnd)
        {
            List<CHECKINOUT> cios;
            var kuList = Dkc.KqUsers.ToList();
            int recordId = WriteSyncRecord(datetimeBegin, datetimeEnd);
            List<string> ddlist = attRecords.Select(p => p.DdId).Distinct().ToList();
            ddlist.ForEach(ddid =>
            {
                var u = kuList.FirstOrDefault(p => p.DdId == ddid);
                if (u == null)
                {
                    //人员未同步
                    Dkc.FailRecords.Add(new FailRecord
                    {
                        DDid = ddid,
                        BegingDate = datetimeBegin,
                        EndDate = datetimeEnd,
                        SyncId = recordId,
                        IsSucess = false,
                        Note = DateTime.Now + "_未同步到考勤系统"

                    });

                }
                else if (u.AttUserId == 0)
                {
                    //考勤系统未添加
                    Dkc.FailRecords.Add(new FailRecord
                    {
                        DDid = ddid,
                        BegingDate = datetimeBegin,
                        EndDate = datetimeEnd,
                        SyncId = recordId,
                        IsSucess = false,
                        UserName = u.UserName,
                        DeptName = u.DepartName,
                        Note = "考勤系统中未找到该员工"
                    });
                }
                else
                {
                    cios = new List<CHECKINOUT>();
                    attRecords.Where(a => a.DdId == ddid).ToList().ForEach(p =>
                         {
                             if (p.TimeResult != "NotSigned")
                             {
                                 int c = Ac.CHECKINOUT.Count(ci => ci.USERID == u.AttUserId && ci.CHECKTIME == p.CheckTime);
                                 if (c == 0)
                                 {
                                     cios.Add(new CHECKINOUT
                                     {
                                         USERID = u.AttUserId,
                                         CHECKTYPE = "I",
                                         CHECKTIME = p.CheckTime,
                                         VERIFYCODE = 1,
                                         SENSORID = "3",
                                         sn = "0246361160468",
                                     });
                                 }
                             }
                         });
                    var list = cios.Distinct(new CheckInoutComparer()).ToList();
                    Ac.CHECKINOUT.AddRange(list);

                }
                Dkc.SaveChanges();
                Ac.SaveChanges();

            });


        }

        public class CheckInoutComparer : IEqualityComparer<CHECKINOUT>
        {
            public bool Equals(CHECKINOUT x, CHECKINOUT y)
            {
                return x.USERID == y.USERID && x.CHECKTIME == y.CHECKTIME;
            }

            public int GetHashCode(CHECKINOUT obj)
            {
                return obj.ToString().GetHashCode();
            }
        }
        private int WriteSyncRecord(DateTime datetimeBegin, DateTime datetimeEnd)
        {

            AttSyncRecord asr = new AttSyncRecord
            {
                SyncTime = DateTime.Now,
                BeginTime = datetimeBegin,
                EndTime = datetimeEnd,
                Note = "手动操作同步"

            };
            Dkc.AttSyncRecords.Add(asr);
            Dkc.SaveChanges();
            return asr.Id;
        }


    }
}
