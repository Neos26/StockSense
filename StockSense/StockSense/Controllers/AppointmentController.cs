// Controllers/AppointmentsController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockSense.Data;
namespace StockSense.Shared;

[ApiController]
[Route("api/[controller]")]
public class AppointmentsController : ControllerBase
{
    private readonly ApplicationDbContext _db; // Assumed your DB context name

    public AppointmentsController(ApplicationDbContext db) => _db = db;

    [HttpPost]
    public async Task<IActionResult> Create(Appointment appt)
    {
        _db.Appointments.Add(appt);
        await _db.SaveChangesAsync();
        return Ok(new { message = "Appointment booked successfully!" });
    }




    [HttpGet("booked-slots")]
    public async Task<ActionResult<List<string>>> GetBookedSlots([FromQuery] DateTime date)
    {
        // Search the database for appointments matching the requested date
        // and return ONLY the time slots (e.g., "09:00", "14:30")
        var bookedSlots = await _db.Appointments
            .Where(a => a.AppointmentDate.Date == date.Date)
            .Select(a => a.TimeSlot)
            .ToListAsync();

        return Ok(bookedSlots);
    }

    // 1. Get ALL appointments for the Admin table
    [HttpGet("all")]
    public async Task<ActionResult<List<Appointment>>> GetAllAppointments()
    {
        // Fetches all appointments and sorts them by Date, then by Time
        var appointments = await _db.Appointments
            .OrderByDescending(a => a.AppointmentDate)
            .ThenBy(a => a.TimeSlot)
            .ToListAsync();

        return Ok(appointments);
    }

    // 2. Update the status of a specific appointment
    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] string newStatus)
    {
        var appointment = await _db.Appointments.FindAsync(id);
        if (appointment == null) return NotFound();

        appointment.Status = newStatus;
        await _db.SaveChangesAsync();

        return Ok();
    }


    [HttpGet("my-bookings")]
    public async Task<ActionResult<List<Appointment>>> GetMyBookings([FromQuery] string name)
    {
        // Search the database for appointments matching THIS specific customer
        var myBookings = await _db.Appointments
            .Where(a => a.CustomerName == name)
            .OrderByDescending(a => a.AppointmentDate)
            .ThenBy(a => a.TimeSlot)
            .ToListAsync();

        return Ok(myBookings);
    }
}


