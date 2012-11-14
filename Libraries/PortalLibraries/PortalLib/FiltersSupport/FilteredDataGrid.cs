using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using ConfirmIt.PortalLib.DataSource;
using UIPProcess.DataSources;

namespace ConfirmIt.PortalLib.FiltersSupport
{
    public abstract class FilteredDataGrid : UserControl
    {
        #region Fields

        private IFilterControl m_FilterControl;

        #endregion

        #region Properties

        /// <summary>
        /// Filter control.
        /// </summary>
        public IFilterControl FilterControl
        {
            get { return m_FilterControl; }
            set { m_FilterControl = value; }
        }

        #endregion

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (FilterControl != null && FilterControl.FilterChanged)
                BindData();
        }

        protected void OnObjectDataSourceCreated(object sender, ObjectDataSourceEventArgs e)
        {
            PrepareDataSourceObject(e.ObjectInstance as IDataSourceBase);
        }

        public virtual void PrepareDataSourceObject(IDataSourceBase objectDataSource)
        {
            if (FilterControl == null)
                return;

            objectDataSource.Filter = FilterControl.Filter;
        }

        public abstract void BindData();
    }
}