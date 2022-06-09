using IGHportalAPI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IGHportalAPI.Models
{
    public class PayrollReport
    {

        public PayrollReport()
        {
            PayrollReportsDetails = new HashSet<PayrollReportsDetails>();

            PayrollDepartmentGoals = new HashSet<PayrollDepartmentGoals>();
        }

        public int Id { get; set; }
        public DateTime ReportFrom { get; set; }
        public DateTime ReportTo { get; set; }
        public int TotalRoomsCleaned { get; set; }
        public int TotalRoomSold { get; set; }
        public bool Active { get; set; }


        [DataType(DataType.Date)]
        public DateTime CreatedOn { get; set; }



        [ForeignKey("Hotel")]
        public int HotelId { get; set; }
        public virtual Hotel Hotel { get; set; }


        [ForeignKey("User")]
        public string CreatedBy { get; set; }
        public virtual User User { get; set; }

        public virtual ICollection<PayrollReportsDetails> PayrollReportsDetails { get; set; }

        public virtual ICollection<PayrollDepartmentGoals> PayrollDepartmentGoals { get; set; }


    }
}
