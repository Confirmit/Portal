using System;
using System.Web.UI.WebControls;

using Core;

using UIPProcess.DataSources;
using UIPProcess.UIP.Navigators;

namespace UIPProcess.Controllers.GridView
{
    public abstract class FilteredGridViewController<TFilter, TEntity> : GridViewDataSourceController<TEntity>
        where TFilter : new()
        where TEntity : BasePlainObject, new()
    {
        public FilteredGridViewController(Navigator navigator)
            : base(navigator)
        {}

        public override void OnDataSourceCreated(Object source, ObjectDataSourceEventArgs e)
        {
            if (Filter == null)
                Filter = new TFilter();

            IDataSourceBase dataSource = e.ObjectInstance as IDataSourceBase;
            if (dataSource == null)
                return;

            dataSource.Filter = Filter;

            base.OnDataSourceCreated(source, e);
        }

        /// <summary>
        /// State mapped property
        /// </summary>
        public virtual TFilter Filter
        {
            get { return default(TFilter); }
            set { }
        }
    }
}