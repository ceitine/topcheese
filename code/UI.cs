using Sandbox;
using Sandbox.Internal;
using Sandbox.UI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace top_n_cheeses
{
	class Slide : Panel
	{
		public string Text { get; set; } = null;
		public string URL { get; set; } = null;

		public Slide()
		{
			UI.Current = this;
			UI.SinceSlide = 0;
		}

		public Slide Initialize()
		{
			if ( Text != null )
			{
				var Label = AddChild<Label>( "Label" );
				Label.Text = Text;
			}
			
			if ( URL != null )
			{
				var Image = AddChild<Image>( "Image" );

				var URI = new Uri( URL );
				var Result = new Http( URI )
					.GetStreamAsync()
					.Result;

				try
				{
					Image.Texture = Sandbox.TextureLoader.Image.Load( Result );

					var Title = Image.AddChild<Label>( "Title" );
					Title.Text = Cheese.GetName();
					Title.Style.FontColor = Color.Random;
				}
				catch
				{
					Log.Error( "whoopsie, next!!" );
					UI.Current?.Delete();
				}
				
			}

			return this;
		}
	}

	class UI : RootPanel
	{
		public static TimeSince SinceSlide;
		public static Slide Current;

		Sound CurrentSong;

		List<string> CheeseQuery;
		int SlideNumber = 0;

		string[] Songs =
		{
			"music-01",
			"music-02",
			"music-03",
			"music-04",
			"music-05",
			"music-06",
			"music-07"
		};

		void GenerateCheese()
		{
			var CheeseJSON = Cheese.Get().Result;
			var Result = Cheese.ToFlickr( CheeseJSON.Substring( 1, CheeseJSON.Length - 2 ) );

			CheeseQuery = Result.Items
				.Select( Res => Res.Media.FirstOrDefault().Value )
				.ToList();
		}

		public UI()
		{
			StyleSheet.Load( "Style.scss" );

			GenerateCheese();

			new Slide { Text = "top n amount of cheeses" }
				.Initialize()
				.Parent = this;

			CurrentSong = Sound.FromScreen( Songs[new Random().Next( 0, Songs.Length - 1 )] );
		}

		public override void Tick()
		{
			if ( CheeseQuery == null || CheeseQuery.Count <= 0 )
				GenerateCheese();

			if ( SinceSlide > 3.5f )
			{
				Current?.Delete();

				if ( !Current.IsVisible )
				{
					if ( Current.Text == null || SlideNumber < 1 )
					{
						SlideNumber++;

						new Slide { Text = $"number n-{SlideNumber}" }
							.Initialize()
							.Parent = this;
					} 
					else
					{
						var URL = CheeseQuery.FirstOrDefault();

						new Slide { URL = URL }
							.Initialize()
							.Parent = this;

						CheeseQuery.Remove( URL );
					}
				}
			}

			if ( CurrentSong.Index == 0 )
				CurrentSong = Sound.FromScreen( Songs[new Random().Next( 0, Songs.Length - 1 )] );
		}
	}
}
