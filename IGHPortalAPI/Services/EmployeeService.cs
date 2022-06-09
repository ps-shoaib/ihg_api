using AutoMapper;
using IGHportalAPI.DataContext;
using IGHportalAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


using IGHportalAPI.Models.Enums;
using IGHportalAPI.ViewModels.EmployeeViewModels;
using IGHportalAPI.Models;

namespace IGHportalAPI.Services
{
    public class EmployeeService  : IEmployeeService
    {
        private readonly ILogger<EmployeeService> logger;
        private readonly DataContext_ context;
        private readonly IMapper mapper;

        public EmployeeService(ILogger<EmployeeService> logger, DataContext_ context, IMapper mapper)
        {
            this.logger = logger;
            this.context = context;
            this.mapper = mapper;
        }                     

        public List<EmployeeViewModel> GetEmployees(int hotelId)
        {
            var maxUpdStatus = UpdStatus.Deleted;

            var EmployeesList = context.Employees.Where(a => a.UpdatedStatus < (short)maxUpdStatus && a.HotelId == hotelId).ToList();

            var MappedList = mapper.Map<List<EmployeeViewModel>>(EmployeesList);



            foreach (var emp in MappedList)
            {
                var EmployeeObj = mapper.Map<Employee>(emp);

                var DepartmentNamesOfEmp = context.employeesDepartments.Where(a => a.EmployeeId == emp.Id).Where(a => a.Department.UpdatedStatus < (short)UpdStatus.Deleted).Select(a => a.Department.Name).ToList();

                if (DepartmentNamesOfEmp.Count > 0)
                {
                    emp.DepartmentNames = DepartmentNamesOfEmp;
                }
            }
                
            return MappedList.OrderBy(a => a.DepartmentNames.FirstOrDefault()).ToList();
        }



        //-------------------------------------------------------------------
        //-------------------------------------------------------------------

