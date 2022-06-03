using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Airport_Management_System.Models;
using System.Diagnostics;

namespace Airport_Management_System.Controllers
{
    public class FlightDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/FlightData/ListFlights
        [HttpGet]
        public IEnumerable<FlightDto> ListFlights()
        {
            List<Flight> Flights = db.Flights.ToList();
            List<FlightDto> FlightDtos = new List<FlightDto>();

            Flights.ForEach(a => FlightDtos.Add(new FlightDto()
            {
                FlightId = a.FlightId,
                Airline = a.Airline,
                FlightNum = a.FlightNum,
                Terminal = a.Terminal,
                Gate = a.Gate,
                Destination = a.Destination,
                DepartureTime = a.DepartureTime,
                ArrivalTime = a.ArrivalTime,
                FlightStatus = a.FlightStatus
            }));

            return FlightDtos;
        }

        // GET: api/FlightData/FindFlight/21
        [ResponseType(typeof(Flight))]
        [HttpGet]
        public IHttpActionResult FindFlight(int id)
        {
            Flight Flight = db.Flights.Find(id);
            FlightDto FlightDto = new FlightDto()
            {
                FlightId = Flight.FlightId,
                Airline = Flight.Airline,
                FlightNum = Flight.FlightNum,
                Terminal = Flight.Terminal,
                Gate = Flight.Gate,
                Destination = Flight.Destination,
                DepartureTime = Flight.DepartureTime,
                ArrivalTime = Flight.ArrivalTime,
                FlightStatus = Flight.FlightStatus

            };
            if (Flight == null)
            {
                return NotFound();
            }

            return Ok(FlightDto);
        }


        // POST: api/FlightData/UpdateFlight/21
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateFlight(int id, Flight flight)
        {
            Debug.WriteLine("I have reached the update passenger method!");
            if (!ModelState.IsValid)
            {
                Debug.WriteLine("Model state is invalid");
                return BadRequest(ModelState);
            }

            if (id != flight.FlightId)
            {
                Debug.WriteLine("ID mismatch");
                Debug.WriteLine("GET paramter" + id);
                Debug.WriteLine("POST paramter" + flight.FlightId);
                return BadRequest();
            }

            db.Entry(flight).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FlightExists(id))
                {
                    Debug.WriteLine("Passenger not found!");
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            Debug.WriteLine("None of the conditions trigger.");
            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/FlightData/AddFlight
        [ResponseType(typeof(Flight))]
        [HttpPost]
        public IHttpActionResult AddFlight(Flight flight)
        {


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Flights.Add(flight);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = flight.FlightId }, flight);
        }



        // POST: api/FlightData/DeleteFlight/21
        [ResponseType(typeof(Flight))]
        [HttpPost]
        public IHttpActionResult DeleteFlight(int id)
        {
            Flight flight = db.Flights.Find(id);
            if (flight == null)
            {
                return NotFound();
            }

            db.Flights.Remove(flight);
            db.SaveChanges();

            return Ok();
        }





        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool FlightExists(int id)
        {
            return db.Flights.Count(e => e.FlightId == id) > 0;
        }
    }
}