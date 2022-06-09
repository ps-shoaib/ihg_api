using AutoMapper;
using IGHportalAPI.DataContext;
using IGHportalAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IGHportalAPI.ViewModels.HotelViewModels;
using IGHportalAPI.Models.Enums;
using IGHportalAPI.Models;
using Microsoft.AspNetCore.Identity;
using System.IO;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;

using Microsoft.AspNetCore.Mvc;
using Azure.Core;
//using IGHportalAPI.Services;

namespace IGHportalAPI.Services
{
    public class HotelService  : IHotelService
    {
        private readonly ILogger<HotelService> logger;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<User> userManager;
        private readonly DataContext_ context;
        private readonly IMapper mapper;
        private readonly IWebHostEnvironment _hostEnvironment;


        public HotelService(ILogger<HotelService> logger, DataContext_ context, IMapper mapper, UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IWebHostEnvironment hostEnvironment)
        {
            this.logger = logger;
            this.context = context;
            this.mapper = mapper;
            this.userManager = userManager;
            this.roleManager = roleManager;
            _hostEnvironment = hostEnvironment;
        }


        public List<HotelNamesViewModel> GetHotelsNames()
        {
           return  context.Hotels.Where(a => a.UpdatedStatus < (short) UpdStatus.Deleted).Select(a => new HotelNamesViewModel() { Id = a.Id, Name = a.Name }).ToList();
        }

        public async Task<List<HotelViewModel>> GetHotelsByUserId(string userId, HttpRequest httpRequest)
        {
            //var UserId = userService.GetUserId();


            var   userObj =  context.Users.FirstOrDefault(a => a.Id == userId);
           var UserRoles = await userManager.GetRolesAsync(userObj);

            var maxUpdStatus = UpdStatus.Deleted;

            if (UserRoles.Contains("Admin"))
            {

                //return mapper.Map<List<HotelViewModel>>(context.Hotels.Where(a => a.UpdatedStatus < (short)maxUpdStatus).ToList());

               return  context.Hotels.Where(a => a.UpdatedStatus < (short)maxUpdStatus)
                    .Select(x => new HotelViewModel()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Address = x.Address,
                        ImageURL = String.Format("{0}://{1}{2}/StaticFiles/Images/{3}", httpRequest.Scheme, httpRequest.Host, httpRequest.PathBase, x.ImageURL)
                    }).ToList();

            }
            else
            {
               var HotelsList = context.HotelUsers
                    .Where(a => a.UsersId == userId)
                    .Select(a => a.Hotel)
                    .Where(a => a.UpdatedStatus < (short)maxUpdStatus)
                    .Select(x => new HotelViewModel()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Address = x.Address,
                        ImageURL = String.Format("{0}://{1}{2}/StaticFiles/Images/{3}", httpRequest.Scheme, httpRequest.Host, httpRequest.PathBase, x.ImageURL)
                    })
                    .ToList();

                //var Obj = mapper.Map<List<HotelViewModel>>(HotelsList);
                return HotelsList;

