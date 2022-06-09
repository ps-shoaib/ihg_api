using IGHportalAPI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IGHportalAPI.Models
{
    public class WeeklyWrapUp
    {
        public WeeklyWrapUp()
        {
            WeeklyWrapUp_BankDeposits = new HashSet<WeeklyWrapUp_BankDeposit>();
        }
        public int Id { get; set; }

        public DateTime ReportFrom { get; set; }

        public DateTime ReportTo { get; set; }
        public float Current_MTD_Revenue { get; set; }
        public float Current_YTD_Revenue { get; set; }
        public float  Previous_Year_MTD_Revenue { get; set; }
        public float Previous_Year_YTD_Revenue { get; set; }

        public string Comments_Variance { get; set; }
        public string Commments_GOP { get; set; }
        public string Summary { get; set; }
        public string Explanations { get; set; }

        public string files { get; set; }

        [ForeignKey("Hotel")]
        public int HotelId { get; set; }
        public virtual Hotel Hotel { get; set; }


        [ForeignKey("User")]
        public string CreatedBy { get; set; }
        public virtual User User { get; set; }

        public virtual ICollection<WeeklyWrapUp_BankDeposit> WeeklyWrapUp_BankDeposits { get; set; }
        public virtual WeeklyWrapUp_Operations WeeklyWrapUp_Operations { get; set; }

    }
}
