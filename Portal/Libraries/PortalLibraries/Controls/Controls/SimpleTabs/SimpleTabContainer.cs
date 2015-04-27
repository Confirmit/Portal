using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Web.Script.Serialization;
using System.Web.UI;

using Controls.ExtenderBase;
using Controls.ExtenderBase.Attributes;

#region [ Resources ]

[assembly: WebResource("Controls.SimpleTabs.SimpleTabs.js", "application/x-javascript")]
[assembly: WebResource("Controls.SimpleTabs.SimpleTabs.css", "text/css", PerformSubstitution = true)]
[assembly: WebResource("Controls.SimpleTabs.tab-line.gif", "image/gif")]
[assembly: WebResource("Controls.SimpleTabs.tab.gif", "image/gif")]
[assembly: WebResource("Controls.SimpleTabs.tab-left.gif", "image/gif")]
[assembly: WebResource("Controls.SimpleTabs.tab-right.gif", "image/gif")]
[assembly: WebResource("Controls.SimpleTabs.tab-hover.gif", "image/gif")]
[assembly: WebResource("Controls.SimpleTabs.tab-hover-left.gif", "image/gif")]
[assembly: WebResource("Controls.SimpleTabs.tab-hover-right.gif", "image/gif")]
[assembly: WebResource("Controls.SimpleTabs.tab-active.gif", "image/gif")]
[assembly: WebResource("Controls.SimpleTabs.tab-active-left.gif", "image/gif")]
[assembly: WebResource("Controls.SimpleTabs.tab-active-right.gif", "image/gif")]

#endregion

namespace Controls
{
    [ParseChildren(true)]
    [ClientCssResource("Controls.SimpleTabs.SimpleTabs.css")]
    [ClientScriptResource("Controls.SimpleTabContainer", "Controls.SimpleTabs.SimpleTabs.js")]
    public class SimpleTabContainer : ScriptControlBase, IPostBackEventHandler
    {
        #region [ Constructors ]

        public SimpleTabContainer () : base (true, HtmlTextWriterTag.Div)
        { }

        #endregion

        #region [ Fields ]

        private ITemplate m_ContentTemplate;
        private SimpleTabHeaderCollection m_Headers;

        private int m_ActiveHeaderIndex = -1;
        private bool m_AutoPostBack = false;

        #endregion

        #region [ Events ]

        public delegate void ActiveTabChangedEventHandler(object sender, int headerIndex);
        public event ActiveTabChangedEventHandler ActiveTabChanged;

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Collection of child panes in the Accordion
        /// </summary>
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public SimpleTabHeaderCollection Headers
        {
            get
            {
                if (m_Headers == null)
                    m_Headers = new SimpleTabHeaderCollection(this);
                return m_Headers;
            }
        }

        [PersistenceMode(PersistenceMode.InnerProperty)]
        [TemplateInstance(TemplateInstance.Single)]
        [Browsable(false)]
        [MergableProperty(false)]
        public ITemplate ContentTemplate
        {
            get { return m_ContentTemplate; }
            set { m_ContentTemplate = value; }
        }

