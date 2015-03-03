using System.Collections.Generic;
using System.Web.UI;

namespace Portal.Helpers
{
    public class ControlFinder<T> where T : Control
    {
        private readonly List<T> _foundControls = new List<T>();

        public IEnumerable<T> FoundControls
        {
            get { return _foundControls; }
        }

        public void FindControls(Control control)
        {
            foreach (Control childControl in control.Controls)
            {
                if (childControl.GetType() == typeof (T))
                {
                    _foundControls.Add((T) childControl);
                }
                else
                {
                    FindControls(childControl);
                }
            }
        }
    }
}