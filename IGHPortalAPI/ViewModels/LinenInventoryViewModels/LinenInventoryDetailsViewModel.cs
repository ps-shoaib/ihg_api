using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IGHportalAPI.ViewModels.LinenInventoryViewModels
{
    public class LinenInventoryDetailsViewModel
    {

        public int Id { get; set; }
        public int OneTurnForAllRooms { get; set; }
        public int RequiredTurns { get; set; }
        public int RequiredPar { get; set; }
        public int LastMonthEndingBalance { get; set; }

        public int NewPurchases { get; set; }
        public int firstFloorStorage { get; set; }
        public int secondFloorStorage { get; set; }
        public int thirdFloorStorage { get; set; }
        public int fourthFloorStorage { get; set; }
        public int fifthFloorStorage { get; set; }
        public int InRooms { get; set; }
        public int Dirty { get; set; }

        public int EndingBalanceOfMonth { get; set; }
        public int MonthlyTotalLoss { get; set; }
        public int QuantityTobeOrdered { get; set; }


        public string CreatedBy { get; set; }

        public int MonthNumber { get; set; }

        public string LinenItemName { get; set; }


    }
}
