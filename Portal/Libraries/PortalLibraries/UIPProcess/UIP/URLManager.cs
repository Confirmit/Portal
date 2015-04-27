using System;
using System.Globalization;
using System.Web;

namespace UIProcess
{
    internal class URLManager
    {
        public static String CutLeftPart(String viewFilePath)
        {
            String strUrlAuthority = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);
            viewFilePath = cutOffLeftPartIfExist(viewFilePath, strUrlAuthority);

            String viewAppPath = HttpContext.Current.Request.ApplicationPath;
            viewFilePath = cutOffLeftPartIfExist(viewFilePath, viewAppPath);

            return cutOffLeftPartIfExist(viewFilePath, "/");
        }

        private static String cutOffLeftPartIfExist(String src, String check)
        {
            if (String.Compare(src, 0, check, 0, check.Length
                               , true, CultureInfo.InvariantCulture) == 0)
            {
                return src.Substring(check.Length, src.Length - check.Length);
            }

            return src;
        }
    }
}