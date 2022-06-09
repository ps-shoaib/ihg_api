using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using IGHportalAPI.ViewModels.Scores_IssuesViewModels;

namespace IGHportalAPI.Services
{
    public interface IScores_IssuesService
    {
        Task AddScores_Issues(Scores_IssuesViewModel model);
        //List<Scores_IssuesViewModel> GetScores_Issues(int hotelId);

        List<Scores_IssuesViewModel> GetScores_Issues();

        Task UpdateScores_Issue(Scores_IssuesViewModel model);
        
        Task DeleteScores_Issue(int id);

        Scores_IssuesViewModel GetScores_Issue(int id);

        
        bool SystemNameAlreadyExist(Scores_IssuesViewModel model);

       

    }
}
