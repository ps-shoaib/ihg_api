using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IGHportalAPI.ViewModels.WeeklyWrapUp_BankDepositViewModels;
using IGHportalAPI.ViewModels.WeeklyWrapUp_OperationsViewModels;
using IGHportalAPI.ViewModels.WeeklyWrapUpViewModels;
using Microsoft.AspNetCore.Http;

namespace IGHportalAPI.Services
{
    public interface IWeeklyWrapUpService
    {
        Task AddWeeklyWrapUp(WeeklyWrapUpViewModel model);
        List<WeeklyWrapUpViewModel> GetWeeklyWrapUps(int hotelId);
        Task UpdateWeeklyWrapUp(WeeklyWrapUpViewModel model);
        
        Task DeleteWeeklyWrapUp(int id);

        WeeklyWrapUpViewModel GetWeeklyWrapUp(int id);

        Task<List<string>> SaveFiles(ICollection<IFormFile> Files);


        bool IsWeeklyReportAlreadyExist(WeeklyWrapUpViewModel model);







    }
}
