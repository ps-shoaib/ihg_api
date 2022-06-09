using System;
using System.Collections;
using System.Collections.Generic;

namespace IGHportalAPI.ViewModels.WeeklyWrapUp_OperationsViewModels
{
    public class WeeklyWrapUp_OperationsViewModel
    {
        public WeeklyWrapUp_OperationsViewModel()
        {
            weeklyWrapUp_OperationsDetails = new HashSet<WeeklyWrapUp_OperationsDetailsViewModel>();
        }


        public int Id { get; set; }
        public DateTime ReportFrom { get; set; }
        public DateTime ReportTo { get; set; }

        public string Brand_Name { get; set; }
        public string Brand_Zone_Designation { get; set; }



        public float PTD_Resolution_Score { get; set; }
        public float OSAT_Rolling { get; set; }
        public float HCM_score { get; set; }
        public float Current_Training_Credits { get; set; }


        public string OSAT_Response { get; set; }
        public string Plan_To_Improve_Resolution { get; set; }

        public string Maintenance_Issues { get; set; }

        public string PIP_Issues { get; set; }

        public string Out_of_order_room_notes { get; set; }

        public int WeeklyWrapUpId { get; set; }


        public virtual ICollection<WeeklyWrapUp_OperationsDetailsViewModel> weeklyWrapUp_OperationsDetails { get; set; }






    }
}
