using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IGHportalAPI.ViewModels.AdministrationViewModels
{
    public class EditUserViewModel
    {
        public EditUserViewModel()
        {
            //Claims = new List<string>();
            Roles = new List<string>();

            //Roles = new List<UserRoleDTO>();


        }

        public string Id { get; set; }
        public string FirstName { get; set; }
        public string   LastName { get; set; }

        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string CNIC { get; set; }


        //public List<string> Claims { get; set; }
        public IList<string> Roles { get; set; }

        public IList<string> Hotels { get; set; }



        //public IList<UserRoleDTO> Roles { get; set; }

    }
}
