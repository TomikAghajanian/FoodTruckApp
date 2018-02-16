using System;
using System.Collections.Generic;
using System.Text;

namespace FoodTruckApp.DeserializeJSON
{
    class FoodTruck
    {
        public int foodTruckID { get; set; }
        public string licensePlate { get; set; }
        public string truckMake { get; set; }
        public string truckModel { get; set; }
        public string color { get; set; }
        public string name { get; set; }
        public int year { get; set; }
        public int mealType { get; set; }
        public int cuisineCategory { get; set; }
        public Driver driver { get; set; }
        public CookInfo cookInfo { get; set; }
        public int maxCapacityPerMeal { get; set; }
        public string contacts { get; set; }
        public string startDate { get; set; }
        public string healthCode { get; set; }
        public string description { get; set; }
        public string areaOfOperation { get; set; }
        public string areaOfOperationString { get; set; }
        public string additionalInfo { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
    }
}
