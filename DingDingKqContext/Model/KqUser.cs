using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DdApi.Model;

namespace DingDingKq.Model
{
   public class KqUser:User
    {
        public int Id { get; set; }
        public int AttUserId { get; set; }
        public int AttDeptId { get; set; }

        
    }
}
