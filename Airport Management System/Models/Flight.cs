using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Airport_Management_System.Models
{
    public class Flight
    {
        [Key]
        public int FlightId { get; set; }

        public string Airline { get; set; }

        public string FlightNum  { get; set; }

        public int Terminal { get; set; }

        public string Gate { get; set; }

        public string Destination { get; set; }

        public DateTime DepartureTime { get; set; }

        public DateTime ArrivalTime { get; set; }

        public string FlightStatus { get; set; }

    }public class FlightDto
    {
        public int FlightId { get; set; }
        public string Airline { get; set; }

        public string FlightNum { get; set; }

        public int Terminal { get; set; }

        public string Gate { get; set; }

        public string Destination { get; set; }

        public DateTime DepartureTime { get; set; }

        public DateTime ArrivalTime { get; set; }

        public string FlightStatus { get; set; }

        public ICollection<Passenger> Passengers { get; set; }
    }
}