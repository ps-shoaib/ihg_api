using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using IGHportalAPI.ViewModels.DepartmentViewModels;

namespace IGHportalAPI.Services
{
    public interface IDepartmentService
    {
        Task AddDepartment(DepartmentViewModel model);
        List<DepartmentViewModel> GetDepartments(int hotelId);
        Task UpdateDepartment(DepartmentViewModel model);
        
        Task DeleteDepartment(int id);

        DepartmentViewModel GetDepartment(int id);

        
        bool SystemNameAlreadyExist(DepartmentViewModel model);

       

    }
}
