using IGHportalAPI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IGHportalAPI.Models
{
    public class SundriesShopProduct
    {
        public SundriesShopProduct()
        {
            SundriesShopInventories = new HashSet<SundriesShopInventoryDetails>();
        }
        public int Id { get; set; }
        public string Name { get; set; }




        [ForeignKey("Hotel")]
        public int HotelId { get; set; }
        public virtual Hotel Hotel { get; set; }

        [ForeignKey("User")]
        public string CreatedBy { get; set; }
        public virtual User User { get; set; }

        public virtual ICollection<SundriesShopInventoryDetails> SundriesShopInventories { get; set; }



    }
}
