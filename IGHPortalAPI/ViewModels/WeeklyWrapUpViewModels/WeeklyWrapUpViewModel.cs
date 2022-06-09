using IGHportalAPI.ViewModels.WeeklyWrapUp_BankDepositViewModels;
using IGHportalAPI.ViewModels.WeeklyWrapUp_OperationsViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IGHportalAPI.ViewModels.WeeklyWrapUpViewModels
{
    public class WeeklyWrapUpViewModel
    {
        public WeeklyWrapUpViewModel()
        {
            fileNames = new List<string>();
            OldFileNames = new List<string>();
        }
        public int Id { get; set; }

        public DateTime ReportFrom { get; set; }

        public DateTime ReportTo { get; set; }

        public float Current_MTD_Revenue { get; set; }
        public float Current_YTD_Revenue { get; set; }
        public float Previous_Year_MTD_Revenue { get; set; }
        public float Previous_Year_YTD_Revenue { get; set; }

        public string Comments_Variance { get; set; }
        public string Commments_GOP { get; set; }
        public string Summary { get; set; }

        public string Explanations { get; set; }


        public List<string> fileNames { get; set; }
        public List<string> OldFileNames { get; set; }



        public int HotelId { get; set; }
        public string CreatedBy { get; set; }


        public  List<WeeklyWrapUp_BankDepositViewModel> WeeklyWrapUp_BankDepositViewModel { get; set; }

        public WeeklyWrapUp_OperationsViewModel WeeklyWrapUp_OperationsViewModel { get; set; }



    }
}
