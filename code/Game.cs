using Sandbox;

namespace top_n_cheeses
{
	class TopCheese : Game
	{
		public TopCheese()
		{
			if ( IsClient )
				new UI();
		}
	}
}
