using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Portal.Controls.RulesControls.RuleConfigurationControls
{
    public class InsertTimeOffRuleControl : UserControl
    {
        public InsertTimeOffRuleControl()
        {
            var table = new Table();
            var label = new Label {Text = "Time Interval:"};
            var tableRow = new TableRow();
            var cellWithLabel = new TableCell();
            cellWithLabel.Controls.Add(label);
            tableRow.Cells.Add(cellWithLabel);

            var timeSelectorControl = (TimeSelectorControl)LoadControl("~/Controls/RulesControls/TimeSelectorControl.ascx");
            var cellWithTimeSelector = new TableCell();
            cellWithTimeSelector.Controls.Add(timeSelectorControl);
            tableRow.Cells.Add(cellWithTimeSelector);

            table.Rows.Add(tableRow);
            Controls.Add(table);
            TimeIntervalSelector = timeSelectorControl;
        }

        public TimeSelectorControl TimeIntervalSelector { get; set; }

        public TimeSpan TimeInterval
        {
            get
            {
                return new TimeSpan(TimeIntervalSelector.Hours, TimeIntervalSelector.Minutes, TimeIntervalSelector.Seconds);
            }
            set
            {
                TimeIntervalSelector.Hours = value.Hours;
                TimeIntervalSelector.Minutes = value.Minutes;
                TimeIntervalSelector.Seconds = value.Seconds;
            }
        }
    }
}