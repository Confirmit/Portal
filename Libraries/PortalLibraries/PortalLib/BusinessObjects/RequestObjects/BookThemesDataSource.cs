using System;
using System.Data;
using System.Collections.Generic;

using UIPProcess.DataSources;
using ConfirmIt.PortalLib.DataSource;
using Confirmit.PortalLib.BusinessObjects.RequestObjects;
using ConfirmIt.PortalLib.DAL;

namespace ConfirmIt.PortalLib.BusinessObjects.RequestObjects
{
    public class BookThemesDataSource : DataSourceBase<BookTheme>
    {
        public override int SelectCount()
        {
            return SiteProvider.RequestObjects.GetAllBookThemesCount();
        }

        public override IList<BookTheme> Select(string SortExpression, int maximumRows, int startRowIndex)
        {
            return SiteProvider.RequestObjects.GetAllBookThemes(SortExpression, maximumRows, startRowIndex);
        }

        public override object Filter
        {
            get { return null; }
            set { }
        }
    }
}