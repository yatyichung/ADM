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

        // GET: api/PassengerData/ListPassengers
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

        // GET: api/PassengerData/FindPassenger/17
        // GET: api/PassengerData/FindPassenger/26

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
        

        // POST: api/PassengerData/UpdatePassenger/5
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

        // POST: api/PassengerData/AddPassenger
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






        // POST: api/PassengerData/DeletePassenger/18
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