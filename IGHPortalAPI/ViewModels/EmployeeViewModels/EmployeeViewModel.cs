using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace IGHportalAPI.ViewModels.EmployeeViewModels
{
    public class EmployeeViewModel
    {
        public EmployeeViewModel()
        {
            DepartmentNames = new List<string>();
            DepartmentIds = new List<int>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public List<string> DepartmentNames { get; set; }

        public string  DepartmentName { get; set; }

        public int DepartmentId { get; set; }
        public string ContactNo { get; set; }
        public string Email { get; set; }
        public float HourlyRate { get; set; }
        public float OverTimeRate { get; set; }


        public int HotelId { get; set; }
        public List<int> DepartmentIds { get; set; }

    }
}
