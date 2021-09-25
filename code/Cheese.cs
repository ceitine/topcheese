using Sandbox.Internal;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace top_n_cheeses
{
	class Cheese
	{
		public static readonly Uri STATS_API_URL = new( "https://flickr-cheese-api.herokuapp.com/api/stats" );
		public static readonly string API_URL = "https://flickr-cheese-api.herokuapp.com/api/cheese/";
		public static List<string> Pool = new();

		public delegate void CheeseCallback();

		public static async Task<int> GetPagesCount()
		{
			while ( true )
			{
				string Result;
				try
				{
					Result = await new Http( STATS_API_URL )
					  .GetStringAsync();
				}
				catch ( Exception )
				{
					continue;
				}

				var cpc = JsonSerializer.Deserialize<StatsScheme>( Result, new JsonSerializerOptions()
				{
					PropertyNameCaseInsensitive = true
				} ).CheesePagesCount;
				if ( cpc >= 0 )
					return cpc;
				await Task.Delay( 5000 );
			}
		}

		public static async Task<string> GetPage( int page )
		{
			Uri URI = new( API_URL + page.ToString() );
			return await new Http( URI )
				.GetStringAsync();
		}

		public static async Task Get( CheeseCallback callback )
		{
			var cpc = await GetPagesCount();

			for ( int i = 0; i < cpc; i++ )
			{
				var page = await GetPage( i );
				Pool.AddRange( page.Split( '\n' ) );

				if ( i == 0 )
				{
					callback();
				}
			}
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
	}
}
