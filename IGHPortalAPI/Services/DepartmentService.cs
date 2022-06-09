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
using IGHportalAPI.ViewModels.DepartmentViewModels;
using IGHportalAPI.Models;

namespace IGHportalAPI.Services
{
    public class DepartmentService  : IDepartmentService
    {
        private readonly ILogger<DepartmentService> logger;
        private readonly DataContext_ context;
        private readonly IMapper mapper;

        public DepartmentService(ILogger<DepartmentService> logger, DataContext_ context, IMapper mapper)
        {
            this.logger = logger;
            this.context = context;
            this.mapper = mapper;
        }                     

        public List<DepartmentViewModel> GetDepartments(int hotelId)
        {
            var maxUpdStatus = UpdStatus.Deleted;

            return mapper.Map<List<DepartmentViewModel>>
                (context.Departments.Where(a => a.UpdatedStatus < (short)maxUpdStatus && a.HotelId == hotelId).OrderBy(a => a.Name).ToList());
        }

        public DepartmentViewModel GetDepartment(int id)
        {
            var maxUpdStatus = UpdStatus.Deleted;

            var Department = context.Departments.Where(a => a.UpdatedStatus < (short)maxUpdStatus).FirstOrDefault(x => x.Id == id);

            if (Department == null)
            {
                return null;
            }
            return mapper.Map<DepartmentViewModel>(Department);
        }

        public bool SystemNameAlreadyExist(DepartmentViewModel model)
        {
            //context.Departments.Include(a => a.Id).
            var maxUpdStatus = UpdStatus.Deleted;

            var SystemName = 
                context
                .Departments
                .Where(a => a.Name == model.Name 
                      && a.HotelId == model.HotelId 
                      && a.UpdatedStatus < (short)maxUpdStatus 
                      && a.Id != model.Id)
                .FirstOrDefault();

            if (SystemName == null)
            {
                return false;
            }
            return true;
        }

        public async Task AddDepartment(DepartmentViewModel model)
        {
            logger.LogInformation("Started adding Department");
            
            var MappedObj = mapper.Map<Department>(model);
            MappedObj.CreatedOn = DateTime.UtcNow;
            

            MappedObj.UpdatedStatus = (short)UpdStatus.Created;

            context.Departments.Add(
                MappedObj
            );
            

            await context.SaveChangesAsync();


            logger.LogInformation("Completed adding Department");
        }
        

        public async Task UpdateDepartment(DepartmentViewModel model)
        {
            logger.LogInformation("Started updating Department");

            var Department = context.Departments.AsNoTracking().FirstOrDefault(x => x.Id == model.Id);

            var Mappedsystem = new Department();
            
            if (Department == null)
            {
                return;
            }
            //------------------------------------------------------------
            //------------------------------------------------------------

            //------------------------------------------------------------
            //------------------------------------------------------------
            Mappedsystem = mapper.Map<Department>(model);
            Mappedsystem.Id = Department.Id;
            Mappedsystem.UpdatedOn = DateTime.UtcNow;
                

            Mappedsystem.CreatedOn = Department.CreatedOn;


            Mappedsystem.UpdatedStatus = (short)UpdStatus.Updated;

            context.Entry(Mappedsystem).State = EntityState.Modified;

            await context.SaveChangesAsync();

            logger.LogInformation("Completed updating Department");
        }

        public async Task DeleteDepartment(int id)
        {
            var Department = context.Departments.AsNoTracking().FirstOrDefault(x => x.Id == id);


            Department.UpdatedStatus = (short)UpdStatus.Deleted;

            context.Entry(Department).State = EntityState.Modified;

            await context.SaveChangesAsync();


        }


    }
}
