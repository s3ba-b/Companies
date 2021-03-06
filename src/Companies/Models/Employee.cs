using System;

namespace TestBackendDeveloper.Models
{
    public class Employee
    {
        public long EmployeeId { get; set; }
        #nullable enable
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        #nullable disable
        public DateTime? DateOfBirth { get; set; }
        public EmployeeJobTitle? JobTitle { get; set; }
        
        public long CompanyId { get; set; }
        public Company Company { get; set; }
    }
    
    public enum EmployeeJobTitle
    {
        Administrator,
        Developer,
        Architect,
        Manager
    }
}