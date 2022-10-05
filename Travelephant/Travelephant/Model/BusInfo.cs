﻿using System.ComponentModel.DataAnnotations;

namespace Travelephant.Model
{
    public class BusInfo
    {
        public int BusId { get; set; }
        [Required]
        [MaxLength(20)]
        public string Name { get; set; }
        [Required]
        [MaxLength(20)]
        public string Departure { get; set; }
        [Required]
        [MaxLength(20)]
        public string Destination { get; set; }
        [Required]
        public int DepartureTime { get; set; }
        public int ArrivalTime { get; set; }
        [Required]
        public int TotalSeat { get; set; }
        public int AvailableSeat { get; set; }
        [Required]
        public double Price { get; set; }
        public bool IsActive { get; set; }
    }
}
