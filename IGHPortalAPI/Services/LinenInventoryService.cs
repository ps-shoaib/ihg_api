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
using IGHportalAPI.ViewModels.LinenInventoryViewModels;
using IGHportalAPI.Models;

namespace IGHportalAPI.Services
{
    public class LinenInventoryService  : ILinenInventoryService
    {
        private readonly ILogger<LinenInventoryService> logger;
        private readonly DataContext_ context;
        private readonly IMapper mapper;

        public LinenInventoryService(ILogger<LinenInventoryService> logger, DataContext_ context, IMapper mapper)
        {
            this.logger = logger;
            this.context = context;
            this.mapper = mapper;
        }                     

        public List<LinenInventoryViewModel> GetLinenInventories(int hotelId , string Month , string Year)
        {

            var AllLinenInvetories = context.LinenInventories.Where(a => a.HotelId == hotelId).ToList();

            if ((!string.IsNullOrEmpty(Month)) && string.IsNullOrEmpty(Year))
            {
                AllLinenInvetories = AllLinenInvetories.Where(a => a.ReportMonthYear.ToString("MMMM") == Month).ToList();
            }
            else if ((!string.IsNullOrEmpty(Year)) && string.IsNullOrEmpty(Month))
            {
                AllLinenInvetories = AllLinenInvetories.Where(a => a.ReportMonthYear.Year.ToString() == Year).ToList();
            }
            else if ((!string.IsNullOrEmpty(Year)) && (!string.IsNullOrEmpty(Month)))
            {
                AllLinenInvetories =
                        AllLinenInvetories
                        .Where(a => a.ReportMonthYear.ToString("MMMM") == Month
                        && a.ReportMonthYear.Year.ToString() == Year).ToList();
            }
            //else
            //{
            //    AllLinenInvetories = context.LinenInventories.Where(a => a.HotelId == hotelId &&
            //      a.ReportMonthYear.Year.ToString() == DateTime.UtcNow.Year.ToString()
            //    ).ToList();
            //}



            //&& a.Month == "" && a.CreatedOn.Year.ToString() == ""



            var ListLinenInvetoriesViewModel = new List<LinenInventoryViewModel>();

            foreach (var linenitem in AllLinenInvetories)
            {
                ListLinenInvetoriesViewModel.Add(new LinenInventoryViewModel()
                {
                    Id = linenitem.Id,

                    ReportMonthYear = linenitem.ReportMonthYear,
                    HotelId = linenitem.HotelId,
                    CreatedBy = linenitem.CreatedBy,
                    //Year = linenitem.CreatedOn.Year.ToString()
                });
            }

            return ListLinenInvetoriesViewModel;


            //return mapper.Map<List<LinenInventoryViewModel>>(context.LinenInventories.Where(a => a.HotelId == hotelId).ToList());
        }

        public LinenInventoryViewModel GetLinenInventory(int id, int hotelId)
        {
      
            var LinenInventory = context.LinenInventories.Where(a => a.HotelId == hotelId).FirstOrDefault(x => x.Id == id);

            if (LinenInventory == null)
            {
                return null;
            }

            var linenInventoryViewModel = new LinenInventoryViewModel()
            {
                Id = LinenInventory.Id,
                HotelId = LinenInventory.HotelId,
                CreatedBy = LinenInventory.CreatedBy,
                ReportMonthYear = LinenInventory.ReportMonthYear,
                //Year = LinenInventory.CreatedOn.Year.ToString()
            };

            var AllLinenInventoryDetails = context.LinenInventoryDetails.Where(a => a.LinenInventoryId == LinenInventory.Id).ToList();

            foreach (var item in AllLinenInventoryDetails)
            {
                linenInventoryViewModel.LinenInventoryDetails.Add(
                                new LinenInventoryDetailsViewModel()
                               {
                                   Id = item.Id,
                                   OneTurnForAllRooms = item.OneTurnForAllRooms,
                                   RequiredTurns = item.RequiredTurns,
                                   RequiredPar = item.RequiredPar,
                                   LastMonthEndingBalance = item.LastMonthEndingBalance,
                                   NewPurchases = item.NewPurchases,
                                   firstFloorStorage = item.firstFloorStorage,
                                   secondFloorStorage = item.secondFloorStorage,
                                   thirdFloorStorage = item.thirdFloorStorage,
                                   fourthFloorStorage = item.fourthFloorStorage,
                                   fifthFloorStorage = item.fifthFloorStorage,
                                   InRooms = item.InRooms,
                                   Dirty = item.Dirty,
                                   EndingBalanceOfMonth = item.EndingBalanceOfMonth,
                                   MonthlyTotalLoss = item.MonthlyTotalLoss,
                                   QuantityTobeOrdered = item.QuantityTobeOrdered,

                                   LinenItemName = context.LinenItems.Where(a => a.Id == item.LinenItemId).Select(a => a.Name).FirstOrDefault(),


                               }
                    );
            }


 
            return linenInventoryViewModel;
            //return mapper.Map<LinenInventoryViewModel>(LinenInventory);
        }


