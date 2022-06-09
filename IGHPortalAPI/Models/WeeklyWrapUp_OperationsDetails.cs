using System.ComponentModel.DataAnnotations.Schema;

namespace IGHportalAPI.Models
{
    public class WeeklyWrapUp_OperationsDetails
    {
        public int Id { get; set; }
        public float Current_Month_Score { get; set; }
        public float Previous_Month_Score { get; set; }
        public float YTD_score { get; set; }
        public float Brand_Average { get; set; }
        public float Annual_Goal { get; set; }


        [ForeignKey("ScoresIssues")]
        public int Scores_IssuesId { get; set; }
        public virtual Scores_Issues Scores_Issues { get; set; }

        [ForeignKey("WeeklyWrapUpOperations")]
        public int WeeklyWrapUp_OperationsId { get; set; }
        public virtual WeeklyWrapUp_Operations WeeklyWrapUp_Operations { get; set; }


    }
}
