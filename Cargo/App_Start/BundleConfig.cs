namespace Cargo
{
	using System.Web.Optimization;

	public class BundleConfig
	{
		public static void RegisterBundles(BundleCollection bundles)
		{
			AddJavaScript(bundles);

			BundleTable.EnableOptimizations = true;
		}

		private static void AddJavaScript(BundleCollection bundles)
		{
			bundles.Add(new ScriptBundle("~/js/cargo")
				.Include("~/content/js/plugins/drag-and-drop.js")
				.Include("~/content/js/cargo/column-mapping.js")
			);
		}
	}
}