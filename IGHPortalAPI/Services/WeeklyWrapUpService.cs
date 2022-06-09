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
using IGHportalAPI.ViewModels.WeeklyWrapUpViewModels;
using IGHportalAPI.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using IGHportalAPI.ViewModels.WeeklyWrapUp_OperationsViewModels;
using IGHportalAPI.ViewModels.WeeklyWrapUp_BankDepositViewModels;

namespace IGHportalAPI.Services
{
    public class WeeklyWrapUpService  : IWeeklyWrapUpService
    {
        private readonly ILogger<WeeklyWrapUpService> logger;
        private readonly DataContext_ context;
        private readonly IMapper mapper;
        private readonly IWebHostEnvironment _hostEnvironment;


        public WeeklyWrapUpService(ILogger<WeeklyWrapUpService> logger, DataContext_ context, IMapper mapper, IWebHostEnvironment hostEnvironment)
        {
            this.logger = logger;
            this.context = context;
            this.mapper = mapper;
            _hostEnvironment = hostEnvironment;
        }

        public List<WeeklyWrapUpViewModel> GetWeeklyWrapUps(int hotelId)
        {
            //var ListWeekWrapUps = context.WeeklyWrapUps.Where(a => a.HotelId == hotelId).ToList();
            //if (ListWeekWrapUps == null)
            //{
            //    return null;
            //}

            //var ListWeeklyWrapUpsViewModel = mapper.Map<List<WeeklyWrapUpViewModel>>(ListWeekWrapUps);

            //foreach (var wrapUpViewModel in ListWeeklyWrapUpsViewModel)
            //{
            //    foreach (var WrapUpobject in ListWeekWrapUps)
            //    {
            //        if(wrapUpViewModel.Id == WrapUpobject.Id) { 
            //            if (WrapUpobject.files != null)
            //            {
            //                var allfiles = WrapUpobject.files.Split(" | ");

            //                foreach (var file in allfiles)
            //                {
            //                    wrapUpViewModel.fileNames.Add(file);
            //                }
            //            }
            //        }
            //    }
            //}

            //return ListWeeklyWrapUpsViewModel;

            return mapper.Map<List<WeeklyWrapUpViewModel>>
                (context.WeeklyWrapUps.Where(a => a.HotelId == hotelId).ToList());

        }

        public WeeklyWrapUpViewModel GetWeeklyWrapUp(int id)
        {
            var WeeklyWrapUp = context.WeeklyWrapUps.FirstOrDefault(x => x.Id == id);

            if (WeeklyWrapUp == null)
            {
                return null;
            }

            var ViewModel = mapper.Map<WeeklyWrapUpViewModel>(WeeklyWrapUp);

            if (WeeklyWrapUp.files != null)
            {
                var allfiles = WeeklyWrapUp.files.Split(" | ");

                foreach (var file in allfiles)
                {
                    ViewModel.fileNames.Add(file);
                }
            }

              ViewModel.WeeklyWrapUp_BankDepositViewModel = GetWeeklyWrapUp_BankDeposit(WeeklyWrapUp.Id);

            ViewModel.WeeklyWrapUp_OperationsViewModel = GetWeeklyWrapUp_Operations(WeeklyWrapUp.Id);
            

            return ViewModel;
        }


        public async Task<string> SaveFile(IFormFile FileFile)
        {
            //string FileName = new String(Path.GetFileNameWithoutExtension(FileFile.FileName).Take(10).ToArray()).Replace(' ', '-');
            //FileName = FileName + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(FileFile.FileName);
            var FilePath = Path.Combine(_hostEnvironment.ContentRootPath, "StaticFiles/Files", FileFile.FileName);
            using (var fileStream = new FileStream(FilePath, FileMode.Create))
            {
                await FileFile.CopyToAsync(fileStream);
            }
            return FileFile.FileName;
        }

        public void DeleteFile(string FileName)
        {
            var FilePath = Path.Combine(_hostEnvironment.ContentRootPath, "StaticFiles/Files", FileName);
            if (System.IO.File.Exists(FilePath))
                System.IO.File.Delete(FilePath);
        }


