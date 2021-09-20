using Sandbox.Internal;
using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using topncheeses;

namespace top_n_cheeses
{
	class Cheese
	{
		public static readonly string API_URL = "https://api.flickr.com/services/feeds/photos_public.gne"
			+ "?format=json"
			+ "&tags=cheese"
			+ "&lang=en-us"
			+ "&format=json"
			+ "&jsoncallback=";
		public static async Task<string> Get()
		{
			var URI = new Uri( API_URL );
			var Result = await new Http( URI )
				.GetStringAsync();

			return Result;
		}

		static string[] Prefixes = { "cool", "epic", "Swagness", "ball", "sweaty", "cum", "piss", "shid", "fart", "Nerdy", "stanky", "shart", "nutty", "fortnite" };
		static string[] Cheeses = { "CheddaR", "Parmesane", "cockcheese", "mozereal", "Chdere", "eleMntal", "goDa", "feta", "mascaraBoner", "shart", "swag meal" };

		public static string GetName()
		{
			var Rand = new Random();
			var Result = "";

			if ( Rand.Next( 0, 1 ) == 0 )
				Result += " " + Prefixes[Rand.Next( 0, Prefixes.Length )];

			if ( Rand.Next( 0, 10 ) == 10 )
				Result += " " + Prefixes[Rand.Next( 0, Prefixes.Length )];

			Result += " " + Cheeses[Rand.Next( 0, Cheeses.Length )];

			return Result;
		}

		public static FlickrResult ToFlickr( string JSON )
		{
			return JsonSerializer.Deserialize<FlickrResult>( JSON );
		}
	}
}
