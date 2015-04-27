using System;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media.Imaging;
using System.Windows.Resources;

namespace Helpers
{
    public class ResourceManager
    {
        /// <summary>
        /// Get Resource dictionary from xaml resource file.
        /// </summary>
        /// <param name="resourceName">Resource name.</param>
        /// <returns>ResourceDictionary.</returns>
        public static ResourceDictionary GetResourceDictionary(string resourceName)
        {
            return GetResourceDictionary(Assembly.GetExecutingAssembly(), resourceName);
        }

        /// <summary>
        /// Get Resource dictionary from xaml resource file.
        /// </summary>
        /// <param name="assembly">Assembly of resource.</param>
        /// <param name="resourceName">Resource name.</param>
        /// <returns>ResourceDictionary.</returns>
        public static ResourceDictionary GetResourceDictionary(Assembly assembly, string resourceName)
        {
            using(Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                    return null;

                string xaml = new StreamReader(stream).ReadToEnd();
                return (ResourceDictionary) XamlReader.Load(xaml);
            }
        }

        public static string ExecutingAssemblyName
        {
            get { return GetAssemblyName(Assembly.GetExecutingAssembly()); }
        }

        public static string GetAssemblyName(Assembly assembly)
        {
            string name = assembly.FullName;
            return (name != null)
                       ? name.Substring(0, name.IndexOf(','))
                       : string.Empty;
        }

        public static Stream GetStream(string relativeUri, string assemblyName)
        {
            StreamResourceInfo res = Application.GetResourceStream(new Uri(assemblyName + ";component/" + relativeUri, UriKind.Relative));
            if (res == null)
                res = Application.GetResourceStream(new Uri(relativeUri, UriKind.Relative));

            return res != null
                       ? res.Stream
                       : null;
        }

        public static Stream GetStream(string relativeUri)
        {
            return GetStream(relativeUri, ExecutingAssemblyName);
        }

        /// <summary>
        /// Return bitmap resource of executing assembly.
        /// </summary>
        /// <param name="relativeUri">Path to resource.</param>
        public static BitmapImage GetBitmap(string relativeUri)
        {
            return GetBitmap(relativeUri, ExecutingAssemblyName);
        }

        /// <summary>
        /// Return bitmap resource.
        /// </summary>
        /// <param name="relativeUri">Path to resource.</param>
        /// <param name="assemblyName">Assembly name.</param>
        public static BitmapImage GetBitmap(string relativeUri, string assemblyName)
        {
            Stream s = GetStream(relativeUri, assemblyName);
            if (s == null) 
                return null;

            using (s)
            {
                BitmapImage bmp = new BitmapImage();
                bmp.SetSource(s);
                return bmp;
            }
        }

        public static string GetString(string relativeUri)
        {
            return GetString(relativeUri, ExecutingAssemblyName);
        }

        public static string GetString(string relativeUri, string assemblyName)
        {
            Stream s = GetStream(relativeUri, assemblyName);
            if (s == null) 
                return null;

            using (StreamReader reader = new StreamReader(s))
            {
                return reader.ReadToEnd();
            }
        }

        public static FontSource GetFontSource(string relativeUri)
        {
            return GetFontSource(relativeUri, ExecutingAssemblyName);
        }

        public static FontSource GetFontSource(string relativeUri, string assemblyName)
        {
            Stream s = GetStream(relativeUri, assemblyName);
            if (s == null) return null;
            using (s)
            {
                return new FontSource(s);
            }
        }

        public static object GetXamlObject(string relativeUri)
        {
            return GetXamlObject(relativeUri, ExecutingAssemblyName);
        }

        public static object GetXamlObject(string relativeUri, string assemblyName)
        {
            string str = GetString(relativeUri, assemblyName);
            if (str == null) return null;
            object obj = XamlReader.Load(str);
            return obj;
        }
    }
}
