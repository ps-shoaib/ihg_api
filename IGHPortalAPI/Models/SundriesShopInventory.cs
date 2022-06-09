using IGHportalAPI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IGHportalAPI.Models
{
    public class SundriesShopInventory
    {
        public SundriesShopInventory()
        {
            SundriesShopInventoryDetails = new HashSet<SundriesShopInventoryDetails>();
        }

        public int Id { get; set; }
        [DataType(DataType.Date)]
        public DateTime CreatedOn { get; set; }



        [DataType(DataType.Date)]
        public DateTime ReportMonthYear { get; set; }



        [ForeignKey("Hotel")]
        public int HotelId { get; set; }
        public virtual Hotel Hotel { get; set; }

        [ForeignKey("User")]
        public string CreatedBy { get; set; }
        public virtual User User { get; set; }


        public virtual ICollection<SundriesShopInventoryDetails> SundriesShopInventoryDetails { get; set; }




    }
}
