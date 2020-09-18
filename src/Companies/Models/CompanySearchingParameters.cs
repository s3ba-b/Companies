using System;

namespace TestBackendDeveloper.Models
{
    public class CompanySearchingParameters
    {
        public string Keyword { get; set; }
        public DateTime? EmployeeDateOfBirthFrom { get; set; }
        public DateTime? EmployeeDateOfBirthTo { get; set; }
        public EmployeeJobTitle? EmployeeJobTitles { get; set; }
    }
}