using System;
using System.Web.UI;

namespace Portal.Controls.RulesControls
{
    public partial class TimeSelectorControl : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitializeAllTimeListBoxes();
            }
        }

        public void InitializeAllTimeListBoxes()
        {
            //InitializeListBox(HoursListBox, 0, 23);
            //InitializeListBox(MinutesListBox, 0, 59);
            //InitializeListBox(SecondsListBox, 0, 59);
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (ViewState["HoursListBox"] != null && ViewState["MinutesListBox"] != null
                && ViewState["SecondsListBox"] != null)
            {
                TimeSelector.Hour = int.Parse((string) ViewState["HoursListBox"]);
                TimeSelector.Minute = int.Parse((string) ViewState["MinutesListBox"]);
                TimeSelector.Second = int.Parse((string) ViewState["SecondsListBox"]);
            }
        }

        public int Hours
        {
            get { return TimeSelector.Hour; }
            set { ViewState["HoursListBox"] = value.ToString(); }
        }

        public int Minutes
        {
            get { return TimeSelector.Minute; }
            set { ViewState["MinutesListBox"] = value.ToString(); }
        }

        public int Seconds
        {
            get { return TimeSelector.Second; }
            set { ViewState["SecondsListBox"] = value.ToString(); }
        }
    }
}