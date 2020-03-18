namespace AttDbContext
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class AttContext : DbContext
    {
        public AttContext()
            : base("name=AttContext")
        {
        }

        public virtual DbSet<CHECKINOUT> CHECKINOUT { get; set; }
        public virtual DbSet<CIOT> CIOT { get; set; }
        public virtual DbSet<DEPARTMENTS> DEPARTMENTS { get; set; }
        public virtual DbSet<DeptUsedSchs> DeptUsedSchs { get; set; }
        public virtual DbSet<HOLIDAYS> HOLIDAYS { get; set; }
        public virtual DbSet<USERINFO> USERINFO { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<USERINFO>()
                .Property(e => e.PASSWORD)
                .IsUnicode(false);

            modelBuilder.Entity<USERINFO>()
                .Property(e => e.upsize_ts)
                .IsFixedLength();

            modelBuilder.Entity<USERINFO>()
                .Property(e => e.IDCardNo)
                .IsUnicode(false);

            modelBuilder.Entity<USERINFO>()
                .Property(e => e.IDCardValidTime)
                .IsUnicode(false);
        }
    }
}
