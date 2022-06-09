using System.Collections;
using System.Collections.Generic;

namespace IGHportalAPI.ViewModels.EmployeeViewModels
{
    public class AllOldNewEmployeesViewModel
    {
        public AllOldNewEmployeesViewModel()
        {
            OldEmployees = new HashSet<EmployeeViewModel>();
            NewEmployees = new HashSet<EmployeeViewModel>();
        }


        public ICollection<EmployeeViewModel> OldEmployees { get; set; }
        public ICollection<EmployeeViewModel> NewEmployees { get; set; }

    }
}