                //return mapper.Map<List<HotelViewModel>>(context.Hotels.Where(a => a.UpdatedStatus < (short)maxUpdStatus).ToList());
            }


        }


        public async Task<List<HotelViewModel>> GetHotels(HttpRequest httpRequest)
        {


            var maxUpdStatus = UpdStatus.Deleted;


            return await context.Hotels.Where(a => a.UpdatedStatus < (short)maxUpdStatus)
               .Select(x => new HotelViewModel()
               {
                   Id = x.Id,
                   Name = x.Name,
                   Address = x.Address,
                   ImageURL = String.Format("{0}://{1}{2}/StaticFiles/Images/{3}", httpRequest.Scheme, httpRequest.Host, httpRequest.PathBase, x.ImageURL)
               })
               .ToListAsync();
        }

        public HotelViewModel GetHotel(int id, HttpRequest httpRequest)
        {
            var maxUpdStatus = UpdStatus.Deleted;

            var Hotel = context.Hotels.Where(a => a.UpdatedStatus < (short)maxUpdStatus)
                .Select(x => new HotelViewModel()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Address = x.Address,
                    ImageURL = String.Format("./StaticFiles/Images/{0}", x.ImageURL)
                }).FirstOrDefault(x => x.Id == id);

            //using (var stream = System.IO.File.OpenRead(Hotel.ImageURL))
            //{
            //    Hotel.file = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name));
            //};










            if (Hotel == null)
            {
                return null;
            }
            //return mapper.Map<HotelViewModel>(Hotel);
            return Hotel;
        }

        public bool SystemNameAlreadyExist(HotelViewModel model)
        {
            //context.Hotels.Include(a => a.Id).
            var SystemName = context.Hotels.Where(a => a.Name == model.Name && a.Id != model.Id).FirstOrDefault();

            if (SystemName == null)
            {
                return false;
            }
            return true;
        }

        public async Task AddHotel(HotelViewModel model)
        {
            logger.LogInformation("Started adding Hotel");

            if (model.file != null)
            {
                DeleteImage(model.ImageURL);
                model.ImageURL = await SaveImage(model.file);
            }   
            
            //------------------------------------------------------------------
            //------------------------------------------------------------------


            var MappedObj = mapper.Map<Hotel>(model);
            
            MappedObj.CreatedOn = DateTime.UtcNow;
            
            MappedObj.UpdatedStatus = (short)UpdStatus.Created;

            //MappedObj.UserId = model.UserId;

            context.Hotels.Add(MappedObj);
            
            await context.SaveChangesAsync();


            

            //HotelUser hotelUser = new HotelUser()
            //{
            //    UsersId = model.UserId,
            //    HotelsId = MappedObj.Id
            //};
            //context.HotelUsers.Add(hotelUser);

            //await context.SaveChangesAsync();

            logger.LogInformation("Completed adding Hotel");
        }


        public async Task<string> SaveImage(IFormFile imageFile)
        {
            string imageName = new String(Path.GetFileNameWithoutExtension(imageFile.FileName).Take(10).ToArray()).Replace(' ', '-');
            imageName = imageName + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(imageFile.FileName);
            var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, "StaticFiles/Images", imageName);
            using (var fileStream = new FileStream(imagePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }
            return imageName;
        }

        public void DeleteImage(string imageName)
        {
            var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, "StaticFiles/Images", imageName);
            if (System.IO.File.Exists(imagePath))
                System.IO.File.Delete(imagePath);
        }

        public async Task UpdateHotel(HotelViewModel model)
        {
            logger.LogInformation("Started updating Hotel");

            var Hotel = context.Hotels.AsNoTracking().FirstOrDefault(x => x.Id == model.Id);

            var Mappedsystem = new Hotel();
            
            if (Hotel == null)
            {
                return;
            }
            //------------------------------------------------------------
            Mappedsystem = mapper.Map<Hotel>(model);

            if (model.file != null)
            {
                var imageURL = model.ImageURL.Replace("./Images/", "");

                DeleteImage(imageURL);

                Mappedsystem.ImageURL = await SaveImage(model.file);
            }
            //------------------------------------------------------------------
            Mappedsystem.Id = Hotel.Id;
            Mappedsystem.UpdatedOn = DateTime.UtcNow;
            Mappedsystem.CreatedOn = Hotel.CreatedOn;

            Mappedsystem.UpdatedStatus = (short)UpdStatus.Updated;

            context.Entry(Mappedsystem).State = EntityState.Modified;

            await context.SaveChangesAsync();

            logger.LogInformation("Completed updating Hotel");
        }

        public async Task DeleteHotel(int id)
        {
            var Hotel = context.Hotels.AsNoTracking().FirstOrDefault(x => x.Id == id);


            Hotel.UpdatedStatus = (short)UpdStatus.Deleted;

            context.Entry(Hotel).State = EntityState.Modified;

            await context.SaveChangesAsync();


        }

    }
}