        public async Task AddLinenInventory(LinenInventoryViewModel model)
        {
            logger.LogInformation("Started adding LinenInventory");

            var Transaction = context.Database.BeginTransaction();

            var linenInventoryObj = new LinenInventory()
            {
                ReportMonthYear = model.ReportMonthYear,
                HotelId = model.HotelId,
                CreatedBy = model.CreatedBy,
                CreatedOn = DateTime.UtcNow
            };

            context.LinenInventories.Add(linenInventoryObj);

            await context.SaveChangesAsync();

            var LastMonthReportObj = new LinenInventory();
            var LinenInventoryDetails = new List<LinenInventoryDetails>();

            if (model.ReportMonthYear.Month != 1)
            {
                LastMonthReportObj = context.LinenInventories.Where(a => a.ReportMonthYear.Month == model.ReportMonthYear.Month - 1 && a.HotelId == model.HotelId).FirstOrDefault();
            }
            else
            {
                LastMonthReportObj = context.LinenInventories.Where(a => a.ReportMonthYear.Month == 12 && a.ReportMonthYear.Year == a.ReportMonthYear.Year - 1 && a.HotelId == model.HotelId).FirstOrDefault();
            }

            if (LastMonthReportObj != null)
            {
                //LinenInventoryDetails = LastMonthReportObj.LinenInventoryDetails.ToList();
                LinenInventoryDetails = context.LinenInventoryDetails.Where(a => a.LinenInventoryId == LastMonthReportObj.Id).ToList();
            }

            if(LastMonthReportObj != null &&  LinenInventoryDetails != null) { 
                foreach (var lastMonthitem in LinenInventoryDetails)
                {
                    foreach (var item in model.LinenInventoryDetails)
                    {

                        if (context.LinenItems.Where(a => a.Name == item.LinenItemName && a.HotelId == model.HotelId).Select(a => a.Id).FirstOrDefault()
                            == lastMonthitem.LinenItemId
                            )
                        {
                            var linenInventoryDetailsObj = new LinenInventoryDetails();
                            //{

                            linenInventoryDetailsObj.OneTurnForAllRooms = item.OneTurnForAllRooms; 
                            linenInventoryDetailsObj.RequiredTurns = item.RequiredTurns;
                            linenInventoryDetailsObj.RequiredPar = item.OneTurnForAllRooms * item.RequiredTurns;
                            linenInventoryDetailsObj.LastMonthEndingBalance = lastMonthitem.EndingBalanceOfMonth;


                            var LastMonthBalance = lastMonthitem.EndingBalanceOfMonth;

                            if (item.OneTurnForAllRooms == 0 && item.RequiredTurns == 0 && item.NewPurchases == 0)
                            {
                                LastMonthBalance = 0;
                            }


                            linenInventoryDetailsObj.NewPurchases = item.NewPurchases;
                            linenInventoryDetailsObj.firstFloorStorage = item.firstFloorStorage;
                            linenInventoryDetailsObj.secondFloorStorage = item.secondFloorStorage;
                            linenInventoryDetailsObj.thirdFloorStorage = item.thirdFloorStorage;
                            linenInventoryDetailsObj.fourthFloorStorage = item.fourthFloorStorage;
                            linenInventoryDetailsObj.fifthFloorStorage = item.fifthFloorStorage;
                            linenInventoryDetailsObj.InRooms = item.InRooms;
                            linenInventoryDetailsObj.Dirty = item.Dirty; 
                            linenInventoryDetailsObj.EndingBalanceOfMonth = (item.fifthFloorStorage + item.secondFloorStorage + item.thirdFloorStorage + item.fourthFloorStorage + item.fifthFloorStorage + item.InRooms + item.Dirty);
                            linenInventoryDetailsObj.MonthlyTotalLoss = (LastMonthBalance + item.NewPurchases) - item.EndingBalanceOfMonth;
                            linenInventoryDetailsObj.QuantityTobeOrdered = (item.OneTurnForAllRooms * item.RequiredTurns) - (item.fifthFloorStorage + item.secondFloorStorage + item.thirdFloorStorage + item.fourthFloorStorage + item.fifthFloorStorage + item.InRooms + item.Dirty);
                            linenInventoryDetailsObj.LinenItemId = context.LinenItems.Where(a => a.Name == item.LinenItemName && a.HotelId == model.HotelId).Select(a => a.Id).FirstOrDefault();

                                linenInventoryDetailsObj.LinenInventoryId = linenInventoryObj.Id;

                            //};
                            context.LinenInventoryDetails.Add(linenInventoryDetailsObj);
                            await context.SaveChangesAsync();
                        }
                    }   
                }



            }
            else
            {
                foreach (var item in model.LinenInventoryDetails)
                {

                   
                        var linenInventoryDetailsObj = new LinenInventoryDetails()
                        {

                            OneTurnForAllRooms = item.OneTurnForAllRooms,
                            RequiredTurns = item.RequiredTurns,

                            RequiredPar = item.OneTurnForAllRooms * item.RequiredTurns,

                            LastMonthEndingBalance = 0,

                            NewPurchases = item.NewPurchases,
                            firstFloorStorage = item.firstFloorStorage,
                            secondFloorStorage = item.secondFloorStorage,
                            thirdFloorStorage = item.thirdFloorStorage,
                            fourthFloorStorage = item.fourthFloorStorage,
                            fifthFloorStorage = item.fifthFloorStorage,
                            InRooms = item.InRooms,
                            Dirty = item.Dirty,

                            EndingBalanceOfMonth = (item.fifthFloorStorage + item.secondFloorStorage + item.thirdFloorStorage + item.fourthFloorStorage + item.fifthFloorStorage + item.InRooms + item.Dirty),
                            MonthlyTotalLoss = (item.LastMonthEndingBalance + item.NewPurchases) - item.EndingBalanceOfMonth,
                            QuantityTobeOrdered = (item.OneTurnForAllRooms * item.RequiredTurns) - (item.fifthFloorStorage + item.secondFloorStorage + item.thirdFloorStorage + item.fourthFloorStorage + item.fifthFloorStorage + item.InRooms + item.Dirty),

                            LinenItemId = context.LinenItems.Where(a => a.Name == item.LinenItemName && a.HotelId == model.HotelId).Select(a => a.Id).FirstOrDefault(),

                            LinenInventoryId = linenInventoryObj.Id

                        };
                        context.LinenInventoryDetails.Add(linenInventoryDetailsObj);
                        await context.SaveChangesAsync();
                    
                }
            }


            Transaction.Commit();

            logger.LogInformation("Completed adding LinenInventory");
        }
        

