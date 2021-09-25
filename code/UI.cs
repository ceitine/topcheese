using Sandbox;
using Sandbox.Internal;
using Sandbox.UI;
using System;

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

					/*var Title = Image.AddChild<Label>( "Title" );
					Title.Text = Cheese.GetName();
					Title.Style.FontColor = Color.Random;*/
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
		int SlideNumber = 0;

		WMMRendering rendering;

		bool isActive = false;

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

		public UI()
		{
			StyleSheet.Load( "Style.scss" );

			AddChild<Panel>( "Background" );
			rendering = AddChild<WMMRendering>();

			_ = Cheese.Get( StartSlideshow );
		}

		public void StartSlideshow()
		{
			rendering.Delete();

			new Slide { Text = "top n amount of cheeses" }
				.Initialize()
				.Parent = this;

			CurrentSong = Sound.FromScreen( Songs[new Random().Next( 0, Songs.Length - 1 )] );

			isActive = true;
		}

		public override void Tick()
		{
			if ( !isActive )
				return;

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
						new Slide { URL = Cheese.Pool[(SlideNumber - 1) % Cheese.Pool.Count] }
							.Initialize()
							.Parent = this;
					}
				}
			}

			if ( CurrentSong.Index == 0 )
				CurrentSong = Sound.FromScreen( Songs[new Random().Next( 0, Songs.Length - 1 )] );
		}
	}
}
