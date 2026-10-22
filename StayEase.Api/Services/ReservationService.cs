using Microsoft.EntityFrameworkCore;
using StayEase.Api.Data;
using StayEase.Api.Dtos;
using StayEase.Api.Models;

namespace StayEase.Api.Services;

public class ReservationService : IReservationService
{
    private readonly AppDbContext _db;

    public ReservationService(AppDbContext db)
    {
        _db = db;
    }

    private IQueryable<Reservation> QueryWithDetails()
    {
        return _db.Reservations
            .AsNoTracking()
            .Include(r => r.Room)
            .Include(r => r.Guest);
    }

    public Task<List<Reservation>> GetActiveAsync()
    {
        return QueryWithDetails()
            .Where(r => !r.IsCancelled && r.ActualCheckOut == null)
            .ToListAsync();
    }

    public Task<List<Reservation>> GetAllAsync()
    {
        return QueryWithDetails().ToListAsync();
    }

    public Task<Reservation?> GetByIdAsync(int id)
    {
        return QueryWithDetails().FirstOrDefaultAsync(r => r.ReservationId == id);
    }

    public Task<List<Reservation>> GetByGuestAsync(int guestId)
    {
        return QueryWithDetails().Where(r => r.GuestId == guestId).ToListAsync();
    }

    public Task<List<Reservation>> GetByRoomAsync(int roomId)
    {
        return QueryWithDetails().Where(r => r.RoomId == roomId).ToListAsync();
    }

    public async Task<Reservation> CreateAsync(CreateReservationDto dto)
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
        return reservation;
    }

    public async Task<bool> CheckInAsync(int id)
    {
        var reservation = await _db.Reservations.FindAsync(id);
        if (reservation == null)
        {
            return false;
        }
        reservation.CheckIn();
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> CheckOutAsync(int id)
    {
        var reservation = await _db.Reservations.FindAsync(id);
        if (reservation == null)
        {
            return false;
        }
        reservation.CheckOut();
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> CancelAsync(int id)
    {
        var reservation = await _db.Reservations.FindAsync(id);
        if (reservation == null)
        {
            return false;
        }
        reservation.Cancel();
        await _db.SaveChangesAsync();
        return true;
    }
}
