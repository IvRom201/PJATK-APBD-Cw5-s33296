namespace RoomReservationApi.Models;

using System.ComponentModel.DataAnnotations;

public class Reservation : IValidatableObject
{
    public int Id { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "RoomId musi być większe od zera")]
    public int RoomId { get; set; }

    [Required]
    public string OrganizerName { get; set; } = string.Empty;

    [Required]
    public string Topic { get; set; } = string.Empty;

    public DateOnly Date { get; set; }

    public TimeSpan StartTime { get; set; }

    public TimeSpan EndTime { get; set; }

    [Required]
    public string Status { get; set; } = string.Empty;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Date == default)
        {
            yield return new ValidationResult(
                "Date jest wymagane",
                new[] { nameof(Date) });
        }

        if (EndTime <= StartTime)
        {
            yield return new ValidationResult(
                "EndTime musi być późniejsze niż StartTime",
                new[] { nameof(EndTime), nameof(StartTime) });
        }

        string[] allowedStatuses = { "planned", "confirmed", "cancelled" };

        if (!allowedStatuses.Contains(Status, StringComparer.OrdinalIgnoreCase))
        {
            yield return new ValidationResult(
                "Status musi mieć wartość: planned, confirmed albo cancelled",
                new[] { nameof(Status) });
        }
    }
}
