using IGHportalAPI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IGHportalAPI.Models
{
    public class Hotel
    {
        public Hotel()
        {
            Users = new HashSet<User>();
            Employees = new HashSet<Employee>();
            PayrollReports = new HashSet<PayrollReport>();
            PayrollReportsDetails = new HashSet<PayrollReportsDetails>();

        }
        public int Id { get; set; }
        public string Name { get; set; }

        public string Address { get; set; }


        public string ImageURL { get; set; }

        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public short UpdatedStatus { get; set; }

        public virtual ICollection<PayrollReportsDetails> PayrollReportsDetails { get; set; }


        //[ForeignKey("User")]
        //public string UserId { get; set; }
        //public virtual User User { get; set; }

        public virtual ICollection<PayrollReport> PayrollReports { get; set; }



        public virtual ICollection<User> Users { get; set; }



        public virtual ICollection<Employee> Employees { get; set; }


    }
}
