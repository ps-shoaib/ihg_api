using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IGHportalAPI.Models;
using IGHportalAPI.Models;

namespace IGHportalAPI.DataContext
{
    public class DataContext_ : IdentityDbContext<User>
    {
        public DataContext_(DbContextOptions<DataContext_> options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
                .Entity<HotelUser>()
                .HasKey(
                nameof(HotelUser.UsersId),
                nameof(HotelUser.HotelsId)
                );

            builder
                .Entity<EmployeesDepartments>()
                .HasKey(
                nameof(EmployeesDepartments.EmployeeId),
                nameof(EmployeesDepartments.DepartmentId)
                );


            builder.Entity<SundriesShopInventoryDetails>()
                .HasOne(a => a.SundriesShopProduct)
                .WithMany(a => a.SundriesShopInventories)
                .OnDelete(DeleteBehavior.Cascade);

            //builder.Entity<PayrollDepartmentGoals>()
            //    .HasOne(a => a.PayrollReport)
            //    .WithMany(a => a.PayrollDepartmentGoals)
            //    .OnDelete(DeleteBehavior.Cascade);



            //builder.Entity<PayrollReportsDetails>()
            //    .HasOne(a => a.PayrollReport)
            //    .WithMany(a => a.PayrollReportsDetails)
            //    .OnDelete(DeleteBehavior.Cascade);


            base.OnModelCreating(builder);
        }

        public DbSet<Hotel> Hotels { get; set; }

        public DbSet<HotelUser> HotelUsers { get; set; }

        public DbSet<PayrollDepartmentGoals> PayrollDepartmentGoals { get; set; }


        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department>  Departments { get; set; }

        public DbSet<EmployeesDepartments> employeesDepartments { get; set; }


        public DbSet<PayrollReport> PayrollReports { get; set; }
        public DbSet<PayrollReportsDetails> PayrollReportsDetails { get; set; }


        public DbSet<LinenInventory> LinenInventories { get; set; }
        public DbSet<LinenInventoryDetails> LinenInventoryDetails { get; set; }


        public DbSet<LinenItem> LinenItems { get; set; }


        public DbSet<SundriesShopInventory> SundriesShopInventories { get; set; }

        public DbSet<SundriesShopInventoryDetails> SundriesShopInventoryDetails { get; set; }

        public DbSet<SundriesShopProduct> SundriesShopProducts { get; set; }



        public DbSet<WeeklyWrapUp> WeeklyWrapUps { get; set; }

        public DbSet<WeeklyWrapUp_BankDeposit> WeeklyWrapUpBankDeposits { get; set; }
        public DbSet<WeeklyWrapUp_Operations> WeeklyWrapUpOperations { get; set; }
        public DbSet<WeeklyWrapUp_OperationsDetails> WeeklyWrapUpOperationsDetails { get; set; }
        public DbSet<Scores_Issues> ScoresIssues { get; set; }




    }
}