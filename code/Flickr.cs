using System.Collections.Generic;
using System.Text.Json.Serialization;


namespace topncheeses
{
	struct FlickrPostResult
	{
		[JsonPropertyName( "title" )]
		public string Title { get; set; }

		[JsonPropertyName( "link" )]
		public string Link { get; set; }

		[JsonPropertyName( "media" )]
		public Dictionary<string, string> Media { get; set; }

		[JsonPropertyName( "author" )]
		public string Author { get; set; }

		[JsonPropertyName( "tags" )]
		public string Tags { get; set; }
	}

	struct FlickrResult
	{
		[JsonPropertyName( "title" )]
		public string Title { get; set; }

		[JsonPropertyName( "link" )]
		public string Link { get; set; }

		[JsonPropertyName( "description" )]
		public string Description { get; set; }

		[JsonPropertyName( "items" )]
		public IEnumerable<FlickrPostResult> Items { get; set; }
	}
}
