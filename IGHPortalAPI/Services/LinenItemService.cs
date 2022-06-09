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
using IGHportalAPI.ViewModels.LinenItemViewModels;
using IGHportalAPI.Models;

namespace IGHportalAPI.Services
{
    public class LinenItemService  : ILinenItemService
    {
        private readonly ILogger<LinenItemService> logger;
        private readonly DataContext_ context;
        private readonly IMapper mapper;

        public LinenItemService(ILogger<LinenItemService> logger, DataContext_ context, IMapper mapper)
        {
            this.logger = logger;
            this.context = context;
            this.mapper = mapper;
        }                     

        public List<LinenItemViewModel> GetLinenItems(int hotelId)
        {
            return mapper.Map<List<LinenItemViewModel>>(context.LinenItems.Where(a => a.HotelId == hotelId).ToList());
        }

        public LinenItemViewModel GetLinenItem(int id)
        {
            //var maxUpdStatus = UpdStatus.Deleted;

            var LinenItem = context.LinenItems.FirstOrDefault(x => x.Id == id);

            if (LinenItem == null)
            {
                return null;
            }
            return mapper.Map<LinenItemViewModel>(LinenItem);
        }

        public bool SystemNameAlreadyExist(LinenItemViewModel model)
        {
            var SystemName = context.LinenItems
                .Where(a => a.Name == model.Name && a.HotelId == model.HotelId && a.Id != model.Id)
                .FirstOrDefault();

            if (SystemName == null)
            {
                return false;
            }
            return true;
        }

        public async Task AddLinenItem(LinenItemViewModel model)
        {
            logger.LogInformation("Started adding LinenItem");
            
            var MappedObj = mapper.Map<LinenItem>(model);
            //MappedObj.CreatedOn = DateTime.UtcNow;
            

            //MappedObj.UpdatedStatus = (short)UpdStatus.Created;

            context.LinenItems.Add(
                MappedObj
            );
            

            await context.SaveChangesAsync();


            logger.LogInformation("Completed adding LinenItem");
        }
        

        public async Task UpdateLinenItem(LinenItemViewModel model)
        {
            logger.LogInformation("Started updating LinenItem");

            var LinenItem = context.LinenItems.AsNoTracking().FirstOrDefault(x => x.Id == model.Id);

            var Mappedsystem = new LinenItem();
            
            if (LinenItem == null)
            {
                return;
            }
            //------------------------------------------------------------
            //------------------------------------------------------------

            //------------------------------------------------------------
            //------------------------------------------------------------
            Mappedsystem = mapper.Map<LinenItem>(model);
            Mappedsystem.Id = LinenItem.Id;
            //Mappedsystem.UpdatedOn = DateTime.UtcNow;
                

            //Mappedsystem.CreatedOn = LinenItem.CreatedOn;


            //Mappedsystem.UpdatedStatus = (short)UpdStatus.Updated;

            context.Entry(Mappedsystem).State = EntityState.Modified;

            await context.SaveChangesAsync();

            logger.LogInformation("Completed updating LinenItem");
        }

        public async Task DeleteLinenItem(int id)
        {
            var LinenItem = context.LinenItems.AsNoTracking().FirstOrDefault(x => x.Id == id);


            //LinenItem.UpdatedStatus = (short)UpdStatus.Deleted;

            //context.Entry(LinenItem).State = EntityState.Modified;
            context.Entry(LinenItem).State = EntityState.Deleted;


            await context.SaveChangesAsync();


        }


    }
}
