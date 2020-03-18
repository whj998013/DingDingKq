namespace AttDbContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class DEPARTMENTS
    {
        [Key]
        [Column(Order = 0)]
        public int DEPTID { get; set; }

        [StringLength(30)]
        public string DEPTNAME { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int SUPDEPTID { get; set; }

        public short? InheritParentSch { get; set; }

        public short? InheritDeptSch { get; set; }

        public short? InheritDeptSchClass { get; set; }

        public short? AutoSchPlan { get; set; }

        public short? InLate { get; set; }

        public short? OutEarly { get; set; }

        public short? InheritDeptRule { get; set; }

        public int? MinAutoSchInterval { get; set; }

        public short? RegisterOT { get; set; }

        public int? DefaultSchId { get; set; }

        public short? ATT { get; set; }

        public short? Holiday { get; set; }

        public short? OverTime { get; set; }
    }
}
