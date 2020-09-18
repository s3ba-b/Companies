using System.Collections.Generic;

namespace TestBackendDeveloper.Models
{
    public class Company
    {
        public long CompanyId { get; set; }
        #nullable enable
        public string? Name { get; set; }
        #nullable enable
        public int? EstablishmentYear { get; set; }

        public List<Employee> Employees { get; set; } = new List<Employee>();
    }
    
    // DTO: Data Transfer Object
    public class CompanyDTO
    {
        public long CompanyId { get; set; }
    }
}