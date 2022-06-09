using IGHportalAPI.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace IGHportalAPI.Models
{
    public class WeeklyWrapUp_BankDeposit
    {
        public WeeklyWrapUp_BankDeposit()
        {
        }

        public int Id { get; set; }


        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public float Actual_Deposit { get; set; }

        public float System_Deposit_Amount { get; set; }


        [ForeignKey("WeeklyWrapUp")]
        public int WeeklyWrapUpId { get; set; }
        public virtual WeeklyWrapUp WeeklyWrapUp { get; set; }


    }
}
