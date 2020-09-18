using System.Collections.Generic;

namespace TestBackendDeveloper.Models
{
    public class Company
    {
        public long CompanyId { get; set; }
        public string Name { get; set; }
        public int EstablishmentYear { get; set; }
        public List<Employee> Employees { get; set; } = new List<Employee>();
    }
}