        public bool IsWeeklyReportAlreadyExist(WeeklyWrapUpViewModel model)
        {

            var SystemName = 
                context
                .WeeklyWrapUps.Where(a => a.HotelId == model.HotelId
                    && a.ReportFrom == model.ReportFrom
                    && a.ReportTo == model.ReportTo
                    && a.Id != model.Id).FirstOrDefault();
           
            if (SystemName == null)
            {
                return false;
            }
            return true;
        }

        public async Task AddWeeklyWrapUp(WeeklyWrapUpViewModel model)
        {
            logger.LogInformation("Started adding WeeklyWrapUp");


            var Transaction = context.Database.BeginTransaction();


            var MappedObj = mapper.Map<WeeklyWrapUp>(model);

            if (model.fileNames.Count != 0)
            {

                foreach (var file in model.fileNames)
                {
                    if (file != "" && file != null)
                    {
                        DeleteFile(file);
                        MappedObj.files += file + " | ";
                    }
                }
            }
            MappedObj.Id = 0;
            MappedObj.WeeklyWrapUp_BankDeposits = null;
            context.WeeklyWrapUps.Add(
                MappedObj
            );
            

            await context.SaveChangesAsync();

            model.WeeklyWrapUp_OperationsViewModel.WeeklyWrapUpId = MappedObj.Id;

            foreach (var Obj in model.WeeklyWrapUp_BankDepositViewModel)
            {
                Obj.WeeklyWrapUpId = MappedObj.Id;
            }

            await AddWeeklyWrapUp_Operations(model.WeeklyWrapUp_OperationsViewModel);

            await AddWeeklyWrapUp_BankDeposit(model.WeeklyWrapUp_BankDepositViewModel);

            Transaction.Commit();

            logger.LogInformation("Completed adding WeeklyWrapUp");
        }


