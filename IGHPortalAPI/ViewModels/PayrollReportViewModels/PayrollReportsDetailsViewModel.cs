using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IGHportalAPI.ViewModels.PayrollReportViewModels
{
    public class PayrollReportsDetailsViewModel
    {

        public int Id { get; set; }
        public float HourlyRate { get; set; }
        public float TotalHoursWorked { get; set; }
        public string Notes { get; set; }
        public bool Active { get; set; }
        public float OvertimeHourlyRate { get; set; }
        public float TotalOverTimeHoursWorked { get; set; }

        public string EmployeeName { get; set; }
        public string DepartmentName { get; set; }


        //---------------------------------------------
        public int HotelId { get; set; }
        
        //---------------------------------------------
        public int PayrollReportId { get; set; }
        
        //---------------------------------------------
        public int EmployeeId { get; set; }

        //---------------------------------------------
        public int DepartmentId { get; set; }










    }
}
