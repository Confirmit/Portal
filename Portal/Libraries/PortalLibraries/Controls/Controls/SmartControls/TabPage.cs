using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;

namespace SmartControls.Web
{
  /// <summary>
  /// TabPage class represents the individual tab page.
  /// </summary>  
  [
  ToolboxData(@"<{0}:TabPage runat=""server""></{0}:TabPage>"), 
  ToolboxItem(false) , 
  ParseChildren(false)
  ]
  public class TabPage : Panel , INamingContainer
  {    
    /// <summary>
    /// The text of tab page.
    /// </summary>
    public string Text
    {
      get
      {
        object val = this.ViewState["Text"];
        if (val == null) return "";
        return val.ToString();
      }
      set
      {
        this.ViewState["Text"] = value;
      }
    }

    
  }
}
