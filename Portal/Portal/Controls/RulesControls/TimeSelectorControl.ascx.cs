using System;
using System.Web.UI.WebControls;

namespace Portal.Controls.RulesControls
{
    public partial class TimeSelectorControl : System.Web.UI.UserControl
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
            InitializeListBox(HoursListBox, 0, 23);
            InitializeListBox(MinutesListBox, 0, 59);
            InitializeListBox(SecondsListBox, 0, 59);
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (ViewState["HoursListBox"] != null && ViewState["MinutesListBox"] != null
                && ViewState["SecondsListBox"] != null)
            {
                HoursListBox.SelectedValue = ViewState["HoursListBox"] as string;
                MinutesListBox.SelectedValue = ViewState["MinutesListBox"] as string;
                SecondsListBox.SelectedValue = ViewState["SecondsListBox"] as string;
            }
        }

        public int Hours
        {
            get { return int.Parse(HoursListBox.SelectedValue); }
            set { ViewState["HoursListBox"] = value.ToString(); }
        }

        public int Minutes
        {
            get { return int.Parse(MinutesListBox.SelectedValue); }
            set { ViewState["MinutesListBox"] = value.ToString(); }
        }

        public int Seconds
        {
            get { return int.Parse(SecondsListBox.SelectedValue); }
            set { ViewState["SecondsListBox"] = value.ToString(); }
        }

        private void InitializeListBox(ListBox listBox, int beginTimeValue, int endTimeValue)
        {
            for (var i = beginTimeValue; i <= endTimeValue; i++)
                listBox.Items.Add(i.ToString());
        }
    }
}