using System.ComponentModel.DataAnnotations;

namespace NamaTravelApi.Dtos;

public class BookingCreateUpdateDto
{
    [Required, MaxLength(100)]
    public string ClientName { get; set; } = string.Empty;

    [Required, MaxLength(120)]
    public string Destination { get; set; } = string.Empty;

    // We accept yyyy-MM-dd from the browser
    [Required]
    public string TravelDate { get; set; } = string.Empty;

    [Range(1, 999)]
    public int Travelers { get; set; }
}
