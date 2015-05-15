using System;
using System.Collections.Generic;
using System.Reflection;
using System.Globalization;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace Controls.HotGridView
{
	public partial class GridView : System.Web.UI.WebControls.GridView
	{
		private const string HOTGRIDVIEW_JS = "Controls.HotGridView.HotGridView.js";
		private const string HOTGRIDVIEW_CONTROLLER_JS = "Controls.HotGridView.HotGridViewController.js";

		private ArrayList cachedSelectedIndices;

		public GridView()
		{}

		#region [ Properties ]

		/// <summary>
		/// ScriptManager to process JS scripts for asynchronious calls.
		/// </summary>
		public ScriptManager ScriptManager
		{
			set { _scriptManager = value; }
		}
		private ScriptManager _scriptManager = null;

		/// <summary>
		/// The name of the client JS Grid View object.
		/// </summary>
		public String ClientJSObjectName
		{
			get { return String.Format("obj{0}", ClientID); }
		}

		/// <summary>
		/// The name of the client JS Grid View controller object which will process all commands.
		/// </summary>
		public String ControllerJSObjectName
		{
			get 
			{
				if (String.IsNullOrEmpty(_controllerJSObjectName))
					_controllerJSObjectName = String.Format("{0}_controller", ClientID);
				
				return _controllerJSObjectName; 
			}
			set { _controllerJSObjectName = value; }
		}

		private String _controllerJSObjectName = String.Empty;

		/// <summary>
		/// Use custom pager.
		/// </summary>
		public bool UseCustomPager
		{
			get { return (bool?)ViewState["UseCustomPager"] ?? false; }
			set { ViewState["UseCustomPager"] = value; }
		}

		/// <summary>
		/// The ID of the target event element.
		/// </summary>
		public String EventTargetId
		{
			set { _eventTargetId = value; }
		}
		private String _eventTargetId = String.Empty;

		#region Adapter Members

		/// <summary>
		/// <para>The name of the View from configuration file where the System
		/// should navigate when adding of a new entity in a grid. </para>
		/// <para>This method is processed by the corresponding server side grid view controller,
		/// usually this method is accessed through HotGridViewAdapter. </para>
		/// </summary>
		public String NavigateOnAdd
		{
			set { ViewState[keyNavigateOnAdd] = value; }
			get { return (String)ViewState[keyNavigateOnAdd]; }
		}
		private readonly String keyNavigateOnAdd = "NavigateOnAdd";

		/// <summary>
		/// <para><b>True</b> if the entities in the grid are Historical Entities with the 
		/// history of changes. </para>
		/// <para>This method is processed by the corresponding server side grid view controller,
		/// usually this method is accessed through HotGridViewAdapter. </para>
		/// </summary>
		public bool HistoryDataCollection
		{
			set { ViewState[keyHistoryDataCollection] = value; }
			get
			{
				return ViewState[keyHistoryDataCollection] == null
					? true : (bool)ViewState[keyHistoryDataCollection];
			}
		}
		private readonly String keyHistoryDataCollection = "HistoryDataCollection";

		/// <summary>
		/// 
		/// </summary>
		public String BusinessKeyValue
		{
			get
			{
				if (String.IsNullOrEmpty(Page.Request.Form[GetHiddenSlectedBusinessKey]))
					return String.Empty;

				return Page.Request.Form[GetHiddenSlectedBusinessKey];
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public Int32 SelectedEntityId
		{
			get
			{
				if (String.IsNullOrEmpty(Page.Request.Form[GetHiddenSlectedId])
					&& String.IsNullOrEmpty(RadioButtonSelectedIndexValue))
						return 0;

				try
				{
					if (!String.IsNullOrEmpty(Page.Request.Form[GetHiddenSlectedId]))
						return Int32.Parse(Page.Request.Form[GetHiddenSlectedId]);

					if (!String.IsNullOrEmpty(RadioButtonSelectedIndexValue)
						&& String.IsNullOrEmpty(Page.Request.Form[GetHiddenSlectedId]))
						return Int32.Parse(RadioButtonSelectedIndexValue);
				}
				catch {}

				return 0;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public String SelectedEntityType
		{
			get 
			{
				if (String.IsNullOrEmpty(Page.Request.Form[GetHiddenSlectedIndex]))
					return String.Empty;

				int index = Int32.Parse(Page.Request.Form[GetHiddenSlectedIndex]);
				if (Columns[index] is BoundSelectionField)
				{
					return ((BoundSelectionField)Columns[index]).ColumnType;
				}
				else
				{
					return String.Empty;
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public String DefaultRowType
		{
			set { ViewState[keyDefaultRowType] = value; }
			get
			{
				return ViewState[keyDefaultRowType] == null
					? String.Empty : (String)ViewState[keyDefaultRowType];
			}
		}
		private readonly String keyDefaultRowType = "DefaultRowType";

		/// <summary>
		/// 
		/// </summary>
		public String NavigateOnSelect
		{
			get 
			{
				if (String.IsNullOrEmpty(Page.Request.Form[GetHiddenSlectedIndex]))
					return String.Empty;

				int index = Int32.Parse(Page.Request.Form[GetHiddenSlectedIndex]);
				if (Columns[index] is BoundSelectionField)
				{
					return ((BoundSelectionField)Columns[index]).NavigateOnSelect;
				}
				else 
				{
					return String.Empty;
				}
			}
		}

		public void RegisterInitScript(String script)
		{
			_scriptInit = script;
		}
		private String _scriptInit = String.Empty;

		private String GetHiddenSlectedId
		{
			get { return this.ClientID + "_ID"; }
		}

		private String GetHiddenSlectedIndex
		{
			get { return this.ClientID + "_Index"; }
		}

		private String GetHiddenSlectedBusinessKey
		{
			get { return this.ClientID + "_BusinessKey"; }
		}

		#endregion        

		#region Arrows IMG for footer
		// PROPERTY:: ArrowsImg Settings
		protected const String _strRightArrowDisable = "RightArrowDisable";
		protected const String _strLeftArrowDisable = "LeftArrowDisable";
		protected const String _strRightArrowEnable = "RightArrowEnable";
		protected const String _strLeftArrowEnable = "LeftArrowEnable";

		public String RightArrowDisableImg
		{
			get { return (String)ViewState[_strRightArrowDisable]; }
			set { ViewState[_strRightArrowDisable] = value; }
		}

		public String LeftArrowDisableImg
		{
			get { return (String)ViewState[_strLeftArrowDisable]; }
			set { ViewState[_strLeftArrowDisable] = value; }
		}

		public String RightArrowEnableImg
		{
			get { return (String)ViewState[_strRightArrowEnable]; }
			set { ViewState[_strRightArrowEnable] = value; }
		}

		public String LeftArrowEnableImg
		{
			get { return (String)ViewState[_strLeftArrowEnable]; }
			set { ViewState[_strLeftArrowEnable] = value; }
		}
		#endregion

		// PROPERTY:: AutoGenerateCheckBoxColumn

		protected const String _strAutoGenerateCheckBoxColumn = "AutoGenerateCheckBoxColumn";

		// PROPERTY:: AutoGenerateSelectSingleColumn

		protected const String _strAutoGenerateSelectSingleColumn = "AutoGenerateSelectSingleColumn";

		// PROPERTY:: CheckBoxColumnIndex

		protected const String _strCheckBoxColumnIndex = "CheckBoxColumnIndex";
		protected const String _strSelectRowSubmit = "SelectRowSubmit";

		// PROPERTY:: CheckBoxColumnIndex

		protected const String _strRadioButtonColumnIndex = "RadioButtonColumnIndex";

		[Category("Behavior")]
		[Description("Whether a checkbox column is generated automatically at runtime")]
		[DefaultValue(false)]
		public bool AutoGenerateCheckBoxColumn
		{
			get
			{
				object o = ViewState[_strAutoGenerateCheckBoxColumn];
				if (o == null)
					return false;
				return (bool) o;
			}
			set { ViewState[_strAutoGenerateCheckBoxColumn] = value; }
		}

		[Category("Behavior")]
		[Description("Whether a radio buttons column is generated automatically at runtime")]
		[DefaultValue(false)]
		public bool AutoGenerateSelectSingleColumn
		{
			get
			{
				object o = ViewState[_strAutoGenerateSelectSingleColumn];
				if (o == null)
					return false;
				return (bool) o;
			}
			set { ViewState[_strAutoGenerateSelectSingleColumn] = value; }
		}

		[Category("Behavior")]
		[Description("Indicates the 0-based position of the checkbox column")]
		[DefaultValue(0)]
		public int CheckBoxColumnIndex
		{
			get
			{
				object o = ViewState[_strCheckBoxColumnIndex];
				if (o == null)
					return 0;
				return (int) o;
			}
			set { ViewState[_strCheckBoxColumnIndex] = (value < 0 ? 0 : value); }
		}

		[Category("Behavior")]
		[Description("Indicates the 0-based position of the radio button column")]
		[DefaultValue(0)]
		public int RadioButtonColumnIndex
		{
			get
			{
				object o = ViewState[_strRadioButtonColumnIndex];
				if (o == null)
					return 0;
				return (int) o;
			}
			set { ViewState[_strRadioButtonColumnIndex] = (value < 0 ? 0 : value); }
		}

		// METHOD:: GetSelectedIndices

		// PROPERTY:: SelectedIndices
		public virtual String RadioButtonSelectedIndexValue
		{
			get { return Page.Request.Form[UniqueID + InputRadioButtonField.RadioButtonID]; }
		}

		
		/// <summary>
		/// The name of the property, from which the Id for each row should be loaded.
		/// </summary>
		[Category("Behavior")]
		[Description("The name of the property, from which the Id for each row should be loaded.")]
		[DefaultValue("")]
		public String RowIdPropertyName
		{
			set { ViewState[_strRowIdPropertyName] = value; }
			get
			{
				object obj = ViewState[_strRowIdPropertyName];
				if (obj is String)
					return (String) obj;
				else
					return "";
			}
		}
		protected const String _strRowIdPropertyName = "RowIdPropertyName";

		/// <summary>
		/// The name of the property, from which the Business Key for each row should be loaded.
		/// </summary>
		[Category("Behavior")]
		[Description("The name of the property, from which the Business Key for each row should be loaded.")]
		[DefaultValue("")]
		public String RowBusinessKeyPropertyName
		{
			set { ViewState[_strRowBusinessKeyPropertyName] = value; }
			get
			{
				object obj = ViewState[_strRowBusinessKeyPropertyName];
				if (obj is String)
					return (String)obj;
				else
					return "";
			}
		}
		protected const String _strRowBusinessKeyPropertyName = "RowBusinessKeyPropertyName";


		// PROPERTY:: OnSelectRowSubmit
		[Category("Behavior")]
		[Description("Auto Submit on select row")]
		[DefaultValue(false)]
		public Boolean AutoSubmitOnSelectRow
		{
			set { ViewState[_strSelectRowSubmit] = value; }
			get
			{
				object obj = ViewState[_strSelectRowSubmit];
				if (obj is Boolean)
					return (Boolean) obj;
				else
					return false;
			}
		}

		public Boolean IsTitleCheckBoxChecked
		{
			get
			{
				string checkBoxID = String.Format(CheckBoxColumHeaderID, ClientID);
				if (!DesignMode)
				{
					object o = Page.Request[checkBoxID];
					if (o != null)
					{
						return true;
					}
				}
				return false;
			}
		}

		[Category("Behavior")]
		[Themeable(true)]
		[Bindable(BindableSupport.No)]
		public new bool ShowHeaderWhenEmpty
		{
			get
			{
				if (ViewState["ShowHeaderWhenEmpty"] == null)
				{
					ViewState["ShowHeaderWhenEmpty"] = false;
				}

				return (bool) ViewState["ShowHeaderWhenEmpty"];
			}
			set { ViewState["ShowHeaderWhenEmpty"] = value; }
		}

		[Category("Behavior")]
		[Themeable(true)]
		[Bindable(BindableSupport.No)]
		public bool ShowFooterWhenEmpty
		{
			get
			{
				if (ViewState["ShowFooterWhenEmpty"] == null)
				{
					ViewState["ShowFooterWhenEmpty"] = false;
				}

				return (bool) ViewState["ShowFooterWhenEmpty"];
			}
			set { ViewState["ShowFooterWhenEmpty"] = value; }
		}

		/// <summary>
		/// Method for accessing to the array of selected rows indices in a grid.
		/// </summary>
		/// <returns>The array of indices of checked rows in the Grid.</returns>
		public virtual int[] GetCheckBoxesSelectedIndices()
		{
			cachedSelectedIndices = new ArrayList();
			for (int i = 0; i < Rows.Count; i++)
			{
				// Retrieve the reference to the checkbox
				CheckBox cb = (CheckBox)Rows[i].FindControl(InputCheckBoxField.CheckBoxID);

				if (cb == null)
					continue;

				if (cb.Checked)
					cachedSelectedIndices.Add(i);
			}

			return (int[]) cachedSelectedIndices.ToArray(typeof(int));
		}

		/// <summary>
		/// Method for accessing of the array of selected rows ids in a Grid.
		/// </summary>
		/// <returns>The array of enities (rows) selected in the Grid.</returns>
		public virtual Int32[] SelectedRowsIds
		{
			get 
			{
				ArrayList selectedIds = new ArrayList();
				for (int i = 0; i < Rows.Count; i++)
				{
					HiddenField field = (HiddenField)Rows[i].FindControl(InputCheckBoxField.RowIdFieldID);

					CheckBox cb = (CheckBox)Rows[i].FindControl(InputCheckBoxField.CheckBoxID);
					if (cb == null)
						continue;

					if (cb.Checked)
					{
						selectedIds.Add(Int32.Parse(field.Value));
					}
				}

				return (Int32[])selectedIds.ToArray(typeof(Int32));
			}
		}

		#endregion

		/// <summary>
		/// Removing of all fields, which allow
		/// </summary>
		public Boolean SelectionFieldsVisible
		{
			set 
			{
				DataControlField field;
				for (int i = Columns.Count - 1; i >= 0; i--)
				{
					field = Columns[i];
					if (field is BoundSelectionField)
						field.Visible = value;
				}            
			}
		}

		#region Members overrides

		// METHOD:: CreateColumns
		protected override ICollection CreateColumns(PagedDataSource dataSource, bool useDataSource)
		{
			// Let the GridView create the default set of columns
			ICollection columnList = base.CreateColumns(dataSource, useDataSource);
			if (AutoGenerateCheckBoxColumn)
			{
				// Add a checkbox column if required
				ArrayList extendedColumnList = AddCheckBoxColumn(columnList);
				return extendedColumnList;
			}

			if (AutoGenerateSelectSingleColumn)
			{
				ArrayList extendedColumnList = AddRadioButtonsColumn(columnList);
				return extendedColumnList;
			}

			return columnList;
		}

		protected override void PrepareControlHierarchy()
		{
			base.PrepareControlHierarchy();

			if (HasControls() && ShowHeader)
			{
				Table table = this.Controls[0] as Table;

				if (table != null && table.Rows.Count > 0)
				{
					// Need to check first TWO rows because the first row may be a
					// pager row...
					GridViewRow headerRow = table.Rows[0] as GridViewRow;
					if (headerRow.RowType != DataControlRowType.Header && table.Rows.Count > 1)
						headerRow = table.Rows[1] as GridViewRow;

					if (headerRow.RowType == DataControlRowType.Header)
					{
						foreach (TableCell cell in headerRow.Cells)
						{
							DataControlFieldCell gridViewCell = cell as DataControlFieldCell;
							if (gridViewCell != null)
							{
								DataControlField cellsField = gridViewCell.ContainingField;
								if (cellsField != null 
								 && cellsField.HeaderStyle.HorizontalAlign != HorizontalAlign.NotSet)
								{
									gridViewCell.Attributes.CssStyle["text-align"] =
										cellsField.HeaderStyle.HorizontalAlign.ToString().ToLower();
								}
							}
						}
					}
				}
			}
		} 

		// METHOD:: OnLoad
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			adjustControllerSettings();
		}

		private void adjustControllerSettings()
		{
			if (String.IsNullOrEmpty(ControllerJSObjectName))
				return;

			DataControlField field;
			for (int i = 0; i < Columns.Count; i++)
			{
				field = Columns[i];
				if (field is BoundSelectionField)
				{
					BoundSelectionField selectionField = (BoundSelectionField)field;
					selectionField.ControllerObjectName = ControllerJSObjectName;
					selectionField.ColumnIndex = i;
				}
			}
		}

		// METHOD:: OnPreRender
		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);

			String url = Page.ClientScript.GetWebResourceUrl(GetType(), HOTGRIDVIEW_JS);
			if (!Page.ClientScript.IsClientScriptIncludeRegistered(GetType(), HOTGRIDVIEW_JS))
				Page.ClientScript.RegisterClientScriptInclude(GetType(), HOTGRIDVIEW_JS, url);

			url = Page.ClientScript.GetWebResourceUrl(GetType(), HOTGRIDVIEW_CONTROLLER_JS);
			if (!Page.ClientScript.IsClientScriptIncludeRegistered(GetType(), HOTGRIDVIEW_CONTROLLER_JS))
				Page.ClientScript.RegisterClientScriptInclude(GetType(), HOTGRIDVIEW_CONTROLLER_JS, url);

			if (_scriptManager == null || !_scriptManager.IsInAsyncPostBack)
			{
				Page.ClientScript.RegisterStartupScript(GetType()
					, "ExecuteRecreateOldGrid" + ClientID
					, recreateHotGridViewObjectsScript, true);                
			}
			else 
			{
				ScriptManager.RegisterStartupScript(this, GetType()
					, "ExecuteRecreateOldGrid" + ClientID
					, recreateHotGridViewObjectsScript, true);
			}

			adjustDataRows();
		}

		protected override void Render(HtmlTextWriter writer)
		{
			base.Render(writer);
		
			if (!String.IsNullOrEmpty(_scriptInit))
			{
				if (_scriptManager == null || !_scriptManager.IsInAsyncPostBack)
				{
					Page.ClientScript.RegisterStartupScript(GetType()
						, "InitGrid" + ClientID
						, _scriptInit, true);
				}
				else
				{
					ScriptManager.RegisterStartupScript(this, GetType()
						, "InitGrid" + ClientID
						, _scriptInit, true);
				}
			}
		}

		private String recreateHotGridViewObjectsScript
		{
			get 
			{
				StringBuilder script = new StringBuilder();

				//Model object should be created at the moment of rendering
				script.AppendFormat(" if (typeof({0}) === 'undefined') {{ var {0} = null; }} "
					, ClientJSObjectName);
				script.AppendFormat(" if (typeof({0}) !== 'undefined' && {0} !== null) {{ delete {0}; }} "
					, ClientJSObjectName);
				script.AppendFormat(" {0} = new HotGridView('{1}', '{2}');"
					, ClientJSObjectName, ClientID, InputCheckBoxField.CheckBoxID);

				script.AppendFormat(" {0}.SetUniqueId('{1}');"
					, ClientJSObjectName, UniqueID);

				//Controller object should be created after the model have been initialized.
				script.AppendFormat(" if (typeof({0}) === 'undefined') {{ var {0} = null; }} "
					, ControllerJSObjectName);
				script.Append(
					" Sys.Application.add_init(function() {");
				script.AppendFormat(
					" if (typeof({0}) !== 'undefined' && {0} !== null) {{ delete {0}; }} "
						, ControllerJSObjectName);
				script.AppendFormat(
					" {0} = new HotGridViewController({1}, '{2}');"
						, ControllerJSObjectName, ClientJSObjectName, _eventTargetId);
				script.Append(" }); ");

				return script.ToString();
			}
		}

		private void adjustDataRows()
		{
			// Adjust each data row
			foreach (GridViewRow r in Rows)
			{
				// Get the appropriate style object for the row
				TableItemStyle style = GetRowStyleFromState(r.RowState);

				// Retrieve the reference to the checkbox
				CheckBox cb = (CheckBox)r.FindControl(InputCheckBoxField.CheckBoxID);
				if (cb == null)
					continue;

				// Build the ID of the checkbox in the header
				string headerCheckBoxID = String.Format(CheckBoxColumHeaderID, ClientID);

				// Add script code to enable selection
				cb.Attributes["onclick"] = String.Format("{9}.ApplyStyle(this, '{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}')",
														 SelectedRowStyle.CssClass,
														 ColorTranslator.ToHtml(SelectedRowStyle.ForeColor),
														 ColorTranslator.ToHtml(SelectedRowStyle.BackColor),
														(SelectedRowStyle.Font.Bold ? 700 : 400),
														style.CssClass,
														 ColorTranslator.ToHtml(style.ForeColor),
														 ColorTranslator.ToHtml(style.BackColor),
														(style.Font.Bold ? 700 : 400),
														 headerCheckBoxID,
														 ClientJSObjectName);
				// Update the style of the checkbox if checked
				if (cb.Checked)
				{
					r.BackColor = SelectedRowStyle.BackColor;
					r.ForeColor = SelectedRowStyle.ForeColor;
					r.Font.Bold = SelectedRowStyle.Font.Bold;
				}
				else
				{
					r.BackColor = style.BackColor;
					r.ForeColor = style.ForeColor;
					r.Font.Bold = style.Font.Bold;
				}
			}
		}

		protected override void OnRowCreated(GridViewRowEventArgs e)
		{
			base.OnRowCreated(e);

			if (!IsNeedSorting())
				return;

			if (e.Row.RowType == DataControlRowType.Header)
			{
				string ImagePath = (SortDirection == SortDirection.Ascending) ?
										   Page.ClientScript.GetWebResourceUrl(GetType(), "Controls.HotGridView.images.down.gif")
										 : Page.ClientScript.GetWebResourceUrl(GetType(), "Controls.HotGridView.images.up.gif");
				StringBuilder ImageControl = new StringBuilder();
				ImageControl.Append("&nbsp;<img src=\"");
				ImageControl.Append(ImagePath);
				ImageControl.Append("\" border=\"0\" />");

				Int32? indexSort = null;
				// go through all columns
				for (int i = 0; i < Columns.Count; i++)
				{
					string sort = Columns[i].SortExpression;
					if (!String.IsNullOrEmpty(SortExpression)
						&& !String.IsNullOrEmpty(sort)
						&& sort == SortExpression)
					{
						Int32 index = i;
						if (AutoGenerateCheckBoxColumn)
						{
							if (CheckBoxColumnIndex <= index)
								index ++;
						}

						if (AutoGenerateSelectSingleColumn)
						{
							if (RadioButtonColumnIndex <= index)
								index ++;
						}

						e.Row.Cells[index].Controls.Add(new LiteralControl(ImageControl.ToString()));
						indexSort = index;
						break;
					}
				}
				InsertSquareToCells(e, indexSort);
			}
		}

		private void InsertSquareToCells(GridViewRowEventArgs e, Int32? notInIndex)
		{
			StringBuilder ImageControlSquare = new StringBuilder();
			ImageControlSquare.Append("&nbsp;<img src=\"");
			ImageControlSquare.Append(
				Page.ClientScript.GetWebResourceUrl(GetType(), "Controls.HotGridView.images.square.gif"));
			ImageControlSquare.Append("\" border=\"0\" />");

			foreach (TableCell cell in e.Row.Cells)
			{
				Int32 cellIndex = e.Row.Cells.GetCellIndex(cell);
				Int32 columnIndex = cellIndex;
				if (AutoGenerateCheckBoxColumn && CheckBoxColumnIndex < cellIndex)
					columnIndex--;
				if (AutoGenerateSelectSingleColumn && RadioButtonColumnIndex < cellIndex)
					columnIndex--;
				if (columnIndex >= 0 && (notInIndex == null || notInIndex != cellIndex) && (!AutoGenerateCheckBoxColumn ||
					 (AutoGenerateCheckBoxColumn && cellIndex != CheckBoxColumnIndex))
					&&
					(!AutoGenerateSelectSingleColumn ||
					 (AutoGenerateSelectSingleColumn && cellIndex != RadioButtonColumnIndex))
					&& !String.IsNullOrEmpty(Columns[columnIndex].SortExpression))
					cell.Controls.Add(new LiteralControl(ImageControlSquare.ToString()));
			}
		}

		private bool IsNeedSorting()
		{
			bool isSorting = false;

			foreach (DataControlField field in Columns)
			{
				if (!String.IsNullOrEmpty(field.SortExpression))
					isSorting = true;
			}
			return isSorting;
		}

		protected override int CreateChildControls(IEnumerable dataSource, bool dataBinding)
		{
			int rows = base.CreateChildControls(dataSource, dataBinding);

			//  no data rows created, create empty table if enabled
			if (rows == 0 && (ShowFooterWhenEmpty || ShowHeaderWhenEmpty))
			{
				//  create the table
				Table table = CreateChildTable();

				DataControlField[] fields;
				if (AutoGenerateColumns)
				{
					PagedDataSource source = new PagedDataSource();
					source.DataSource = dataSource;

					ICollection autoGeneratedColumns = CreateColumns(source, true);
					fields = new DataControlField[autoGeneratedColumns.Count];
					autoGeneratedColumns.CopyTo(fields, 0);
				}
				else
				{
					ArrayList extendedList = null;
					if (AutoGenerateCheckBoxColumn)
						extendedList = AddCheckBoxColumn(Columns);

					if (AutoGenerateSelectSingleColumn)
						extendedList = AddRadioButtonsColumn(Columns);

					if (extendedList != null)
					{
						fields = new DataControlField[extendedList.Count];
						extendedList.CopyTo(fields, 0);
					}
					else
					{
						fields = new DataControlField[Columns.Count];
						Columns.CopyTo(fields, 0);
					}
				}

				if (ShowHeaderWhenEmpty)
				{
					//  create a new header row
					GridViewRow headerRow =
						base.CreateRow(-1, -1, DataControlRowType.Header, DataControlRowState.Normal);
					InitializeRow(headerRow, fields);
					for (int i = 0; i < headerRow.Cells.Count; i++)
					{
						headerRow.Cells[i].Width = fields[i].ItemStyle.Width;
					}

					//  add the header row to the table
					table.Rows.Add(headerRow);
				}

				//  create the empty row
				GridViewRow emptyRow =
					new GridViewRow(-1, -1, DataControlRowType.EmptyDataRow, DataControlRowState.Normal);
				TableCell cell = new TableCell();
				cell.ColumnSpan = fields.Length;
				cell.Width = Unit.Percentage(100);

				//  respect the precedence order if both EmptyDataTemplate
				//  and EmptyDataText are both supplied ...
				if (EmptyDataTemplate != null)
				{
					EmptyDataTemplate.InstantiateIn(cell);
				}
				else if (!string.IsNullOrEmpty(EmptyDataText))
				{
					cell.Controls.Add(new LiteralControl(EmptyDataText));
				}

				emptyRow.Cells.Add(cell);
				table.Rows.Add(emptyRow);

				if (ShowFooterWhenEmpty)
				{
					//  create footer row
					GridViewRow footerRow =
						base.CreateRow(-1, -1, DataControlRowType.Footer, DataControlRowState.Normal);
					InitializeRow(footerRow, fields);

					//  add the footer to the table
					table.Rows.Add(footerRow);
				}

				Controls.Clear();
				Controls.Add(table);
			}

			return rows;
		}

		#region Custom Events

		public event EventHandler SelectRowAutoSubmit
		{
			add { Events.AddHandler(EventSelectRowSubmit, value); }
			remove { Events.RemoveHandler(EventSelectRowSubmit, value); }
		}

		private static readonly object EventSelectRowSubmit = new object();

		protected virtual void OnSelectRow(EventArgs e)
		{
			EventHandler eh = Events[EventSelectRowSubmit] as EventHandler;
			if (eh != null)
			{
				eh(this, e);
			}
		}

		protected override void RaisePostBackEvent(string eventArgument)
		{
			base.RaisePostBackEvent(eventArgument);

			OnSelectRow(EventArgs.Empty);
		}

		#endregion

		#region Custom Pager support

		protected override void InitializePager(GridViewRow row, int columnSpan, PagedDataSource pagedDataSource)
		{
			if (UseCustomPager)
				CreateCustomPager(row, columnSpan, pagedDataSource);
			else
				base.InitializePager(row, columnSpan, pagedDataSource);
		}

		protected virtual void CreateCustomPager(GridViewRow row, int columnSpan, PagedDataSource pagedDataSource)
		{
			int pageCount = pagedDataSource.PageCount;
			int pageIndex = pagedDataSource.CurrentPageIndex + 1;
			int pageButtonCount = PagerSettings.PageButtonCount;

			TableCell cell = new TableCell();
			row.Cells.Add(cell);
			if (columnSpan > 1) cell.ColumnSpan = columnSpan;

			if (pageCount > 1)
			{
				var pager = new HtmlGenericControl("div");
				pager.Attributes["class"] = "control-line-of-controls";
				pager.Attributes["style"] = "padding-top: 4px;";
				cell.Controls.Add(pager);

				int min = pageIndex - pageButtonCount;
				int max = pageIndex + pageButtonCount;

				if (max > pageCount)
					min -= max - pageCount;
				else if (min < 1)
					max += 1 - min;

				var page = BuildDiv(getFooterPageText(pageIndex, pagedDataSource.DataSourceCount), "left", "padding-top: 2px;");
				pager.Controls.Add(page);

				// Create drop list page size
				var pageSizeSelector = BuildDropListPageSize();
				pager.Controls.Add(pageSizeSelector);

				var rightDiv = BuildDiv(string.Empty, "right", "padding-top: 2px;");

				page = BuildSpan("Page: ", "current");
				rightDiv.Controls.Add(page);

				// Create page buttons
				bool needDiv = false;
				for (int i = 1; i <= pageCount; i++)
				{
					if (i <= 2 || i > pageCount - 2 || (min <= i && i <= max))
					{
						string text = i.ToString(NumberFormatInfo.InvariantInfo);
						page = i == pageIndex
								? BuildSpan(text, "current", true)
								: BuildLinkButton(i - 1, text, "Page", text, null);
						rightDiv.Controls.Add(page);
						needDiv = true;
					}
					else if (needDiv)
					{
						page = BuildSpan("&hellip;", null);
						rightDiv.Controls.Add(page);
						needDiv = false;
					}
				}

				// Create Prev Img
				if (!String.IsNullOrEmpty(LeftArrowEnableImg))
				{
					page = BuildImgButton("Page", "Prev", true, pageIndex, pageCount, "margin-left: 5px;");
					rightDiv.Controls.Add(page);
				}

				// Create "previous" button
				page = pageIndex > 1
								 ? BuildLinkButton(pageIndex - 2, PagerSettings.PreviousPageText, "Page", "Prev", "margin-left: 5px")
								 : BuildSpanWithStyle(PagerSettings.PreviousPageText, "margin-left: 5px");
				rightDiv.Controls.Add(page);

				// Create "next" button
				page = pageIndex < pageCount
						? BuildLinkButton(pageIndex, PagerSettings.NextPageText, "Page", "Next", "margin-left: 5px")
						: BuildSpanWithStyle(PagerSettings.NextPageText, "margin-left: 5px");
				rightDiv.Controls.Add(page);

				// Create Next Img
				if (!String.IsNullOrEmpty(RightArrowEnableImg))
				{
					page = BuildImgButton("Page", "Next", false, pageIndex, pageCount, "margin-left: 5px;");
					rightDiv.Controls.Add(page);
				}

				pager.Controls.Add(rightDiv);
			}
		}

		private Control BuildDropListPageSize()
		{
			// Create drop list page size
			var pageSizeSelector = BuildDiv(string.Empty, "left", string.Empty);

			var ddlPageSize = new DropDownList {AutoPostBack = true, CssClass = "control-dropdownlist"};
			ddlPageSize.SelectedIndexChanged += ddlPageSize_SelectedIndexChanged;

			var itemsPerPage = new List<int> {10, 20, 30, 50};
			itemsPerPage = itemsPerPage.Union(new List<int> {PageSize}).ToList();

			foreach (var itemsCount in itemsPerPage)
			{
				ddlPageSize.Items.Add(new ListItem(itemsCount.ToString(), itemsCount.ToString()));
			}

			ddlPageSize.SelectedValue = PageSize.ToString();

			pageSizeSelector.Controls.Add(ddlPageSize);

			return pageSizeSelector;
		}

		private Control BuildImgButton(string commandName, string commandArgument, bool isLeft, int pageIndex, int pageCount, string style)
		{
			ImageButton image = new ImageButton();
			if (isLeft)
			{
				image.AlternateText = PagerSettings.PreviousPageText;
				image.ImageUrl = pageIndex > 1 ? LeftArrowEnableImg : LeftArrowDisableImg;
				image.Enabled = (pageIndex > 1);
			}
			else
			{
				image.AlternateText = PagerSettings.NextPageText;
				image.ImageUrl = pageIndex < pageCount ? RightArrowEnableImg : RightArrowDisableImg;
				image.Enabled = (pageIndex < pageCount);
			}

			image.CausesValidation = false;
			image.ImageAlign = ImageAlign.Top;
			image.CommandName = commandName;
			image.CommandArgument = commandArgument;

			if (!String.IsNullOrEmpty(style))
				image.Attributes["style"] = String.Format("{0} cursor: {1};"
					, style, (image.Enabled ? "pointer" : "default"));

			return image;
		}

		private Control BuildLinkButton(int pageIndex, string text, string commandName, string commandArgument, string style)
		{
			PagerLinkButton link = new PagerLinkButton(this);
			link.Text = text;
			link.EnableCallback(ParentBuildCallbackArgument(pageIndex));
			link.CommandName = commandName;
			link.CommandArgument = commandArgument;
			if (!String.IsNullOrEmpty(style))
				link.Attributes["style"] = style;
			return link;
		}

		private Control BuildSpan(string text, string cssClass, bool isBold)
		{
			return BuildSpan("[" + text + "]", cssClass);
		}

		private Control BuildSpan(string text, string cssClass)
		{
			HtmlGenericControl span = new HtmlGenericControl("span");
			if (!String.IsNullOrEmpty(cssClass)) span.Attributes["class"] = cssClass;
			span.InnerHtml = text ;
			return span;
		}

		private Control BuildSpanWithStyle(string text, string style)
		{
			HtmlGenericControl span = new HtmlGenericControl("span");
			if (!String.IsNullOrEmpty(style)) span.Attributes["style"] = style;
			span.InnerHtml = text;
			return span;
		}

		private Control BuildDiv(string text, string floatDiv, string style)
		{
			var div = new HtmlGenericControl("div");
			
			if (!string.IsNullOrEmpty(floatDiv)) 
				div.Attributes["style"] = string.Format("float: {0}; {1}", floatDiv, style);

			div.InnerHtml = text;

			return div;
		}

		private String getFooterPageText(int pageIndex, int totalRows)
		{
			String str = String.Empty;
			int beginRow = (pageIndex - 1) * PageSize + 1;
			int endRow = beginRow + PageSize - 1;
			if (endRow > totalRows)
				endRow -= endRow - totalRows;
			str = String.Format("{0}-{1} of {2}", beginRow, endRow, totalRows);
			return str;
		}

		private string ParentBuildCallbackArgument(int pageIndex)
		{
			MethodInfo m =
				typeof(System.Web.UI.WebControls.GridView).GetMethod("BuildCallbackArgument", BindingFlags.NonPublic | BindingFlags.Instance, null,
											new Type[] { typeof(int) }, null);
			return (string)m.Invoke(this, new object[] { pageIndex });
		}

		#endregion
		
		#endregion

		#region EventHandlers

		private void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
		{
			var ddlPageSize = (DropDownList) sender;

			PageSize = int.Parse(ddlPageSize.SelectedValue);
		}

		#endregion
	}
}