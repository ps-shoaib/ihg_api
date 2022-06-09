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
using IGHportalAPI.ViewModels.SundriesShopInventoryViewModels;
using IGHportalAPI.Models;

namespace IGHportalAPI.Services
{
    public class SundriesShopInventoryService  : ISundriesShopInventoryService
    {
        private readonly ILogger<SundriesShopInventoryService> logger;
        private readonly DataContext_ context;
        private readonly IMapper mapper;

        public SundriesShopInventoryService(ILogger<SundriesShopInventoryService> logger, DataContext_ context, IMapper mapper)
        {
            this.logger = logger;
            this.context = context;
            this.mapper = mapper;
        }                     

        public List<SundriesShopInventoryViewModel> GetSundriesShopInventories(int hotelId , string Month , string Year)
        {

            var AllSundriesShopInvetories = context.SundriesShopInventories.Where(a => a.HotelId == hotelId).ToList();

            if ((!string.IsNullOrEmpty(Month)) && string.IsNullOrEmpty(Year))
            {
                AllSundriesShopInvetories = AllSundriesShopInvetories.Where(a => a.ReportMonthYear.ToString("MMMM") == Month).ToList();
            }
            else if ((!string.IsNullOrEmpty(Year)) && string.IsNullOrEmpty(Month))
            {
                AllSundriesShopInvetories = AllSundriesShopInvetories.Where(a => a.ReportMonthYear.Year.ToString() == Year).ToList();
            }
            else if ((!string.IsNullOrEmpty(Year)) && (!string.IsNullOrEmpty(Month)))
            {
                AllSundriesShopInvetories =
                        AllSundriesShopInvetories
                        .Where(a => a.ReportMonthYear.ToString("MMMM") == Month
                        && a.ReportMonthYear.Year.ToString() == Year).ToList();
            }
            //else
            //{
            //    AllSundriesShopInvetories = context.SundriesShopInventories.Where(a => a.HotelId == hotelId &&
            //      a.ReportMonthYear.Year.ToString() == DateTime.UtcNow.Year.ToString()
            //    ).ToList();
            //}



            //&& a.Month == "" && a.CreatedOn.Year.ToString() == ""



            var ListSundriesShopInvetoriesViewModel = new List<SundriesShopInventoryViewModel>();

            foreach (var linenitem in AllSundriesShopInvetories)
            {
                ListSundriesShopInvetoriesViewModel.Add(new SundriesShopInventoryViewModel()
                {
                    Id = linenitem.Id,

                    ReportMonthYear = linenitem.ReportMonthYear,
                    HotelId = linenitem.HotelId,
                    CreatedBy = linenitem.CreatedBy
                });
            }

            return ListSundriesShopInvetoriesViewModel;


            //return mapper.Map<List<SundriesShopInventoryViewModel>>(context.SundriesShopInventories.Where(a => a.HotelId == hotelId).ToList());
        }

        public SundriesShopInventoryViewModel GetSundriesShopInventory(int id, int hotelId)
        {
      
            var SundriesShopInventory = context.SundriesShopInventories.Where(a => a.HotelId == hotelId).FirstOrDefault(x => x.Id == id);

            if (SundriesShopInventory == null)
            {
                return null;
            }

            var linenInventoryViewModel = new SundriesShopInventoryViewModel()
            {
                Id = SundriesShopInventory.Id,
                HotelId = SundriesShopInventory.HotelId,
                CreatedBy = SundriesShopInventory.CreatedBy,
                ReportMonthYear = SundriesShopInventory.ReportMonthYear
            };

            var AllSundriesShopInventoryDetails = context.SundriesShopInventoryDetails.Where(a => a.SundriesShopInventoryId == SundriesShopInventory.Id).ToList();


            foreach (var item in AllSundriesShopInventoryDetails)
            {
                linenInventoryViewModel.SundriesShopInventoryDetails.Add(
                                new SundriesShopInventoryDetailsViewModel()
                                {
                                   Id = item.Id,
                                    LastMonthEndingBalance = item.LastMonthEndingBalance,
                                    NewlyPurchased = item.NewlyPurchased,
                                    OnDisplay = item.OnDisplay,
                                    BackupStore = item.BackupStore,
                                    MonthsEndingBalance = item.MonthsEndingBalance,
                                    ExpiredLogged = item.ExpiredLogged,
                                    CostOfExpiredLogged = item.CostOfExpiredLogged,
                                    ProductSales = item.ProductSales,
                                    ItemWholeSaleCost = item.ItemWholeSaleCost,
                                    CostOfSale = item.CostOfSale,
                                    ItemRetailCost = item.ItemRetailCost,
                                    CurrentStockRetail = item.CurrentStockRetail,
                                    MonthRevenue = item.MonthRevenue,

                                    ProductName = context.SundriesShopProducts.Where(a => a.Id == item.ProductId).Select(a => a.Name).FirstOrDefault(),


                                }
                    );
            }


 
            return linenInventoryViewModel;
            //return mapper.Map<SundriesShopInventoryViewModel>(SundriesShopInventory);
        }


