using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DingDingKq.Model
{
    public class AttSyncRecord
    {
        public int Id { get; set; }

        public DateTime SyncTime { get; set; }
        public DateTime BeginTime { get; set; }
        public DateTime EndTime { get; set; }

        public bool Result { get; set; }

        public string Note { get; set; }

    }
}
