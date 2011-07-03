using System.ServiceModel;
using System.ServiceModel.Web;

namespace HardySoft.CC.OnlinePhotoAlbum.Flickr {
	[ServiceContract]
	[XmlSerializerFormat]
	public interface IFlickrApi {
		[OperationContract]
		[WebGet(BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Xml,
			UriTemplate = "?method=flickr.interestingness.getList&api_key={apiKey}&extras={extras}")]
		FlickrResponse ListInteresting(string apiKey, string extras);
	}
}
