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
using IGHportalAPI.ViewModels.PayrollReportViewModels;
using IGHportalAPI.Models;
using IGHportalAPI.ViewModels.PayrollDepartmentGoalsViewModels;

namespace IGHportalAPI.Services
{
    public class PayrollReportService : IPayrollReportService
    {
        private readonly ILogger<PayrollReportService> logger;
        private readonly DataContext_ context;
        private readonly IMapper mapper;

        public PayrollReportService(ILogger<PayrollReportService> logger, DataContext_ context, IMapper mapper)
        {
            this.logger = logger;
            this.context = context;
            this.mapper = mapper;
        }

        public List<PayrollReportViewModel> GetPayrollReports(int hotelId , string Month , string Year)
        {
            //var maxUpdStatus = UpdStatus.Deleted;

            //var AllParollReporsts = context.PayrollReports.Where(a => a.Active == true).ToList();

            var AllParollReporsts = context.PayrollReports.Where(a => a.HotelId == hotelId).ToList();

           

            if ((!string.IsNullOrEmpty(Month)) && string.IsNullOrEmpty(Year))
            {
                AllParollReporsts = AllParollReporsts.Where(a => a.ReportFrom.ToString("MMMM") == Month).ToList();
            }else if ((!string.IsNullOrEmpty(Year)) && string.IsNullOrEmpty(Month))
            {
                AllParollReporsts = AllParollReporsts.Where(a => a.ReportFrom.Year.ToString() == Year).ToList();
            }
            else if ((!string.IsNullOrEmpty(Year)) && (!string.IsNullOrEmpty(Month)))
            {
                AllParollReporsts =
                        AllParollReporsts
                        .Where(a => a.ReportFrom.ToString("MMMM") == Month
                        && a.ReportFrom.Year.ToString() == Year).ToList();
            }
            //else
            //{
            //    AllParollReporsts = context.PayrollReports.Where(a => a.HotelId == hotelId && 
            //      a.ReportFrom.Year.ToString() == DateTime.UtcNow.Year.ToString()
            //    ).ToList();

            //}

            var ListPayrollViewModel = new List<PayrollReportViewModel>();

            foreach (var report in AllParollReporsts)
            {
                
                ListPayrollViewModel.Add(new PayrollReportViewModel()
                {
                    Id = report.Id,
                    ReportFrom = report.ReportFrom,
                    ReportTo = report.ReportTo,
                    TotalRoomsCleaned = report.TotalRoomsCleaned,
                    TotalRoomSold = report.TotalRoomSold,
                    Active = report.Active,
                    CreatedOn = report.CreatedOn,
                    HotelId = report.HotelId,
                    CreatedBy = report.CreatedBy
                });
            }

            return ListPayrollViewModel;

            //return mapper.Map<List<PayrollReportViewModel>>(context.PayrollReports.Where(a => a.Active == true).ToList());
        }

