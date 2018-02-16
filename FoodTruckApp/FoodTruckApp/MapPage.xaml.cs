
using FoodTruckApp.DataMapping;
using FoodTruckApp.DeserializeJSON;
using FoodTruckApp.Maps;
using FoodTruckApp.Utilities;
using Newtonsoft.Json;
using Plugin.Geolocator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace FoodTruckApp
{
    public partial class MapPage : ContentPage
    {

        private string tag = "GoogleMapScreen.cs";
        public MapPage()
        {

            InitializeComponent();
            
            //change emulator's coordinates to the following for testing => long: -118.28, Lat: 34.16
            //JSON string simulating a response from api call
            var jsonStr = "{\"foodTrucks\":[{\"foodTruckID\":1,\"licensePlate\":\"lp_1\",\"truckMake\":\"RAM\",\"truckModel\":\"1500\",\"color\":\"red\",\"name\":\"My Food Truck 1\",\"year\":2010,\"mealType\":1,\"cuisineCategory\":1,\"driver\":{\"personalInfoID\":2,\"firstName\":\"armen1\",\"lastName\":\"Keshishian1\",\"middleName\":null,\"ssn\":\"102323256\",\"dateOfBirth\":null,\"contacts\":null,\"phoneNumbers\":null,\"role\":1,\"address\":null},\"cookInfo\":{\"personalInfoID\":1,\"firstName\":\"armen1\",\"lastName\":\"keshish\",\"middleName\":null,\"ssn\":\"102154546\",\"dateOfBirth\":null,\"contacts\":null,\"phoneNumbers\":null,\"role\":1,\"address\":null},\"maxCapacityPerMeal\":20,\"contacts\":null,\"startDate\":\"2017-09-04T15:30:42\",\"healthCode\":\"fthcode_1\",\"description\":\"\",\"areaOfOperation\":null,\"areaOfOperationString\":null,\"additionalInfo\":null,\"latitude\":34.18084,\"longitude\":-118.30897},{\"foodTruckID\":2,\"licensePlate\":\"lp_2\",\"truckMake\":\"RAM 2\",\"truckModel\":\"2500\",\"color\":\"blue\",\"name\":\"My Food Truck 2\",\"year\":2012,\"mealType\":2,\"cuisineCategory\":2,\"driver\":{\"personalInfoID\":4,\"firstName\":\"armen1\",\"lastName\":\"Keshishian1\",\"middleName\":null,\"ssn\":\"102323256\",\"dateOfBirth\":null,\"contacts\":null,\"phoneNumbers\":null,\"role\":1,\"address\":null},\"cookInfo\":{\"personalInfoID\":3,\"firstName\":\"armen2\",\"lastName\":\"Keshishian1\",\"middleName\":null,\"ssn\":\"102323256\",\"dateOfBirth\":null,\"contacts\":null,\"phoneNumbers\":null,\"role\":1,\"address\":null},\"maxCapacityPerMeal\":30,\"contacts\":null,\"startDate\":\"2017-09-04T15:31:28\",\"healthCode\":\"fthcode_2\",\"description\":\"\",\"areaOfOperation\":null,\"areaOfOperationString\":null,\"additionalInfo\":null,\"latitude\":34.14251,\"longitude\":-118.25507},{\"foodTruckID\":3,\"licensePlate\":\"lp_3\",\"truckMake\":\"RAM 3\",\"truckModel\":\"3500\",\"color\":\"white\",\"name\":\"My Food Truck 3\",\"year\":2013,\"mealType\":2,\"cuisineCategory\":2,\"driver\":{\"personalInfoID\":6,\"firstName\":null,\"lastName\":null,\"middleName\":null,\"ssn\":null,\"dateOfBirth\":null,\"contacts\":null,\"phoneNumbers\":null,\"role\":0,\"address\":null},\"cookInfo\":{\"personalInfoID\":5,\"firstName\":null,\"lastName\":null,\"middleName\":null,\"ssn\":null,\"dateOfBirth\":null,\"contacts\":null,\"phoneNumbers\":null,\"role\":0,\"address\":null},\"maxCapacityPerMeal\":50,\"contacts\":null,\"startDate\":\"2017-09-04T15:32:01\",\"healthCode\":\"fthcode_3\",\"description\":\"\",\"areaOfOperation\":null,\"areaOfOperationString\":null,\"additionalInfo\":null,\"latitude\":34.149353,\"longitude\":-118.2446},{\"foodTruckID\":4,\"licensePlate\":\"lp_4\",\"truckMake\":\"RAM 3\",\"truckModel\":\"3500\",\"color\":\"black\",\"name\":\"My Food Truck 4\",\"year\":2013,\"mealType\":2,\"cuisineCategory\":2,\"driver\":{\"personalInfoID\":8,\"firstName\":null,\"lastName\":null,\"middleName\":null,\"ssn\":null,\"dateOfBirth\":null,\"contacts\":null,\"phoneNumbers\":null,\"role\":0,\"address\":null},\"cookInfo\":{\"personalInfoID\":7,\"firstName\":null,\"lastName\":null,\"middleName\":null,\"ssn\":null,\"dateOfBirth\":null,\"contacts\":null,\"phoneNumbers\":null,\"role\":0,\"address\":null},\"maxCapacityPerMeal\":50,\"contacts\":null,\"startDate\":\"2017-09-04T15:32:21\",\"healthCode\":\"fthcode_4\",\"description\":\"\",\"areaOfOperation\":null,\"areaOfOperationString\":null,\"additionalInfo\":null,\"latitude\":34.14992,\"longitude\":-118.26}]}";

            //Deserializing JSON string to an object
            FoodTrucks deserializedJSONObj = null;
            try
            {
                deserializedJSONObj = JsonConvert.DeserializeObject<FoodTrucks>(jsonStr);
            }
            catch (Exception error)
            {
               // Log.Error(tag, error.Message, "Unexpected internal error");
                return;
            }

            //GetLocation();

            DisplayTrucksOnMap(deserializedJSONObj);

            FindCorrectZoomLevel(deserializedJSONObj);
        }

        private async void GetLocation()
        {

            //Getting current users location
            await RetrieveLocation();

        }

        private async Task RetrieveLocation()
        {
            try
            {
                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 20;

               // Log.Debug(tag, "Getting user's current location...");
                var position = await locator.GetPositionAsync(TimeSpan.FromMilliseconds(100));

               // Log.Debug(tag, "Received user's current location. lat: " + position.Latitude + " long: " + position.Longitude);
            }
            catch (Exception e)
            {
               // Log.Error(tag, e.Message, "Unexpected internal error");
                return;
            }
        }

        private void DisplayTrucksOnMap(FoodTrucks fTrucks)
        {
            List<CustomPin> customPins = new List<CustomPin>();
            int numOfTrucksAvailable = fTrucks.foodTrucks.Length;
           // Log.Debug(tag, "Found " + numOfTrucksAvailable + " Trucks.");

            for (int i = 0; i < numOfTrucksAvailable; i++)
            {
                string cuisinCategory = DataTranslateCuisineCategory(fTrucks, i);
               // Log.Debug(tag, "Truck number " + i + " cuisine category: " + cuisinCategory);

                string mealType = DataTranslateMealType(fTrucks, i);
              //  Log.Debug(tag, "Truck number " + i + " meal type: " + mealType);

                var pin = new CustomPin
                {
                    Type = PinType.Place,
                    Position = new Position(fTrucks.foodTrucks[i].latitude, fTrucks.foodTrucks[i].longitude),
                    Label = fTrucks.foodTrucks[i].name + "\nCuisine Category: " + cuisinCategory + "\nMeal Type: " + mealType,
                    Id = "FoodTruck" + i,
                    Url = "google.com"
                };

                customPins.Add(pin);
                customMap.CustomPins = customPins;
                customMap.Pins.Add(pin);
            }
        }

        private string DataTranslateCuisineCategory(FoodTrucks fTrucks, int i)
        {
            string cuisinCategory;
            switch (fTrucks.foodTrucks[i].cuisineCategory)
            {
                case (int)CuisineCategoryEnum.None:
                    cuisinCategory = "No Cuisine Category Available";
                    break;
                case (int)CuisineCategoryEnum.Asian:
                    cuisinCategory = "Asian";
                    break;
                case (int)CuisineCategoryEnum.Hawaiian:
                    cuisinCategory = "Hawaiian";
                    break;
                case (int)CuisineCategoryEnum.MiddleEastern:
                    cuisinCategory = "MiddleEastern";
                    break;
                case (int)CuisineCategoryEnum.Other:
                    cuisinCategory = "All Cuisine Types";
                    break;
                default:
                    cuisinCategory = "Not Specified";
                    break;
            }

            return cuisinCategory;
        }

        private string DataTranslateMealType(FoodTrucks fTrucks, int i)
        {
            string mealType;
            switch (fTrucks.foodTrucks[i].mealType)
            {
                case (int)MealTypeEnum.None:
                    mealType = "No Meal Type Available";
                    break;
                case (int)MealTypeEnum.Breakfast:
                    mealType = "Breakfast";
                    break;
                case (int)MealTypeEnum.Lunch:
                    mealType = "Lunch";
                    break;
                case (int)MealTypeEnum.Dinner:
                    mealType = "Dinner";
                    break;
                case (int)MealTypeEnum.Other:
                    mealType = "All Meal Types";
                    break;
                default:
                    mealType = "Not Specified";
                    break;
            }

            return mealType;
        }

        private void FindCorrectZoomLevel(FoodTrucks fTrucks)
        {
            List<double> lats = new List<double>();
            List<double> lngs = new List<double>();

            for (int i = 0; i < fTrucks.foodTrucks.Length; i++)
            {
                lats.Add(fTrucks.foodTrucks[i].latitude);
                lngs.Add(fTrucks.foodTrucks[i].longitude);
            }

            double lowestLat = lats.Min();
            double highestLat = lats.Max();
            double lowestLong = lngs.Min();
            double highestLong = lngs.Max();
            double finalLat = (lowestLat + highestLat) / 2;
            double finalLong = (lowestLong + highestLong) / 2;
            double distance = DistanceCalculation.GeoCodeCalc.CalcDistance(lowestLat, lowestLong, highestLat, highestLong, DistanceCalculation.GeoCodeCalcMeasurement.Kilometers);

            try
            {
                customMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(finalLat, finalLong), Distance.FromKilometers(distance)));
            }
            catch (Exception e)
            {
               // Log.Error(tag, e.Message, "Unexpected internal error");
                return;
            }
        }
    }
}