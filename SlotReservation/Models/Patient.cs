using System.ComponentModel.DataAnnotations;

public class Patient
{
    [Required(ErrorMessage = "Name is required.")]
    [StringLength(50, ErrorMessage = "Name cannot be longer than 50 characters.")]
    public required string Name { get; set; }

    [StringLength(50, ErrorMessage = "Second name cannot be longer than 50 characters.")]
    public string? SecondName { get; set; }

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid Email Address.")]
    public required string Email { get; set; }

    [Required(ErrorMessage = "Phone number is required.")]
    [Phone(ErrorMessage = "Invalid Phone Number.")]
    public required string Phone { get; set; }
}
