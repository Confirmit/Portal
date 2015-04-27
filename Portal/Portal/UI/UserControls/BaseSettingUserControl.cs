using System;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

using ConfirmIt.PortalLib.BAL.Settings;

public class BaseSettingUserControl : BaseUserControl
{
    #region Fields

    private IList<ISetting> m_SettingCollection = new List<ISetting>();
    private readonly string m_objectKey = "object_{0}";

    #endregion

    #region Properties

    public IList<ISetting> SettingCollection
    {
        set { m_SettingCollection = value; }
        get { return m_SettingCollection; }
    }

    private HtmlGenericControl DivBetweenContainer
    {
        get
        {
            HtmlGenericControl div_between = new HtmlGenericControl("div");
            div_between.Attributes.Add("class", "control-line-between");
            return div_between;
        }
    }

    #endregion

    #region Methods

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        foreach (ISetting setting in SettingCollection)
        {
            Controls.Add(DivBetweenContainer);
            Controls.Add(getDataLineContainer(setting));
        }
    }

    private HtmlGenericControl getDataLineContainer(ISetting setting)
    {
        HtmlGenericControl div_line = new HtmlGenericControl("div");
        div_line.Attributes.Add("class", "control-line-of-controls");

        HtmlGenericControl div_data_label = new HtmlGenericControl("div");
        div_data_label.Attributes.Add("style", "padding-top: 3px; float: left; width: 200px");
        div_data_label.Attributes.Add("align", "left");
        div_data_label.Attributes.Add("class", "control-label");
        div_data_label.InnerText = getLocalResource(setting);

        HtmlGenericControl div_data_input = new HtmlGenericControl("div");
        div_data_input.Attributes.Add("style", "float: left; width: 360px");
        TextBox box = new TextBox
                          {
                              Text = setting.Value.ToString()
                              , ID = string.Format(m_objectKey, setting.SettingAttribute.SettingName)
                              , CssClass = "control-textbox"
                              , EnableTheming = false
                          };
        box.Attributes.Add("style", "width: 100%");

        div_data_input.Controls.Add(box);
        div_line.Controls.Add(div_data_label);
        div_line.Controls.Add(div_data_input);

        return div_line;
    }

    public void Save(BaseSettingCollection settings)
    {
        foreach (ISetting setting in SettingCollection)
        {
            TextBox textBox = FindControl(string.Format(m_objectKey, setting.SettingAttribute.SettingName)) as TextBox;
            if (textBox == null || string.IsNullOrEmpty(textBox.Text))//TODO: need to checking!
                continue;

            settings.Save(setting.SettingAttribute, textBox.Text);
        }
    }

    private string getLocalResource(ISetting setting)
    {
        string resourceValue = string.Empty;
        string key = string.Concat(setting.SettingAttribute.SettingName, ".Text");
        
        try
        {
            //resourceValue = GetLocalResourceObject(key) as string;
            switch (setting.SettingAttribute.SettingType)
            {
                case SettingType.Personal:
                    {
                        resourceValue = GetLocalResourceObject(key) as string;
                        break;
                    }
                default:
                    {
                        resourceValue = GetGlobalResourceObject("GlobalSettings.ascx", key) as string;
                        break;
                    }
            }
        }
        catch
        {}

        return string.IsNullOrEmpty(resourceValue) ? setting.SettingAttribute.SettingName : resourceValue;
    }

    #endregion
}
