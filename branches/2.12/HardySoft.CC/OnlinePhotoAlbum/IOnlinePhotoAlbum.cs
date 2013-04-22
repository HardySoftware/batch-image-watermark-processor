using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HardySoft.CC.OnlinePhotoAlbum {
	public interface IOnlinePhotoAlbum {
		void Authenticate(string username, string password);
		PhotoCollection ListInteresting();
	}
}