        public PayrollReportViewModel GetPayrollReport(int id, int hotelId)
        {
            var maxUpdStatus = UpdStatus.Deleted;

            //var PayrollReport = context.PayrollReports.Where(a => a.Active == true).FirstOrDefault(x => x.Id == id);
            var PayrollReport = context.PayrollReports.Where(a => a.HotelId == hotelId).FirstOrDefault(x => x.Id == id);

            if (PayrollReport == null)
            {
                return null;
            }

            var PayrollViewModel = new PayrollReportViewModel()
            {
                Id = PayrollReport.Id,
                ReportFrom = PayrollReport.ReportFrom,
                ReportTo = PayrollReport.ReportTo,
                TotalRoomsCleaned = PayrollReport.TotalRoomsCleaned,
                TotalRoomSold = PayrollReport.TotalRoomSold,
                Active = PayrollReport.Active,
                CreatedOn = PayrollReport.CreatedOn,
                HotelId = PayrollReport.HotelId,
                CreatedBy = PayrollReport.CreatedBy
            };


            var AllPayrollDetails = context.PayrollReportsDetails
                .Where(a => a.PayrollReportId == PayrollReport.Id)
                .OrderBy(a => a.Department.Name)
                .ToList();
            //var AllPayrollDetails =  PayrollReport.PayrollReportsDetails.ToList();

            foreach (var item in AllPayrollDetails)
            {
                var ActiveEmployee = context.Employees
                    .Where(a => a.Id == item.EmployeeId && a.UpdatedStatus < (short)UpdStatus.Deleted)
                    .FirstOrDefault();

              var ActiveDepartment =  context.Departments.Where(a => a.Id == item.DepartmentId && a.UpdatedStatus < (short)maxUpdStatus).FirstOrDefault();
                
                if (ActiveEmployee != null && ActiveDepartment != null) { 
                PayrollViewModel.PayrollReportsDetails.Add(
                    new PayrollReportsDetailsViewModel()
                    {
                        Id = item.Id,
                        HourlyRate = item.HourlyRate,
                        TotalHoursWorked = item.TotalHoursWorked,
                        Notes = item.Notes,
                        Active = item.Active,
                        OvertimeHourlyRate = item.OvertimeHourlyRate,
                        TotalOverTimeHoursWorked = item.TotalOverTimeHoursWorked,
                        HotelId = item.HotelId,
                        PayrollReportId = PayrollReport.Id,
                        EmployeeId = item.EmployeeId,
                        DepartmentId = item.DepartmentId,
                        DepartmentName = context.Departments.Where(a => a.Id == item.DepartmentId && a.UpdatedStatus < (short)maxUpdStatus).Select(a => a.Name).FirstOrDefault(),
                        EmployeeName = context.Employees.Where(a => a.Id == item.EmployeeId && a.UpdatedStatus < (short)maxUpdStatus).Select(a => a.Name).FirstOrDefault()
                    });
                }
            }


            var AllPayrollDepartmentGoals = context.PayrollDepartmentGoals.Where(a => a.PayrollReportId == PayrollReport.Id).ToList();
            var AllPayrollDepartmentGoals2 = PayrollReport.PayrollDepartmentGoals.ToList();


            var AllDepartments = context.Departments.Where(a => a.HotelId == PayrollReport.HotelId && a.UpdatedStatus < (short)maxUpdStatus).ToList();



            foreach (var item in AllPayrollDepartmentGoals)
            {
                
                var ActiveDepartment = 
                    context
                    .Departments
                    .Where(a => a.Id == item.DepartmentId && a.UpdatedStatus < (short)maxUpdStatus).FirstOrDefault();
                if(ActiveDepartment != null) { 
                PayrollViewModel.PayrollDepartmentGoals.Add(
                    new PayrollDepartmentGoalsViewModel()
                    {
                        Id = item.Id,
                        //PayrollReportId = item.Id,
                        Goal = item.Goal,
                        DepartmentName = context.Departments.Where(a => a.Id == item.DepartmentId && a.UpdatedStatus < (short)maxUpdStatus).Select(a => a.Name).FirstOrDefault(),
                    });
                }
            }


            foreach (var department in AllDepartments)
            {
                var DepartmentGoalObj =  PayrollViewModel.PayrollDepartmentGoals.Where(a => a.DepartmentName == department.Name).FirstOrDefault();
                if(DepartmentGoalObj == null)
                {
                    PayrollViewModel.PayrollDepartmentGoals.Add(new PayrollDepartmentGoalsViewModel()
                    {
                        Goal = 0,
                        DepartmentName = department.Name,

                    });
                }
            }


            //return mapper.Map<PayrollReportViewModel>(PayrollReport);
            return PayrollViewModel;
        }



        public bool IsGivenWeeksReportAlreadyExist(PayrollReportViewModel model)
        {

            var report = 
                context
                .PayrollReports
                .Where(a => 
                    ((a.ReportFrom >= model.ReportFrom
                    && a.ReportFrom <= model.ReportTo)
                   ||
                    (a.ReportTo >= model.ReportFrom
                    && a.ReportTo <= model.ReportTo)
                    )
                    && a.Id != model.Id
                    && a.HotelId == model.HotelId
                ).FirstOrDefault();

            if (report == null)
            {
                return false;
            }
            return true;
        }