        public async Task UpdateLinenInventory(LinenInventoryViewModel model)
        {
            logger.LogInformation("Started updating LinenInventory");

            var Transaction = context.Database.BeginTransaction();

            var linenInventory = context.LinenInventories.AsNoTracking()
                                  .Where(a => a.Id == model.Id)
                                  .FirstOrDefault();

            if (linenInventory == null) { return; }

            linenInventory.ReportMonthYear = model.ReportMonthYear;

            //linenInventory.HotelId = model.HotelId;
            //linenInventory.CreatedBy = model.CreatedBy;

            context.Entry(linenInventory).State = EntityState.Modified;

            await context.SaveChangesAsync();



            //------------------------------------------------------------------------

            var LastMonthReportObj = new LinenInventory();
            var sundriesShopInventoryDetails = new List<LinenInventoryDetails>();

            if (model.ReportMonthYear.Month != 1)
            {
                LastMonthReportObj = context.LinenInventories.Where(a => a.ReportMonthYear.Month == model.ReportMonthYear.Month - 1 && a.HotelId == model.HotelId).FirstOrDefault();
            }
            else
            {
                LastMonthReportObj = context.LinenInventories.Where(a => a.ReportMonthYear.Month == 12 && a.ReportMonthYear.Year == a.ReportMonthYear.Year - 1 && a.HotelId == model.HotelId).FirstOrDefault();
            }

            if (LastMonthReportObj != null)
            {
                //sundriesShopInventoryDetails = LastMonthReportObj.sundriesShopInventoryDetails.ToList();
                sundriesShopInventoryDetails = context.LinenInventoryDetails.Where(a => a.LinenInventoryId == LastMonthReportObj.Id).ToList();
            }


            if (LastMonthReportObj != null && sundriesShopInventoryDetails != null)
            {
                foreach (var lastMonthitem in sundriesShopInventoryDetails)
                {
                    foreach (var item in model.LinenInventoryDetails.ToList())
                    {

                        if (item.OneTurnForAllRooms != 0 && item.RequiredTurns != 0 && item.NewPurchases != 0)
                        {
                            if (item.Id != 0)
                            {
                                if (context.LinenItems.Where(a => a.Name == item.LinenItemName && a.HotelId == model.HotelId).Select(a => a.Id).FirstOrDefault()
                                == lastMonthitem.LinenItemId
                                )
                                {


                                    var linenInventoryDetailsObj = context.LinenInventoryDetails.AsNoTracking()
                                                                          .Where(a => a.Id == item.Id)
                                                                          .FirstOrDefault();

                                    linenInventoryDetailsObj.OneTurnForAllRooms = item.OneTurnForAllRooms;
                                    linenInventoryDetailsObj.RequiredTurns = item.RequiredTurns;
                                    linenInventoryDetailsObj.RequiredPar = item.OneTurnForAllRooms * item.RequiredTurns;
                                    linenInventoryDetailsObj.LastMonthEndingBalance = lastMonthitem.EndingBalanceOfMonth;


                                    var LastMonthBalance = lastMonthitem.EndingBalanceOfMonth;

                                    if (item.OneTurnForAllRooms == 0 && item.RequiredTurns == 0 && item.NewPurchases == 0)
                                    {
                                        LastMonthBalance = 0;
                                    }


                                    linenInventoryDetailsObj.NewPurchases = item.NewPurchases;
                                    linenInventoryDetailsObj.firstFloorStorage = item.firstFloorStorage;
                                    linenInventoryDetailsObj.secondFloorStorage = item.secondFloorStorage;
                                    linenInventoryDetailsObj.thirdFloorStorage = item.thirdFloorStorage;
                                    linenInventoryDetailsObj.fourthFloorStorage = item.fourthFloorStorage;
                                    linenInventoryDetailsObj.fifthFloorStorage = item.fifthFloorStorage;
                                    linenInventoryDetailsObj.InRooms = item.InRooms;
                                    linenInventoryDetailsObj.Dirty = item.Dirty;
                                    linenInventoryDetailsObj.EndingBalanceOfMonth = (item.fifthFloorStorage + item.secondFloorStorage + item.thirdFloorStorage + item.fourthFloorStorage + item.fifthFloorStorage + item.InRooms + item.Dirty);
                                    linenInventoryDetailsObj.MonthlyTotalLoss = (LastMonthBalance + item.NewPurchases) - item.EndingBalanceOfMonth;
                                    linenInventoryDetailsObj.QuantityTobeOrdered = (item.OneTurnForAllRooms * item.RequiredTurns) - (item.fifthFloorStorage + item.secondFloorStorage + item.thirdFloorStorage + item.fourthFloorStorage + item.fifthFloorStorage + item.InRooms + item.Dirty);
                                    //linenInventoryDetailsObj.LinenItemId = context.LinenItems.Where(a => a.Name == item.LinenItemName).Select(a => a.Id).FirstOrDefault();






                                    context.Entry(linenInventoryDetailsObj).State = EntityState.Modified;

                                    await context.SaveChangesAsync();

                                    //-----------------------------------------------------------------------------------

                                }
                            }
                            else
                            {

                                var linenInventoryDetailsObj = new LinenInventoryDetails();

                                linenInventoryDetailsObj.OneTurnForAllRooms = item.OneTurnForAllRooms;
                                linenInventoryDetailsObj.RequiredTurns = item.RequiredTurns;
                                linenInventoryDetailsObj.RequiredPar = item.OneTurnForAllRooms * item.RequiredTurns;
                                linenInventoryDetailsObj.LastMonthEndingBalance = lastMonthitem.EndingBalanceOfMonth;


                                var LastMonthBalance = lastMonthitem.EndingBalanceOfMonth;

                                if (item.OneTurnForAllRooms == 0 && item.RequiredTurns == 0 && item.NewPurchases == 0)
                                {
                                    LastMonthBalance = 0;
                                }


                                linenInventoryDetailsObj.NewPurchases = item.NewPurchases;
                                linenInventoryDetailsObj.firstFloorStorage = item.firstFloorStorage;
                                linenInventoryDetailsObj.secondFloorStorage = item.secondFloorStorage;
                                linenInventoryDetailsObj.thirdFloorStorage = item.thirdFloorStorage;
                                linenInventoryDetailsObj.fourthFloorStorage = item.fourthFloorStorage;
                                linenInventoryDetailsObj.fifthFloorStorage = item.fifthFloorStorage;
                                linenInventoryDetailsObj.InRooms = item.InRooms;
                                linenInventoryDetailsObj.Dirty = item.Dirty;
                                linenInventoryDetailsObj.EndingBalanceOfMonth = (item.fifthFloorStorage + item.secondFloorStorage + item.thirdFloorStorage + item.fourthFloorStorage + item.fifthFloorStorage + item.InRooms + item.Dirty);
                                linenInventoryDetailsObj.MonthlyTotalLoss = (LastMonthBalance + item.NewPurchases) - item.EndingBalanceOfMonth;
                                linenInventoryDetailsObj.QuantityTobeOrdered = (item.OneTurnForAllRooms * item.RequiredTurns) - (item.fifthFloorStorage + item.secondFloorStorage + item.thirdFloorStorage + item.fourthFloorStorage + item.fifthFloorStorage + item.InRooms + item.Dirty);

                                linenInventoryDetailsObj.LinenItemId = context.LinenItems.Where(a => a.Name == item.LinenItemName && a.HotelId == model.HotelId).Select(a => a.Id).FirstOrDefault();

                                linenInventoryDetailsObj.LinenInventoryId = linenInventory.Id;





                                context.LinenInventoryDetails.Add(linenInventoryDetailsObj);

                                await context.SaveChangesAsync();

                                model.LinenInventoryDetails.Remove(item);

                            }
                        }

                    }
                }
            }
            else
            {
                foreach (var item in model.LinenInventoryDetails)
                {
                    if (item.OneTurnForAllRooms != 0 && item.RequiredTurns != 0 && item.NewPurchases != 0)
                    {


                        if (item.Id != 0)
                        {
                            var linenInventoryDetailsObj  = context.LinenInventoryDetails.AsNoTracking()
                                                  .Where(a => a.Id == item.Id)
                                                  .FirstOrDefault();

                            linenInventoryDetailsObj.OneTurnForAllRooms = item.OneTurnForAllRooms;
                            linenInventoryDetailsObj.RequiredTurns = item.RequiredTurns;
                            linenInventoryDetailsObj.RequiredPar = item.OneTurnForAllRooms * item.RequiredTurns;
                            linenInventoryDetailsObj.LastMonthEndingBalance = 0;





                            linenInventoryDetailsObj.NewPurchases = item.NewPurchases;
                            linenInventoryDetailsObj.firstFloorStorage = item.firstFloorStorage;
                            linenInventoryDetailsObj.secondFloorStorage = item.secondFloorStorage;
                            linenInventoryDetailsObj.thirdFloorStorage = item.thirdFloorStorage;
                            linenInventoryDetailsObj.fourthFloorStorage = item.fourthFloorStorage;
                            linenInventoryDetailsObj.fifthFloorStorage = item.fifthFloorStorage;
                            linenInventoryDetailsObj.InRooms = item.InRooms;
                            linenInventoryDetailsObj.Dirty = item.Dirty;
                            linenInventoryDetailsObj.EndingBalanceOfMonth = (item.fifthFloorStorage + item.secondFloorStorage + item.thirdFloorStorage + item.fourthFloorStorage + item.fifthFloorStorage + item.InRooms + item.Dirty);
                            linenInventoryDetailsObj.MonthlyTotalLoss = (0 + item.NewPurchases) - item.EndingBalanceOfMonth;
                            linenInventoryDetailsObj.QuantityTobeOrdered = (item.OneTurnForAllRooms * item.RequiredTurns) - (item.fifthFloorStorage + item.secondFloorStorage + item.thirdFloorStorage + item.fourthFloorStorage + item.fifthFloorStorage + item.InRooms + item.Dirty);
                            //linenInventoryDetailsObj.LinenItemId = context.LinenItems.Where(a => a.Name == item.LinenItemName).Select(a => a.Id).FirstOrDefault();





                            context.Entry(linenInventoryDetailsObj).State = EntityState.Modified;

                            await context.SaveChangesAsync();
                        }
                        else
                        {

                            var linenInventoryDetailsObj = new LinenInventoryDetails()
                            {
                                //Id = item.Id,
                                OneTurnForAllRooms = item.OneTurnForAllRooms,
                                RequiredTurns = item.RequiredTurns,
                                RequiredPar = item.OneTurnForAllRooms * item.RequiredTurns,
                                LastMonthEndingBalance = 0,


                                NewPurchases = item.NewPurchases,   
                                firstFloorStorage = item.firstFloorStorage,
                                secondFloorStorage = item.secondFloorStorage,
                                thirdFloorStorage = item.thirdFloorStorage,
                                fourthFloorStorage = item.fourthFloorStorage,
                                fifthFloorStorage = item.fifthFloorStorage,
                                InRooms = item.InRooms,
                                Dirty = item.Dirty,
                                EndingBalanceOfMonth = (item.fifthFloorStorage + item.secondFloorStorage + item.thirdFloorStorage + item.fourthFloorStorage + item.fifthFloorStorage + item.InRooms + item.Dirty),
                                MonthlyTotalLoss = (0 + item.NewPurchases) - item.EndingBalanceOfMonth,
                                QuantityTobeOrdered = (item.OneTurnForAllRooms * item.RequiredTurns) - (item.fifthFloorStorage + item.secondFloorStorage + item.thirdFloorStorage + item.fourthFloorStorage + item.fifthFloorStorage + item.InRooms + item.Dirty),

                                LinenItemId = context.LinenItems.Where(a => a.Name == item.LinenItemName && a.HotelId == model.HotelId).Select(a => a.Id).FirstOrDefault(),
                                LinenInventoryId = linenInventory.Id,


                        };

                            context.LinenInventoryDetails.Add(linenInventoryDetailsObj);

                            await context.SaveChangesAsync();

                        }

                    }

                }

            }
            //-------------------------------------------------------------------------
            
            Transaction.Commit();

            logger.LogInformation("Completed updating LinenInventory");
        }

