using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NamaTravelApi.Data;
using NamaTravelApi.Dtos;
using NamaTravelApi.Models;

namespace NamaTravelApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookingsController : ControllerBase
{
    private readonly AppDbContext _db;

    public BookingsController(AppDbContext db) => _db = db;

    // GET: /api/bookings
    [HttpGet]
    public async Task<ActionResult<List<object>>> GetAll()
    {
        var items = await _db.Bookings
            .OrderBy(b => b.Id)
            .Select(b => new {
                b.Id,
                b.ClientName,
                b.Destination,
                TravelDate = b.TravelDate.ToString("yyyy-MM-dd"),
                Travelers = b.Travelers
            })
            .ToListAsync();

        return Ok(items);
    }

    // GET: /api/bookings/{id}
    [HttpGet("{id:int}")]
    public async Task<ActionResult<object>> GetById(int id)
    {
        var b = await _db.Bookings.FindAsync(id);
        if (b is null) return NotFound();

        return Ok(new {
            b.Id,
            b.ClientName,
            b.Destination,
            TravelDate = b.TravelDate.ToString("yyyy-MM-dd"),
            Travelers = b.Travelers
        });
    }

    // POST: /api/bookings
    [HttpPost]
    public async Task<ActionResult<object>> Create(BookingCreateUpdateDto dto)
    {
        if (!DateOnly.TryParse(dto.TravelDate, out var date))
            return BadRequest("TravelDate must be in yyyy-MM-dd format.");

        var booking = new Booking
        {
            ClientName = dto.ClientName.Trim(),
            Destination = dto.Destination.Trim(),
            TravelDate = date,
            Travelers = dto.Travelers
        };

        _db.Bookings.Add(booking);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = booking.Id }, new {
            booking.Id,
            booking.ClientName,
            booking.Destination,
            TravelDate = booking.TravelDate.ToString("yyyy-MM-dd"),
            Travelers = booking.Travelers
        });
    }

    // PUT: /api/bookings/{id}
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, BookingCreateUpdateDto dto)
    {
        var booking = await _db.Bookings.FindAsync(id);
        if (booking is null) return NotFound();

        if (!DateOnly.TryParse(dto.TravelDate, out var date))
            return BadRequest("TravelDate must be in yyyy-MM-dd format.");

        booking.ClientName = dto.ClientName.Trim();
        booking.Destination = dto.Destination.Trim();
        booking.TravelDate = date;
        booking.Travelers = dto.Travelers;

        await _db.SaveChangesAsync();
        return NoContent();
    }

    // DELETE: /api/bookings/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var booking = await _db.Bookings.FindAsync(id);
        if (booking is null) return NotFound();

        _db.Bookings.Remove(booking);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
