using System;
using System.ComponentModel;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SmartControls.Web
{
    [
        ToolboxData(@"<{0}:TabsView runat=""server""></{0}:TabsView>"),
        Designer(typeof (TabsViewDesigner))
    ]
    public class TabsView : CompositeControl
    {
        //private StringCssStyleCollection selectedTabStyle;
        //private StringCssStyleCollection unSelectedTabStyle;

        #region declarations

        private TabPageCollection tabs;
        private readonly object TabSelectionChangingObject = new object();

        public delegate void TabSelectionChangingHandler(object sender, TabSelectionChangingEventArgs e);

        #endregion

        #region overrides

        protected override void CreateChildControls()
        {
            Controls.Clear();
            Table tbl = new Table();
            string js = "";
            tbl.CellPadding = tbl.CellSpacing = 0;

            //include java script
            //if (ScriptPath.Length > 0)
            //  this.Page.ClientScript.RegisterClientScriptInclude(this.GetType(), "include_js", ScriptPath + "TabsView.js");

            // DG Регистрируем скрипт по своему
            if (!(Page.ClientScript.IsStartupScriptRegistered("TabsView")))
                Page.ClientScript.RegisterStartupScript(GetType(), "TabsView", "<script language=\"javascript\" src=\""
                                                                               +
                                                                               Page.ClientScript.GetWebResourceUrl(
                                                                                   GetType(),
                                                                                   "Controls.SmartControls.TabsView.js") +
                                                                               "\"></script>");

            if (Tabs.Count > 0)
            {
                //create tab headers.
                js = CreateTabHeaders(ref tbl);
                Page.ClientScript.RegisterArrayDeclaration("tabButtons_" + ClientID, js);

                //create tab contents.        
                js = CreateTabContents(ref tbl);
                Page.ClientScript.RegisterArrayDeclaration("tabContents_" + ClientID, js);

                //when the post back is performed than select the current tab.
                //During designer editing Page.Request is null therefore we have to
                //check for current Http context.
                if (!DesignMode /*HttpContext.Current != null*/)
                {
                    //if (!AutoPostBack)
                    {
                        if (Page.Request[ClientID + "$hf"] != null)
                            CurrentTabIndex = int.Parse(Page.Request[ClientID + "$hf"]);

                        SelectTab();
                    }
                    //else
                    //{
                        //if (ViewState["Loaded"] == null)
                        //{
                        //  ViewState["Loaded"] = 1;
                    //    SelectTab();
                        //}
                    //}

                    //SelectTab();
                }
            }

            Controls.Add(tbl);
        }

        #endregion

        #region helper functions

        private void SelectTab()
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(),
                                                    "_SelectTab",
                                                    "<script language='JavaScript'>SelectTab(" + CurrentTabIndex +
                                                    ",'" + ClientID + "','" + ClientID + "_hf" + "','" +
                                                    UnSelectedTabCSSClass + "','" + SelectedTabCSSClass + "')" +
                                                    "</script>");
        }

        /// <summary>
        /// Create Tab headers
        /// </summary>
        /// <param name="tbl">Parent table control whole alignmnet of control</param>
        /// <returns></returns>
        private string CreateTabHeaders(ref Table tbl)
        {
            TableRow tr;
            TableCell tc;
            StringBuilder arrBtns = new StringBuilder(); //contains header js
            
            int i = 0;
            //hidden field used to store the current tab index(for java scripting)
            HiddenField hf = new HiddenField();
            hf.ID = "hf";

            Table tblTabs = new Table();
            tblTabs.CellPadding = tblTabs.CellSpacing = 0;
            tr = new TableRow();

            VerifyTabIndex();

            foreach (TabPage tp in Tabs)
            {
                tc = new TableCell();

                Label lb = new Label();
                lb.ID = "tp" + i;
                lb.Text = "&nbsp;" + tp.Text + "&nbsp;";
                string lkId;
                lkId = ClientID + "_" + lb.ID;
                lb.Attributes["onclick"] = "return OnTabClick(this," + i + ",'" +
                                        ClientID + "','" + ClientID + "_" + hf.ClientID + "','" +
                                        UnSelectedTabCSSClass + "','" + SelectedTabCSSClass + "');";


                //lk = new LinkButton();
                //lk.ID = "tp" + i.ToString();
                //lk.Text = "&nbsp;" + tp.Text + "&nbsp;";
                //lk.CommandArgument = i.ToString();
                //if (AutoPostBack) lk.Click += new EventHandler(lk_Click);

                //lkId = ClientID + "_" + lk.ID;
                arrBtns.AppendFormat("\"{0}\"{1}", lkId, (i == Tabs.Count - 1 ? "" : ","));

                //if (!AutoPostBack)
                //    lk.OnClientClick = "return OnTabClick(this," + i.ToString() + ",'" +
                //                       ClientID + "','" + ClientID + "_" + hf.ClientID + "','" +
                //                       UnSelectedTabCSSClass + "','" + SelectedTabCSSClass + "');";

                tc.Controls.Add(lb);
                tr.Cells.Add(tc);
                ++i;
            }
            tblTabs.Rows.Add(tr);

            tc = new TableCell();
            tc.Controls.Add(tblTabs);
            tc.Controls.Add(hf);

            tr = new TableRow();
            tr.Cells.Add(tc);

            tbl.Rows.Add(tr);
            return arrBtns.ToString();
        }

        /// <summary>
        /// create tab contents.
        /// </summary>
        /// <param name="tbl">parent table refrence.</param>
        /// <returns></returns>
        private string CreateTabContents(ref Table tbl)
        {
            TableRow tr;
            TableCell tc;
            int i = 0;
            string tpId;
            StringBuilder arrTabPages = new StringBuilder();

            Table tblContents = new Table();
            tblContents.CellPadding = tblContents.CellSpacing = 0;

            tr = new TableRow();
            tc = new TableCell();
            //int i = -1;
            foreach (TabPage tp in Tabs)
            {
                tpId = ClientID + "_" + tp.ID;
                tc.Controls.Add(tp);
                if (i == tabs.Count - 1)
                    arrTabPages.AppendFormat("\"{0}\"", tpId);
                else
                    arrTabPages.AppendFormat("\"{0}\",", tpId);
                ++i;
            }
            tr.Cells.Add(tc);
            tblContents.Rows.Add(tr);

            tc = new TableCell();
            tc.Controls.Add(tblContents);
            tr = new TableRow();
            tr.Cells.Add(tc);

            tbl.Rows.Add(tr);
            return arrTabPages.ToString();
        }

        private void lk_Click(object sender, EventArgs e)
        {
            LinkButton lk = (LinkButton) sender;

            CurrentTabIndex = Convert.ToInt32(lk.CommandArgument);

            //select the current tab.
            Page.ClientScript.RegisterStartupScript(Page.GetType(),
                                                    "SelectTab_LinkButton",
                                                    "<script language='JavaScript'>SelectTab(" + CurrentTabIndex +
                                                    ",'" + ClientID + "','" + ClientID + "_hf" + "','" +
                                                    UnSelectedTabCSSClass + "','" + SelectedTabCSSClass + "')" +
                                                    "</script>");
        }

        private void VerifyTabIndex()
        {
            if (CurrentTabIndex >= Tabs.Count)
                throw new Exception("Invalid Tab Index");
        }

        //private string GetTabButtonBorderStyle()
        //{
        //  string btnStyle = "";
        //  string tabButtonBorderColor = TabButtonBorderColor.ToKnownColor().ToString();

        //  //"border-right: skyblue 1px outset; border-top: skyblue 1px outset; border-left: 
        //  //skyblue 1px outset; border-bottom: skyblue 1px outset"
        //  btnStyle += string.Format("border-right:{0} 1px outset;", tabButtonBorderColor);
        //  btnStyle += string.Format("border-top:{0} 1px outset;", tabButtonBorderColor);
        //  btnStyle += string.Format("border-left:{0} 1px outset;", tabButtonBorderColor);
        //  btnStyle += string.Format("border-bottom:{0} 1px outset;", tabButtonBorderColor);

        //  return btnStyle;
        //}

        #endregion

        #region properties

        /// <summary>
        /// Collection of Tabs
        /// </summary>
        [
            PersistenceMode(PersistenceMode.InnerDefaultProperty),
            DefaultValue(null),
            Browsable(false)
        ]
        public virtual TabPageCollection Tabs
        {
            get
            {
                if (tabs == null)
                {
                    tabs = new TabPageCollection(this);
                }

                return tabs;
            }
        }

        // DG чтобы были нормальные ссылки убрал postback
        ///// <summary>
        ///// Whether to auto post back the tab page or not.
        ///// </summary>
        //[
        //    DefaultValue(false)
        //]
        //public bool AutoPostBack
        //{
        //    set { ViewState["AutoPostBack"] = value; }
        //    get
        //    {
        //        object val = ViewState["AutoPostBack"];
        //        if (val == null) return false;
        //        return Convert.ToBoolean(val);
        //    }
        //}

        //public Color TabButtonBorderColor
        //{
        //  get
        //  {
        //    object val = this.ViewState["TabButtonBorderColor"];
        //    if (val == null) return Color.FromName("#d4d0c8");
        //    return (Color)val;        
        //  }
        //  set
        //  {
        //    this.ViewState["TabButtonBorderColor"] = value;
        //  }
        //}

        //public void Refresh()
        //{
        //  CreateChildControls();
        //}

        public int CurrentTabIndex
        {
            get
            {
                object val = ViewState["CurrentTabIndex"];
                if (val == null) return 0;

                return Convert.ToInt32(val);
            }
            set
            {
                object val = ViewState["CurrentTabIndex"];
                if (val != null)
                {
                    int oldIndex = Convert.ToInt32(val);
                    int newIndex = Convert.ToInt32(value);

                    if (oldIndex != newIndex)
                    {
                        TabSelectionChangingEventArgs e = new TabSelectionChangingEventArgs(oldIndex, newIndex);

                        TabSelectionChangingHandler handler =
                            (TabSelectionChangingHandler) Events[TabSelectionChangingObject];
                        if (handler != null)
                            handler(this, e);
                    }
                }

                ViewState["CurrentTabIndex"] = value;
            }
        }

        public string SelectedTabCSSClass
        {
            set { ViewState["SelectedTabCSSClass"] = value; }
            get
            {
                object val = ViewState["SelectedTabCSSClass"];
                if (val == null) return "";
                return val.ToString();
            }
        }

        public string UnSelectedTabCSSClass
        {
            set { ViewState["UnSelectedTabCSSClass"] = value; }
            get
            {
                object val = ViewState["UnSelectedTabCSSClass"];
                if (val == null) return "";
                return val.ToString();
            }
        }

        //public string ScriptPath
        //{
        //    get
        //    {
        //        object val = ViewState["ScriptPath"];
        //        if (val == null) return "TabsView.js";
        //        return val.ToString();
        //    }
        //    set { ViewState["ScriptPath"] = value; }
        //}

        #endregion

        #region events

        public event TabSelectionChangingHandler TabSelectionChanging
        {
            add { Events.AddHandler(TabSelectionChangingObject, value); }
            remove { Events.RemoveHandler(TabSelectionChangingObject, value); }
        }

        #endregion

        //[
        // PersistenceMode(PersistenceMode.Attribute)
        //]
        //public StringCssStyleCollection SelectedTabStyle
        //{
        //  get
        //  {
        //    return selectedTabStyle;
        //    //StringCssStyleCollection s = new StringCssStyleCollection();
        //    //s.CssText = "background-Color:red";
        //    //return s;
        //  }
        //  set
        //  {
        //    selectedTabStyle = value;
        //  }
        //}

        //[
        //  DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        //  NotifyParentProperty(true),
        //  PersistenceMode(PersistenceMode.InnerProperty)
        //]
        //public StringCssStyleCollection UnSelectedTabStyle
        //{
        //  get
        //  {
        //    return unSelectedTabStyle;
        //  }
        //  set
        //  {
        //    unSelectedTabStyle = value;
        //  }
        //}
    }

    public class TabSelectionChangingEventArgs : EventArgs
    {
        private int prevIndex;
        private int newIndex;

        public TabSelectionChangingEventArgs(int prevIndex, int newIndex)
        {
            this.prevIndex = prevIndex;
            this.newIndex = newIndex;
        }

        public int NewIndex
        {
            get { return newIndex; }
        }

        public int PreviousIndex
        {
            get { return prevIndex; }
        }
    }
}