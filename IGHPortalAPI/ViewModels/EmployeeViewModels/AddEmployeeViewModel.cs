using System.Collections.Generic;

namespace IGHPortalAPI.ViewModels.EmployeeViewModels
{
    public class AddEmployeeViewModel
    {
        public AddEmployeeViewModel()
        {
            DepartmentNames = new List<string>();
            DepartmentIds = new List<int>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public List<string> DepartmentNames { get; set; }


        public string ContactNo { get; set; }
        public string Email { get; set; }
        public float HourlyRate { get; set; }
        public float OverTimeRate { get; set; }


        public int HotelId { get; set; }
        public List<int> DepartmentIds { get; set; }

    }
}
