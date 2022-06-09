using IGHportalAPI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IGHportalAPI.Models
{
    public class PayrollReportsDetails
    {
        public int Id { get; set; }
        public float HourlyRate { get; set; }
        public float TotalHoursWorked { get; set; }
        public string Notes { get; set; }
        public bool Active { get; set; }
        public float OvertimeHourlyRate { get; set; }
        public float TotalOverTimeHoursWorked { get; set; }

        //---------------------------------------------
        [ForeignKey("Hotel")]
        public int HotelId { get; set; }
        public virtual Hotel Hotel { get; set; }

        //---------------------------------------------
        [ForeignKey("PayrollReport")]
        public int PayrollReportId { get; set; }
        public virtual PayrollReport PayrollReport { get; set; }
       
        //---------------------------------------------
        [ForeignKey("Employee")]
        public int EmployeeId { get; set; }
        public virtual Employee Employee { get; set; }

        //---------------------------------------------
        [ForeignKey("Department")]
        public int DepartmentId { get; set; }
        public virtual Department Department { get; set; }











    }
}