using System.Web.UI;

public partial class MasterPages_Base : BaseMasterPage
    {
        #region Properties

        public new BaseWebPage Page
        {
            get { return (BaseWebPage)base.Page; }
            set { base.Page = value; }
        }

        #endregion

        public override ScriptManager ScriptManager
        {
            get { return scriptManager; }
        }
    }
