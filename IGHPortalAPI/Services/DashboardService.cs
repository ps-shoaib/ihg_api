using AutoMapper;
using IGHportalAPI.DataContext;
using IGHportalAPI.Services;
using IGHportalAPI.Services.Extensions;
using IGHportalAPI.ViewModels.DashboardViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IGHportalAPI.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly ILogger<DashboardService> logger;
        private readonly DataContext_ context;
        private readonly IMapper mapper;

        public DashboardService(ILogger<DashboardService> logger, DataContext_ context, IMapper mapper)
        {
            this.logger = logger;
            this.context = context;
            this.mapper = mapper;
        }

        public DashboardViewModel GetDashboardData(int hotelId)
        {
            var model = new DashboardViewModel();

            //           public int TotalRoomsCleaned { get; set; }
            //public int TotalRoomsSold { get; set; }
            //public int NumberOfEmployees { get; set; }

            //public int TotalSundriesShopProductsSales { get; set; }

            //public int ExpiredSundriesShopProducts { get; set; }
            //public int TotalLinenInventoriesToBOrdered { get; set; }

            model.TotalRoomsSold = context.PayrollReports.Where(a => a.HotelId == hotelId).Select(a => a.TotalRoomSold).Sum();
            model.TotalRoomsCleaned = context.PayrollReports.Where(a => a.HotelId == hotelId).Select(a => a.TotalRoomsCleaned).Sum();
            model.NumberOfEmployees = context.Employees.Count();

            var SundriesProductsList = context.SundriesShopInventories.Where(a => a.HotelId == hotelId).ToList();

            foreach (var item in SundriesProductsList)
            {
                model.TotalSundriesShopProductsSales
                     += context.SundriesShopInventoryDetails.Where(a => a.SundriesShopInventoryId == item.Id).Select(a => a.ProductSales).Sum();

                model.ExpiredSundriesShopProducts
                    += context.SundriesShopInventoryDetails.Where(a => a.SundriesShopInventoryId == item.Id).Select(a => a.ExpiredLogged).Sum();

            }

            var LinenInventoryList = context.LinenInventories.Where(a => a.HotelId == hotelId).ToList();

            foreach (var item in LinenInventoryList)
            {
                model.TotalLinenInventoriesToBOrdered
                     += context.LinenInventoryDetails.Where(a => a.LinenInventoryId == item.Id).Select(a => a.QuantityTobeOrdered).Sum();
            }



            //    //---------------LastThirtyDays -----------
            //    var endDate = DateTime.Now.Day;

            //    var startDate = DateTime.Today.AddMonths(-1);


            //    for (var i = startDate; i < DateTime.Now; i = i.AddDays(1))
            //    {
            //        model.LastThirtyDays.Add(i.Day);
            //    }

            //    if (model.LastThirtyDays.Last() == DateTime.Now.Day)
            //    {
            //        model.LastThirtyDays.Remove(model.LastThirtyDays.Last());
            //    }
            //    //--------------------------
            //    //--------------------------
            //    //------------------LastThirty Days Member Login Count By date---------


            //    var ListTotalMembersLoginByDay = context.Members.
            //     Where(a => a.CreatedOn >= startDate && a.CreatedOn < DateTime.Now) // If filter needed
            //    .GroupBy(a => a.CreatedOn, (date, values) =>
            //    new
            //    {
            //        Date = date,
            //        Total = values.Count()
            //    })
            //    .OrderBy(summary => summary.Date).ToList();


            //    var LastThirtyDays = model.LastThirtyDays;

            //    var ThirtyDayEmptyList = new List<int>();

            //    for (int i = 0; i < LastThirtyDays.Count; i++)
            //    {
            //        ThirtyDayEmptyList.Add(0);
            //    }

            //    foreach (var TotalsbyDay in ListTotalMembersLoginByDay)
            //    {
            //        foreach (var day in LastThirtyDays)
            //        {
            //            if (day == TotalsbyDay.Date.Day)
            //            {
            //                ThirtyDayEmptyList.Insert(LastThirtyDays.IndexOf(day), TotalsbyDay.Total);
            //                ThirtyDayEmptyList.RemoveAt(ThirtyDayEmptyList.Count - 1);
            //            }
            //        }

            //    }


            //    model.LastThirtyDaysMembersCount = ThirtyDayEmptyList;

            //    //--------------------------
            //    //--------------------------

            var currentMonth = DateTime.Now;

            var LastMonthNumber = DateTime.Now.AddMonths(-1);

            var SecondLastMonthNumber = DateTime.Now.AddMonths(-2);
            var ThirdLastMonthNumber = DateTime.Now.AddMonths(-3);
            var FourthLastMonthNumber = DateTime.Now.AddMonths(-4);




            var arrCurrentMonthWeeks = new List<WeekInfo>();
            var arrLastMonthWeeks = new List<WeekInfo>();
            var arrSecondLastMonthWeeks = new List<WeekInfo>();
            var arrThirdLastMonthWeeks = new List<WeekInfo>();
            var arrFourthLastMonthWeeks = new List<WeekInfo>();


            DateTime dtCurrent_finish = new DateTime();
            DateTime dt_finish = new DateTime();
            DateTime socondDt_finish = new DateTime();
            DateTime thirdDt_finish = new DateTime();
            DateTime fourthDt_finish = new DateTime();



            for (int i = 1; i <= 31; i = i + 14)
            {
                string sCurrentStartDate = currentMonth.Month.ToString() + "/" + i.ToString() + "/" + currentMonth.Year.ToString();

                if (!(sCurrentStartDate.Equals($"2/29/{currentMonth.Year}") || sCurrentStartDate.Equals($"2/30/{currentMonth.Year}")
                    //|| sCurrentStartDate.Equals($"2/31/{currentMonth.Year}")
                    //  || sCurrentStartDate.Equals($"4/31/{currentMonth.Year}") || sCurrentStartDate.Equals($"6/31/{currentMonth.Year}")
                    //  || sCurrentStartDate.Equals($"9/31/{currentMonth.Year}") || sCurrentStartDate.Equals($"11/31/{currentMonth.Year}")
                    ))
                {
                    DateTime dt_start = Convert.ToDateTime(sCurrentStartDate).FirstDayOfWeek();

                    //if (dt_start.Month == (dt_start.AddDays(6)).Month)
                    dtCurrent_finish = dt_start.AddDays(6);

                    //else
                    //    dt_finish = new DateTime(dt_start.Year, dt_start.Month, countDays);

                    WeekInfo WeekInfoObj = new WeekInfo();
                    WeekInfoObj._StartDay = dt_start;

                    WeekInfoObj._EndDay = dt_finish.ToShortDateString();

                    arrCurrentMonthWeeks.Add(WeekInfoObj);
                }

                //-------------------------------------------------------------------------

                string sStartDate =  LastMonthNumber.Month.ToString() + "/" + i.ToString() + "/" + LastMonthNumber.Year.ToString();

                if (!(sStartDate.Equals($"2/29/{LastMonthNumber.Year}") || sStartDate.Equals($"2/30/{LastMonthNumber.Year}")
                    //|| sStartDate.Equals($"2/31/{currentMonth.Year}")
                    //|| sStartDate.Equals($"4/31/{currentMonth.Year}") || sStartDate.Equals($"6/31/{currentMonth.Year}")
                    //  || sStartDate.Equals($"9/31/{currentMonth.Year}") || sStartDate.Equals($"11/31/{currentMonth.Year}")
                    ))
                {
                    DateTime dt_start = Convert.ToDateTime(sStartDate).FirstDayOfWeek();

                    //if (dt_start.Month == (dt_start.AddDays(6)).Month)
                    dt_finish = dt_start.AddDays(6);

                    //else
                    //    dt_finish = new DateTime(dt_start.Year, dt_start.Month, countDays);

                    WeekInfo WeekInfoObj1 = new WeekInfo();
                    WeekInfoObj1._StartDay = dt_start;

                    WeekInfoObj1._EndDay = dt_finish.ToShortDateString();

                    arrLastMonthWeeks.Add(WeekInfoObj1);
                }




                string socondStartDate = SecondLastMonthNumber.Month.ToString() + "/" + i.ToString() + "/" + SecondLastMonthNumber.Year.ToString();

                if (!(socondStartDate.Equals($"2/29/{SecondLastMonthNumber.Year}") || socondStartDate.Equals($"2/30/{SecondLastMonthNumber.Year}")
                     //|| socondStartDate.Equals($"2/31/{currentMonth.Year}")
                     //|| socondStartDate.Equals($"4/31/{currentMonth.Year}") || socondStartDate.Equals($"6/31/{currentMonth.Year}")
                     // || socondStartDate.Equals($"9/31/{currentMonth.Year}") || socondStartDate.Equals($"11/31/{currentMonth.Year}")
                    ))
                {

                    DateTime socondDt_start = Convert.ToDateTime(socondStartDate).FirstDayOfWeek();

                    //if (socondDt_start.Month == (socondDt_start.AddDays(6)).Month)
                    socondDt_finish = socondDt_start.AddDays(6);

                    //else
                    //    socondDt_finish = new DateTime(socondDt_start.Year, socondDt_start.Month, countDays);

                    WeekInfo WeekInfoObj2 = new WeekInfo();
                    WeekInfoObj2._StartDay = socondDt_start;
                    WeekInfoObj2._EndDay = socondDt_finish.ToShortDateString();

                    arrSecondLastMonthWeeks.Add(WeekInfoObj2);

                }



                string thirdStartDate = ThirdLastMonthNumber.Month.ToString() + "/" + i.ToString() + "/" + ThirdLastMonthNumber.Year.ToString();

                if (!(thirdStartDate.Equals($"2/29/{ThirdLastMonthNumber.Year}") || thirdStartDate.Equals($"2/30/{ThirdLastMonthNumber.Year}")
                      //|| thirdStartDate.Equals($"2/31/{currentMonth.Year}")
                      //|| thirdStartDate.Equals($"4/31/{currentMonth.Year}") || thirdStartDate.Equals($"6/31/{currentMonth.Year}")
                      //|| thirdStartDate.Equals($"9/31/{currentMonth.Year}") || thirdStartDate.Equals($"11/31/{currentMonth.Year}")
                    ))
                {

                    DateTime thirdDt_start = Convert.ToDateTime(thirdStartDate).FirstDayOfWeek();

                    //if (socondDt_start.Month == (socondDt_start.AddDays(6)).Month)
                    thirdDt_finish = thirdDt_start.AddDays(6);

                    //else
                    //    socondDt_finish = new DateTime(socondDt_start.Year, socondDt_start.Month, countDays);

                    WeekInfo WeekInfoObj3 = new WeekInfo();
                    WeekInfoObj3._StartDay = thirdDt_start;
                    WeekInfoObj3._EndDay = thirdDt_finish.ToShortDateString();

                    arrThirdLastMonthWeeks.Add(WeekInfoObj3);

                }


                string fourthStartDate = FourthLastMonthNumber.Month.ToString() + "/" + i.ToString() + "/" + FourthLastMonthNumber.Year.ToString();

                if (!(fourthStartDate.Equals($"2/29/{FourthLastMonthNumber.Year}") || fourthStartDate.Equals($"2/30/{FourthLastMonthNumber.Year}")
                    //|| fourthStartDate.Equals($"2/31/{currentMonth.Year}")
                    //|| fourthStartDate.Equals($"4/31/{currentMonth.Year}") || fourthStartDate.Equals($"6/31/{currentMonth.Year}")
                    //  || fourthStartDate.Equals($"9/31/{currentMonth.Year}") || fourthStartDate.Equals($"11/31/{currentMonth.Year}")
                    ))
                {

                    DateTime fourthDt_start = Convert.ToDateTime(fourthStartDate).FirstDayOfWeek();

                    //if (socondDt_start.Month == (socondDt_start.AddDays(6)).Month)
                    fourthDt_finish = fourthDt_start.AddDays(6);

                    //else
                    //    socondDt_finish = new DateTime(socondDt_start.Year, socondDt_start.Month, countDays);

                    WeekInfo WeekInfoObj4 = new WeekInfo();
                    WeekInfoObj4._StartDay = fourthDt_start;
                    WeekInfoObj4._EndDay = fourthDt_finish.ToShortDateString();

                    arrFourthLastMonthWeeks.Add(WeekInfoObj4);
                }
            }



            foreach (var item in arrCurrentMonthWeeks)
            {
                model.FourMonthsWeeksArray.Add(item._StartDay);
            }

            foreach (var item in arrLastMonthWeeks)
            {
                if (!model.FourMonthsWeeksArray.Contains(item._StartDay))
                {
                    model.FourMonthsWeeksArray.Add(item._StartDay);
                }
            }

            foreach (var item in arrSecondLastMonthWeeks)
            {
                if (!model.FourMonthsWeeksArray.Contains(item._StartDay))
                {
                    model.FourMonthsWeeksArray.Add(item._StartDay);
                }
            }

            foreach (var item in arrThirdLastMonthWeeks)
            {
                if (!model.FourMonthsWeeksArray.Contains(item._StartDay))
                {
                    model.FourMonthsWeeksArray.Add(item._StartDay);
                }
            }

            foreach (var item in arrFourthLastMonthWeeks)
            {
                if (!model.FourMonthsWeeksArray.Contains(item._StartDay))
                {
                    model.FourMonthsWeeksArray.Add(item._StartDay);
                }
            }

            model.FourMonthsWeeksArray = model.FourMonthsWeeksArray.OrderByDescending(a => a).Take(7).ToList();

            var ListOfPayrolls = context.PayrollReports.Where(a => a.HotelId == hotelId).ToList();

            var ListOfMPORByDate = new Dictionary<DateTime, double>();
           
            var ListOfTotalPayrollByDate = new Dictionary<DateTime, double>();

            foreach (var item in ListOfPayrolls)
            {
                var ListOfPayrollDetails = context.PayrollReportsDetails.Where(a => a.PayrollReportId == item.Id).ToList();

                var TotalHoursWorked = new List<float>();

                var TotalPayrolls = new List<float>();

                foreach (var Detail in ListOfPayrollDetails)
                {
                    TotalHoursWorked.Add(Detail.TotalHoursWorked);
                    TotalHoursWorked.Add(Detail.TotalOverTimeHoursWorked);

                    TotalPayrolls.Add((Detail.HourlyRate * Detail.TotalHoursWorked) + (Detail.OvertimeHourlyRate * Detail.TotalOverTimeHoursWorked));
                }

                ListOfMPORByDate.Add(item.ReportFrom, Math.Round(TotalHoursWorked.Sum() * 60 / item.TotalRoomsCleaned,2));
                ListOfTotalPayrollByDate.Add(item.ReportFrom, Math.Round(TotalPayrolls.Sum(),3));

            }

            var LastSevenWeeksPayrollsList  = new List<double>();
            for (int i = 0; i < 7; i++)
            {
                LastSevenWeeksPayrollsList.Add(0);
            }

            foreach (var item in ListOfTotalPayrollByDate)
            {
                foreach (var date in model.FourMonthsWeeksArray)
                {
                    if(item.Key == date)
                    {
                        LastSevenWeeksPayrollsList.Insert(model.FourMonthsWeeksArray.IndexOf(date),item.Value);

                        LastSevenWeeksPayrollsList.RemoveAt(model.FourMonthsWeeksArray.IndexOf(date)+1);

                    }

                }
            }

            model.LastSevenWeeksTotalPayrollsList  = LastSevenWeeksPayrollsList.Take(7).Reverse().ToList();
         

            //-----------------------------------------------------------------

            var LastSevenWeeksMPORList = new List<double>();
            for (int i = 0; i < 7; i++)
            {
                LastSevenWeeksMPORList.Add(0);
            }

            foreach (var item in ListOfMPORByDate)
            {
                foreach (var date in model.FourMonthsWeeksArray)
                {
                    if (item.Key == date)
                    {
                        LastSevenWeeksMPORList.Insert(model.FourMonthsWeeksArray.IndexOf(date), item.Value);

                        LastSevenWeeksMPORList.RemoveAt(model.FourMonthsWeeksArray.IndexOf(date) + 1);
                       
                    }

                }
            }

            model.LastSevenWeeksMPORList = LastSevenWeeksMPORList.Take(7).Reverse().ToList();

            //model.LastSevenWeeksMPORList.RemoveAt(model.LastSevenWeeksMPORList.Count - 1);
            

            model.FourMonthsWeeksArray = model.FourMonthsWeeksArray.OrderBy(a => a).ToList();
            

            return model;
            }

        }
    
internal class WeekInfo
{
    public DateTime _StartDay { get; set; }
    public string _EndDay { get; set; }

}

}


