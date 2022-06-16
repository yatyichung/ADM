using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Airport_Management_System.Models
{
    public class Passenger
    {

        //describe a passenger
        [Key]
        public int PassengerId { get; set; } //primary key for passenger key table

        public string PassengerFName { get; set; }
        public string PassengerLName { get; set; }

        public string PassengerPassport { get; set; }

        public DateTime CheckInStatus { get; set; }

        public string PassengerSeatingClasses { get; set; }

        public string PassengerSeatingNum { get; set; }

        //A passenger below to one flight
        //One flight can have many passengers
       [ForeignKey("Flight")]
        public int FlightId { get; set; }
        public virtual Flight Flight { get; set; }

    }

    public class PassengerDto
    {
        public int PassengerId { get; set; } //primary key for passenger key table

        public string PassengerFName { get; set; }
        public string PassengerLName { get; set; }

        public string PassengerPassport { get; set; }

        public DateTime CheckInStatus { get; set; }

        public string PassengerSeatingClasses { get; set; }

        public string PassengerSeatingNum { get; set; }

        public int FlightId { get; set; }
        public string FlightNum { get; set; }





    }
}