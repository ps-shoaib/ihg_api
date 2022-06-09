using IGHportalAPI.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IGHportalAPI.Models
{
    public class User : IdentityUser
    {
        public User()
        {
            Hotels = new HashSet<Hotel>();
            PayrollReports = new HashSet<PayrollReport>();
            LinenInventories = new HashSet<LinenInventory>();


            Departments = new HashSet<Department>();

            LinenItems = new HashSet<LinenItem>();
            SundriesShopProducts = new HashSet<SundriesShopProduct>();

        }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CNIC { get; set; }
        public string Address { get; set; }
        public bool IsActive { get; set; }


        public virtual ICollection<LinenItem> LinenItems { get; set; }




        public virtual ICollection<Hotel> Hotels { get; set; }

        public virtual ICollection<PayrollReport> PayrollReports { get; set; }

        public virtual ICollection<LinenInventory> LinenInventories { get; set; }

        public virtual ICollection<Department> Departments { get; set; }
    
        public virtual ICollection<SundriesShopProduct> SundriesShopProducts { get; set; }

    }
}
