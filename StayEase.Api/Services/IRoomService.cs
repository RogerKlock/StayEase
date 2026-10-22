using StayEase.Api.Dtos;
using StayEase.Api.Models;

namespace StayEase.Api.Services;

public interface IRoomService
{
    Task<List<Room>> GetAllAsync();

    Task<Room> CreateAsync(CreateRoomDto dto);

    /// <returns>The updated room, or null when it does not exist.</returns>
    Task<Room?> UpdateAsync(int id, UpdateRoomDto dto);

    /// <returns>False when the room does not exist.</returns>
    Task<bool> DeleteAsync(int id);
}
