using System;
using System.Collections.Generic;
using UIPProcess.DataBinding;
using UIPProcess.UIP.Views;

namespace UIPProcess.Views
{
    /// <summary>
    /// The base class for controls which allows to add/edit entities and entities' 
    /// information.
    /// </summary>
    public class AddEditEntityInformationView : ControlViewBase
    {
        /// <summary>
        /// <b>State mapped property.</b> Selected Entity for edit.
        /// </summary>
        public virtual Object SelectedEntity
        {
            set
            {
                _selectedEntity = value;

                if (_viewStorage == null)
                    DataBinder.DataBindToControlAttributedProps(value, this);
            }
        }
        protected Object _selectedEntity = null;

        /// <summary>
        /// <para><b>State mapped property.</b> Temporary View Storage, automatically filled by the
        /// corresponding Controller on page PostBack and used to store data, entered by User
        /// during navigation to Parameters Tables, changing of Screen Resolution, etc.
        /// </para>
        /// </summary>
        public virtual IDictionary<String, Object> ViewStorage
        {
            set
            {
                _viewStorage = value;

                if (_viewStorage != null && _viewStorage.Count > 0)
                    DataBinder.DataBindToControlAttributedProps(value, this);
            }
        }
        protected IDictionary<String, Object> _viewStorage = null;
    }
}