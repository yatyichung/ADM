﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Diagnostics;
using Airport_Management_System.Models;
using System.Web.Script.Serialization;
using Airport_Management_System.Models.ViewModels;

namespace Airport_Management_System.Controllers
{
    public class PassengerController : Controller
    {

        private static readonly HttpClient client;
        JavaScriptSerializer jss = new JavaScriptSerializer();

        static PassengerController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44345/api/");
        }







        // GET: Passenger/List
        public ActionResult List()
        {
            //objective: communicate with our passenger data api to retrieve a list of passengers
            //curl https://localhost:44345/api/passengerdata/listpassengers

           
            string url = "passengerdata/listpassengers";
            HttpResponseMessage response = client.GetAsync(url).Result;

           // Debug.WriteLine("The response code is ");
           // Debug.WriteLine(response.StatusCode);

            IEnumerable<PassengerDto> passengers = response.Content.ReadAsAsync<IEnumerable<PassengerDto>>().Result;
            //Debug.WriteLine("Number of passenger receieved: ");
            //Debug.WriteLine(passengers.Count());

            return View(passengers);
        }

        // GET: Passenger/Details/17
        public ActionResult Details(int id)
        {
            //objective: communicate with our passenger data api to retrieve one passenger
            //curl https://localhost:44345/api/passengerdata/findpassenger/{id}

            string url = "passengerdata/findpassenger/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

           // Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

           PassengerDto SelectedPassenger = response.Content.ReadAsAsync<PassengerDto>().Result;
            //Debug.WriteLine("Passenger receieved: ");
            //Debug.WriteLine(SelectedPassenger.PassengerLName);

            return View(SelectedPassenger);
        }

        public ActionResult Error()
        {
            return View(); 
        }



        // GET: Passenger/New
        public ActionResult New()
        {
            //information about all flights in the system
            //GET api/flightdata/listflights
            string url = "flightdata/listflights";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<FlightDto> FlightsOptions = response.Content.ReadAsAsync<IEnumerable<FlightDto>>().Result;


            return View(FlightsOptions);
        }

        // POST: Passenger/Create
        [HttpPost]
        public ActionResult Create(Passenger passenger)
        {
            Debug.WriteLine("the input passenger payload is: ");
            //Debug.WriteLine(passenger.PassengerFName);

            //objective: add a new passenger into the system using the API
            //curl -H "Content-Type:application/json" -d @passenger.json https://localhost:44345/api/passengerdata/addpassenger
            string url = "passengerdata/addpassenger";


            string jsonpayload = jss.Serialize(passenger);
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


        // POST: Passenger/Edit/17
      public ActionResult Edit(int id)
        {
            UpdatePassenger ViewModel = new UpdatePassenger();

            //the existing passenger information 
            string url = "passengerdata/findpassenger/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            PassengerDto SelectedPassenger = response.Content.ReadAsAsync<PassengerDto>().Result;
            ViewModel.SelectedPassenger = SelectedPassenger;

            // all flights to choose from when updating this passenger
            url = "flightdata/listflights/";
            response = client.GetAsync(url).Result;
           IEnumerable<FlightDto> FlightsOptions = response.Content.ReadAsAsync<IEnumerable<FlightDto>>().Result;

            ViewModel.FlightsOptions = FlightsOptions;
            return View(ViewModel);

        }

        //POST: Passenger/Update/17
        [HttpPost]
        public ActionResult Update(int id, Passenger passenger)
        {
            //objective: update the passenger info in the system
            //curl -H "Content-Type:application/json" -d @passenger.json https://localhost:44345/api/passengerdata/updatepassenger
            string url = "passengerdata/UpdatePassenger/" + id;
            string jsonpayload = jss.Serialize(passenger);

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

         // GET: Passenger/Delete/28
        public ActionResult DeleteConfirm(int id)
        {
            string url = "passengerdata/findpassenger/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            PassengerDto selectedpassenger = response.Content.ReadAsAsync<PassengerDto>().Result;
            return View(selectedpassenger);
        }


        // POST: Passenger/Delete/28
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "passengerdata/deletepassenger/" + id;
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
