using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IGHportalAPI.ViewModels.AccountViewModels
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }



        public string Password { get; set; }
        public string CNIC { get; set; }

        public string Address { get; set; }

        //public bool IsActive { get; set; }

        public IList<string> Roles { get; set; }

        public IList<string> Hotels { get; set; }


    }
}
