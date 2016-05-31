using System.Web;
using System.Web.Optimization;

namespace SimpleCMS
{
    public class BundleMobileConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            SimpleCMSBundleConfig.LoadMobileBundles(bundles);
        }
    }
}