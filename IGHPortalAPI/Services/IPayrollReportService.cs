using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using IGHportalAPI.ViewModels.PayrollReportViewModels;

namespace IGHportalAPI.Services
{
    //IPayrollReportService
    public interface IPayrollReportService
    {
        Task AddPayrollReport(PayrollReportViewModel model);

        
        Task CopyPayrollReport(PayrollReportViewModel model);


        List<PayrollReportViewModel> GetPayrollReports(int hotelId , string Month , string Year);
        Task UpdatePayrollReport(PayrollReportViewModel model);
        
        Task DeletePayrollReport(int id);

        
        PayrollReportViewModel GetPayrollReport(int id, int hotelId);


        bool IsGivenWeeksReportAlreadyExist(PayrollReportViewModel model);




    }
}
