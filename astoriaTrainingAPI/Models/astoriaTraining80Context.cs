using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace astoriaTrainingAPI.Models
{
    public partial class astoriaTraining80Context : DbContext
    {
      

        public astoriaTraining80Context(DbContextOptions<astoriaTraining80Context> options)
            : base(options)
        {
        }

        public virtual DbSet<AllowanceMaster> AllowanceMaster { get; set; }
        public virtual DbSet<CompanyMaster> CompanyMaster { get; set; }
        public virtual DbSet<DesignatioMaster> DesignatioMaster { get; set; }
        public virtual DbSet<EmployeeAllowanceDetals> EmployeeAllowanceDetals { get; set; }
        public virtual DbSet<EmployeeAttendance> EmployeeAttendance { get; set; }
        public virtual DbSet<EmployeeMaster> EmployeeMaster { get; set; }
        public virtual DbSet<UserInfo> UserInfo { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AllowanceMaster>(entity =>
            {
                entity.HasKey(e => e.AllowanceId)
                    .HasName("PK__Allowanc__7B12D041C3B3544C");

                entity.Property(e => e.AllowanceId).HasColumnName("AllowanceID");

                entity.Property(e => e.AllowaceDescrition)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.AllowanceName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CompanyMaster>(entity =>
            {
                entity.HasKey(e => e.CompanyId)
                    .HasName("PK__CompanyM__2D971C4C9C2E7386");

                entity.Property(e => e.CompanyId).HasColumnName("CompanyID");

                entity.Property(e => e.CompanyDescription)
                    .HasMaxLength(400)
                    .IsUnicode(false);

                entity.Property(e => e.CompanyName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CreationDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<DesignatioMaster>(entity =>
            {
                entity.HasKey(e => e.DesignationId)
                    .HasName("PK__Designat__BABD603EED5D9840");

                entity.Property(e => e.DesignationId).HasColumnName("DesignationID");

                entity.Property(e => e.CreationDate).HasColumnType("datetime");

                entity.Property(e => e.DesignationName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<EmployeeAllowanceDetals>(entity =>
            {
                entity.HasKey(e => new { e.EmployeeKey, e.AllowanceId, e.ClockDate })
                    .HasName("PK__Employee__BDFD953F8A967E2D");

                entity.Property(e => e.AllowanceId).HasColumnName("AllowanceID");

                entity.Property(e => e.ClockDate).HasColumnType("date");

                entity.Property(e => e.AllowanceAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.CreationDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.HasOne(d => d.Allowance)
                    .WithMany(p => p.EmployeeAllowanceDetals)
                    .HasForeignKey(d => d.AllowanceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__EmployeeA__Allow__4316F928");

                entity.HasOne(d => d.EmployeeKeyNavigation)
                    .WithMany(p => p.EmployeeAllowanceDetals)
                    .HasForeignKey(d => d.EmployeeKey)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__EmployeeA__Emplo__4222D4EF");
            });

            modelBuilder.Entity<EmployeeAttendance>(entity =>
            {
                entity.HasKey(e => new { e.EmployeeKey, e.ClockDate })
                    .HasName("PK__Employee__571C3619E4FAA3D0");

                entity.Property(e => e.ClockDate).HasColumnType("date");

                entity.Property(e => e.CreationDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Remarks)
                    .IsRequired()
                    .HasMaxLength(400)
                    .IsUnicode(false);

                entity.Property(e => e.TimeIn).HasColumnType("datetime");

                entity.Property(e => e.TimeOut)
                    .HasColumnName("Time_Out")
                    .HasColumnType("datetime");

                entity.HasOne(d => d.EmployeeKeyNavigation)
                    .WithMany(p => p.EmployeeAttendance)
                    .HasForeignKey(d => d.EmployeeKey)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__EmployeeA__Emplo__440B1D61");
            });

            modelBuilder.Entity<EmployeeMaster>(entity =>
            {
                entity.HasKey(e => e.EmployeeKey)
                    .HasName("PK__Employee__8749E50AE41E0620");

                entity.Property(e => e.CreationDate).HasColumnType("datetime");

                entity.Property(e => e.EmpCompanyId).HasColumnName("EmpCompanyID");

                entity.Property(e => e.EmpDesignationId).HasColumnName("EmpDesignationID");

                entity.Property(e => e.EmpFirstName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.EmpGender)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.EmpHourlySalaryRate).HasColumnType("decimal(18, 3)");

                entity.Property(e => e.EmpJoingDate).HasColumnType("datetime");

                entity.Property(e => e.EmpLastName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.EmpResinationDate).HasColumnType("datetime");

                entity.Property(e => e.EmployeeId)
                    .IsRequired()
                    .HasColumnName("EmployeeID")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.HasOne(d => d.EmpCompany)
                    .WithMany(p => p.EmployeeMaster)
                    .HasForeignKey(d => d.EmpCompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__EmployeeM__EmpCo__44FF419A");

                entity.HasOne(d => d.EmpDesignation)
                    .WithMany(p => p.EmployeeMaster)
                    .HasForeignKey(d => d.EmpDesignationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__EmployeeM__EmpDe__45F365D3");
            });

            modelBuilder.Entity<UserInfo>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.CreationDate).HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.UserId).ValueGeneratedOnAdd();

                entity.Property(e => e.UserName)
                    .HasMaxLength(30)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
