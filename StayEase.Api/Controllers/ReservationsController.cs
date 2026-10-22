using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StayEase.Api.Data;
using StayEase.Api.Dtos;
using StayEase.Api.Models;

namespace StayEase.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReservationsController : ControllerBase
{
    private readonly AppDbContext _db;

    public ReservationsController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<IActionResult> GetActiveReservations()
    {
        var reservations = await GetReservations()
            .Where(r => !r.IsCancelled && r.ActualCheckOut == null)
            .ToListAsync();
        return Ok(reservations);
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAllReservations()
    {
        var reservations = await GetReservations().ToListAsync();
        return Ok(reservations);
    }

    private IQueryable<Reservation> GetReservations()
    {
        return _db.Reservations
                .AsNoTracking()
                .Include(r => r.Room)
                .Include(r => r.Guest);
    }

    [HttpPost]
    public async Task<IActionResult> CreateReservation(CreateReservationDto dto)
    {
        var reservation = new Reservation
        {
            RoomId = dto.RoomId,
            GuestId = dto.GuestId,
            ScheduledCheckIn = dto.ScheduledCheckIn,
            ScheduledCheckOut = dto.ScheduledCheckOut
        };
        await _db.Reservations.AddAsync(reservation);
        await _db.SaveChangesAsync();
        return Ok(reservation);
    }

    [HttpPatch("{id}/check-in")]
    public async Task<IActionResult> CheckIn(int id)
    {
        var reservation = await GetReservationById(id);
        if (reservation == null)
            return NotFound(new { message = "Reservation not found" });

        reservation.CheckIn();
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpPatch("{id}/check-out")]
    public async Task<IActionResult> CheckOut(int id)
    {
        var reservation = await GetReservationById(id);
        if (reservation == null)
            return NotFound(new { message = "Reservation not found" });

        reservation.CheckOut();
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpPatch("{id}/cancel")]
    public async Task<IActionResult> Cancel(int id)
    {
        var reservation = await GetReservationById(id);
        if (reservation == null)
            return NotFound(new { message = "Reservation not found" });

        reservation.Cancel();
        await _db.SaveChangesAsync();
        return NoContent();
    }

    private async Task<Reservation?> GetReservationById(int id)
    {
        return await _db.Reservations.FindAsync(id);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetReservation(int id)
    {
        var reservation = await GetReservations()
            .FirstOrDefaultAsync(r => r.ReservationId == id);
        if (reservation == null)
            return NotFound(new { message = "Reservation not found" });
        return Ok(reservation);
    }

    [HttpGet("guest/{guestId}")]
    public async Task<IActionResult> GetReservationsByGuest(int guestId)
    {
        var reservations = await GetReservations()
            .Where(r => r.GuestId == guestId)
            .ToListAsync();
        return Ok(reservations);
    }

    [HttpGet("room/{roomId}")]
    public async Task<IActionResult> GetReservationsByRoom(int roomId)
    {
        var reservations = await GetReservations()
            .Where(r => r.RoomId == roomId)
            .ToListAsync();
        return Ok(reservations);
    }
}
