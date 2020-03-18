using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DingDingKq.Model
{
    public class FailRecord
    {
        public int Id { get; set; }

        public int SyncId { get; set; }

        public string UserName { get; set; }

        public string DeptName { get; set; }

        public string DDid { get; set; }

        public DateTime BegingDate { get; set; }

        public DateTime EndDate { get; set; }

        public bool IsSucess { get; set; } = false;

        public string Note { get; set; }

    }
}
