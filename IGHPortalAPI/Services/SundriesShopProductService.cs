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
using IGHportalAPI.ViewModels.SundriesShopProductViewModels;
using IGHportalAPI.Models;

namespace IGHportalAPI.Services
{
    public class SundriesShopProductService  : ISundriesShopProductService
    {
        private readonly ILogger<SundriesShopProductService> logger;
        private readonly DataContext_ context;
        private readonly IMapper mapper;

        public SundriesShopProductService(ILogger<SundriesShopProductService> logger, DataContext_ context, IMapper mapper)
        {
            this.logger = logger;
            this.context = context;
            this.mapper = mapper;
        }                     

        public List<SundriesShopProductViewModel> GetSundriesShopProducts(int hotelId)
        {
            return mapper.Map<List<SundriesShopProductViewModel>>(context.SundriesShopProducts.Where(a => a.HotelId == hotelId).ToList());
        }

        public SundriesShopProductViewModel GetSundriesShopProduct(int id)
        {
            //var maxUpdStatus = UpdStatus.Deleted;

            var SundriesShopProduct = context.SundriesShopProducts.FirstOrDefault(x => x.Id == id);

            if (SundriesShopProduct == null)
            {
                return null;
            }
            return mapper.Map<SundriesShopProductViewModel>(SundriesShopProduct);
        }

        public bool SystemNameAlreadyExist(SundriesShopProductViewModel model)
        {
            //context.SundriesShopProducts.Include(a => a.Id).
            var SystemName = context.SundriesShopProducts
                .Where(a => a.Name == model.Name && a.HotelId == model.HotelId && a.Id != model.Id)
                .FirstOrDefault();
            if (SystemName == null)
            {
                return false;
            }
            return true;
        }

        public async Task AddSundriesShopProduct(SundriesShopProductViewModel model)
        {
            logger.LogInformation("Started adding SundriesShopProduct");
            
            var MappedObj = mapper.Map<SundriesShopProduct>(model);
            //MappedObj.CreatedOn = DateTime.UtcNow;
            

            //MappedObj.UpdatedStatus = (short)UpdStatus.Created;

            context.SundriesShopProducts.Add(
                MappedObj
            );
            

            await context.SaveChangesAsync();


            logger.LogInformation("Completed adding SundriesShopProduct");
        }
        

        public async Task UpdateSundriesShopProduct(SundriesShopProductViewModel model)
        {
            logger.LogInformation("Started updating SundriesShopProduct");

            var SundriesShopProduct = context.SundriesShopProducts.AsNoTracking().FirstOrDefault(x => x.Id == model.Id);

            var Mappedsystem = new SundriesShopProduct();
            
            if (SundriesShopProduct == null)
            {
                return;
            }
            //------------------------------------------------------------
            //------------------------------------------------------------

            //------------------------------------------------------------
            //------------------------------------------------------------
            Mappedsystem = mapper.Map<SundriesShopProduct>(model);
            Mappedsystem.Id = SundriesShopProduct.Id;
            //Mappedsystem.UpdatedOn = DateTime.UtcNow;
                

            //Mappedsystem.CreatedOn = SundriesShopProduct.CreatedOn;


            //Mappedsystem.UpdatedStatus = (short)UpdStatus.Updated;

            context.Entry(Mappedsystem).State = EntityState.Modified;

            await context.SaveChangesAsync();

            logger.LogInformation("Completed updating SundriesShopProduct");
        }

        public async Task DeleteSundriesShopProduct(int id)
        {
            var SundriesShopProduct = context.SundriesShopProducts.FirstOrDefault(x => x.Id == id);


            //SundriesShopProduct.UpdatedStatus = (short)UpdStatus.Deleted;

            //context.Entry(SundriesShopProduct).State = EntityState.Modified;
            //context.Entry(SundriesShopProduct).State = EntityState.Deleted;

            context.Remove(SundriesShopProduct);


            await context.SaveChangesAsync();


        }


    }
}