        public async Task AddSundriesShopInventory(SundriesShopInventoryViewModel model)
        {
            logger.LogInformation("Started adding SundriesShopInventory");

            var Transaction = context.Database.BeginTransaction();

            var sundriesShopObj = new SundriesShopInventory()
            {
                ReportMonthYear = model.ReportMonthYear,
                HotelId = model.HotelId,
                CreatedBy = model.CreatedBy,
                CreatedOn = DateTime.UtcNow
            };

            context.SundriesShopInventories.Add(sundriesShopObj);

            await context.SaveChangesAsync();


            var LastMonthReportObj = new SundriesShopInventory();
            var sundriesShopInventoryDetails = new List<SundriesShopInventoryDetails>();

            if (model.ReportMonthYear.Month != 1)
            {
                LastMonthReportObj = context.SundriesShopInventories.Where(a => a.ReportMonthYear.Month == model.ReportMonthYear.Month - 1 && a.HotelId == model.HotelId).FirstOrDefault();
            }
            else
            {
                LastMonthReportObj = context.SundriesShopInventories.Where(a => a.ReportMonthYear.Month == 12 && a.ReportMonthYear.Year == a.ReportMonthYear.Year - 1 && a.HotelId == model.HotelId).FirstOrDefault();
            }

            if (LastMonthReportObj != null)
            {
                //sundriesShopInventoryDetails = LastMonthReportObj.sundriesShopInventoryDetails.ToList();
                sundriesShopInventoryDetails = context.SundriesShopInventoryDetails.Where(a => a.SundriesShopInventoryId == LastMonthReportObj.Id).ToList();
            }


            if (LastMonthReportObj != null && sundriesShopInventoryDetails != null)
            {
                foreach (var lastMonthitem in sundriesShopInventoryDetails)
                {
                    foreach (var item in model.SundriesShopInventoryDetails)
                    {
                        if (context.SundriesShopProducts.Where(a => a.Name == item.ProductName && a.HotelId == model.HotelId).Select(a => a.Id).FirstOrDefault()
                        == lastMonthitem.ProductId
                        )
                        {

                            var linenInventoryDetailsObj = new SundriesShopInventoryDetails();

                            //Id = item.Id,
                            linenInventoryDetailsObj.LastMonthEndingBalance = lastMonthitem.MonthsEndingBalance;

                            var LastMonthBalance = lastMonthitem.MonthsEndingBalance;

                            if (item.NewlyPurchased == 0 && item.BackupStore == 0 && item.OnDisplay == 0)
                            {
                                LastMonthBalance = 0;
                            }



                            linenInventoryDetailsObj.NewlyPurchased = item.NewlyPurchased;
                                linenInventoryDetailsObj.OnDisplay = item.OnDisplay;
                                linenInventoryDetailsObj.BackupStore = item.BackupStore;

                                linenInventoryDetailsObj.MonthsEndingBalance = item.OnDisplay + item.BackupStore;

                                linenInventoryDetailsObj.ExpiredLogged = item.ExpiredLogged;

                                linenInventoryDetailsObj.CostOfExpiredLogged = item.ExpiredLogged * item.ItemWholeSaleCost;

                                linenInventoryDetailsObj.ProductSales = (LastMonthBalance + item.NewlyPurchased) - (item.OnDisplay + item.BackupStore) - item.ExpiredLogged;

                                linenInventoryDetailsObj.ItemWholeSaleCost = item.ItemWholeSaleCost;

                                linenInventoryDetailsObj.CostOfSale = ((LastMonthBalance + item.NewlyPurchased) - (item.OnDisplay + item.BackupStore) - item.ExpiredLogged)
                            * item.ItemWholeSaleCost;

                                linenInventoryDetailsObj.ItemRetailCost = item.ItemRetailCost;

                                linenInventoryDetailsObj.CurrentStockRetail = (item.OnDisplay + item.BackupStore) * item.ItemRetailCost;

                                linenInventoryDetailsObj.MonthRevenue = ((LastMonthBalance + item.NewlyPurchased) - (item.OnDisplay + item.BackupStore) - item.ExpiredLogged)
                            * item.ItemRetailCost;

                                linenInventoryDetailsObj.ProductId = context.SundriesShopProducts.Where(a => a.Name == item.ProductName && a.HotelId == model.HotelId).Select(a => a.Id).FirstOrDefault();
                                linenInventoryDetailsObj.SundriesShopInventoryId = sundriesShopObj.Id;



                            context.SundriesShopInventoryDetails.Add(linenInventoryDetailsObj);

                            await context.SaveChangesAsync();
                        }
                        

                    }            
                }
            }
            else
            {
                foreach (var item in model.SundriesShopInventoryDetails)
                {

                        var linenInventoryDetailsObj = new SundriesShopInventoryDetails()
                        {
                            //Id = item.Id,
                            LastMonthEndingBalance = 0,

                            NewlyPurchased = item.NewlyPurchased,
                            OnDisplay = item.OnDisplay,
                            BackupStore = item.BackupStore,

                            MonthsEndingBalance = item.OnDisplay + item.BackupStore,

                            ExpiredLogged = item.ExpiredLogged,

                            CostOfExpiredLogged = item.ExpiredLogged * item.ItemWholeSaleCost,

                            ProductSales = item.NewlyPurchased - (item.OnDisplay + item.BackupStore) - item.ExpiredLogged,

                            ItemWholeSaleCost = item.ItemWholeSaleCost,

                            CostOfSale = (item.NewlyPurchased - (item.OnDisplay + item.BackupStore) - item.ExpiredLogged) * item.ItemWholeSaleCost,

                            ItemRetailCost = item.ItemRetailCost,

                            CurrentStockRetail = (item.OnDisplay + item.BackupStore) * item.ItemRetailCost,

                            MonthRevenue = (item.NewlyPurchased - (item.OnDisplay + item.BackupStore) - item.ExpiredLogged) * item.ItemRetailCost,

                            ProductId = context.SundriesShopProducts.Where(a => a.Name == item.ProductName && a.HotelId == model.HotelId).Select(a => a.Id).FirstOrDefault(),
                            SundriesShopInventoryId = sundriesShopObj.Id

                        };

                        context.SundriesShopInventoryDetails.Add(linenInventoryDetailsObj);

                        await context.SaveChangesAsync();
                    
                    
                }

            }

            Transaction.Commit();

            logger.LogInformation("Completed adding SundriesShopInventory");
        }
        

