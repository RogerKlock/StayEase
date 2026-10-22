using Microsoft.EntityFrameworkCore;
using StayEase.Api.Data;
using StayEase.Api.Dtos;
using StayEase.Api.Models;

namespace StayEase.Api.Services;

public class UserService : IUserService
{
    private readonly AppDbContext _db;

    public UserService(AppDbContext db)
    {
        _db = db;
    }

    public Task<List<User>> GetAllAsync()
    {
        return _db.Users.AsNoTracking().ToListAsync();
    }

    public async Task<User> CreateAsync(CreateUserDto dto)
    {
        var user = new User
        {
            Email = dto.Email,
            Role = dto.Role
        };
        await _db.Users.AddAsync(user);
        await _db.SaveChangesAsync();
        return user;
    }

    public async Task<User?> UpdateAsync(int id, UpdateUserDto dto)
    {
        var user = await _db.Users.FindAsync(id);
        if (user == null)
        {
            return null;
        }
        user.Role = dto.Role;
        await _db.SaveChangesAsync();
        return user;
    }

    public async Task<User?> DeleteAsync(int id)
    {
        var user = await _db.Users.FindAsync(id);
        if (user == null)
        {
            return null;
        }
        _db.Users.Remove(user);
        await _db.SaveChangesAsync();
        return user;
    }
}
