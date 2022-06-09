namespace IGHportalAPI.ViewModels.WeeklyWrapUp_OperationsViewModels
{
    public class WeeklyWrapUp_OperationsDetailsViewModel
    {
        public int Id { get; set; }
        public float Current_Month_Score { get; set; }
        public float Previous_Month_Score { get; set; }
        public float YTD_score { get; set; }
        public float Brand_Average { get; set; }
        public float Annual_Goal { get; set; }

        public int Scores_IssuesId { get; set; }

        public string Scores_Issue { get; set; }

    }
}
