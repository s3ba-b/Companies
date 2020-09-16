public class Employee {
    public long Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public EmployeeJobTitles JobTitle { get; set; }

    public long CompanyId { get; set; }
    public Company Company { get; set; }
}
public enum EmployeeJobTitles {
    Administrator,
    Developer,
    Architect,
    Manager
}