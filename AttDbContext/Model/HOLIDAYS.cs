namespace AttDbContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class HOLIDAYS
    {
        [Key]
        public int HOLIDAYID { get; set; }

        [StringLength(20)]
        public string HOLIDAYNAME { get; set; }

        public short? HOLIDAYYEAR { get; set; }

        public short? HOLIDAYMONTH { get; set; }

        public short? HOLIDAYDAY { get; set; }

        public DateTime? STARTTIME { get; set; }

        public short? DURATION { get; set; }

        public short? HOLIDAYTYPE { get; set; }

        [StringLength(4)]
        public string XINBIE { get; set; }

        [StringLength(50)]
        public string MINZU { get; set; }

        public short? DeptID { get; set; }

        public int? timezone { get; set; }
    }
}
