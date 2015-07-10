namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules.DetailsOfRules
{
    public class RuleDetails
    {
        public TimeEntity TimeInformation { get; set; }

        public RuleDetails() { }

        public RuleDetails(TimeEntity timeInformation)
        {
            TimeInformation = timeInformation;
        }
    }
}