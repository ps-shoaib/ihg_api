using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IGHportalAPI.Models
{
    public class PayrollDepartmentGoals
    {

        public int Id { get; set; }
        public int Goal { get; set; }

        [ForeignKey("Department")]
        public int DepartmentId { get; set; }
        public virtual Department Department { get; set; }


        [ForeignKey("PayrollReport")]
        public int PayrollReportId { get; set; }
        public virtual PayrollReport PayrollReport { get; set; }


    }
}
