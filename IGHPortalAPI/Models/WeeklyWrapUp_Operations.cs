using IGHportalAPI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IGHportalAPI.Models
{
    public class WeeklyWrapUp_Operations
    {

        public WeeklyWrapUp_Operations()
        {
            weeklyWrapUp_OperationsDetails = new HashSet<WeeklyWrapUp_OperationsDetails>();
        }
        public int Id { get; set; }

        [DataType(DataType.Date)]
        public DateTime ReportFrom { get; set; }


        [DataType(DataType.Date)]
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


        public virtual ICollection<WeeklyWrapUp_OperationsDetails>  weeklyWrapUp_OperationsDetails { get; set; }




        [ForeignKey("WeeklyWrapUp")]
        public int WeeklyWrapUpId { get; set; }
        public virtual WeeklyWrapUp WeeklyWrapUp { get; set; }









    }
}
