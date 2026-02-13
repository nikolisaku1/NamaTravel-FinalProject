using System.ComponentModel.DataAnnotations;

namespace NamaTravelApi.Models;

public class Booking
{
    public int Id { get; set; }

    [Required, MaxLength(100)]
    public string ClientName { get; set; } = string.Empty;

    [Required, MaxLength(120)]
    public string Destination { get; set; } = string.Empty;

    [Required]
    public DateOnly TravelDate { get; set; }

    [Range(1, 999)]
    public int Travelers { get; set; }
}
