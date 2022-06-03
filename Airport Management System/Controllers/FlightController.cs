using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Diagnostics;
using Airport_Management_System.Models;
using Airport_Management_System.Models.ViewModels;
using System.Web.Script.Serialization;

namespace Airport_Management_System.Controllers
{
    public class FlightController : Controller
    {

        private static readonly HttpClient client;
        JavaScriptSerializer jss = new JavaScriptSerializer();

        static FlightController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44345/api/");
        }

        // GET: Flight/List
        public ActionResult List()
        {
            string url = "flightdata/listflights";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<FlightDto> flights = response.Content.ReadAsAsync<IEnumerable<FlightDto>>().Result;

            return View(flights);
        }

        // GET: Flight/Details/21
        public ActionResult Details(int id)
        {
            //objective: communicate with our flight data api to retrieve one flight
            //curl https://localhost:44345/api/flightdata/findflight/{id}

            DetailsFlight ViewModel = new DetailsFlight();

            string url = "flightdata/findflight/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            // Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            FlightDto SelectedFlight = response.Content.ReadAsAsync<FlightDto>().Result;
            //Debug.WriteLine("Passenger receieved: ");
            //Debug.WriteLine(selectedpassenger.PassengerLName);

            ViewModel.SelectedFlight = SelectedFlight;
            //showcase informatino about passengers related to this flight
            //send a request to gather information about passengers related to a particular flight ID
            url = "passengerdata/listpassengersforflight/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<PassengerDto> RelatedPassengers = response.Content.ReadAsAsync<IEnumerable<PassengerDto>>().Result; ;

            ViewModel.RelatedPassengers = RelatedPassengers;

            return View(ViewModel);
        }

        // GET: Flight/New
        public ActionResult New()
        {
            return View();
        }

        // POST: Flight/Create
        [HttpPost]
        public ActionResult Create(Flight flight)
        {
            Debug.WriteLine("the input flight payload is: ");
            //Debug.WriteLine(flight.FlightNum);

            //objective: add a new passenger into the system using the API
            //curl -H "Content-Type:application/json" -d @flight.json https://localhost:44345/api/flightdata/addflight
            string url = "flightdata/addflight";


            string jsonpayload = jss.Serialize(flight);
            Debug.WriteLine(jsonpayload);


            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";


            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // POST: Flight/Edit/21
        public ActionResult Edit(int id)
        {
            string url = "flightdata/findflight/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            FlightDto selectedflight = response.Content.ReadAsAsync<FlightDto>().Result;
            return View(selectedflight);

        }
        //POST: Flight/Update/21
        [HttpPost]
        public ActionResult Update(int id, Flight flight)
        {
            //objective: update the flight info in the system
            //curl -H "Content-Type:application/json" -d @flight.json https://localhost:44345/api/flightdata/updateflight
            string url = "flightdata/UpdateFlight/" + id;
            string jsonpayload = jss.Serialize(flight);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            Debug.WriteLine(content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }

        }

        // GET: Flight/Delete/21
        public ActionResult DeleteConfirm(int id)
        {
            string url = "flightdata/findflight/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            FlightDto selectedflight = response.Content.ReadAsAsync<FlightDto>().Result;
            return View(selectedflight);
        }


        // POST: Flight/Delete/28
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "flightdata/deleteflight/" + id;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");

            }
            else
            {
                return RedirectToAction("Error");
            }
        }




    }
}
