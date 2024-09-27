using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace HotelSystem
{
    internal class Program
    {
        public class Guest
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string RoomType { get; set; }
            public decimal Price { get; set; }
            public int RoomNumber { get; set; }

            public string PhoneGuest { get; set; }
            public DateTime ReservationDate { get; set; }
            public DateTime CheckInDate { get; set; }
            public DateTime CheckOutDate { get; set; }

            public int NumberOfNights
            {
                get
                {
                    return (CheckOutDate - CheckInDate).Days;
                }
            }

            public string GetDetails()
            {
                return $"{FirstName},{LastName}, {PhoneGuest}, {RoomType},{Price},{ReservationDate},{CheckInDate},{CheckOutDate},{RoomNumber}";
            }
        }

        public class Hotel
        {
            private List<Guest> guests = new List<Guest>();
            private const string reservationsFile = "reservations.txt";

            private readonly List<string> roomTypes = new List<string>
        {
            "Единична стая",
            "Двойна стая",
            "Семейна стая",
            "Друга" 
        };

            public Hotel()
            {
                LoadReservations();
            }

            public void MakeReservation()
            {
                Console.Clear();
                Console.WriteLine("=== Нова резервация ===");
                Console.Write("Въведете име: ");
                string firstName = Console.ReadLine();
                Console.Write("Въведете фамилия: ");
                string lastName = Console.ReadLine();

                Console.Write("Въведете телефонен номер: ");
                string PhoneGuest = Console.ReadLine();

                Console.WriteLine("Изберете тип стая:");
                for (int i = 0; i < roomTypes.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {roomTypes[i]}");
                }

                Console.Write("Въведете ваш тип на стаята: ");
                int roomTypeIndex = int.Parse(Console.ReadLine()) - 1;

                string roomType = roomTypes[roomTypeIndex];
                if (roomType == "Друга")
                {
                    Console.Write("Въведете типа на стаята: ");
                    roomType = Console.ReadLine();
                }

                Console.Write("Въведете номер на стаята: ");
                int roomNumber = int.Parse(Console.ReadLine());

                Console.Write("Въведете цена на стаята: ");
                decimal price = decimal.Parse(Console.ReadLine());

                Console.Write("Въведете дата на резервация (формат: dd.MM.yyyy): ");
                DateTime reservationDate = DateTime.Parse(Console.ReadLine());

                Console.Write("Въведете дата на настаняване (формат: dd.MM.yyyy): ");
                DateTime checkInDate = DateTime.Parse(Console.ReadLine());

                Console.Write("Въведете дата на напускане (формат: dd.MM.yyyy): ");
                DateTime checkOutDate = DateTime.Parse(Console.ReadLine());


                Guest guest = new Guest
                {
                    FirstName = firstName,
                    LastName = lastName,
                    RoomType = roomType,
                    Price = price,
                    ReservationDate = reservationDate,
                    CheckInDate = checkInDate,
                    CheckOutDate = checkOutDate,
                    PhoneGuest = PhoneGuest,
                    RoomNumber = roomNumber
                };

                guests.Add(guest);
                SaveReservation(guest);

                Console.WriteLine("=============================");
                Console.WriteLine("Резервацията е успешна!");
            }


            private void SaveReservation(Guest guest)
            {
                using (StreamWriter sw = new StreamWriter(reservationsFile, true))
                {
                    sw.WriteLine(guest.GetDetails());
                }
            }

            public void ViewReservations()
            {
                Console.Clear();
                Console.WriteLine("=== Преглед на резервации ===");
                foreach (var guest in guests)
                {
                    decimal totalCost = guest.Price * guest.NumberOfNights;

                    Console.WriteLine($"Име: {guest.FirstName}");
                    Console.WriteLine($"Фамилия: {guest.LastName}");
                    Console.WriteLine($"Телефонен номер: {guest.PhoneGuest}");
                    Console.WriteLine($"Номер на стая: {guest.RoomNumber}");
                    Console.WriteLine($"Дата на настаняване: {guest.CheckInDate}");
                    Console.WriteLine($"Дата на напускане: {guest.CheckOutDate}");
                    Console.WriteLine($"Цена: {guest.Price}лв. на вечер");
                    Console.WriteLine($"Брой нощувки: {guest.NumberOfNights}");
                    Console.WriteLine($"Обща цена: {totalCost} лв.");

                    Console.WriteLine("-----------------------------------");
                }
            }

            public void LoadReservations()
            {
                if (File.Exists(reservationsFile))
                {
                    var lines = File.ReadAllLines(reservationsFile);
                    foreach (var line in lines)
                    {
                        var parts = line.Split(',');
                        if (parts.Length == 7)
                        {
                            guests.Add(new Guest
                            {
                                FirstName = parts[0],
                                LastName = parts[1],
                                RoomType = parts[2],
                                Price = decimal.Parse(parts[3]),
                                ReservationDate = DateTime.Parse(parts[4]),
                                CheckInDate = DateTime.Parse(parts[5]),
                                CheckOutDate = DateTime.Parse(parts[6])
                            });
                        }
                    }
                }
            }

            public void CheckIn()
            {
                Console.Clear();
                Console.WriteLine("=== Настаняване ===");
                Console.Write("Въведете име на госта: ");
                string firstName = Console.ReadLine();

                Console.Write("Въведете фамилия на госта: ");
                string lastName = Console.ReadLine();

                Console.Write("Въведете номер на стаята: ");
                int roomNumber = int.Parse(Console.ReadLine());

                var guest = guests.Find(g => g.FirstName == firstName && g.LastName == lastName && g.RoomNumber == roomNumber);

                if (guest != null)
                {
                    Console.WriteLine("=============================");
                    Console.WriteLine("Гостът е настанен успешно!");
                }
                else
                {
                    Console.WriteLine("=============================");
                    Console.WriteLine("Гостът не е намерен.");
                }
            }

            public void ViewCheckIns()
            {
                Console.Clear();
                Console.WriteLine("=== Преглед на настанявания ===");
                foreach (var guest in guests)
                {
                    Console.WriteLine($"Име: {guest.FirstName}");
                    Console.WriteLine($"Фамилия: {guest.LastName}");
                    Console.WriteLine($"Тип: {guest.RoomType}");
                    Console.WriteLine($"Номер на стая: {guest.RoomNumber}");
                    Console.WriteLine($"Цена: {guest.Price}лв. на вечер");

                    decimal totalCost = guest.Price * guest.NumberOfNights;
                    Console.WriteLine($"Брой нощувки: {guest.NumberOfNights}");
                    Console.WriteLine($"Обща цена: {totalCost} лв.");
                    Console.WriteLine("-----------------------------------");
                }
            }

            public void CancelReservation()
            {
                Console.Clear();
                Console.WriteLine("=== Отказ на резервация ===");
                Console.Write("Въведете име на госта: ");
                string firstName = Console.ReadLine();
                Console.Write("Въведете фамилия на госта: ");
                string lastName = Console.ReadLine();
                var guest = guests.Find(g => g.FirstName == firstName && g.LastName == lastName);

                if (guest != null)
                {
                    guests.Remove(guest);
                    SaveAllReservations();
                    Console.WriteLine("Резервацията е отменена успешно!");
                }
                else
                {
                    Console.WriteLine("Гостът не е намерен.");
                }
            }

            private void SaveAllReservations()
            {
                using (StreamWriter sw = new StreamWriter(reservationsFile, false))
                {
                    foreach (var guest in guests)
                    {
                        sw.WriteLine(guest.GetDetails());
                    }
                }
            }
        }

        public class main
        {
            public static void Main(string[] args)
            {
                Hotel hotel = new Hotel();
                bool exit = false;

                while (!exit)
                {
                    Console.Clear();
                    Console.WriteLine("=== Хотелска резервационна система ===");
                    Console.WriteLine("1. Нова резервация");
                    Console.WriteLine("2. Настаняване");
                    Console.WriteLine("3. Преглед на резервации");
                    Console.WriteLine("4. Преглед на настанявания");
                    Console.WriteLine("5. Отказ на резервация");
                    Console.WriteLine("6. Изход");
                    Console.Write("\nИзберете опция: ");

                    string choice = Console.ReadLine();
                    switch (choice)
                    {
                        case "1":
                            hotel.MakeReservation();
                            Console.ReadKey();
                            break;
                        case "2":
                            hotel.CheckIn();
                            Console.ReadKey();
                            break;
                        case "3":
                            hotel.ViewReservations();
                            Console.ReadKey();
                            break;
                        case "4":
                            hotel.ViewCheckIns();
                            Console.ReadKey();
                            break;
                        case "5":
                            hotel.CancelReservation();
                            Console.ReadKey();
                            break;
                        case "6":
                            exit = true;
                            break;
                        default:
                            Console.WriteLine("Невалиден избор, опитайте отново.");
                            Console.ReadKey();
                            break;
                    }
                }
            }
        }
    }
}