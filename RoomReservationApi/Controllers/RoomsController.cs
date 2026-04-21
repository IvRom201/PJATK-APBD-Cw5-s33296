using Microsoft.AspNetCore.Mvc;
using RoomReservationApi.Data;
using RoomReservationApi.Models;

[ApiController]
[Route("api/[controller]")]
public class RoomsController : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<Room>> GetAll(
        [FromQuery] int? minCapacity,
        [FromQuery] bool? hasProjector,
        [FromQuery] bool? activeOnly)
    {
        IEnumerable<Room> rooms = AppData.Rooms;

        if (minCapacity.HasValue)
        {
            rooms = rooms.Where(r => r.Capacity >= minCapacity.Value);
        }

        if (hasProjector.HasValue)
        {
            rooms = rooms.Where(r => r.HasProjector == hasProjector.Value);
        }

        if (activeOnly == true)
        {
            rooms = rooms.Where(r => r.IsActive);
        }

        return Ok(rooms);
    }

    [HttpGet("{id:int}")]
    public ActionResult<Room> GetById(int id)
    {
        var room = AppData.Rooms.FirstOrDefault(r => r.Id == id);

        if (room is null)
        {
            return NotFound(new { message = $"Sala o id {id} nie istnieje." });
        }

        return Ok(room);
    }

    [HttpGet("building/{buildingCode}")]
    public ActionResult<IEnumerable<Room>> GetByBuilding(string buildingCode)
    {
        var rooms = AppData.Rooms
            .Where(r => r.BuildingCode.Equals(buildingCode, StringComparison.OrdinalIgnoreCase))
            .ToList();

        return Ok(rooms);
    }

    [HttpPost]
    public ActionResult<Room> Create([FromBody] Room room)
    {
        room.Id = AppData.Rooms.Any() ? AppData.Rooms.Max(r => r.Id) + 1 : 1;

        AppData.Rooms.Add(room);

        return CreatedAtAction(nameof(GetById), new { id = room.Id }, room);
    }

    [HttpPut("{id:int}")]
    public IActionResult Update(int id, [FromBody] Room updatedRoom)
    {
        var existingRoom = AppData.Rooms.FirstOrDefault(r => r.Id == id);

        if (existingRoom is null)
        {
            return NotFound(new { message = $"Sala o id {id} nie istnieje." });
        }

        existingRoom.Name = updatedRoom.Name;
        existingRoom.BuildingCode = updatedRoom.BuildingCode;
        existingRoom.Floor = updatedRoom.Floor;
        existingRoom.Capacity = updatedRoom.Capacity;
        existingRoom.HasProjector = updatedRoom.HasProjector;
        existingRoom.IsActive = updatedRoom.IsActive;

        return Ok(existingRoom);
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        var room = AppData.Rooms.FirstOrDefault(r => r.Id == id);

        if (room is null)
        {
            return NotFound(new { message = $"Sala o id {id} nie istnieje." });
        }

        bool hasRelatedReservations = AppData.Reservations.Any(r => r.RoomId == id);

        if (hasRelatedReservations)
        {
            return Conflict(new
            {
                message = "Nie można usunąć sali, ponieważ ma powiązane rezerwacje."
            });
        }

        AppData.Rooms.Remove(room);

        return NoContent();
    }
}    







