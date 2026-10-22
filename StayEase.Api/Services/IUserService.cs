using StayEase.Api.Dtos;
using StayEase.Api.Models;

namespace StayEase.Api.Services;

public interface IUserService
{
    Task<List<User>> GetAllAsync();

    Task<User> CreateAsync(CreateUserDto dto);

    /// <returns>The updated user, or null when it does not exist.</returns>
    Task<User?> UpdateAsync(int id, UpdateUserDto dto);

    /// <returns>The removed user, or null when it does not exist.</returns>
    Task<User?> DeleteAsync(int id);
}
