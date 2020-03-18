using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using DingDingKq.Model;
namespace DingDingKq
{
    public class DingDingKqContext : DbContext
    {
        /// <summary>
        /// 用户表
        /// </summary>
        public DbSet<KqUser> KqUsers { get; set; }
        /// <summary>
        /// 同步信息表
        /// </summary>
        public DbSet<AttSyncRecord> AttSyncRecords { get; set; }
        /// <summary>
        /// 同步失败记录
        /// </summary>
        public DbSet<FailRecord> FailRecords { get; set; }
    }

}
