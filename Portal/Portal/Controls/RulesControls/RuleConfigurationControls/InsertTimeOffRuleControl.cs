using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules.DetailsOfRules;

namespace Portal.Controls.RulesControls.RuleConfigurationControls
{
    public class InsertTimeOffRuleControl : BaseRuleControl, IRuleInitializer, IRuleCreator, IRuleInitializable
    {
        public InsertTimeOffRuleControl()
        {
            var table = new Table {Width = Unit.Percentage(100)};
            var label = new Label {Text = "Time Interval:"};
            var tableRow = new TableRow();
            var cellWithLabel = new TableCell { Width = Unit.Percentage(50) };
            cellWithLabel.Controls.Add(label);
            tableRow.Cells.Add(cellWithLabel);

            var timeSelectorControl = (TimeSelectorControl)LoadControl("~/Controls/RulesControls/TimeSelectorControl.ascx");
            var cellWithTimeSelector = new TableCell { Width = Unit.Percentage(50) };
            cellWithTimeSelector.Controls.Add(timeSelectorControl);
            tableRow.Cells.Add(cellWithTimeSelector);

            table.Rows.Add(tableRow);
            Controls.Add(table);
            TimeIntervalSelector = timeSelectorControl;

            //TODO CHECK FOR CORRECTNESS!!
            TimeIntervalSelector.InitializeAllTimeListBoxes();
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

        public Rule InitializeRule(Rule rule, string description, TimeEntity timeInformation)
        {
            var insertTimeOffRule = (InsertTimeOffRule)rule;
            insertTimeOffRule.Interval = TimeInterval;
            insertTimeOffRule.Description = description;
            insertTimeOffRule.TimeInformation = timeInformation;
            return insertTimeOffRule;
        }

        public Rule CreateRule(string description, TimeEntity timeInformation)
        {
            var insertTimeOffRule = new InsertTimeOffRule(description, TimeInterval, timeInformation);
            return insertTimeOffRule;
        }

        public void InitializeRuleControl(Rule rule)
        {
            var insertTimeOffRule = (InsertTimeOffRule)rule;
            TimeIntervalSelector.InitializeAllTimeListBoxes();
            TimeInterval = insertTimeOffRule.Interval;
        }

        public override void Accept(RuleControlVisitor ruleControlVisitor)
        {
            ruleControlVisitor.Visit(this);
        }

        public override void DataBind(Rule rule)
        {
            //TODO
        }
    }
}