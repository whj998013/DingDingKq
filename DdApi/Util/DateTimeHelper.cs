using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DdApi.Util
{
    public class DateTimeHelper
    {
        /// <summary>
        /// 获取时间戳转时间
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static DateTime StampToDateTime(string time)
        {
            time = time.Substring(0, 10);
            double timestamp = Convert.ToInt64(time);
            System.DateTime dateTime = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);
            dateTime = dateTime.AddSeconds(timestamp).ToLocalTime();
            return dateTime;
        }
         
        /// <summary>  
        /// 获取时间戳Timestamp    
        /// </summary>  
        /// <param name="dt"></param>  
        /// <returns></returns>  
        public static int GetTimeStamp(DateTime dt)
        {
            DateTime dateStart = new DateTime(1970, 1, 1, 8, 0, 0);
            int timeStamp = Convert.ToInt32((dt - dateStart).TotalSeconds);
            return timeStamp;
        }

        /// <summary>
        /// 日期转字符串
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string DatetimeToString(DateTime dt)
        {
            return string.Format("{0}-{1}-{2} {3}:{4}:{5}",dt.Year,dt.Month.ToString("D2"),dt.Day.ToString("D2"), dt.Hour.ToString("D2"), dt.Minute.ToString("D2"), dt.Second.ToString("D2"));
        }
    }
}
