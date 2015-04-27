using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConfirmIt.PortalLib.DataSource
{
    public abstract class ObjectDataSource
    {
        public abstract System.Data.DataSet Select(string SortExpression, int maximumRows, int startRowIndex);

        public abstract int SelectCount();

        public abstract void DeleteEntity(int id);
    }
}
