using Microsoft.AspNetCore.Mvc;
using RoomReservationApi.Data;
using RoomReservationApi.Models;

namespace RoomReservationApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReservationsController : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<Reservation>> GetAll(
        [FromQuery] DateOnly? date,
        [FromQuery] string? status,
        [FromQuery] int? roomId)
    {
        IEnumerable<Reservation> reservations = AppData.Reservations;

        if (date.HasValue)
        {
            reservations = reservations.Where(r => r.Date == date.Value);
        }

        if (!string.IsNullOrWhiteSpace(status))
        {
            reservations = reservations.Where(r => r.Status.Equals(status, StringComparison.OrdinalIgnoreCase));
        }

        if (roomId.HasValue)
        {
            reservations = reservations.Where(r => r.RoomId == roomId.Value);
        }

        return Ok(reservations);
    }

    [HttpGet("{id:int}")]
    public ActionResult<Reservation> GetById(int id)
    {
        var reservation = AppData.Reservations.FirstOrDefault(r => r.Id == id);

        if (reservation is null)
        {
            return NotFound(new { message = $"Rezerwacja o id {id} nie istnieje" });
        }

        return Ok(reservation);
    }

    [HttpPost]
    public ActionResult<Reservation> Create([FromBody] Reservation reservation)
    {
        var room = AppData.Rooms.FirstOrDefault(r => r.Id == reservation.RoomId);

        if (room is null)
        {
                return NotFound(new { message = "Nie można dodać rezerwacji dla nieistniejącej sali" });
        }

        if (!room.IsActive)
        {
            return BadRequest(new { message = "Nie można dodać rezerwacji dla nieaktywnej sali" });
        }

        if (HasTimeConflict(reservation))
        {
            return Conflict(new
            {
                message = "Rezerwacja koliduje czasowo z inną rezerwacją tej samej sali"
            });
        }

        reservation.Id = AppData.Reservations.Any() ? AppData.Reservations.Max(r => r.Id) + 1 : 1;
        AppData.Reservations.Add(reservation);

        return CreatedAtAction(nameof(GetById), new { id = reservation.Id }, reservation);
    }

    [HttpPut("{id:int}")]
    public IActionResult Update(int id, [FromBody] Reservation updatedReservation)
    {
        var existingReservation = AppData.Reservations.FirstOrDefault(r => r.Id == id);

        if (existingReservation is null)
        {
            return NotFound(new { message = $"Rezerwacja o id {id} nie istnieje" });
        }

        var room = AppData.Rooms.FirstOrDefault(r => r.Id == updatedReservation.RoomId);

        if (room is null)
        {
            return NotFound(new { message = "Nie można przypisać rezerwacji do nieistniejącej sali" });
        }

        if (!room.IsActive)
        {
            return BadRequest(new { message = "Nie można przypisać rezerwacji do nieaktywnej sali" });
        }

        updatedReservation.Id = id;

        if (HasTimeConflict(updatedReservation, id))
        {
            return Conflict(new
            {
                message = "Zmieniona rezerwacja koliduje czasowo z inną rezerwacją tej samej sali"
            });
        }

        existingReservation.RoomId = updatedReservation.RoomId;
        existingReservation.OrganizerName = updatedReservation.OrganizerName;
        existingReservation.Topic = updatedReservation.Topic;
        existingReservation.Date = updatedReservation.Date;
        existingReservation.StartTime = updatedReservation.StartTime;
        existingReservation.EndTime = updatedReservation.EndTime;
        existingReservation.Status = updatedReservation.Status;

        return Ok(existingReservation);
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        var reservation = AppData.Reservations.FirstOrDefault(r => r.Id == id);

        if (reservation is null)
        {
            return NotFound(new { message = $"Rezerwacja o id {id} nie istnieje" });
        }

        AppData.Reservations.Remove(reservation);

        return NoContent();
    }

    private static bool HasTimeConflict(Reservation newReservation, int? ignoredReservationId = null)
    {
        if (newReservation.Status.Equals("cancelled", StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        return AppData.Reservations.Any(existing =>
            existing.RoomId == newReservation.RoomId &&
            existing.Date == newReservation.Date &&
            !existing.Status.Equals("cancelled", StringComparison.OrdinalIgnoreCase) &&
            (!ignoredReservationId.HasValue || existing.Id != ignoredReservationId.Value) &&
            existing.StartTime < newReservation.EndTime &&
            newReservation.StartTime < existing.EndTime
            );
    }
}