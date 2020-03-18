namespace AttDbContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("USERINFO")]
    public partial class USERINFO
    {
        [Key]
        [Column(Order = 0)]
        public int USERID { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(24)]
        public string Badgenumber { get; set; }

        [StringLength(20)]
        public string SSN { get; set; }

        [StringLength(40)]
        public string Name { get; set; }

        [StringLength(8)]
        public string Gender { get; set; }

        [StringLength(20)]
        public string TITLE { get; set; }

        [StringLength(20)]
        public string PAGER { get; set; }

        public DateTime? BIRTHDAY { get; set; }

        public DateTime? HIREDDAY { get; set; }

        [StringLength(80)]
        public string street { get; set; }

        [StringLength(2)]
        public string CITY { get; set; }

        [StringLength(2)]
        public string STATE { get; set; }

        [StringLength(12)]
        public string ZIP { get; set; }

        [StringLength(20)]
        public string OPHONE { get; set; }

        [StringLength(20)]
        public string FPHONE { get; set; }

        public short? VERIFICATIONMETHOD { get; set; }

        public short? DEFAULTDEPTID { get; set; }

        public short? SECURITYFLAGS { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short ATT { get; set; }

        [Key]
        [Column(Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short INLATE { get; set; }

        [Key]
        [Column(Order = 4)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short OUTEARLY { get; set; }

        [Key]
        [Column(Order = 5)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short OVERTIME { get; set; }

        [Key]
        [Column(Order = 6)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short SEP { get; set; }

        [Key]
        [Column(Order = 7)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short HOLIDAY { get; set; }

        [StringLength(8)]
        public string MINZU { get; set; }

        [StringLength(50)]
        public string PASSWORD { get; set; }

        public short? LUNCHDURATION { get; set; }

        [Column(TypeName = "image")]
        public byte[] PHOTO { get; set; }

        [StringLength(10)]
        public string mverifypass { get; set; }

        [Column(TypeName = "image")]
        public byte[] Notes { get; set; }

        public int? privilege { get; set; }

        public short? InheritDeptSch { get; set; }

        public short? InheritDeptSchClass { get; set; }

        public short? AutoSchPlan { get; set; }

        public int? MinAutoSchInterval { get; set; }

        public short? RegisterOT { get; set; }

        public short? InheritDeptRule { get; set; }

        public short? EMPRIVILEGE { get; set; }

        [StringLength(20)]
        public string CardNo { get; set; }

        [Column(TypeName = "timestamp")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [MaxLength(8)]
        public byte[] upsize_ts { get; set; }

        public int? FaceGroup { get; set; }

        public int? AccGroup { get; set; }

        public int? UseAccGroupTZ { get; set; }

        public int? VerifyCode { get; set; }

        public int? Expires { get; set; }

        public int? ValidCount { get; set; }

        public DateTime? ValidTimeBegin { get; set; }

        public DateTime? ValidTimeEnd { get; set; }

        public int? TimeZone1 { get; set; }

        public int? TimeZone2 { get; set; }

        public int? TimeZone3 { get; set; }

        [StringLength(18)]
        public string IDCardNo { get; set; }

        [StringLength(21)]
        public string IDCardValidTime { get; set; }
    }
}
