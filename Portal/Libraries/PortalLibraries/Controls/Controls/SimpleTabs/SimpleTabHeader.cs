using System.ComponentModel;
using System.Web.UI;

using Controls.ExtenderBase;
using Controls.ExtenderBase.Attributes;

namespace Controls
{
    [RequiredScript(typeof(SimpleTabContainer))]
    [ClientCssResource("Controls.SimpleTabs.SimpleTabs.css")]
    [ClientScriptResource("Controls.SimpleTabHeader", "Controls.SimpleTabs.SimpleTabs.js")]
    [ToolboxItem(false)]
    public class SimpleTabHeader : ScriptControlBase
    {
        #region [ Fields ]

        private ITemplate m_HeaderTemplate = null;
        private Control m_HeaderControl = null;

        private bool _active = false;
        private SimpleTabContainer _owner;

        #endregion

        #region [ Constructors ]
        
        public SimpleTabHeader() : base(false)
        {}

        #endregion

        #region [ Properties ]

        [DefaultValue("")]
        [Category("Appearance")]
        public string HeaderText
        {
            get { return (string)(ViewState["HeaderText"] ?? string.Empty); }
            set { ViewState["HeaderText"] = value; }
        }

        [PersistenceMode(PersistenceMode.InnerProperty)]
        [TemplateInstance(TemplateInstance.Single)]
        [Browsable(false)]
        [MergableProperty(false)]
        public ITemplate HeaderTemplate
        {
            get { return m_HeaderTemplate; }
            set { m_HeaderTemplate = value; }
        }

        [DefaultValue("")]
        [Category("Behavior")]
        [ExtenderControlEvent]
        [ClientPropertyName("click")]
        public string OnClientClick
        {
            get { return (string)(ViewState["OnClientClick"] ?? string.Empty); }
            set { ViewState["OnClientClick"] = value; }
        }

        internal bool Active
        {
            get { return _active; }
            set { _active = value; }
        }

        #endregion

        #region [ Methods ]

        protected override void OnInit(System.EventArgs e)
        {
            base.OnInit(e);

            if (HeaderTemplate != null)
            {
                m_HeaderControl = new Control();
                HeaderTemplate.InstantiateIn(m_HeaderControl);
                Controls.Add(m_HeaderControl);
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            //if (m_HeaderControl != null)
            //    m_HeaderControl.Visible = false;

            //writer.AddAttribute(HtmlTextWriterAttribute.Id, ClientID);
            //if (!Active)
            //{
            //    writer.AddStyleAttribute(HtmlTextWriterStyle.Display, "none");
            //    writer.AddStyleAttribute(HtmlTextWriterStyle.Visibility, "hidden");
            //}
            //writer.RenderBeginTag(HtmlTextWriterTag.Div);
            //RenderChildren(writer);
            //writer.RenderEndTag();
            ScriptManager.RegisterScriptDescriptors(this);
        }

        protected override void DescribeComponent(ScriptComponentDescriptor descriptor)
        {
            base.DescribeComponent(descriptor);

            //descriptor.AddElementProperty("headerTab", ClientID);
            if (_owner != null)
                descriptor.AddComponentProperty("owner", _owner.ClientID);
        }

        protected internal virtual void RenderHeader(HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Id, ClientID);
            writer.RenderBeginTag(HtmlTextWriterTag.Span);
            {
                if (m_HeaderControl != null)
                    m_HeaderControl.RenderControl(writer);
                else
                    writer.Write(HeaderText);
            }
            writer.RenderEndTag();
        }

        internal void SetOwner(SimpleTabContainer owner)
        {
            _owner = owner;
        }

        #endregion
    }
}