        public async Task<List<string>> SaveFiles(ICollection<IFormFile> Files)
        {
            //string imageName = new String(Path.GetFileNameWithoutExtension(imageFile.FileName).Take(10).ToArray()).Replace(' ', '-');
            //imageName = imageName + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(imageFile.FileName);
            var FilesNamesArray = new List<string>();
            foreach (var file in Files)
            {
                DeleteFile(file.FileName);
            
                string imageName = file.FileName;
                var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, "StaticFiles/Files/", imageName);
                using (var fileStream = new FileStream(imagePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                FilesNamesArray.Add("StaticFiles/Files/" + imageName);
            }
            return FilesNamesArray;
        }

        public async Task UpdateWeeklyWrapUp(WeeklyWrapUpViewModel model)
        {
            logger.LogInformation("Started updating WeeklyWrapUp");

            var Transaction = context.Database.BeginTransaction();

            var WeeklyWrapUp = context.WeeklyWrapUps.AsNoTracking().FirstOrDefault(x => x.Id == model.Id);

            var Mappedsystem = new WeeklyWrapUp();
            
            if (WeeklyWrapUp == null)
            {
                return;
            }
            //------------------------------------------------------------
            //------------------------------------------------------------

            //------------------------------------------------------------
            //------------------------------------------------------------
            Mappedsystem = mapper.Map<WeeklyWrapUp>(model);


            Mappedsystem.files = "";

            var NewUniqueFilesArary = new List<string>();
            foreach (var file in model.OldFileNames)
            {
                NewUniqueFilesArary.Add(file);
            }

            foreach (var file in model.fileNames)
            {
                if (!NewUniqueFilesArary.Contains(file))
                {
                    NewUniqueFilesArary.Add(file);
                }
            }


    
            foreach (var file in NewUniqueFilesArary)
            {
                if (file != "" && file != null)
                {
                    Mappedsystem.files += file + " | ";
                }
            }


            context.Entry(Mappedsystem).State = EntityState.Modified;

            await context.SaveChangesAsync();

            await UpdateWeeklyWrapUp_BankDeposit(model.WeeklyWrapUp_BankDepositViewModel);
            await UpdateWeeklyWrapUp_Operations(model.WeeklyWrapUp_OperationsViewModel);


            Transaction.Commit();

            logger.LogInformation("Completed updating WeeklyWrapUp");
        }

        public async Task DeleteWeeklyWrapUp(int id)
        {
            var WeeklyWrapUp = context.WeeklyWrapUps.AsNoTracking().FirstOrDefault(x => x.Id == id);

            if(WeeklyWrapUp.files != null)
            {
              var allfiles =  WeeklyWrapUp.files.Split(" | ");

                foreach (var file in allfiles)
                {
                    DeleteFile(file.Replace("StaticFiles/Files", ""));
                }
            }




            context.Entry(WeeklyWrapUp).State = EntityState.Deleted;

            await context.SaveChangesAsync();


        }



        //----------------------BankDeposit Implementations------------
        //-------------------------------------------------------------
        //-------------------------------------------------------------
        //-------------------------------------------------------------
        //-------------------------------------------------------------

        private List<WeeklyWrapUp_BankDepositViewModel> GetWeeklyWrapUp_BankDeposit(int WeeklyWrapUpId)
        {

            var weeklyWrapUp_BankDeposit = context.WeeklyWrapUpBankDeposits.Where(a => a.WeeklyWrapUpId == WeeklyWrapUpId).ToList();

            return mapper.Map<List<WeeklyWrapUp_BankDepositViewModel>>(weeklyWrapUp_BankDeposit);
        }



        private async Task AddWeeklyWrapUp_BankDeposit(List<WeeklyWrapUp_BankDepositViewModel> model)
        {
            logger.LogInformation("Started adding WeeklyWrapUp_BankDeposit");

            
            foreach (var Item in model)
            {
                var BankDepositObj = new WeeklyWrapUp_BankDeposit()
                {
                    Actual_Deposit = Item.Actual_Deposit,
                    System_Deposit_Amount = Item.System_Deposit_Amount,
                    WeeklyWrapUpId = Item.WeeklyWrapUpId,
                    Date = Item.Date,
                };
                
                BankDepositObj.WeeklyWrapUp = null;

                context.WeeklyWrapUpBankDeposits.Add(BankDepositObj);

                await context.SaveChangesAsync();

            }



            logger.LogInformation("Completed adding WeeklyWrapUp_BankDeposit");
        }


        private async Task UpdateWeeklyWrapUp_BankDeposit(List<WeeklyWrapUp_BankDepositViewModel> model)
        {
            logger.LogInformation("Started updating WeeklyWrapUp_BankDeposit");


            foreach (var modelItem in model)
            {
                var BankDepositObj = context.WeeklyWrapUpBankDeposits.AsNoTracking()
                      .Where(a => a.Id == modelItem.Id)
                      .FirstOrDefault();

                if (BankDepositObj == null) { return; }

                //var MappedBankDepositObj = mapper.Map<WeeklyWrapUp_BankDeposit>(model);

                //MappedBankDepositObj.WeeklyWrapUpId = modelItem.WeeklyWrapUpId;
                BankDepositObj.Date = modelItem.Date;
                BankDepositObj.System_Deposit_Amount = modelItem.System_Deposit_Amount;
                BankDepositObj.Actual_Deposit = modelItem.Actual_Deposit;
                

                context.Entry(BankDepositObj).State = EntityState.Modified;

                await context.SaveChangesAsync();

            }



            //------------------------------------------------------------------------




            logger.LogInformation("Completed updating WeeklyWrapUp_BankDeposit");
        }









        /*----------------------Operations Implementations------------
        //------------------------------------------------------------
        //-------------------------------------------------------------
        //-------------------------------------------------------------
        //-------------------------------------------------------------
        //-------------------------------------------------------------
        */


        private WeeklyWrapUp_OperationsViewModel GetWeeklyWrapUp_Operations(int WeeklyWrapUpId)
        {

            var weeklyWrapUp_Operations = context.WeeklyWrapUpOperations.Where(a => a.WeeklyWrapUpId == WeeklyWrapUpId).FirstOrDefault();

            var MappedViewModel = mapper.Map<WeeklyWrapUp_OperationsViewModel>(weeklyWrapUp_Operations);

            MappedViewModel.weeklyWrapUp_OperationsDetails = mapper.Map<List<WeeklyWrapUp_OperationsDetailsViewModel>>(context.WeeklyWrapUpOperationsDetails.Where(a => a.WeeklyWrapUp_OperationsId == weeklyWrapUp_Operations.Id).ToList());

            foreach (var item in MappedViewModel.weeklyWrapUp_OperationsDetails)
            {
                item.Scores_Issue = context.ScoresIssues.Where(a => a.Id == item.Scores_IssuesId).Select(a => a.Name).FirstOrDefault();
            }

            return MappedViewModel;
        }

        

        private async Task AddWeeklyWrapUp_Operations(WeeklyWrapUp_OperationsViewModel model)
        {
            logger.LogInformation("Started adding WeeklyWrapUpOperations");

            var wrapUpOperationsObj = mapper.Map<WeeklyWrapUp_Operations>(model);
            wrapUpOperationsObj.Id = 0;

            wrapUpOperationsObj.weeklyWrapUp_OperationsDetails = null;

            context.WeeklyWrapUpOperations.Add(wrapUpOperationsObj);

            await context.SaveChangesAsync();


            var LastMonthReportObj = new WeeklyWrapUp_Operations();
            var sundriesShopInventoryDetails = new List<WeeklyWrapUp_OperationsDetails>();

            if (model.ReportFrom.Month != 1)
            {
                LastMonthReportObj = context.WeeklyWrapUpOperations.Where(a => a.ReportFrom.Month == model.ReportFrom.Month - 1 && a.WeeklyWrapUpId == model.WeeklyWrapUpId).FirstOrDefault();
            }
            else
            {
                LastMonthReportObj = context.WeeklyWrapUpOperations.Where(a => a.ReportFrom.Month == 12 && a.ReportFrom.Year == a.ReportFrom.Year - 1 && a.WeeklyWrapUpId == model.WeeklyWrapUpId).FirstOrDefault();
            }

            if (LastMonthReportObj != null)
            {
                //sundriesShopInventoryDetails = LastMonthReportObj.sundriesShopInventoryDetails.ToList();
                sundriesShopInventoryDetails = context.WeeklyWrapUpOperationsDetails.Where(a => a.WeeklyWrapUp_OperationsId == LastMonthReportObj.Id).ToList();
            }


            if (LastMonthReportObj != null && sundriesShopInventoryDetails != null)
            {
                foreach (var lastMonthitem in sundriesShopInventoryDetails)
                {
                    foreach (var item in model.weeklyWrapUp_OperationsDetails)
                    {
                        //if (context.Scores_Issues.Where(a => a.Name == item.Scores_Issue && a.WeeklyWrapUpId == model.WeeklyWrapUpId).Select(a => a.Id).FirstOrDefault()

                        if (context.ScoresIssues.Where(a => a.Name == item.Scores_Issue).Select(a => a.Id).FirstOrDefault()
                        == lastMonthitem.Scores_IssuesId
                        )
                        {

                            var OperationsDetailsObj = new WeeklyWrapUp_OperationsDetails();

                            //Id = item.Id,
                            //OperationsDetailsObj.Scores_IssuesId = context.Scores_Issues.Where(a => a.Name == item.ProductName && a.WeeklyWrapUpId == model.WeeklyWrapUpId).Select(a => a.Id).FirstOrDefault();

                            OperationsDetailsObj.Current_Month_Score = item.Current_Month_Score;
                            OperationsDetailsObj.Previous_Month_Score = lastMonthitem.Previous_Month_Score;
                            OperationsDetailsObj.YTD_score = item.YTD_score;
                            OperationsDetailsObj.Brand_Average = item.Brand_Average;
                            OperationsDetailsObj.Annual_Goal = item.Annual_Goal;



                            OperationsDetailsObj.Scores_IssuesId = context.ScoresIssues.Where(a => a.Name == item.Scores_Issue).Select(a => a.Id).FirstOrDefault();

                            OperationsDetailsObj.WeeklyWrapUp_OperationsId = wrapUpOperationsObj.Id;


                            context.WeeklyWrapUpOperationsDetails.Add(OperationsDetailsObj);

                            await context.SaveChangesAsync();
                        }


                    }
                }
            }
            else
            {
                foreach (var item in model.weeklyWrapUp_OperationsDetails)
                {

                    var OperationsDetailsObj = new WeeklyWrapUp_OperationsDetails()
                    {
                        //Id = item.Id,
                        Current_Month_Score = item.Current_Month_Score,
                        Previous_Month_Score = item.Previous_Month_Score,
                        YTD_score = item.YTD_score,
                        Brand_Average = item.Brand_Average,
                        Annual_Goal = item.Annual_Goal,
                        Scores_IssuesId = context.ScoresIssues.Where(a => a.Name == item.Scores_Issue).Select(a => a.Id).FirstOrDefault(),
                        WeeklyWrapUp_OperationsId = wrapUpOperationsObj.Id

                    };

                    context.WeeklyWrapUpOperationsDetails.Add(OperationsDetailsObj);

                    await context.SaveChangesAsync();


                }

            }


            logger.LogInformation("Completed adding WeeklyWrapUpOperations");
        }


        private async Task UpdateWeeklyWrapUp_Operations(WeeklyWrapUp_OperationsViewModel model)
        {
            logger.LogInformation("Started updating WeeklyWrapUpOperations");

            var OperationsObj = context.WeeklyWrapUpOperations.AsNoTracking()
                                  .Where(a => a.Id == model.Id)
                                  .FirstOrDefault();

            if (OperationsObj == null) { return; }


            var MappedOperationsObj = mapper.Map<WeeklyWrapUp_Operations>(model);

            MappedOperationsObj.WeeklyWrapUpId = OperationsObj.WeeklyWrapUpId;
            MappedOperationsObj.Id = OperationsObj.Id;
            MappedOperationsObj.weeklyWrapUp_OperationsDetails = null;

            context.Entry(MappedOperationsObj).State = EntityState.Modified;

            await context.SaveChangesAsync();


            //------------------------------------------------------------------------

            var LastMonthReportObj = new WeeklyWrapUp_Operations();
            var sundriesShopInventoryDetails = new List<WeeklyWrapUp_OperationsDetails>();

            if (model.ReportFrom.Month != 1)
            {
                LastMonthReportObj = context.WeeklyWrapUpOperations.Where(a => a.ReportFrom.Month == model.ReportFrom.Month - 1 && a.WeeklyWrapUpId == model.WeeklyWrapUpId).FirstOrDefault();
            }
            else
            {
                LastMonthReportObj = context.WeeklyWrapUpOperations.Where(a => a.ReportFrom.Month == 12 && a.ReportFrom.Year == a.ReportFrom.Year - 1 && a.WeeklyWrapUpId == model.WeeklyWrapUpId).FirstOrDefault();
            }

            if (LastMonthReportObj != null)
            {
                //sundriesShopInventoryDetails = LastMonthReportObj.sundriesShopInventoryDetails.ToList();
                sundriesShopInventoryDetails = context.WeeklyWrapUpOperationsDetails.Where(a => a.WeeklyWrapUp_OperationsId == LastMonthReportObj.Id).ToList();
            }


            if (LastMonthReportObj != null && sundriesShopInventoryDetails != null)
            {
                foreach (var lastMonthitem in sundriesShopInventoryDetails)
                {
                    foreach (var item in model.weeklyWrapUp_OperationsDetails.ToList())
                    {

                        if (item.Id != 0)
                        {
                            //if (context.Scores_Issues.Where(a => a.Name == item.Scores_Issue && a.WeeklyWrapUpId == model.WeeklyWrapUpId).Select(a => a.Id).FirstOrDefault()
                            if (context.ScoresIssues.Where(a => a.Name == item.Scores_Issue).Select(a => a.Id).FirstOrDefault()
                            == lastMonthitem.Scores_IssuesId
                            )
                            {


                                var WeeklyWrapUp_OperationsObj = context.WeeklyWrapUpOperationsDetails.AsNoTracking()
                                                                        .Where(a => a.Id == item.Id)
                                                                        .FirstOrDefault();



                                WeeklyWrapUp_OperationsObj.Current_Month_Score = item.Current_Month_Score;
                                WeeklyWrapUp_OperationsObj.YTD_score = item.YTD_score;
                                WeeklyWrapUp_OperationsObj.Brand_Average = item.Brand_Average;
                                WeeklyWrapUp_OperationsObj.Annual_Goal = item.Annual_Goal;






                                context.Entry(WeeklyWrapUp_OperationsObj).State = EntityState.Modified;

                                await context.SaveChangesAsync();

                                //-----------------------------------------------------------------------------------

                            }
                        }
                        else
                        {

                            var OperationsDetailsObj = new WeeklyWrapUp_OperationsDetails();

                            //Id = item.Id,

                            OperationsDetailsObj.Current_Month_Score = item.Current_Month_Score;

                            OperationsDetailsObj.Previous_Month_Score = lastMonthitem.Previous_Month_Score;

                            OperationsDetailsObj.YTD_score = item.YTD_score;
                            OperationsDetailsObj.Brand_Average = item.Brand_Average;
                            OperationsDetailsObj.Annual_Goal = item.Annual_Goal;
                            OperationsDetailsObj.Scores_IssuesId = context.ScoresIssues.Where(a => a.Name == item.Scores_Issue).Select(a => a.Id).FirstOrDefault();

                            OperationsDetailsObj.WeeklyWrapUp_OperationsId = OperationsObj.Id;



                            context.WeeklyWrapUpOperationsDetails.Add(OperationsDetailsObj);

                            await context.SaveChangesAsync();

                            model.weeklyWrapUp_OperationsDetails.Remove(item);

                        }


                    }
                }
            }
            else
            {
                foreach (var item in model.weeklyWrapUp_OperationsDetails)
                {



                    if (item.Id != 0)
                    {
                        var WeeklyWrapUp_OperationsObj = context.WeeklyWrapUpOperationsDetails.AsNoTracking()
                                              .Where(a => a.Id == item.Id)
                                              .FirstOrDefault();

                        //WeeklyWrapUp_OperationsObj.LastMonthEndingBalance = item.LastMonthEndingBalance;

                        //WeeklyWrapUp_OperationsObj.ProductId = context.SundriesShopProducts.Where(a => a.Name == item.ProductName).Select(a => a.Id).FirstOrDefault(),
                        //WeeklyWrapUp_OperationsId = wrapUpOperationsObj.Id

                        WeeklyWrapUp_OperationsObj.Current_Month_Score = item.Current_Month_Score;
                        WeeklyWrapUp_OperationsObj.YTD_score = item.YTD_score;
                        WeeklyWrapUp_OperationsObj.Brand_Average = item.Brand_Average;
                        WeeklyWrapUp_OperationsObj.Annual_Goal = item.Annual_Goal;





                        context.Entry(WeeklyWrapUp_OperationsObj).State = EntityState.Modified;

                        await context.SaveChangesAsync();
                    }
                    else
                    {

                        var OperationsDetailsObj = new WeeklyWrapUp_OperationsDetails()
                        {
                            //Id = item.Id,

                            Current_Month_Score = item.Current_Month_Score,
                            Previous_Month_Score = item.Previous_Month_Score,
                            YTD_score = item.YTD_score,
                            Brand_Average = item.Brand_Average,
                            Annual_Goal = item.Annual_Goal,
                            Scores_IssuesId = context.ScoresIssues.Where(a => a.Name == item.Scores_Issue).Select(a => a.Id).FirstOrDefault(),
                            WeeklyWrapUp_OperationsId = OperationsObj.Id,


                        };

                        context.WeeklyWrapUpOperationsDetails.Add(OperationsDetailsObj);

                        await context.SaveChangesAsync();

                    }



                }

            }
            //-------------------------------------------------------------------------





            logger.LogInformation("Completed updating WeeklyWrapUpOperations");
        }




    }
}