        public async Task AddPayrollReport(PayrollReportViewModel model)
        {
            logger.LogInformation("Started adding PayrollReport");

            var Transaction = context.Database.BeginTransaction();

            PayrollReport payrollReport = new PayrollReport()
            {
                ReportFrom = model.ReportFrom,
                ReportTo = model.ReportTo,
                TotalRoomsCleaned = model.TotalRoomsCleaned,
                TotalRoomSold = model.TotalRoomSold,
                Active = model.Active,
                CreatedOn = DateTime.UtcNow,
                HotelId = model.HotelId,
                CreatedBy = model.CreatedBy
            };

            //var MappedObj = mapper.Map<PayrollReport>(model);
            //MappedObj.CreatedOn = DateTime.UtcNow;
            

            //MappedObj.UpdatedStatus = (short)UpdStatus.Created;

            context.PayrollReports.Add(
                payrollReport
            );

            await context.SaveChangesAsync();

            foreach (var payrollDetialItem in model.PayrollReportsDetails)
            {
                var payrollDetialObj = new PayrollReportsDetails();

                payrollDetialObj.HourlyRate = payrollDetialItem.HourlyRate;
                payrollDetialObj.TotalHoursWorked = payrollDetialItem.TotalHoursWorked;
                payrollDetialObj.Notes = payrollDetialItem.Notes;
                payrollDetialObj.Active = payrollDetialItem.Active;
                payrollDetialObj.OvertimeHourlyRate = payrollDetialItem.OvertimeHourlyRate;
                payrollDetialObj.TotalOverTimeHoursWorked = payrollDetialItem.TotalOverTimeHoursWorked;
                payrollDetialObj.HotelId = payrollDetialItem.HotelId;
                payrollDetialObj.PayrollReportId = payrollReport.Id;
                payrollDetialObj.EmployeeId = payrollDetialItem.EmployeeId;
                payrollDetialObj.DepartmentId = payrollDetialItem.DepartmentId;


                context.PayrollReportsDetails.Add(payrollDetialObj);
                
                await context.SaveChangesAsync();
            }


            //----------------------------------------------------------------

            //var AllPayrollDepartmentGoals = context.PayrollDepartmentGoals.Where(a => a.PayrollReportId == PayrollReport.Id).ToList();
            //var AllPayrollDepartmentGoals2 = PayrollReport.PayrollDepartmentGoals.ToList();

            foreach (var item in model.PayrollDepartmentGoals)
            {
                if(item.DepartmentName != null && item.Goal != 0) { 
                var payrollDepartmentGoalsObj = new PayrollDepartmentGoals();



                payrollDepartmentGoalsObj.DepartmentId = context.Departments.Where(a => a.Name == item.DepartmentName).Select(a => a.Id).FirstOrDefault();
                payrollDepartmentGoalsObj.PayrollReportId = payrollReport.Id;
                payrollDepartmentGoalsObj.Goal = item.Goal;



                context.PayrollDepartmentGoals.Add(payrollDepartmentGoalsObj);

                await context.SaveChangesAsync();
                }
            }


            //----------------------------------------------------------------
            Transaction.Commit();

            logger.LogInformation("Completed adding PayrollReport");
        }


        public async Task CopyPayrollReport(PayrollReportViewModel model)
        {
            var ReportToBcopied = GetPayrollReport(model.Id, model.HotelId);

            ReportToBcopied.ReportFrom = model.ReportFrom;
            ReportToBcopied.ReportTo = model.ReportTo;

          await  AddPayrollReport(ReportToBcopied);
        }