        public async Task DeleteLinenInventory(int id)
        {
            var LinenInventory = context.LinenInventories.AsNoTracking().FirstOrDefault(x => x.Id == id);


            //LinenInventory.UpdatedStatus = (short)UpdStatus.Deleted;

            //context.Entry(LinenInventory).State = EntityState.Modified;

            context.Entry(LinenInventory).State = EntityState.Deleted;

            await context.SaveChangesAsync();


        }

        public bool ReportAlreadyExistOfCurrentMonth(LinenInventoryViewModel model)
        {
            //context.Departments.Include(a => a.Id).
            var LinenInventoryObjOfSameMonth =  new LinenInventory();

            //if(model.Id != 0)
            //{
                LinenInventoryObjOfSameMonth = context
                  .LinenInventories
                  .Where(a => a.ReportMonthYear.Month == model.ReportMonthYear.Month
                         && a.ReportMonthYear.Year == model.ReportMonthYear.Year && a.HotelId == model.HotelId && a.Id != model.Id)
                  .FirstOrDefault();

            //}
            //else { 
            //LinenInventoryObjOfSameMonth = context
            //      .LinenInventories
            //      .Where(a => a.ReportMonthYear.Month == model.ReportMonthYear.Month
            //             && a.ReportMonthYear.Year == model.ReportMonthYear.Year && a.HotelId == model.HotelId)
            //      .FirstOrDefault();
            //}


            if (LinenInventoryObjOfSameMonth == null)
            {
                return false;
            }
            return true;
        }



    }
}