        public AllOldNewEmployeesViewModel GetActiveDepartmentEmployees(int hotelId, int PayrollReportId)
        {
            var maxUpdStatus = UpdStatus.Deleted;

            var Allemps = context.Employees;

            var EmployeesListOld = new List<EmployeeViewModel>();
            var EmployeesListNew = new List<EmployeeViewModel>();

            var EmployeesList = context.Employees.Where(a => a.UpdatedStatus < (short)maxUpdStatus && a.HotelId == hotelId).ToList();

            var EmployeesMappedList = mapper.Map<List<EmployeeViewModel>>(EmployeesList);

            //------------------------------------------------------
            foreach (var emp in EmployeesMappedList.ToList())
            {
                var EmployeeObj = mapper.Map<Employee>(emp);

                var DepartmentNamesOfEmp = context.employeesDepartments
                    .Where(a => a.EmployeeId == emp.Id && a.Department.UpdatedStatus < (short)UpdStatus.Deleted)
                    .Select(a => new { a.Department.Name, a.DepartmentId}).ToList();

                if (DepartmentNamesOfEmp.Count == 1)
                {
                    emp.DepartmentName = DepartmentNamesOfEmp[0].Name;
                    emp.DepartmentId = DepartmentNamesOfEmp[0].DepartmentId;
                }
                if (DepartmentNamesOfEmp.Count > 1)
                {
                    EmployeesMappedList.Remove(emp);

                    foreach (var department in DepartmentNamesOfEmp)
                    {

                        
                        EmployeesMappedList.Add(new EmployeeViewModel() { ContactNo = emp.ContactNo , DepartmentName = department.Name, DepartmentId = department.DepartmentId, Email = emp.Email
                        , HotelId = emp.HotelId, DepartmentIds= emp.DepartmentIds, DepartmentNames = emp.DepartmentNames, HourlyRate = emp.HourlyRate,
                            Id = emp.Id, Name = emp.Name, OverTimeRate = emp.OverTimeRate});

                    }
                }
            }






            //------------------------------------------------------

            foreach (var employee in EmployeesMappedList)
            {


                    var dptObj = context.Departments.Where(a => a.Id == employee.DepartmentId
                    && a.UpdatedStatus < (short)maxUpdStatus 
                    &&  a.HotelId == employee.HotelId).FirstOrDefault();

                var detailsObj = new PayrollReportsDetails();

                     if (PayrollReportId != 0)
                     {
                        detailsObj = context.PayrollReportsDetails.Where(a => a.EmployeeId == employee.Id
                        && a.PayrollReportId == PayrollReportId
                        && a.DepartmentId == dptObj.Id).FirstOrDefault();

                     }
                     else
                     {
                        detailsObj = context.PayrollReportsDetails.Where(a => a.EmployeeId == employee.Id
                            && a.PayrollReportId == 0
                            && a.DepartmentId == dptObj.Id).FirstOrDefault();

                     }



                if (detailsObj != null)
                    {
                        EmployeesListOld.Add(employee);
                    }
                    else
                    {
                        EmployeesListNew.Add(employee);
                    }

            }



            foreach (var mappedItem in EmployeesListOld.ToList())
            {
                foreach (var depName in mappedItem.DepartmentNames)
                {

                    if (mappedItem.DepartmentNames.Contains(depName))
                    {
                        var dptObj = context.Departments.Where(a => a.Name == depName && a.UpdatedStatus < (short)maxUpdStatus && a.HotelId == mappedItem.HotelId).FirstOrDefault();
                        if (dptObj == null)
                        {
                            EmployeesListOld.Remove(mappedItem);
                        }
                        else
                        {
                            mappedItem.DepartmentIds.Add(dptObj.Id);

                        }
                    }
                }

            }



            foreach (var mappedItem in EmployeesListNew.ToList())
            {
                foreach (var depName in mappedItem.DepartmentNames)
                {

                    if (mappedItem.DepartmentNames.Contains(depName))
                    {
                        var dptObj = context.Departments.Where(a => a.Name == depName && a.UpdatedStatus < (short)maxUpdStatus).FirstOrDefault();
                        if (dptObj == null)
                        {
                            EmployeesListNew.Remove(mappedItem);
                        }
                        else
                        {
                            mappedItem.DepartmentIds.Add(dptObj.Id);

                        }
                    }
                }

            }




            var AllEmpsList = new AllOldNewEmployeesViewModel();

            AllEmpsList.OldEmployees = EmployeesListOld.OrderBy(a => a.DepartmentNames.FirstOrDefault()).ToList();
            AllEmpsList.NewEmployees = EmployeesListNew.OrderBy(a => a.DepartmentNames.FirstOrDefault()).ToList();

            return AllEmpsList;
        }
        //-------------------------------------------------------------------
        //-------------------------------------------------------------------


        public EmployeeViewModel GetEmployee(int id)
        {
            var maxUpdStatus = UpdStatus.Deleted;

            var Employee = context.Employees.Where(a => a.UpdatedStatus < (short)maxUpdStatus).FirstOrDefault(x => x.Id == id);

            if (Employee == null)
            {
                return null;
            }

            var Obj = mapper.Map<EmployeeViewModel>(Employee);


            var AllDps = context.employeesDepartments.Where(a => a.EmployeeId == Obj.Id).Select(a => a.Department.Name).Distinct();

            if (AllDps.Count() > 0)
            {
                foreach (var dep in AllDps)
                {
                    Obj.DepartmentNames.Add(dep);
                }
            }



            //var dptObj = context.Departments.Where(a => a.Id == Obj.DepartmentId && a.UpdatedStatus < (short) maxUpdStatus).FirstOrDefault();
            //if (dptObj != null)
            //{

            //    Obj.DepartmentName = dptObj.Name;
            //}
            return Obj;
        }

        public bool IsEmailAlreadyExist(EmployeeViewModel model)
        {
            //context.Employees.Include(a => a.Id).

            var employee = context
                .Employees
                .Where(a => a.Email == model.Email 
                     && a.HotelId == model.HotelId
                     && a.Id != model.Id)
                .FirstOrDefault();

            if (employee == null)
            {
                return false;
            }
            return true;
        }

