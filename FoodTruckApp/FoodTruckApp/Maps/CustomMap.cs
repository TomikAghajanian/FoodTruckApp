using System.Collections.Generic;
using Xamarin.Forms.Maps;

namespace FoodTruckApp.Maps
{
	public class CustomMap: Map
	{
		public List<CustomPin> CustomPins { get; set; }
	}
}
