using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using IGHportalAPI.ViewModels.EmployeeViewModels;

namespace IGHportalAPI.Services
{
    public interface IEmployeeService
    {
        Task AddEmployee(EmployeeViewModel model);
        List<EmployeeViewModel> GetEmployees(int hotelId);
        Task UpdateEmployee(EmployeeViewModel model);

        AllOldNewEmployeesViewModel GetActiveDepartmentEmployees(int hotelId, int PayrollReportId);


        Task DeleteEmployee(int id);

        EmployeeViewModel GetEmployee(int id);

        
        bool IsEmailAlreadyExist(EmployeeViewModel model);

       
    }
}
