using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IGHportalAPI.ViewModels.AccountViewModels
{
    public class AuthViewModel
    {
        public string Id { get; set; }
        public string accessToken { get; set; }

        public string Name { get; set; }

        public string refreshToken { get; set; }
        public DateTime expiresIn { get; set; }

        public string LoggedInUserId { get; set; }

        public IList<string> Roles { get; set; }

        public string  FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }




    }

}
