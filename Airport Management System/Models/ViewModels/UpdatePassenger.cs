using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Airport_Management_System.Models.ViewModels
{
    public class UpdatePassenger
    {
        //this viewmodel is a class which stores information that need to present to /Passenger/Update/{}

        //the existing passenger information 

        public PassengerDto SelectedPassenger { get; set; }

        // all flights to choose from when updating this passenger

        public IEnumerable<FlightDto> FlightsOptions { get; set; }
    }
}