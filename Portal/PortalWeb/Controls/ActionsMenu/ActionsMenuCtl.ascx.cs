using System;
using System.Text;
using System.Web.UI;

using UIPProcess.Controllers.ActionMenu;
using UIPProcess.UIP.Views;

public partial class ActionsMenuCtl : ControlViewBase
{
    #region Properties

    public new IActionMenuControllerBase Controller
    {
        get { return (IActionMenuControllerBase)base.Controller; }
    }

    public String EventTargetId
    {
        set { _eventTargetId = value; }
    }
    protected String _eventTargetId = String.Empty;

    public ScriptManager ScriptManager
    {
        set
        {
            if (menu != null)
                menu.ScriptManager = value;

            _scriptManager = value;
        }
    }
    private ScriptManager _scriptManager = null;

    public String ProcessedGridObject
    {
        set { processedGridObject = value; }
    }
    protected String processedGridObject = String.Empty;

    /// <summary>
    /// Menu which is displayed in all cases
    /// </summary>
    public String MenuForAnyPossibleActionsCriteria
    {
        set
        {
            if (menu != null)
                menu.XmlPath = value;

            menuForAnyPossibleActionsCriteria = value;
        }
    }
    private String menuForAnyPossibleActionsCriteria = String.Empty;

    /// <summary>
    /// Menu which is displayed if Entity is selected
    /// </summary>
    public String MenuForSelectedObjectActionsCriteria
    {
        set
        {
            menuForSelectedObjectActionsCriteria = value;

            if (menu != null)
                adjustMenu();
        }
    }
    private String menuForSelectedObjectActionsCriteria =
        "./controls/ActionsMenu/a_s_s_and_new_d.xml";

    /// <summary>
    /// Menu which is displayed if there is no selected Entity
    /// </summary>
    public String MenuForNotSelectedObjectActionsCriteria
    {
        set
        {
            menuForNotSelectedObjectActionsCriteria = value;

            if (menu != null)
                adjustMenu();
        }
    }
    private String menuForNotSelectedObjectActionsCriteria =
        "./controls/ActionsMenu/s_s_and_new_d.xml";

    #endregion

    #region Events processing

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        menu.ControllerObjectName = ControllerJSObjectName;
        menu.ScriptManager = _scriptManager;
    }

    public String JSClientObjectName
    {
        get { return ControllerJSObjectName; }
    }

    public String AfterInitScript
    {
        set { _afterInitScript = value; }
    }
    String _afterInitScript = String.Empty;

    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);

        if (!(Page.ClientScript.IsClientScriptIncludeRegistered("ActionsMenuController")))
            Page.ClientScript.RegisterClientScriptInclude(GetType(), "ActionsMenuController"
                , "../controls/ActionsMenu/ActionsMenuController.js");

        //MenuProcessor.RemoveACLDisallowedActions(
        //    (_currentUser != null) ? _currentUser.ACL : null, SecuritableTypeId
        //  , menu.RootNode);

        if (Controller != null)
            Controller.RemoveDisallowedActions(menu.RootNode);

        menu.RegisterBeforeMenuCreationScript(createControllerJSScript);
        menu.RegisterInitMenuScript(initMenuJSScript + _afterInitScript);
    }

    private String createControllerJSScript
    {
        get
        {
            StringBuilder script = new StringBuilder();
            script.AppendFormat(" if (typeof({0}) != 'undefined' && {0} != null) delete {0}; "
                , ControllerJSObjectName);

            script.AppendFormat(" var {0} = new ActionsMenuController('{1}', '{2}'); "
                , ControllerJSObjectName
                , ClientID
                , (String.IsNullOrEmpty(_eventTargetId) ? UniqueID : _eventTargetId));

            return script.ToString();
        }
    }

    private String initMenuJSScript
    {
        get
        {
            StringBuilder script = new StringBuilder();
            script.Append(" Sys.Application.add_init(function() {");

            script.AppendFormat(" if (typeof({0}) != 'undefined' && {0} != null) {{"
                , menu.MenuJSObjectName
            );

            if (!String.IsNullOrEmpty(processedGridObject))
            {
                script.AppendFormat("var delete_item = {0}.jdMenuItem('{1}');"
                    , menu.MenuJSObjectName, "delete"
                );

                script.AppendFormat("{0}.SetMenuDeleteItem(delete_item);"
                    , ControllerJSObjectName
                );

                script.AppendFormat("var close_item = {0}.jdMenuItem('{1}');"
                    , menu.MenuJSObjectName, "close"
                );

                script.AppendFormat("{0}.SetMenuCloseItem(close_item);"
                    , ControllerJSObjectName
                );

                script.AppendFormat(" if (typeof({1}) != 'undefined') {0}.SetProcessedGrid({1});"
                    , ControllerJSObjectName, processedGridObject
                );
            }

            script.Append("}");
            script.Append(" }); ");

            return script.ToString();
        }
    }

    /// <summary>
    /// State mapped property
    /// </summary>
    public Object VisibilityCriteria
    {
        set { menu.Visible = (value != null); }
    }

    /// <summary>
    /// State mapped property
    /// </summary>
    public Object PossibleActionsCriteria
    {
        set
        {
            _selectedObject = value;

            adjustMenu();
        }
    }
    private Object _selectedObject = null;

    private void adjustMenu()
    {
        if (!String.IsNullOrEmpty(menuForAnyPossibleActionsCriteria))
            return;

        menu.XmlPath = ((_selectedObject == null)
                            ? menuForNotSelectedObjectActionsCriteria
                            : menuForSelectedObjectActionsCriteria);
    }

    public readonly String POSSIBLE_ACTIONS_CRITERIA_PROPERTY = "PossibleActionsCriteria";

    /// <summary>
    /// State mapped property.
    /// </summary>
    public Boolean SaveErrorDisable
    {
        set { _saveErrorDisable = value; }
    }
    private Boolean _saveErrorDisable = false;

    #endregion
}