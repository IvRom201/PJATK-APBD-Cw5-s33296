using RoomReservationApi.Models;

namespace RoomReservationApi.Data;

public static class AppData
    {
        public static List<Room> Rooms { get; } =
        [
            new Room
            {
                Id = 1,
                Name = "Aula A101",
                BuildingCode = "A",
                Floor = 1,
                Capacity = 80,
                HasProjector = true,
                IsActive = true
            },
            new Room
            {
                Id = 2,
                Name = "Lab B204",
                BuildingCode = "B",
                Floor = 2,
                Capacity = 24,
                HasProjector = true,
                IsActive = true
            },
            new Room
            {
                Id = 3,
                Name = "Sala C12",
                BuildingCode = "C",
                Floor = 1,
                Capacity = 16,
                HasProjector = false,
                IsActive = true
            },
            new Room
            {
                Id = 4,
                Name = "Sala D305",
                BuildingCode = "D",
                Floor = 3,
                Capacity = 12,
                HasProjector = true,
                IsActive = false
            },
            new Room
            {
                Id = 5,
                Name = "Warsztat B110",
                BuildingCode = "B",
                Floor = 1,
                Capacity = 30,
                HasProjector = false,
                IsActive = true
            }
        ];

        public static List<Reservation> Reservations { get; } =
        [
            new Reservation
            {
                Id = 1,
                RoomId = 2,
                OrganizerName = "Anna Kowalska",
                Topic = "Warsztaty z HTTP i REST",
                Date = new DateOnly(2026, 5, 10),
                StartTime = new TimeSpan(10, 0, 0),
                EndTime = new TimeSpan(12, 30, 0),
                Status = "confirmed"
            },
            new Reservation
            {
                Id = 2,
                RoomId = 1,
                OrganizerName = "Jan Nowak",
                Topic = "Szkolenie z wystąpień publicznych",
                Date = new DateOnly(2026, 5, 10),
                StartTime = new TimeSpan(13, 0, 0),
                EndTime = new TimeSpan(16, 0, 0),
                Status = "planned"
            },
            new Reservation
            {
                Id = 3,
                RoomId = 5,
                OrganizerName = "Marta Zielińska",
                Topic = "Konsultacje projektowe",
                Date = new DateOnly(2026, 5, 11),
                StartTime = new TimeSpan(9, 0, 0),
                EndTime = new TimeSpan(10, 30, 0),
                Status = "confirmed"
            },
            new Reservation
            {
                Id = 4,
                RoomId = 3,
                OrganizerName = "Piotr Wiśniewski",
                Topic = "Spotkanie zespołu",
                Date = new DateOnly(2026, 5, 12),
                StartTime = new TimeSpan(11, 0, 0),
                EndTime = new TimeSpan(12, 0, 0),
                Status = "planned"
            },
            new Reservation
            {
                Id = 5,
                RoomId = 2,
                OrganizerName = "Ewa Lewandowska",
                Topic = "Postman w praktyce",
                Date = new DateOnly(2026, 5, 12),
                StartTime = new TimeSpan(14, 0, 0),
                EndTime = new TimeSpan(15, 0, 0),
                Status = "cancelled"
            }
        ];
    }