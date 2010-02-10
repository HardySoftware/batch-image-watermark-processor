using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;

namespace HardySoft.CC.OnlinePhotoAlbum.Flickr {
	class FlickrClient : ClientBase<IFlickrApi>, IFlickrApi {
		private string apiKey;

		public FlickrClient(string apiKey) {
			this.apiKey = apiKey;
		}

		public FlickrClient(string apiKey, string endpointConfigurationName) :
			base(endpointConfigurationName) {
			this.apiKey = apiKey;
		}

		public FlickrClient(string apiKey, string endpointConfigurationName, string remoteAddress) :
			base(endpointConfigurationName, remoteAddress) {
			this.apiKey = apiKey;
		}

		public FlickrClient(string apiKey, string endpointConfigurationName, EndpointAddress remoteAddress) :
			base(endpointConfigurationName, remoteAddress) {
			this.apiKey = apiKey;
		}

		public FlickrClient(string apiKey, Binding binding, EndpointAddress remoteAddress) :
			base(binding, remoteAddress) {
			this.ChannelFactory.Endpoint.Behaviors.Add(new WebHttpBehavior());
			this.apiKey = apiKey;
		}

		public FlickrResponse ListInteresting(string extras) {
			return base.Channel.ListInteresting(apiKey, extras);
		}

		public FlickrResponse ListInteresting(string apiKey, string extras) {
			throw new System.NotImplementedException();
		}
	}
}
