using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace HardySoft.CC.OnlinePhotoAlbum.Flickr {
	[XmlRoot("rsp")]
	public class FlickrResponse {
		[XmlElement("photos")]
		public FlickrPhotoCollection PhotoCollection {
			get;
			set;
		}

		[XmlAttribute("stat")]
		public string Status {
			get;
			set;
		}
	}

	[Serializable]
	public class FlickrPhotoCollection {
		[XmlAttribute("page")]
		public int Page {
			get;
			set;
		}

		[XmlElement("photo")]
		public FlickrPhoto[] Photos {
			get;
			set;
		}
	}

	public class FlickrPhoto {
		[XmlAttribute("id")]
		public string Id {
			get;
			set;
		}

		[XmlAttribute("title")]
		public string Title {
			get;
			set;
		}
	}
}
