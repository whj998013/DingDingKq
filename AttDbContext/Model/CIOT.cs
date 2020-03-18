namespace AttDbContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CIOT")]
    public partial class CIOT
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int USERID { get; set; }

        [Key]
        [Column(Order = 1)]
        public DateTime CHECKTIME { get; set; }

        [StringLength(1)]
        public string CHECKTYPE { get; set; }

        public int? VERIFYCODE { get; set; }

        [StringLength(5)]
        public string SENSORID { get; set; }

        [StringLength(30)]
        public string Memoinfo { get; set; }

        public int? WorkCode { get; set; }

        [StringLength(20)]
        public string sn { get; set; }

        public short? UserExtFmt { get; set; }
    }
}
