using StayEase.Api.Dtos;
using StayEase.Api.Models;

namespace StayEase.Api.Services;

public interface IReservationService
{
    /// <summary>Reservations that are neither cancelled nor checked out.</summary>
    Task<List<Reservation>> GetActiveAsync();

    Task<List<Reservation>> GetAllAsync();

    Task<Reservation?> GetByIdAsync(int id);

    Task<List<Reservation>> GetByGuestAsync(int guestId);

    Task<List<Reservation>> GetByRoomAsync(int roomId);

    Task<Reservation> CreateAsync(CreateReservationDto dto);

    /// <returns>False when the reservation does not exist.</returns>
    Task<bool> CheckInAsync(int id);

    /// <returns>False when the reservation does not exist.</returns>
    Task<bool> CheckOutAsync(int id);

    /// <returns>False when the reservation does not exist.</returns>
    Task<bool> CancelAsync(int id);
}
