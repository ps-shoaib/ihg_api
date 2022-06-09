using System;
using System.Collections.Generic;

namespace IGHportalAPI.ViewModels.WeeklyWrapUp_BankDepositViewModels
{
    public class WeeklyWrapUp_BankDepositViewModel
    {

        public WeeklyWrapUp_BankDepositViewModel()
        {
        }

        public int Id { get; set; }


        public DateTime Date { get; set; }

        public float Actual_Deposit { get; set; }

        public float System_Deposit_Amount { get; set; }

        public int WeeklyWrapUpId { get; set; }


    }
}
