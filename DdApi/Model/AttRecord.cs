using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DdApi.Model
{
    public class AttRecord
    {
        public string DdId { get; set; }
        public DateTime WorkDate { get; set; }
        public DateTime BaseCheckTime { get; set; }

        public DateTime CheckTime { get; set; }

        /// <summary>
        /// 时间结果
        ///  Normal：正常;
        ///Early：早退;
        ///Late：迟到;
        ///SeriousLate：严重迟到；
        ///Absenteeism：旷工迟到；
        ///NotSigned：未打卡
        /// </summary>
        public string TimeResult { get; set; }
        /// <summary>
        /// 考勤类型 OnDuty：上班 OffDuty：下班
        /// </summary>
        public string CheckType { get; set; }

        public bool IsChecked
        {
            get
            {
                return TimeResult != "NotSigned";
            }
        }
    }
}
