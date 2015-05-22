using System;
using System.Collections.Generic;

namespace UIPProcess.DataSources
{
    /// <summary>
    /// Interface which should be implemented by any <b>Object Data Source</b>
    /// for Persistent Entities.
    /// </summary>
    public interface IDataSourceBase
    {
        object Filter { set; get; }
    }

    public abstract class DataSourceBase<TObject> : IDataSourceBase
    {
        public abstract int SelectCount();

        public abstract IList<TObject> Select(String SortExpression, int maximumRows, int startRowIndex);

        public abstract object Filter { set; get; }
    }
}