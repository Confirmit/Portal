using System.Threading;

namespace Events.Resources
{
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "2.0.0.0")]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resource
    {
        #region Fields

        private static global::System.Resources.ResourceManager resourceMan;
        private static global::System.Globalization.CultureInfo resourceCulture;

        #endregion

        #region Constructor

        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resource(){
        }

        #endregion

        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager
        {
            get
            {
                if (object.ReferenceEquals(resourceMan, null))
                {
                    global::System.Resources.ResourceManager temp;
                    switch (Thread.CurrentThread.CurrentCulture.Name)
                    {
                        case "ru-RU":
                            temp = new global::System.Resources.ResourceManager("Events.Resources.RUResource", typeof(Resource).Assembly);
                            break;

                        case "en-US":
                        default:
                            temp = new global::System.Resources.ResourceManager("Events.Resources.ENResource", typeof(Resource).Assembly);
                            break;
                    }
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }

        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture
        {
            get
            {
                return resourceCulture;
            }
            set
            {
                resourceCulture = value;
            }
        }

        #region Properties

        /// <summary>
        ///   Looks up a localized string similar to Событие:.
        /// </summary>
        public string SLEvent
        {
            get
            {
                return ResourceManager.GetString("SLEvent", resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Дата:.
        /// </summary>
        public string SLEventDate
        {
            get
            {
                return ResourceManager.GetString("SLEventDate", resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Времени:.
        /// </summary>
        public string SLTime
        {
            get
            {
                return ResourceManager.GetString("SLTime", resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to До него осталось:.
        /// </summary>
        public string SLTimeCaption
        {
            get
            {
                return ResourceManager.GetString("SLTimeCaption", resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Из них рабочих:.
        /// </summary>
        public string SLWorkDays
        {
            get
            {
                return ResourceManager.GetString("SLWorkDays", resourceCulture);
            }
        }

        #endregion
    }
}