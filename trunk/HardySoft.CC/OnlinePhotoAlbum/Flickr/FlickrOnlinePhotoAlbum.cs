using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Channels;
using System.ServiceModel;

namespace HardySoft.CC.OnlinePhotoAlbum.Flickr {
	public class FlickrOnlinePhotoAlbum : IOnlinePhotoAlbum {
		private string username;
		private string password;
		private string api_key;
		private FlickrClient flickrClient;

		public FlickrOnlinePhotoAlbum(string api_key) {
			this.api_key = api_key;

			Binding binding = new WebHttpBinding(WebHttpSecurityMode.None);
			EndpointAddress address = new EndpointAddress("http://api.flickr.com/services/rest");
			this.flickrClient = new FlickrClient(this.api_key, binding, address);
		}

		public void Authenticate(string username, string password) {
			this.username = username;
			this.password = password;
		}

		public PhotoCollection ListInteresting() {
			FlickrResponse flickrResponse = flickrClient.ListInteresting("");

			var photoCollection = new PhotoCollection();
			List<Photo> photos = new List<Photo>();
			for (int i = 0; i < flickrResponse.PhotoCollection.Photos.Length; i++) {
				photos.Add(new Photo() {
					ID = flickrResponse.PhotoCollection.Photos[i].Id,
					Title = flickrResponse.PhotoCollection.Photos[i].Title
				});
			}

			photoCollection.Photos = photos.ToArray();

			return photoCollection;
		}
	}
}
