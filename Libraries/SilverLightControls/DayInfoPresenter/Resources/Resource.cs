using System.Threading;

namespace DayInfoPresenter.Resources
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
                            temp = new global::System.Resources.ResourceManager("DayInfoPresenter.Resources.RUResource", typeof(Resource).Assembly);
                            break;

                        case "en-US":
                        default:
							temp = new global::System.Resources.ResourceManager("DayInfoPresenter.Resources.ENResource", typeof(Resource).Assembly);
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
        ///   Looks up a localized string.
        /// </summary>
        public string SLWorkedTime
        {
            get
            {
                return ResourceManager.GetString("SLWorkedTime", resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string.
        /// </summary>
        public string SLMustWorkTime
        {
            get
            {
                return ResourceManager.GetString("SLMustWorkTime", resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string.
        /// </summary>
        public string SLWeekTime
        {
            get
            {
                return ResourceManager.GetString("SLWeekTime", resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string.
        /// </summary>
        public string SLMonthTime
        {
            get
            {
                return ResourceManager.GetString("SLMonthTime", resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string.
        /// </summary>
        public string SLTill
        {
            get
            {
                return ResourceManager.GetString("SLTill", resourceCulture);
            }
        }

        #region Time Properties

        public string msg_HoursI
        {
            get
            {
                return ResourceManager.GetString("msg_HoursI", resourceCulture);
            }
        }

        public string msg_HoursR1
        {
            get
            {
                return ResourceManager.GetString("msg_HoursR1", resourceCulture);
            }
        }

        public string msg_HoursRm
        {
            get
            {
                return ResourceManager.GetString("msg_HoursRm", resourceCulture);
            }
        }

        public string msg_MinutesI
        {
            get
            {
                return ResourceManager.GetString("msg_MinutesI", resourceCulture);
            }
        }

        public string msg_MinutesR1
        {
            get
            {
                return ResourceManager.GetString("msg_MinutesR1", resourceCulture);
            }
        }

        public string msg_MinutesRm
        {
            get
            {
                return ResourceManager.GetString("msg_MinutesRm", resourceCulture);
            }
        }

        public string msg_SecondsI
        {
            get
            {
                return ResourceManager.GetString("msg_SecondsI", resourceCulture);
            }
        }

        public string msg_SecondsR1
        {
            get
            {
                return ResourceManager.GetString("msg_SecondsR1", resourceCulture);
            }
        }

        public string msg_SecondsRm
        {
            get
            {
                return ResourceManager.GetString("msg_SecondsRm", resourceCulture);
            }
        }

        #endregion

        #region Grid Headers

        public string headerBegin
        {
            get
            {
                return ResourceManager.GetString("headerBegin", resourceCulture);
            }
        }

        public string headerEnd
        {
            get
            {
                return ResourceManager.GetString("headerEnd", resourceCulture);
            }
        }

        public string headerDuration
        {
            get
            {
                return ResourceManager.GetString("headerDuration", resourceCulture);
            }
        }

        public string headerEventType
        {
            get
            {
                return ResourceManager.GetString("headerEventType", resourceCulture);
            }
        }

        #endregion

        #region Buttons Caption

        public string btnWorkBegin
        {
            get
            {
                return ResourceManager.GetString("btnWorkBegin", resourceCulture);
            }
        }

        public string btnWorkEnd
        {
            get
            {
                return ResourceManager.GetString("btnWorkEnd", resourceCulture);
            }
        }

        public string btnTimeOff
        {
            get
            {
                return ResourceManager.GetString("btnTimeOff", resourceCulture);
            }
        }

        public string btnTimeOn
        {
            get
            {
                return ResourceManager.GetString("btnTimeOn", resourceCulture);
            }
        }

        public string btnDinnerOn
        {
            get
            {
                return ResourceManager.GetString("btnDinnerOn", resourceCulture);
            }
        }

        public string btnDinnerOff
        {
            get
            {
                return ResourceManager.GetString("btnDinnerOff", resourceCulture);
            }
        }

        public string btnStudyOn
        {
            get
            {
                return ResourceManager.GetString("btnStudyOn", resourceCulture);
            }
        }

        public string btnStudyOff
        {
            get
            {
                return ResourceManager.GetString("btnStudyOff", resourceCulture);
            }
        }

        #endregion

        #endregion
    }
}