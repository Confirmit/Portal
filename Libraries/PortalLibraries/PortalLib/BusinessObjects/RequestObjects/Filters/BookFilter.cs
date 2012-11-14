using System;
using System.Diagnostics;
using System.Collections.Generic;

using ConfirmIt.PortalLib.FiltersSupport;
using Core.ORM.Attributes;

namespace Confirmit.PortalLib.BusinessObjects.RequestObjects.Filters
{
    [DBTable("Books_Books", true)]
    public class BookFilter : RequestObjectFilter
    {
        #region [ Constructors ]

        [DebuggerStepThrough]
        public BookFilter()
        {
            FromPublishingYear = 1900;
            ToPublishingYear = DateTime.Now.Year;
        }

        #endregion

        #region [ Properties ]

        [DBFilterField("Authors")]
        public string Authors { get; set; }

        [DBFilterField("Annotation")]
        public string Annotation { get; set; }

        [DBFilterTable("ID", "Books_BookThemes", "BookID", "ThemeID", Operator = "IN", IsObjectProperty = false)]
        public IList<int> Themes { get; set; }

        [DBFilterField("PublishingYear", 1900, Operator = ">=" )]
        public int FromPublishingYear { get; set; }

        [DBFilterField("PublishingYear", Operator = "<=")]
        public int ToPublishingYear { get; set; }

        [DBFilterField("Language")]
        public string Language { get; set; }

        [DBFilterField("IsElectronic")]
        public bool IsElectronic { get; set; }
        
        #endregion
    }
}