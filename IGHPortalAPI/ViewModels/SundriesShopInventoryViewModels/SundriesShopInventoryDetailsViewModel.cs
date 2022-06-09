using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IGHportalAPI.ViewModels.SundriesShopInventoryViewModels
{
    public class SundriesShopInventoryDetailsViewModel
    {
        public int Id { get; set; }
        public int LastMonthEndingBalance { get; set; }
        public int NewlyPurchased { get; set; }
        public int OnDisplay { get; set; }
        public int BackupStore { get; set; }
        public int MonthsEndingBalance { get; set; }
        public int ExpiredLogged { get; set; }
        public float CostOfExpiredLogged { get; set; }
        public int ProductSales { get; set; }
        public float ItemWholeSaleCost { get; set; }
        public float CostOfSale { get; set; }
        public float ItemRetailCost { get; set; }
        public float CurrentStockRetail { get; set; }
        public float MonthRevenue { get; set; }







        public string ProductName { get; set; }

        //public int SundriesShopInventoryId { get; set; }

    }
}
