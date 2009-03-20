using HardySoft.UI.BatchImageProcessor.View;

namespace HardySoft.UI.BatchImageProcessor.Presenter {
	public abstract class Presenter<TView> where TView : IView {
		public TView View {
			get;
			set;
		}

		public abstract void SetView(TView view);

		/*public virtual void OnViewInitialized() {
		}

		public virtual void OnViewLoaded() {
		}*/
	}
}
