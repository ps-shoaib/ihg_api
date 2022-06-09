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
using IGHportalAPI.ViewModels.Scores_IssuesViewModels;
using IGHportalAPI.Models;

namespace IGHportalAPI.Services
{
    public class Scores_IssuesService  : IScores_IssuesService
    {
        private readonly ILogger<Scores_IssuesService> logger;
        private readonly DataContext_ context;
        private readonly IMapper mapper;

        public Scores_IssuesService(ILogger<Scores_IssuesService> logger, DataContext_ context, IMapper mapper)
        {
            this.logger = logger;
            this.context = context;
            this.mapper = mapper;
        }

        //public List<Scores_IssuesViewModel> GetScores_Issues(int hotelId)

        public List<Scores_IssuesViewModel> GetScores_Issues()
        {
            //return mapper.Map<List<Scores_IssuesViewModel>>(context.ScoresIssues.Where(a => a.HotelId == hotelId).ToList());

            return mapper.Map<List<Scores_IssuesViewModel>>(context.ScoresIssues.ToList());
        }

        public Scores_IssuesViewModel GetScores_Issue(int id)
        {
            //var maxUpdStatus = UpdStatus.Deleted;

            var ScoresIssues = context.ScoresIssues.FirstOrDefault(x => x.Id == id);

            if (ScoresIssues == null)
            {
                return null;
            }
            return mapper.Map<Scores_IssuesViewModel>(ScoresIssues);
        }

        public bool SystemNameAlreadyExist(Scores_IssuesViewModel model)
        {
            //var SystemName = context.ScoresIssues
            //    .Where(a => a.Name == model.Name && a.HotelId == model.HotelId && a.Id != model.Id)
            //    .FirstOrDefault();

            var SystemName = context.ScoresIssues
                .Where(a => a.Name == model.Name && a.Id != model.Id)
                .FirstOrDefault();

            if (SystemName == null)
            {
                return false;
            }
            return true;
        }

        public async Task AddScores_Issues(Scores_IssuesViewModel model)
        {
            logger.LogInformation("Started adding ScoresIssues");
            
            var MappedObj = mapper.Map<Scores_Issues>(model);
            //MappedObj.CreatedOn = DateTime.UtcNow;
            

            //MappedObj.UpdatedStatus = (short)UpdStatus.Created;

            context.ScoresIssues.Add(
                MappedObj
            );
            

            await context.SaveChangesAsync();


            logger.LogInformation("Completed adding ScoresIssues");
        }
        

        public async Task UpdateScores_Issue(Scores_IssuesViewModel model)
        {
            logger.LogInformation("Started updating ScoresIssues");

            var ScoresIssues = context.ScoresIssues.AsNoTracking().FirstOrDefault(x => x.Id == model.Id);

            var Mappedsystem = new Scores_Issues();
            
            if (ScoresIssues == null)
            {
                return;
            }
            //------------------------------------------------------------
            //------------------------------------------------------------

            //------------------------------------------------------------
            //------------------------------------------------------------
            Mappedsystem = mapper.Map<Scores_Issues>(model);
            Mappedsystem.Id = ScoresIssues.Id;
            //Mappedsystem.UpdatedOn = DateTime.UtcNow;
                

            //Mappedsystem.CreatedOn = ScoresIssues.CreatedOn;


            //Mappedsystem.UpdatedStatus = (short)UpdStatus.Updated;

            context.Entry(Mappedsystem).State = EntityState.Modified;

            await context.SaveChangesAsync();

            logger.LogInformation("Completed updating ScoresIssues");
        }

        public async Task DeleteScores_Issue(int id)
        {
            var ScoresIssues = context.ScoresIssues.AsNoTracking().FirstOrDefault(x => x.Id == id);


            //ScoresIssues.UpdatedStatus = (short)UpdStatus.Deleted;

            //context.Entry(ScoresIssues).State = EntityState.Modified;
            context.Entry(ScoresIssues).State = EntityState.Deleted;


            await context.SaveChangesAsync();


        }


    }
}
