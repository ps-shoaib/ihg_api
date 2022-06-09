using IGHportalAPI.ViewModels.PayrollDepartmentGoalsViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IGHportalAPI.ViewModels.PayrollReportViewModels
{
    public class PayrollReportViewModel
    {
        public PayrollReportViewModel()
        {

            PayrollReportsDetails = new HashSet<PayrollReportsDetailsViewModel>();

            PayrollDepartmentGoals = new HashSet<PayrollDepartmentGoalsViewModel>();
        
        }

        public int Id { get; set; }
        public DateTime ReportFrom { get; set; }
        public DateTime ReportTo { get; set; }
        public int TotalRoomsCleaned { get; set; }
        public int TotalRoomSold { get; set; }
        public bool Active { get; set; }

        [DataType(DataType.Date)]
        public DateTime CreatedOn { get; set; }

        public int HotelId { get; set; }
        
        public string CreatedBy { get; set; }

        public virtual ICollection<PayrollDepartmentGoalsViewModel> PayrollDepartmentGoals{ get; set; }

        public virtual ICollection<PayrollReportsDetailsViewModel> PayrollReportsDetails { get; set; }


    }
}
