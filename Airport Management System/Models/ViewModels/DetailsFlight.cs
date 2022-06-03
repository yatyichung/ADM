using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Airport_Management_System.Models.ViewModels
{
    public class DetailsFlight
    {
        public FlightDto SelectedFlight { get; set; }

        public IEnumerable<PassengerDto> RelatedPassengers { get; set; }
    }
}