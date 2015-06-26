namespace Portal.TestingEntitiesManipulation
{
    public partial class UsersManipulationPage : BaseWebPage
    {
        public int CurrentEntityId
        {
            get { return ViewState["CurrentWrapperEntityId"] is int ? (int)ViewState["CurrentWrapperEntityId"] : -1; }
            set { ViewState["CurrentWrapperEntityId"] = value; }
        }
    }
}