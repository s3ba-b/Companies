using System;

namespace TestBackendDeveloper.Models
{
    public class CompanySearchingParameters
    {
        #nullable enable
        public string? Keyword { get; set; }
        #nullable disable
        public DateTime? EmployeeDateOfBirthFrom { get; set; }
        public DateTime? EmployeeDateOfBirthTo { get; set; }
        public EmployeeJobTitle? EmployeeJobTitles { get; set; }
    }
}