        public async Task UpdateSundriesShopInventory(SundriesShopInventoryViewModel model)
        {
            logger.LogInformation("Started updating SundriesShopInventory");

            var Transaction = context.Database.BeginTransaction();


            var linenInventory = context.SundriesShopInventories.AsNoTracking()
                                  .Where(a => a.Id == model.Id)
                                  .FirstOrDefault();

            if (linenInventory == null) { return; }

            linenInventory.ReportMonthYear = model.ReportMonthYear;

            //linenInventory.HotelId = model.HotelId;
            //linenInventory.CreatedBy = model.CreatedBy;

            context.Entry(linenInventory).State = EntityState.Modified;

            await context.SaveChangesAsync();


            //------------------------------------------------------------------------

            var LastMonthReportObj = new SundriesShopInventory();
            var sundriesShopInventoryDetails = new List<SundriesShopInventoryDetails>();

            if (model.ReportMonthYear.Month != 1)
            {
                LastMonthReportObj = context.SundriesShopInventories.Where(a => a.ReportMonthYear.Month == model.ReportMonthYear.Month - 1 && a.HotelId == model.HotelId).FirstOrDefault();
            }
            else
            {
                LastMonthReportObj = context.SundriesShopInventories.Where(a => a.ReportMonthYear.Month == 12 && a.ReportMonthYear.Year == a.ReportMonthYear.Year - 1 && a.HotelId == model.HotelId).FirstOrDefault();
            }

            if (LastMonthReportObj != null)
            {
                //sundriesShopInventoryDetails = LastMonthReportObj.sundriesShopInventoryDetails.ToList();
                sundriesShopInventoryDetails = context.SundriesShopInventoryDetails.Where(a => a.SundriesShopInventoryId == LastMonthReportObj.Id).ToList();
            }


            if (LastMonthReportObj != null && sundriesShopInventoryDetails != null)
            {
                foreach (var lastMonthitem in sundriesShopInventoryDetails)
                {
                    foreach (var item in model.SundriesShopInventoryDetails.ToList())
                    {
                        if (item.NewlyPurchased != 0 && item.BackupStore != 0 && item.OnDisplay != 0)
                        {
                            if (item.Id != 0)
                            {
                                if (context.SundriesShopProducts.Where(a => a.Name == item.ProductName && a.HotelId == model.HotelId).Select(a => a.Id).FirstOrDefault()
                                == lastMonthitem.ProductId
                                )
                                {


                                    var SundriesShopInventoryObj = context.SundriesShopInventoryDetails.AsNoTracking()
                                                                          .Where(a => a.Id == item.Id)
                                                                          .FirstOrDefault();

                                    //SundriesShopInventoryObj.LastMonthEndingBalance = item.LastMonthEndingBalance;


                                    SundriesShopInventoryObj.NewlyPurchased = item.NewlyPurchased;
                                    SundriesShopInventoryObj.OnDisplay = item.OnDisplay;
                                    SundriesShopInventoryObj.BackupStore = item.BackupStore;

                                    SundriesShopInventoryObj.MonthsEndingBalance = item.OnDisplay + item.BackupStore;

                                    SundriesShopInventoryObj.ExpiredLogged = item.ExpiredLogged;

                                    SundriesShopInventoryObj.CostOfExpiredLogged = item.ExpiredLogged * item.ItemWholeSaleCost;

                                    SundriesShopInventoryObj.ProductSales = (SundriesShopInventoryObj.LastMonthEndingBalance + item.NewlyPurchased) - (item.OnDisplay + item.BackupStore) - item.ExpiredLogged;

                                    SundriesShopInventoryObj.ItemWholeSaleCost = item.ItemWholeSaleCost;

                                    SundriesShopInventoryObj.CostOfSale = ((SundriesShopInventoryObj.LastMonthEndingBalance + item.NewlyPurchased) - (item.OnDisplay + item.BackupStore) - item.ExpiredLogged)
                                                                           * item.ItemWholeSaleCost;

                                    SundriesShopInventoryObj.ItemRetailCost = item.ItemRetailCost;

                                    SundriesShopInventoryObj.CurrentStockRetail = (item.OnDisplay + item.BackupStore) * item.ItemRetailCost;

                                    SundriesShopInventoryObj.MonthRevenue = ((SundriesShopInventoryObj.LastMonthEndingBalance + item.NewlyPurchased) - (item.OnDisplay + item.BackupStore) - item.ExpiredLogged)
                                                                             * item.ItemRetailCost;

                                    //SundriesShopInventoryObj.ProductId = context.SundriesShopProducts.Where(a => a.Name == item.ProductName).Select(a => a.Id).FirstOrDefault(),
                                    //SundriesShopInventoryId = sundriesShopObj.Id





                                    context.Entry(SundriesShopInventoryObj).State = EntityState.Modified;

                                    await context.SaveChangesAsync();

                                    //-----------------------------------------------------------------------------------

                                }
                            }
                            else
                            {

                                    var linenInventoryDetailsObj = new SundriesShopInventoryDetails();

                                    //Id = item.Id,
                                    linenInventoryDetailsObj.LastMonthEndingBalance = lastMonthitem.MonthsEndingBalance;

                                    var LastMonthBalance = lastMonthitem.MonthsEndingBalance;
                                    
                                    if (item.NewlyPurchased == 0 && item.BackupStore == 0 && item.OnDisplay == 0)
                                    {
                                        LastMonthBalance = 0;
                                    }

                                    linenInventoryDetailsObj.NewlyPurchased = item.NewlyPurchased;
                                        linenInventoryDetailsObj.OnDisplay = item.OnDisplay;
                                    linenInventoryDetailsObj.BackupStore = item.BackupStore;

                                    linenInventoryDetailsObj.MonthsEndingBalance = item.OnDisplay + item.BackupStore;

                                    linenInventoryDetailsObj.ExpiredLogged = item.ExpiredLogged;

                                    linenInventoryDetailsObj.CostOfExpiredLogged = item.ExpiredLogged * item.ItemWholeSaleCost;

                                    linenInventoryDetailsObj.ProductSales = (item.NewlyPurchased + LastMonthBalance) - (item.OnDisplay + item.BackupStore) - item.ExpiredLogged;
                                    linenInventoryDetailsObj.ItemWholeSaleCost = item.ItemWholeSaleCost;

                                    linenInventoryDetailsObj.CostOfSale = ((item.NewlyPurchased + LastMonthBalance) - (item.OnDisplay + item.BackupStore) - item.ExpiredLogged) * item.ItemWholeSaleCost;
                                    linenInventoryDetailsObj.ItemRetailCost = item.ItemRetailCost;

                                    linenInventoryDetailsObj.CurrentStockRetail = (item.OnDisplay + item.BackupStore) * item.ItemRetailCost;
                                    linenInventoryDetailsObj.MonthRevenue = ((item.NewlyPurchased + LastMonthBalance) - (item.OnDisplay + item.BackupStore) - item.ExpiredLogged) * item.ItemRetailCost;
                                    linenInventoryDetailsObj.ProductId = context.SundriesShopProducts.Where(a => a.Name == item.ProductName && a.HotelId == model.HotelId).Select(a => a.Id).FirstOrDefault();
                                    linenInventoryDetailsObj.SundriesShopInventoryId = linenInventory.Id;

                                

                                    context.SundriesShopInventoryDetails.Add(linenInventoryDetailsObj);

                                    await context.SaveChangesAsync();

                                model.SundriesShopInventoryDetails.Remove(item);

                            }
                        }
                        
                    }
                }
            }
            else
            {
                foreach (var item in model.SundriesShopInventoryDetails)
                {
                    if (item.NewlyPurchased != 0 && item.BackupStore != 0 && item.OnDisplay != 0)
                    {


                        if (item.Id != 0)
                        {
                            var SundriesShopInventoryObj = context.SundriesShopInventoryDetails.AsNoTracking()
                                                  .Where(a => a.Id == item.Id)
                                                  .FirstOrDefault();

                            //SundriesShopInventoryObj.LastMonthEndingBalance = item.LastMonthEndingBalance;
                            SundriesShopInventoryObj.NewlyPurchased = item.NewlyPurchased;
                            SundriesShopInventoryObj.OnDisplay = item.OnDisplay;
                            SundriesShopInventoryObj.BackupStore = item.BackupStore;

                            SundriesShopInventoryObj.MonthsEndingBalance = item.OnDisplay + item.BackupStore;

                            SundriesShopInventoryObj.ExpiredLogged = item.ExpiredLogged;

                            SundriesShopInventoryObj.CostOfExpiredLogged = item.ExpiredLogged * item.ItemWholeSaleCost;

                            SundriesShopInventoryObj.ProductSales = (SundriesShopInventoryObj.LastMonthEndingBalance + item.NewlyPurchased) - (item.OnDisplay + item.BackupStore) - item.ExpiredLogged;

                            SundriesShopInventoryObj.ItemWholeSaleCost = item.ItemWholeSaleCost;

                            SundriesShopInventoryObj.CostOfSale = ((SundriesShopInventoryObj.LastMonthEndingBalance + item.NewlyPurchased) - (item.OnDisplay + item.BackupStore) - item.ExpiredLogged)
                                                                   * item.ItemWholeSaleCost;

                            SundriesShopInventoryObj.ItemRetailCost = item.ItemRetailCost;

                            SundriesShopInventoryObj.CurrentStockRetail = (item.OnDisplay + item.BackupStore) * item.ItemRetailCost;

                            SundriesShopInventoryObj.MonthRevenue = ((SundriesShopInventoryObj.LastMonthEndingBalance + item.NewlyPurchased) - (item.OnDisplay + item.BackupStore) - item.ExpiredLogged)
                                                                     * item.ItemRetailCost;

                            //SundriesShopInventoryObj.ProductId = context.SundriesShopProducts.Where(a => a.Name == item.ProductName).Select(a => a.Id).FirstOrDefault(),
                            //SundriesShopInventoryId = sundriesShopObj.Id





                            context.Entry(SundriesShopInventoryObj).State = EntityState.Modified;

                            await context.SaveChangesAsync();
                        }
                        else
                        {

                            var linenInventoryDetailsObj = new SundriesShopInventoryDetails()
                            {
                                //Id = item.Id,
                                LastMonthEndingBalance = 0,

                                NewlyPurchased = item.NewlyPurchased,
                                OnDisplay = item.OnDisplay,
                                BackupStore = item.BackupStore,



                                MonthsEndingBalance = item.OnDisplay + item.BackupStore,

                                ExpiredLogged = item.ExpiredLogged,

                                CostOfExpiredLogged = item.ExpiredLogged * item.ItemWholeSaleCost,

                                ProductSales = item.NewlyPurchased - (item.OnDisplay + item.BackupStore) - item.ExpiredLogged,

                                ItemWholeSaleCost = item.ItemWholeSaleCost,

                                CostOfSale = (item.NewlyPurchased - (item.OnDisplay + item.BackupStore) - item.ExpiredLogged) * item.ItemWholeSaleCost,

                                ItemRetailCost = item.ItemRetailCost,

                                CurrentStockRetail = (item.OnDisplay + item.BackupStore) * item.ItemRetailCost,

                                MonthRevenue = (item.NewlyPurchased - (item.OnDisplay + item.BackupStore) - item.ExpiredLogged) * item.ItemRetailCost,

                                ProductId = context.SundriesShopProducts.Where(a => a.Name == item.ProductName && a.HotelId == model.HotelId).Select(a => a.Id).FirstOrDefault(),
                                SundriesShopInventoryId = linenInventory.Id

                            };

                            context.SundriesShopInventoryDetails.Add(linenInventoryDetailsObj);

                            await context.SaveChangesAsync();

                        }
                    
                    }

                }

            }
            //-------------------------------------------------------------------------



            Transaction.Commit();
            
            logger.LogInformation("Completed updating SundriesShopInventory");
        }

        public async Task DeleteSundriesShopInventory(int id)
        {
            var SundriesShopInventory = context.SundriesShopInventories.FirstOrDefault(x => x.Id == id);


            //SundriesShopInventory.UpdatedStatus = (short)UpdStatus.Deleted;

            //context.Entry(SundriesShopInventory).State = EntityState.Modified;

            context.Entry(SundriesShopInventory).State = EntityState.Deleted;

            await context.SaveChangesAsync();


        }


        public bool ReportAlreadyExistOfCurrentMonth(SundriesShopInventoryViewModel model)
        {
            //context.Departments.Include(a => a.Id).
            var SundriesShopInventoryObjOfSameMonth = context
                  .SundriesShopInventories
                  .Where(a => a.ReportMonthYear.Month == model.ReportMonthYear.Month
                         && a.ReportMonthYear.Year == model.ReportMonthYear.Year && a.HotelId == model.HotelId && model.Id != a.Id)
                  .FirstOrDefault();


            if (SundriesShopInventoryObjOfSameMonth == null)
            {
                return false;
            }
            return true;
        }






    }
}
