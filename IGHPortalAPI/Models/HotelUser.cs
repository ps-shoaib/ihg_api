using IGHportalAPI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IGHportalAPI.Models
{
    public class HotelUser
    {

        [ForeignKey("Hotel")]
        public int HotelsId { get; set; }

        [ForeignKey("User")]
        public string UsersId { get; set; }


        public virtual Hotel Hotel { get; set; }

        public virtual User User { get; set; }


    }
}                    