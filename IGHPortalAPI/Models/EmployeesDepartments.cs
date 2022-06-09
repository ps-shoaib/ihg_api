using System.ComponentModel.DataAnnotations.Schema;

namespace IGHportalAPI.Models
{
    public class EmployeesDepartments
    {

        [ForeignKey("Employee")]
        public int EmployeeId { get; set; }

        [ForeignKey("Department")]
        public int DepartmentId { get; set; }




        public virtual Employee Employee { get; set; }
        public virtual Department Department { get; set; }


    }
}
