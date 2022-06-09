using IGHportalAPI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IGHportalAPI.Models
{
    public class Department
    {
        public Department()
        {
            Employees = new HashSet<Employee>();

            PayrollReportsDetails = new HashSet<PayrollReportsDetails>();

            PayrollDepartmentGoals = new HashSet<PayrollDepartmentGoals>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public short UpdatedStatus { get; set; }



        [ForeignKey("Hotel")]
        public int HotelId { get; set; }
        public virtual Hotel Hotel { get; set; }


        public virtual ICollection<Employee> Employees { get; set; }

        public virtual ICollection<PayrollReportsDetails> PayrollReportsDetails { get; set; }


        public virtual ICollection<PayrollDepartmentGoals> PayrollDepartmentGoals { get; set; }
    }
}
