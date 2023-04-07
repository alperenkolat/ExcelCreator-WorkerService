using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ExcelCreatorWorkerService.Models;

public partial class ExcelCreaterContext : DbContext
{
    public ExcelCreaterContext()
    {
    }

    public ExcelCreaterContext(DbContextOptions<ExcelCreaterContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Emp> Emps { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Emp>(entity =>
        {
            entity.HasKey(e => e.Empno).HasName("PK__emp__AF4C318A88A4DE4A");

            entity.ToTable("emp");

            entity.Property(e => e.Empno)
                .ValueGeneratedNever()
                .HasColumnName("empno");
            entity.Property(e => e.Comm)
                .HasColumnType("numeric(7, 2)")
                .HasColumnName("comm");
            entity.Property(e => e.Dept).HasColumnName("dept");
            entity.Property(e => e.Ename)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("ename");
            entity.Property(e => e.Hiredate)
                .HasColumnType("datetime")
                .HasColumnName("hiredate");
            entity.Property(e => e.Job)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("job");
            entity.Property(e => e.Mgr).HasColumnName("mgr");
            entity.Property(e => e.Sal)
                .HasColumnType("numeric(7, 2)")
                .HasColumnName("sal");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
