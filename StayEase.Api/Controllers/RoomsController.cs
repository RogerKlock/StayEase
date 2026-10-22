using Microsoft.AspNetCore.Mvc;
using StayEase.Api.Dtos;
using StayEase.Api.Services;

namespace StayEase.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RoomsController : ControllerBase
{
    private readonly IRoomService _roomService;

    public RoomsController(IRoomService roomService)
    {
        _roomService = roomService;
    }

    [HttpGet]
    public async Task<IActionResult> GetRooms()
    {
        return Ok(await _roomService.GetAllAsync());
    }

    [HttpPost]
    public async Task<IActionResult> AddRoom(CreateRoomDto dto)
    {
        return Ok(await _roomService.CreateAsync(dto));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateRoom(int id, UpdateRoomDto dto)
    {
        var room = await _roomService.UpdateAsync(id, dto);
        return room == null ? NotFound() : NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRoom(int id)
    {
        return await _roomService.DeleteAsync(id) ? NoContent() : NotFound();
    }
}