        public async Task UpdatePayrollReport(PayrollReportViewModel model)
        {
            logger.LogInformation("Started updating PayrollReport");

            var Transaction = context.Database.BeginTransaction();

            var PayrollReport = context.PayrollReports.AsNoTracking().FirstOrDefault(x => x.Id == model.Id);

            
            if (PayrollReport == null)
            {
                return;
            }
            //------------------------------------------------------------
            //------------------------------------------------------------
            PayrollReport.ReportFrom = model.ReportFrom;
            PayrollReport.ReportTo = model.ReportTo;
            PayrollReport.TotalRoomsCleaned = model.TotalRoomsCleaned;
            PayrollReport.TotalRoomSold = model.TotalRoomSold;
            PayrollReport.Active = model.Active;
            //PayrollReport.CreatedOn = model.CreatedOn;
            PayrollReport.HotelId = model.HotelId;
            //PayrollReport.CreatedBy = model.CreatedBy;

            context.Entry(PayrollReport).State = EntityState.Modified;

            await context.SaveChangesAsync();




            foreach (var PayrollDetailsForUpdate in model.PayrollReportsDetails)
                {
                  var PayrollDetail = context.PayrollReportsDetails
                                      .AsNoTracking()
                                      .FirstOrDefault(a => a.Id == PayrollDetailsForUpdate.Id);

                if(PayrollDetail != null) { 

                        PayrollDetail.PayrollReportId = PayrollReport.Id;

                        PayrollDetail.HourlyRate = PayrollDetailsForUpdate.HourlyRate;
                        PayrollDetail.TotalHoursWorked = PayrollDetailsForUpdate.TotalHoursWorked;
                        PayrollDetail.Notes = PayrollDetailsForUpdate.Notes;
                        PayrollDetail.Active = PayrollDetailsForUpdate.Active;
                        PayrollDetail.OvertimeHourlyRate = PayrollDetailsForUpdate.OvertimeHourlyRate;
                        PayrollDetail.TotalOverTimeHoursWorked = PayrollDetailsForUpdate.TotalOverTimeHoursWorked;
                        PayrollDetail.HotelId = PayrollDetailsForUpdate.HotelId;
                        PayrollDetail.EmployeeId = PayrollDetailsForUpdate.EmployeeId;
                        PayrollDetail.DepartmentId = PayrollDetailsForUpdate.DepartmentId;

                        PayrollDetail.Hotel = null;
                        PayrollDetail.Employee = null;
                        PayrollDetail.Department = null;

                        context.Entry(PayrollDetail).State = EntityState.Modified;
                        await context.SaveChangesAsync();

                }
                else
                {
                    var payrollDetialObj = new PayrollReportsDetails();

                    payrollDetialObj.HourlyRate = PayrollDetailsForUpdate.HourlyRate;
                    payrollDetialObj.TotalHoursWorked = PayrollDetailsForUpdate.TotalHoursWorked;
                    payrollDetialObj.Notes = PayrollDetailsForUpdate.Notes;
                    payrollDetialObj.Active = PayrollDetailsForUpdate.Active;
                    payrollDetialObj.OvertimeHourlyRate = PayrollDetailsForUpdate.OvertimeHourlyRate;
                    payrollDetialObj.TotalOverTimeHoursWorked = PayrollDetailsForUpdate.TotalOverTimeHoursWorked;
                    payrollDetialObj.HotelId = PayrollDetailsForUpdate.HotelId;
                    payrollDetialObj.PayrollReportId = PayrollReport.Id;
                    payrollDetialObj.EmployeeId = PayrollDetailsForUpdate.EmployeeId;
                    payrollDetialObj.DepartmentId = PayrollDetailsForUpdate.DepartmentId;


                    context.PayrollReportsDetails.Add(payrollDetialObj);
                    await context.SaveChangesAsync();

                }



            }

            foreach (var item in model.PayrollDepartmentGoals)
            {
                if (item.DepartmentName != null) { 
                    var payrollDepartmentGoalsObj = context.PayrollDepartmentGoals
                    .AsNoTracking()
                    .FirstOrDefault(a => a.Id == item.Id);

                if (payrollDepartmentGoalsObj != null)
                {

                    //payrollDepartmentGoalsObj.DepartmentId = context.Departments.Where(a => a.Name == item.DepartmentName).Select(a => a.Id).FirstOrDefault();
                    //payrollDepartmentGoalsObj.PayrollReportId = payrollReport.Id;
                    payrollDepartmentGoalsObj.Goal = item.Goal;

                    context.Entry(payrollDepartmentGoalsObj).State = EntityState.Modified;
                    await context.SaveChangesAsync();

                }
                else
                {

                    if (item.Goal != 0) { 

                        var payrollDepartmentGoal = new PayrollDepartmentGoals();

                        payrollDepartmentGoal.DepartmentId = context.Departments.Where(a => a.Name == item.DepartmentName).Select(a => a.Id).FirstOrDefault();
                        payrollDepartmentGoal.PayrollReportId = PayrollReport.Id;
                        payrollDepartmentGoal.Goal = item.Goal;



                        context.PayrollDepartmentGoals.Add(payrollDepartmentGoal);
                        await context.SaveChangesAsync();
                        }
                    }
                
                }
            }

            Transaction.Commit();

            logger.LogInformation("Completed updating PayrollReport");


        }

        public async Task DeletePayrollReport(int id)
        {
            var Transaction = context.Database.BeginTransaction();
            var PayrollReport = context.PayrollReports.AsNoTracking().FirstOrDefault(x => x.Id == id);

            var AllPayrollDetails = context.PayrollReportsDetails.AsNoTracking().Where(a => a.PayrollReportId == PayrollReport.Id).ToList();

            var AllPayrollDepartmentGoals = context.PayrollDepartmentGoals.AsNoTracking().Where(a => a.PayrollReportId == PayrollReport.Id).ToList();

            //PayrollReport.UpdatedStatus = (short)UpdStatus.Deleted;

            context.Entry(PayrollReport).State = EntityState.Deleted;

            
            foreach (var payrollDetailsObj in AllPayrollDetails)
            {
                context.Entry(payrollDetailsObj).State = EntityState.Deleted;
            }

            foreach (var payrollDetailsObj in AllPayrollDepartmentGoals)
            {
                context.Entry(payrollDetailsObj).State = EntityState.Deleted;
            }

            await context.SaveChangesAsync();


            Transaction.Commit();

        }


    }
}
