namespace Products.Model;
public class Employee : IdentityUser
{
    [PersonalData]
    [MinLength(2, ErrorMessage = "A single letter First Name?")]
    [Display(Name = "First Name")]
    public string? FirstName { get; set; }

    [PersonalData]
    [MinLength(2, ErrorMessage = "A single letter Last Name?")]
    [Display(Name = "Last Name")]
    public string? LastName { get; set; }

    [PersonalData]
    [Required]
    [Display(Name = "Job Title")]
    public Position Title { get; set; }

    [PersonalData]
    [RegularExpression(@"^\(?([0-9]{3})\)?[-.●]?([0-9]{3})[-.●]?([0-9]{4})$", ErrorMessage = "Phone number format is incorrect!")]
    public string? Phone { get; set; }
}

public enum Position { Staff, Supervisor }