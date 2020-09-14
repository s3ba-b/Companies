using System.Collections.Generic;
public class Company
{
    public long Id { get; set; }
    public string Name { get; set; }
    public int EstablishmentYear { get; set; }
    public List<Employee> Employees { get; set; } = new List<Employee>();
}