        [DefaultValue(-1)]
        [Category("Behavior")]
        [ExtenderControlProperty]
        [ClientPropertyName("activeHeaderIndex")]
        public virtual int ActiveHeaderIndex
        {
            get
            {
                //if (_cachedActiveTabIndex > -1)
                //    return _cachedActiveTabIndex;

                if (Headers.Count == 0)
                    return -1;

                if (m_ActiveHeaderIndex == -1)
                    m_ActiveHeaderIndex = 0;

                return m_ActiveHeaderIndex;
            }
            set
            {
                if (value < -1)
                    throw new ArgumentOutOfRangeException("value");

                //if (Headers.Count == 0 && !m_Initialized)
                //{
                //    _cachedActiveTabIndex = value;
                //}
                //else
                //{
                if (value >= Headers.Count)
                    throw new ArgumentOutOfRangeException("value");

                if (ActiveHeaderIndex != value)
                {
                    if (ActiveHeaderIndex != -1 && ActiveHeaderIndex < Headers.Count)
                        Headers[ActiveHeaderIndex].Active = false;

                    m_ActiveHeaderIndex = value;
                    //_cachedActiveTabIndex = -1;

                    if (ActiveHeaderIndex != -1 && ActiveHeaderIndex < Headers.Count)
                        Headers[ActiveHeaderIndex].Active = true;
                }
                //}
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public SimpleTabHeader ActiveHeader
        {
            get
            {
                int i = ActiveHeaderIndex;
                if (i < 0 || i >= Headers.Count)
                    return null;

                ensureActiveHeader();
                return Headers[i];
            }
            set
            {
                int i = Headers.IndexOf(value);
                if (i < 0)
                    throw new ArgumentOutOfRangeException("value");
                ActiveHeaderIndex = i;
            }
        }

        [DefaultValue("")]
        [Category("Behavior")]
        [ExtenderControlEvent]
        [ClientPropertyName("activeHeaderChanged")]
        public string OnClientActiveTabChanged
        {
            get { return (string)(ViewState["OnClientActiveHeaderChanged"] ?? string.Empty); }
            set { ViewState["OnClientActiveHeaderChanged"] = value; }
        }

        /// <summary>
        /// To enable AutoPostBack, we need to call an ASP.NET script method with the UniqueId
        /// on the client side.  To do this, we just use this property as the one to serialize and
        /// alias it's name.
        /// </summary>
        [ExtenderControlProperty]
        [ClientPropertyName("autoPostBackId")]
        public new string UniqueID
        {
            get { return base.UniqueID; }
            set 
            {
                // need to add a setter for serialization to work properly.
            }
        }

        [DefaultValue(false)]
        [Category("Behavior")]
        public bool AutoPostBack
        {
            get { return m_AutoPostBack; }
            set { m_AutoPostBack = value; }
        }

        #endregion

        #region [ Methods ]

        private void ensureActiveHeader()
        {

            if (ActiveHeaderIndex < 0 || ActiveHeaderIndex >= Headers.Count)
                ActiveHeaderIndex = 0;

            for (int i = 0; i < Headers.Count; i++)
            {
                Headers[i].Active = (i == ActiveHeaderIndex);
            }
        }

        // has to be public so reflection can get at it...
        // this method determines if the UniqueID property will
        // be code generated.
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeUniqueID()
        {
            return IsRenderingScript && AutoPostBack;
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            Page.RegisterRequiresControlState(this);

            if (ContentTemplate != null)
            {
                Control ctrl = new Control();
                ContentTemplate.InstantiateIn(ctrl);
                Controls.Add(ctrl);
            }

            //if (_cachedActiveTabIndex > -1)
            //{
            //    ActiveHeaderIndex = _cachedActiveTabIndex;
            //}
            //else 
        }

        protected override void RenderContents(HtmlTextWriter writer)
        {
            Page.VerifyRenderingInServerForm(this);

            RenderHeader(writer);

            writer.AddAttribute(HtmlTextWriterAttribute.Id, ClientID + "_body");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            {
                RenderChildren(writer);
            }
            writer.RenderEndTag();
        }

        protected virtual void RenderHeader(HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Id, ClientID + "_header");
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "ajax__tab_header");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            foreach (SimpleTabHeader header in Headers)
            {
                header.RenderHeader(writer);
            }
            writer.RenderEndTag();
        }

        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "ajax__tab_xp");
            base.AddAttributesToRender(writer);
        }

        /// <summary>
        /// Empty out the child Header's collection
        /// </summary>
        internal void ClearHeaders()
        {
            for (int i = Controls.Count - 1; i >= 0; i--)
            {
                if (Controls[i] is SimpleTabHeader)
                    Controls.RemoveAt(i);
            }
        }

        protected override string SaveClientState()
        {
            Dictionary<string, object> state = new Dictionary<string, object>();
            state["ActiveHeaderIndex"] = ActiveHeaderIndex;

            List<object> headerState = new List<object>();
            foreach (SimpleTabHeader panel in Headers)
            {
                headerState.Add(panel.Enabled);
            }
            state["HeaderState"] = headerState;
            return new JavaScriptSerializer().Serialize(state);
        }

        protected override void LoadClientState(string clientState)
        {
            Dictionary<string, object> state = (Dictionary<string, object>)new JavaScriptSerializer().DeserializeObject(clientState);
            if (state != null)
            {
                ActiveHeaderIndex = (int)state["ActiveHeaderIndex"];
                object[] tabState = (object[])state["HeaderState"];

                for (int i = 0; i < tabState.Length; i++)
                {
                    Headers[i].Enabled = (bool)tabState[i];
                }
            }
        }

        protected override void LoadControlState(object savedState)
        {
            Pair p = (Pair)savedState;
            if (p != null)
            {
                base.LoadControlState(p.First);
                ActiveHeaderIndex = (int)p.Second;
            }
            else
                base.LoadControlState(null);
        }

        protected override object SaveControlState()
        {
            Pair p = new Pair { First = base.SaveControlState(), Second = ActiveHeaderIndex };
            if (p.First == null && p.Second == null)
                return null;

            return p;
        }

        #endregion

        #region [ IPostBackEventHandler Members ]

        public void RaisePostBackEvent(string eventArgument)
        {
            if (eventArgument.StartsWith("activeHeaderChanged", StringComparison.Ordinal))
            {
                // change the active tab.
                int parseIndex = eventArgument.IndexOf(":", StringComparison.Ordinal);
                Debug.Assert(parseIndex != -1, "Expected new active tab index!");

                if (parseIndex != -1 && Int32.TryParse(eventArgument.Substring(parseIndex + 1), out parseIndex))
                {
                    ActiveHeaderIndex = parseIndex;
                    if (ActiveTabChanged != null)
                        ActiveTabChanged(this, parseIndex);
                }
            }
        }

        #endregion
    }
}