        public async Task AddEmployee(EmployeeViewModel model)
        {
            logger.LogInformation("Started adding Employee");
            
            //var MappedObj = mapper.Map<Employee>(model);
            var MappedObj = new Employee()
            {
                Name = model.Name,
                ContactNo = model.ContactNo,
                Email = model.Email,
                HourlyRate = model.HourlyRate,
                OverTimeRate = model.OverTimeRate,
                HotelId = model.HotelId
            };


            MappedObj.CreatedOn = DateTime.UtcNow;

            //MappedObj.HotelId = 7;

            MappedObj.UpdatedStatus = (short)UpdStatus.Created;

            context.Employees.Add(
                MappedObj
            );
           

            await context.SaveChangesAsync();

            if (model.DepartmentNames.Count >=1) {
                foreach (var depName in model.DepartmentNames)
                {
                    var DepObj = context.Departments.FirstOrDefault(a => a.Name == depName && a.HotelId == model.HotelId);

                    context.employeesDepartments.Add(new EmployeesDepartments() { EmployeeId = MappedObj.Id, DepartmentId = DepObj.Id });
                    await context.SaveChangesAsync();

                }
            }




            logger.LogInformation("Completed adding Employee");
        }
        

        public async Task UpdateEmployee(EmployeeViewModel model)
        {
            logger.LogInformation("Started updating Employee");

            var Employee = context.Employees.AsNoTracking().FirstOrDefault(x => x.Id == model.Id);

            var Mappedsystem = new Employee();
            
            if (Employee == null)
            {
                return;
            }
            //------------------------------------------------------------
            //------------------------------------------------------------

            //------------------------------------------------------------
            //------------------------------------------------------------
            Mappedsystem = mapper.Map<Employee>(model);
            Mappedsystem.Id = Employee.Id;
            Mappedsystem.UpdatedOn = DateTime.UtcNow;
                

            Mappedsystem.CreatedOn = Employee.CreatedOn;


            Mappedsystem.UpdatedStatus = (short)UpdStatus.Updated;

            context.Entry(Mappedsystem).State = EntityState.Modified;

            await EditEmployeeDepartments(model.DepartmentNames, Employee);

            await context.SaveChangesAsync();






            logger.LogInformation("Completed updating Employee");
        }

        private async Task EditEmployeeDepartments(IList<string> SelectedDepartments, Employee emp)
        {
            var maxUpdStatus = UpdStatus.Deleted;

            var AllDepsFromDB = context.Departments.Where(a => a.HotelId == emp.HotelId && a.UpdatedStatus < (short)maxUpdStatus).ToList();


            foreach (var depFromDb in AllDepsFromDB)
            {
                var EmployeesDepsObj = context.employeesDepartments.AsNoTracking().Where(a => a.DepartmentId == depFromDb.Id && a.EmployeeId == emp.Id).FirstOrDefault();

                if ((EmployeesDepsObj != null) && SelectedDepartments.Contains(depFromDb.Name))
                {
                    continue;
                }
                else if ((EmployeesDepsObj != null) && (!SelectedDepartments.Contains(depFromDb.Name)))
                {

                    context.Entry(EmployeesDepsObj).State = EntityState.Deleted;
                    await context.SaveChangesAsync();
                }
                else if ((EmployeesDepsObj == null) && (SelectedDepartments.Contains(depFromDb.Name)))
                {
                    context.employeesDepartments.Add(new IGHportalAPI.Models.EmployeesDepartments() { DepartmentId = depFromDb.Id, EmployeeId = emp.Id });
                    await context.SaveChangesAsync();
                }
            }




        }





        public async Task DeleteEmployee(int id)
        {
            var Employee = context.Employees.AsNoTracking().FirstOrDefault(x => x.Id == id);

            Employee.UpdatedStatus = (short)UpdStatus.Deleted;

            context.Entry(Employee).State = EntityState.Modified;

            await context.SaveChangesAsync();

        }


    }
}
