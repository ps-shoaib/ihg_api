using IGHportalAPI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IGHportalAPI.Models
{
    public class Employee
    {
        public Employee()
        {
            Departments = new HashSet<Department>();

            PayrollReportsDetails = new HashSet<PayrollReportsDetails>();



    }
    public int Id { get; set; }
        public string Name { get; set; }
        public string ContactNo { get; set; }
        public string Email { get; set; }
        public float HourlyRate { get; set; }
        public float OverTimeRate { get; set; }        
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public short UpdatedStatus { get; set; }


        public virtual ICollection<PayrollReportsDetails> PayrollReportsDetails { get; set; }





        //[ForeignKey("Department")]
        //public int DepartmentId { get; set; }
        public virtual ICollection<Department> Departments { get; set; }


        [ForeignKey("Hotel")]
        public int HotelId { get; set; }
        public virtual Hotel Hotel { get; set; }


    }
}
