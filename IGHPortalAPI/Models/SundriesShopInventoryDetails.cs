using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IGHportalAPI.Models
{
    public class SundriesShopInventoryDetails
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







        [ForeignKey("SundriesShopProduct")]
        public int ProductId { get; set; }
        public virtual SundriesShopProduct SundriesShopProduct { get; set; }



        [ForeignKey("SundriesShopInventory")]
        public int SundriesShopInventoryId { get; set; }
        public virtual SundriesShopInventory SundriesShopInventory { get; set; }

    }
}
