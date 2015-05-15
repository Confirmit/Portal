using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AspNetForums.Components;

namespace AspNetForums.Controls
{
    public class CurrentTime : Control
    {
        public CurrentTime()
        {
            Label label;

            // Add display text
            label = new Label();
            label.CssClass = "normalTextSmallBold";
            label.Text = HttpContext.GetLocalResourceObject(Globals.SkinsDir + "CurrentTime", "CurTime").ToString();
            Controls.Add(label);

            // Add formatted time
            label = new Label();
            label.CssClass = "normalTextSmall";

            label.Text = DateTime.Now.ToString("MMM d, HH:mm tt");

            Controls.Add(label);

        }
    }
}
