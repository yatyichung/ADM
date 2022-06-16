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
    public class PassengerDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();




        /// <summary>
        /// Returns all passengers in the system.
        /// </summary>
        /// <returns>
        /// CONTENT: all passengers in the database, including the flight they are taking.
        /// </returns>
        /// <example>
        /// GET: api/PassengerData/ListPassengers
        /// </example>
        [HttpGet]

        public IEnumerable<PassengerDto> ListPassengers()
        {
            List<Passenger> Passengers= db.Passengers.ToList();
            List<PassengerDto> PassengerDtos = new List<PassengerDto>();

            Passengers.ForEach(a => PassengerDtos.Add(new PassengerDto()
            {
                PassengerId = a.PassengerId,
                PassengerFName = a.PassengerFName,
                PassengerLName = a.PassengerLName,
                PassengerPassport = a.PassengerPassport,
                CheckInStatus = a.CheckInStatus,
                PassengerSeatingClasses = a.PassengerSeatingClasses,
                PassengerSeatingNum = a.PassengerSeatingNum,
                FlightNum = a.Flight.FlightNum,
                 FlightId = a.Flight.FlightId
            }));

            return PassengerDtos;
        }

        /// <summary>
        /// Gathers information about all passengers related to a particular flight ID
        /// </summary>
        /// <returns>
        /// CONTENT: all passengers in the database, including the flight they are taking.
        /// </returns>
        /// <param name="id">Flight ID.</param>
        /// <example>
        /// GET: api/PassengerData/ListPassengersForFlight/17
        /// </example>

        [HttpGet]
        public IEnumerable<PassengerDto> ListPassengersForFlight(int id)
        {
            List<Passenger> Passengers = db.Passengers.Where(a=>a.FlightId==id).ToList();
            List<PassengerDto> PassengerDtos = new List<PassengerDto>();

            Passengers.ForEach(a => PassengerDtos.Add(new PassengerDto()
            {
                PassengerId = a.PassengerId,
                PassengerFName = a.PassengerFName,
                PassengerLName = a.PassengerLName,
                PassengerPassport = a.PassengerPassport,
                CheckInStatus = a.CheckInStatus,
                PassengerSeatingClasses = a.PassengerSeatingClasses,
                PassengerSeatingNum = a.PassengerSeatingNum,
                FlightNum = a.Flight.FlightNum,
                FlightId = a.Flight.FlightId
            }));

            return PassengerDtos;
        }



        /// <summary>
        /// Returns all passengers in the system.
        /// </summary>
        /// <returns>
        /// CONTENT: A passenger in the system matching up to the passenger ID primary key
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <param name="id">The primary key of the passenger</param>
        /// <example>
        /// GET: api/PassengerData/FindPassenger/17
        /// </example>


        [ResponseType(typeof(Passenger))]
        [HttpGet]
        public IHttpActionResult FindPassenger(int id)
        {
            Passenger Passenger = db.Passengers.Find(id);
            PassengerDto PassengerDto = new PassengerDto()
            {
                PassengerId = Passenger.PassengerId,
                PassengerFName = Passenger.PassengerFName,
                PassengerLName = Passenger.PassengerLName,
                PassengerPassport = Passenger.PassengerPassport,
                CheckInStatus = Passenger.CheckInStatus,
                PassengerSeatingClasses = Passenger.PassengerSeatingClasses,
                PassengerSeatingNum = Passenger.PassengerSeatingNum,
                FlightNum= Passenger.Flight.FlightNum,
                FlightId = Passenger.Flight.FlightId

            };
            if (Passenger == null)
            {
                return NotFound();
            }

            return Ok(PassengerDto);
        }

        /// <summary>
        /// Update a passenger information in the system using POST data input
        /// </summary>
        /// <param name="id">Represents the Passenger ID primary key</param>
        /// <param name="passenger">JSON FORM DATA of a passenger</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// POST: api/PassengerData/UpdatePassenger/5
        /// FORM DATA: Passenger JSON Object
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdatePassenger(int id, Passenger passenger)
        {
            Debug.WriteLine("I have reached the update passenger method!");
            if (!ModelState.IsValid)
            {
                Debug.WriteLine("Model state is invalid");
                return BadRequest(ModelState);
            }

            if (id != passenger.PassengerId)
            {
                Debug.WriteLine("ID mismatch");
                Debug.WriteLine("GET paramter" +id);
                Debug.WriteLine("POST paramter" + passenger.PassengerId);
                return BadRequest();
            }

            db.Entry(passenger).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PassengerExists(id))
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


        /// <summary>
        /// Add a passenger to the system
        /// </summary>
        /// <param name="passenger">JSON FORM DATA of a passenger</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: Passenger ID, Passenger Data
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        /// POST: api/PassengerData/AddPassenger
        /// FORM DATA: Passenger JSON Object
        /// </example>

        [ResponseType(typeof(Passenger))]
        [HttpPost]
        public IHttpActionResult AddPassenger(Passenger passenger)
        {
      

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Passengers.Add(passenger);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = passenger.PassengerId }, passenger);
        }






        /// <summary>
        /// Deletes a passenger from the system by it's ID.
        /// </summary>
        /// <param name="id">The primary key of the passenger</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST: api/PassengerData/DeletePassenger/18
        /// FORM DATA: (empty)
        /// </example>
        [ResponseType(typeof(Passenger))]
        [HttpPost]
        public IHttpActionResult DeletePassenger(int id)
        {
            Passenger passenger = db.Passengers.Find(id);
            if (passenger == null)
            {
                return NotFound();
            }

            db.Passengers.Remove(passenger);
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

        private bool PassengerExists(int id)
        {
            return db.Passengers.Count(e => e.PassengerId == id) > 0;
        }
    }
}