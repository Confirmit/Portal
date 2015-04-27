using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections;
using System.Text;
using System.ComponentModel;
using Core;
using Core.Security;
using EPAMSWeb.UI;
using System.Reflection;
using System.Security.Policy;
using System.Resources;

using CheckBox = EPAMSWeb.UI.CheckBox;

#region enum EditModes - ������ �������������� ���������
/// <summary>
/// ������ �������������� ���������.
/// </summary>
public enum EditModes
{
	/// <summary>
	/// � ���� ������ �� ������� ������������� ����������� ������ ����������� ����������� ������� Grid'�.
	/// </summary>
	Custom,
	/// <summary>
	/// 
	/// </summary>
	Simple,
	/// <summary>
	/// 
	/// </summary>
	WithMarker,
	/// <summary>
	/// 
	/// </summary>
	WithParentMarker
}

#endregion

#region ������ ���������� �������
/// <summary>
/// ����� ��� ���������� ������� �� ��������.
/// </summary>
public class GridDeleteEventArgs : EventArgs
{
	private DataKeyArray m_DataKeys;

	public GridDeleteEventArgs(GridDeleteEventArgs args)
		: this(args.DataKeys)
	{
	}

	public GridDeleteEventArgs(DataKeyArray keys)
	{
		m_DataKeys = keys;
	}

	/// <summary>
	/// ������ ������ � ��������, ��� ������� ����� ���������� ��������.
	/// </summary>
	public DataKeyArray DataKeys
	{
		get
		{
			return m_DataKeys;
		}
	}
}

/// <summary>
/// ����� ��� ���������� ������� �� ��������������.
/// </summary>
public class GridEditEventArgs : EventArgs
{
	private string m_DataKeyValue;

	public GridEditEventArgs(GridEditEventArgs args)
		: this(args.DataKeyValue)
	{
	}
	public GridEditEventArgs(string dataKeyValue)
	{
		m_DataKeyValue = dataKeyValue;
	}

	/// <summary>
	/// �������� �����
	/// </summary>
	public string DataKeyValue
	{
		get
		{
			return m_DataKeyValue;
		}
	}
}

/// <summary>
/// ����� ��� ���������� ������� �� ���������.
/// </summary>
public class GridSelectEventArgs : EventArgs
{
	private int m_SelectedIndex;
	private bool m_Selected;

	public GridSelectEventArgs( GridSelectEventArgs args )
		: this( args.SelectedIndex, args.Selected )
	{
	}
	public GridSelectEventArgs( int selectedIndex, bool selected )
	{
		m_SelectedIndex = selectedIndex;
		m_Selected = selected;
	}

	/// <summary>
	/// ������ ���������� ������.
	/// </summary>
	public int SelectedIndex
	{
		get
		{
			return m_SelectedIndex;
		}
	}

	/// <summary>
	/// True, ���� ������ ��������. Fals�, ���� ������ ��������.
	/// </summary>
	public bool Selected
	{
		get
		{
			return m_Selected;
		}
	}
}
#endregion

#region ��������
/// <summary>
/// 
/// </summary>
/// <param name="sender"></param>
/// <param name="args"></param>
/// <returns></returns>
public delegate PagingResult GridRequestDatasourceHandler(object sender, PagingArgs args);
/// <summary>
/// 
/// </summary>
/// <param name="sender"></param>
/// <param name="args"></param>
public delegate void GridDeleteEventHandler(object sender, GridDeleteEventArgs args);
/// <summary>
/// 
/// </summary>
/// <param name="sender"></param>
/// <param name="args"></param>
public delegate void GridEditEventHandler(object sender, GridEditEventArgs args);
/// <summary>
/// 
/// </summary>
/// <returns></returns>
public delegate Dictionary<string, string> GridAdditionalParametersHandler();
/// <summary>
/// 
/// </summary>
/// <param name="sender"></param>
/// <param name="args"></param>
public delegate void GridSelectEventHandler( object sender, GridSelectEventArgs args );
/// <summary>
/// 
/// </summary>
/// <param name="sender"></param>
/// <returns></returns>
public delegate GridSelection GridResolveSelectionEventHandler(object sender);

#endregion

#region HierarchicalRowState - ��������� ���������� ��������.
/// <summary>
/// ��������� ���������� ��������.
/// </summary>
public enum HierarchicalRowState
{
	Collapsed,
	Expanded
}

#endregion

#region HierarchicalStateCollection - ��������� ��� ������� � ��������� ������������ ��������.
/// <summary>
/// ��������� ��� ������� � ��������� ������������ ��������.
/// </summary>
[Serializable]
public class HierarchicalRowStateCollection
{
	private Dictionary<int, HierarchicalRowState> m_States;

	protected Dictionary<int, HierarchicalRowState> States
	{
		get
		{
			if (m_States == null)
			{
				m_States = new Dictionary<int, HierarchicalRowState>();
			}
			return m_States;
		}
	}

	/// <summary>
	/// ���������� ��� ������������� ��������� ������ � �������� ��������.
	/// ��������: ��� ��������� ������ ��������� ������ ����� 
	/// ����������� ��������� ������ ����� (����� RefreshData(false)).
	/// </summary>
	/// <param name="index">������ ������ (rowIndex).</param>
	/// <returns></returns>
	public HierarchicalRowState this[int index]
	{
		get
		{
			return States.ContainsKey(index) ? States[index] : HierarchicalRowState.Collapsed;
		}
		set
		{
			States[index] = value;
		}
	}

	/// <summary>
	/// ���������� ��� ��������� � "��������".
	/// </summary>
	public void Clear()
	{
		States.Clear();
	}
}

#endregion

#region GridSelection - �����, ��������������� ������ � ����������
/// <summary>
/// �������������� ������ � ���������� � �����.
/// </summary>
public class GridSelection
{
	#region ������������

	public GridSelection( Grid owner )
	{
		m_Owner = owner;
	}

	#endregion

	#region ����

	private Grid m_Owner;

	#endregion

	#region ��������

	/// <summary>
	/// ����, ��� �������� ������ ������ ���������� ���������.
	/// </summary>
	public Grid Owner
	{
		get
		{
			return m_Owner;
		}
	}

	/// <summary>
	/// ������������� ��� ���������� ���� ��������� ������ � ��������� ������.
	/// </summary>
	/// <param name="key"></param>
	/// <returns></returns>
	public virtual bool this[DataKey key]
	{
		get
		{
			return AllSelectedKeys.ContainsKey( key.Value );
		}
		set
		{
			if(value)
			{
				if(!AllSelectedKeys.ContainsKey( key.Value ))
				{
					AllSelectedKeys.Add( key.Value, key );
				}
			}
			else
			{
				AllSelectedKeys.Remove( key.Value );
			}
		}
	}

	/// <summary>
	/// ������� � ���������������� ���� ��������� �����.
	/// </summary>
	private Dictionary<object, DataKey> AllSelectedKeys
	{
		get
		{
			string key = String.Format( "AllSelectedKeys_{0}", m_Owner.UniqueID );
			Dictionary<object, DataKey> o = (Dictionary<object, DataKey>)HttpContext.Current.Session[key];
			if(o == null)
			{
				o = new Dictionary<object, DataKey>();
				HttpContext.Current.Session[key] = o;
			}
			return o;
		}
	}

	/// <summary>
	/// ��������� ������ ��������� �����
	/// </summary>
	public virtual DataKeyArray SelectedKeys
	{
		get
		{
			ArrayList list = new ArrayList();
			foreach(KeyValuePair<object, DataKey> de in AllSelectedKeys)
			{
				list.Add( de.Value );
			}
			return new DataKeyArray( list );
		}
	}

	/// <summary>
	/// ���������� ���������� ���������.
	/// </summary>
	public virtual int Count
	{
		get
		{
			return AllSelectedKeys.Count;
		}
	}

	#endregion

	#region ������
	/// <summary>
	/// ���������� ���������.
	/// </summary>
	public virtual void Clear()
	{
		AllSelectedKeys.Clear();
	}

	/// <summary>
	/// ���������� ������, �������������� �� �������� ���������� ������, ������������� ����� �������.
	/// </summary>
	/// <returns></returns>
	public override string ToString()
	{
		StringBuilder keys = new StringBuilder();
		foreach(DataKey key in SelectedKeys)
		{
			if(keys.Length > 0) keys.Append( "," );
			keys.Append( key.Value.ToString() );
		}
		return keys.ToString();
	}
	#endregion
}

#endregion

/// <summary>
/// ������� "����", ����������� �������� ������ � ���� �������.
/// </summary>
public partial class Grid : BaseUserControl, IAccessible, IGridRowsContainer, IGridSelection
{
	#region ����

	private ITemplate m_HierarchicalRowTemplate;
	private ITemplate m_ButtonContainerTemplate;
	private GridSelection m_Selection;

	#endregion

	#region �������
	/// <summary>
	/// �������, �����������, ����� ����� ���������� �������� ������.
	/// </summary>
	public event GridRequestDatasourceHandler RequestDatasource;
	/// <summary>
	/// �������, �����������, ����� ��������� �������� ���������� ��������� � �����.
	/// ������������ �� ��� �������, ����� ��������� ����������� ���������� ��������� ������, 
	/// ��� �������� �������� ������ ��������� � �������� ���������� �����.
	/// </summary>
	public event GridResolveSelectionEventHandler ResolveSelection;
	/// <summary>
	/// �������, ����������� ����� ����, ��� ���� ������� ������ � �������� ���� (����� ���������� ������ � �����).
	/// </summary>
	public event EventHandler DataBound;
	/// <summary>
	/// �������, ����������� �� �������� ������ �����.
	/// </summary>
	public event GridViewRowEventHandler RowCreated;
	/// <summary>
	/// �������, ����������� �� ������������ ������ �����.
	/// </summary>
	public event GridViewRowEventHandler RowDataBound;
	/// <summary>
	/// �������, ����������� ����� ��������� ������ ���������� ��������.
	/// �� ������ ����� ���� ����������� �������� ����� ���������� ��������.
	/// ������ ������� ��������� ������ � ��� ������, ����� ��������� ������� �������.
	/// </summary>
	public event GridViewRowEventHandler HierarchicalRowPreCreated;
	/// <summary>
	/// �������, ����������� ����� ����� �������� ������ ���������� ��������.
	/// �� ������ ����� ���� ����������� ��������� ��������, ����������� � �������, ��� ������������ ������.
	/// ������ ������� ��������� ������ � ��� ������, ����� ��������� ������� �������.
	/// </summary>
	public event GridViewRowEventHandler HierarchicalRowCreated;
	/// <summary>
	/// ������, ����������� ��� �������� ������ � ���������� ��������.
	/// �� ������ ����� ���� ����������� ��������� �������� ���������� ��������.
	/// ������ ������� ��������� ������ � ��� ������, ����� ��������� ������� �������.
	/// </summary>
	public event GridViewRowEventHandler HierarchicalRowDataBound;
	/// <summary>
	/// �������, ����������� �� ������� �����.
	/// </summary>
	public event GridViewCommandEventHandler RowCommand;
	/// <summary>
	/// ��������: ������� �� ������� �����, ���� ���-������ �� ���� ���������.
	/// </summary>
	public event GridViewEditEventHandler RowEdit;
	/// <summary>
	/// �������, ����������� �� ����� �������� �����.
	/// </summary>
	public event GridViewSelectEventHandler RowSelect;
	/// <summary>
	/// �������, ����������� ��� ������� �� ������ Delete (� ������ AutoGenerateDeleteButton=true).
	/// </summary>
	public event GridDeleteEventHandler AutoDeleting;
	/// <summary>
	/// �������, ����������� ��� ������� �� ������ Add (� ������ AutoGenerateAddButton=true � EditMode=Custom ).
	/// </summary>
	public event EventHandler AutoAdding;
	/// <summary>
	/// �������, ����������� ��� ������� �� ������ Edit (� ������ AutoGenerateEditButton=true � EditMode=Custom).
	/// </summary>
	public event GridEditEventHandler AutoEditing;
	/// <summary>
	/// �������, ����������� ����� ������������� �������� �����.
	/// � ��� ����� ���������� �������� �����-������ �������.
	/// </summary>
	public event EventHandler CreateColumns;
	/// <summary>
	/// �������, ����������� ��� ��������� �������������� ����������, 
	/// ������������ ����� Url �������� �������������� �������.
	/// </summary>
	public event GridAdditionalParametersHandler ResolveAddEditParameters;
	/// <summary>
	/// �������, ����������� ��� ��������� ������ � ����� �� �������.
	/// </summary>
	public event GridSelectEventHandler RowClientSelect;

	#endregion

	#region �������� ����������� �����, ��������������� �� ������� �������

	private DataControlFieldCollection m_columns = new DataControlFieldCollection();

	/// <summary>
	/// 
	/// </summary>
	[PersistenceMode(PersistenceMode.InnerProperty)]
	public DataControlFieldCollection Columns
	{
		get
		{
			return m_columns;
		}
	}

	/// <summary>
	/// 
	/// </summary>
	[Browsable(false)]
	public GridViewRowCollection Rows
	{
		get
		{
			return innerGrid.Rows;
		}
	}

	/// <summary>
	/// 
	/// </summary>
	[Browsable(false)]
	public GridViewRow HeaderRow
	{
		get
		{
			return innerGrid.HeaderRow;
		}
	}

	/// <summary>
	/// ���������� ������� (������������ ��������).
	/// </summary>
	[Browsable(true)]
	public bool ShowPager
	{
		get
		{
			object o = ViewState["ShowPager"];
			return o != null ? (bool)o : true;
		}
		set
		{
			ViewState["ShowPager"] = value;
		}
	}

	/// <summary>
	/// ���������� ��������� �������.
	/// </summary>
	[Browsable(true)]
	public bool ShowHeader
	{
		get
		{
			return ViewState["ShowHeader"] == null ? true : (bool)ViewState["ShowHeader"];
		}
		set
		{
			ViewState["ShowHeader"] = value;
			if (innerGrid != null)
			{
				innerGrid.ShowHeader = value;
			}
		}
	}

	/// <summary>
	/// ������� �� ����� ������.
	/// </summary>
	[Browsable(true)]
	public bool SearchMode
	{
		get
		{
			return ViewState["SearchMode"] != null ? (bool)ViewState["SearchMode"] : false;
		}
		set
		{
			ViewState["SearchMode"] = value;
		}
	}

	#endregion

	#region ��������, ���������� �� ���������
	/// <summary>
	/// URL ������� ��� �������������� �������� (��� ������� �������������� Simple, WithMarker � WithParentMarker).
	/// </summary>
	[Browsable(true)]
	public string EditURL
	{
		get
		{
			object o = ViewState["EditURL"];
			return o != null ? (string)o : String.Empty;
		}
		set
		{
			ViewState["EditURL"] = value;
		}
	}

	/// <summary>
	/// ����� ��������������.
	/// </summary>
	[Browsable(true)]
	public EditModes EditMode
	{
		get
		{
			object o = ViewState["EditMode"];
			return o != null ? (EditModes)o : EditModes.Custom;
		}
		set
		{
			ViewState["EditMode"] = value;
		}
	}

	/// <summary>
	/// ��������� ������� ��������������.
	/// </summary>
	[Browsable(true)]
	public bool AutoGenerateEditButton
	{
		get
		{
			object o = ViewState["AutoGenerateEditButton"];
			return o != null ? (bool)o : false;
		}
		set
		{
			ViewState["AutoGenerateEditButton"] = value;
		}
	}

	/// <summary>
	/// ��������� ������ ��������������.
	/// </summary>
	[Browsable(true)]
	public bool AutoGenerateDeleteButton
	{
		get
		{
			object o = ViewState["AutoGenerateDeleteButton"];
			return o != null ? (bool)o : false;
		}
		set
		{
			ViewState["AutoGenerateDeleteButton"] = value;
		}
	}

	/// <summary>
	/// ������������� ��������������� ������ ��������
	/// </summary>
	public System.Web.UI.WebControls.Button DeleteButton
	{
		get
		{
			return AutoGenerateDeleteButton ? btnDelete_AutoGenerated : null;
		}
	}

	/// <summary>
	/// ��������� ������ �������� ������ �������.
	/// </summary>
	[Browsable(true)]
	public bool AutoGenerateAddButton
	{
		get
		{
			object o = ViewState["AutoGenerateAddButton"];
			return o != null ? (bool)o : false;
		}
		set
		{
			ViewState["AutoGenerateAddButton"] = value;
		}
	}

	/// <summary>
	/// ������������� ��������������� ������ ����������
	/// </summary>
	public System.Web.UI.WebControls.Button AddButton
	{
		get
		{
			return AutoGenerateAddButton ? btnAdd_AutoGenerated : null;
		}
	}

	/// <summary>
	/// ���������� ������ �����, ������� �������� ����������
	/// </summary>
	[Browsable(true)]
	public string AllowedRolesToAdd
	{
		get
		{
			object o = ViewState["AllowedRolesToAdd"];
			return o != null ? (string)o : String.Empty;
		}
		set
		{
			ViewState["AllowedRolesToAdd"] = value;
		}
	}

	/// <summary>
	/// ���������� ������ �����, ������� �������� ��������������
	/// </summary>
	[Browsable(true)]
	public string AllowedRolesToEdit
	{
		get
		{
			object o = ViewState["AllowedRolesToEdit"];
			return o != null ? (string)o : String.Empty;
		}
		set
		{
			ViewState["AllowedRolesToEdit"] = value;
		}
	}

	/// <summary>
	/// ���������� ������ �����, ������� �������� ��������
	/// </summary>
	[Browsable(true)]
	public string AllowedRolesToDelete
	{
		get
		{
			object o = ViewState["AllowedRolesToDelete"];
			return o != null ? (string)o : String.Empty;
		}
		set
		{
			ViewState["AllowedRolesToDelete"] = value;
		}
	}

	/// <summary>
	/// �������������� ��������, ������������ � ���������� � ���������� ������� ������ "��������" � "�������������"
	/// </summary>
	[Browsable(true)]
	public string GridMarker
	{
		get
		{
			return ViewState["GridMarker"] == null ? "" : (string)ViewState["GridMarker"];
		}
		set
		{
			ViewState["GridMarker"] = value;
		}
	}

	/// <summary>
	/// ������ ��� �������������� ��������� (������), 
	/// ������� ����� ������������� ��� ������ ������ �� ������������� ��������������� ������.
	/// </summary>
	[DefaultValue((string)null)]
	[TemplateInstance(TemplateInstance.Single)]
	[PersistenceMode(PersistenceMode.InnerProperty)]
	public virtual ITemplate ButtonContainerTemplate
	{
		get
		{
			return this.m_ButtonContainerTemplate;
		}
		set
		{
			this.m_ButtonContainerTemplate = value;
		}
	}

	#endregion

	#region �������� ���������� UpdatePanel
	/// <summary>
	/// �������� �� ���������� �����.
	/// </summary>
	[DefaultValue((string)null)]
	[PersistenceMode(PersistenceMode.InnerProperty)]
	public UpdatePanelTriggerCollection Triggers
	{
		get
		{
			return updatePanel.Triggers;
		}
	}

	/// <summary>
	/// ��������� ���������� ������.
	/// ����� ������������ ���� ����� ��� ���������� ����� 
	/// � ������ ����������� ��������� �� ����� �������� Triggers, � ����� ������ ScriptManager,
	/// ��������, RegisterAsyncPostBackControl().
	/// </summary>
	public void Update()
	{
		updatePanel.Update();
	}

	#endregion

	#region ��������, ��������� � ����������� � ����������

	/// <summary>
	/// ��������� � ���������� ���������� �������� ����� ��� �������� � ������ ���������� �����.
	/// </summary>
	/// <param name="key">�������� ����� (��������, SortExpression)</param>
	/// <returns></returns>
	public string GetUniqueSessionKey(string key)
	{
		return String.Format("{0}_{1}_{2}", Request.RawUrl, UniqueID, key);
	}

	/// <summary>
	/// �������� �� ���������� �� �����������.
	/// </summary>
	protected bool SortOrderAsc
	{
		get
		{
			object o = Session[GetUniqueSessionKey("SortOrderAsc")];
			return o != null ? (bool)o : true;
		}
		set
		{
			Session[GetUniqueSessionKey("SortOrderAsc")] = value;
		}
	}


	/// <summary>
	/// ��������� ��� ����������.
	/// </summary>
	protected string SortExpression
	{
		get
		{
			object o = Session[GetUniqueSessionKey("SortExpression")];
			return o != null ? (string)o : String.Empty;
		}
		set
		{
			Session[GetUniqueSessionKey("SortExpression")] = value;
		}
	}

	/// <summary>
	/// ������ �������� (���������� ������� �� ��������).
	/// </summary>
	protected int PageSize
	{
		get
		{
			if (!ShowPager) return PagingArgs.MaxPageSize; // ���� �������� ��������, ������ ���������� ������ "�����������" ��������
			
			//if(User.Current.Role == Role.Anonymous)
			{
				object o = Session[GetUniqueSessionKey("PageSize")];
				return o != null ? (int)o : 10;
			}
			/*else if (!User.Current.DefaultPageSize.HasValue)
			{
				return 10;
			}

			return (int)User.Current.DefaultPageSize;

            */
		}
		set
		{
			/*if (User.Current.i == Role.Anonymous)
			{
				Session[GetUniqueSessionKey("PageSize")] = value;
			}
			else
			{
				User.Current.DefaultPageSize = value;
				User.Current.Save();
			}*/
            Session[GetUniqueSessionKey("PageSize")] = value;
		}
	}

	/// <summary>
	/// ������ �������� (�������� ���������� � 0).
	/// </summary>
	protected int PageIndex
	{
		get
		{
			if (!ShowPager) return 0; // ���� �������� ��������, ������ ���������� ������ ��������
			
			string key = GetUniqueSessionKey( "PageIndex" );
			if(Session[key] == null)
			{
				Session[key] = 0;
			}
			return (int)Session[key];
		}
		set
		{
			Session[GetUniqueSessionKey( "PageIndex" )] = value;
		}
	}
	#endregion

	#region ��������, ��������� � ������������ ��������������

	/// <summary>
	/// ����������, ���� �� ���������� ������� ��� ������ ��������� (checkboxes)
	/// </summary>
	[Browsable(true)]
	public bool AllowSelection
	{
		get
		{
			object o = ViewState["AllowSelection"];
			return o != null ? (bool)o : false;
		}
		set
		{
			ViewState["AllowSelection"] = value;
		}
	}

	/// <summary>
	/// ���������� ������ �����, ������� �������� ��������������
	/// </summary>
	[Browsable(true)]
	public string AllowedRolesToSelection
	{
		get
		{
			object o = ViewState["AllowedRolesToSelection"];
			return o != null ? (string)o : String.Empty;
		}
		set
		{
			ViewState["AllowedRolesToSelection"] = value;
		}
	}

	/// <summary>
	/// ��� ���������� ����� � �������
	/// </summary>
	[Browsable(true)]
	public string DataKeyName
	{
		get
		{
			return ViewState["DataKeyName"] == null ? "" : (string)ViewState["DataKeyName"];
		}
		set
		{
			ViewState["DataKeyName"] = value;
			AssureDataKeys();
		}
	}

	/// <summary>
	/// 
	/// </summary>
	private void AssureDataKeys()
	{
		if (innerGrid != null && DataKeyName != "")
		{
			if (innerGrid.DataKeyNames.Length > 0)
			{
				innerGrid.DataKeyNames[0] = DataKeyName;
			}
			else
			{
				innerGrid.DataKeyNames = new string[] { DataKeyName };
			}
		}
	}

	/// <summary>
	/// ��������� ���� ������.
	/// </summary>
	[Browsable( false )]
	public DataKeyArray DataKeys
	{
		get
		{
			return innerGrid.DataKeys;
		}
	}

	/// <summary>
	/// ��������� ������ ��������� �����.
	/// </summary>
	[Browsable(false)]
	public DataKeyArray SelectedKeys
	{
		get
		{
			return Selection.SelectedKeys;
		}
	}

	/// <summary>
	/// ������, ������������� �� ��������� � �����. ��������� �������� � ���������� ��������� ������ �����.
	/// </summary>
	[Browsable( false )]
	public GridSelection Selection
	{
		get
		{
			if(m_Selection == null)
			{
				// ��������� ������� ��������� ����������� ���������
				if(ResolveSelection != null)
				{
					m_Selection = ResolveSelection( this );
				}
				else
				{
					// ���� �� ������� ����� �� ��������, �� ������� ��������� ����������� ��-���������
					m_Selection = new GridSelection( this );
				}
			}
			return m_Selection;
		}
	}

	/// <summary>
	/// ����������� ��������� ����������.
	/// </summary>
	public int SelectionCount
	{
		get
		{
			return Selection.Count;
		}
	}

	/// <summary>
	/// ������� � ���������������� ������ ��� ����������������� ��������.
	/// � �������� ����� - url �������� �� ������� ��������.
	/// � �������� �������� - ������������� �������.
	/// </summary>
	protected Dictionary<string, object> LastEditedObjects
	{
		get
		{
			string key = "LastEditedObjects";
			Dictionary<string, object> o = (Dictionary<string, object>)Session[key];
			if (o == null)
			{
				o = new Dictionary<string, object>();
				Session[key] = o;
			}
			return o;
		}
	}

	#endregion

	#region ��������, ��������� �� ���������
	/// <summary>
	/// ��������� �������� ��������� ��������� ��������� �����.
	/// � ���� ������ ���������� �������������� ������� � �������� ��� ������ ������, 
	/// �� ������� ������� ��� ������� ���������������� �������������� ������, 
	/// ���������� ������� ����� ���������� � ������� �������� HierarchicalRowTemplate.
	/// �� ��������� �������� ���������.
	/// </summary>
	[Browsable(true)]
	public bool AllowHierarchy
	{
		get
		{
			object o = ViewState["AllowHierarchy"];
			return o != null ? (bool)o : false;
		}
		set
		{
			ViewState["AllowHierarchy"] = value;
		}
	}

	/// <summary>
	/// ��������� ��������� ���������� ����� ������������.
	/// �� ��������� ���������.
	/// </summary>
	[Browsable(true)]
	public bool AllowMultiExpanding
	{
		get
		{
			object o = ViewState["AllowMultiExpanding"];
			return o != null ? (bool)o : true;
		}
		set
		{
			ViewState["AllowMultiExpanding"] = value;
		}
	}

	/// <summary>
	/// ������ ����������� ������, ������� ���������� ����� ������� �� ������ ���������.
	/// </summary>
	[DefaultValue((string)null)]
	[PersistenceMode(PersistenceMode.InnerProperty)]
	[TemplateContainer(typeof(GridViewRow))]
	[Browsable(false)]
	public virtual ITemplate HierarchicalRowTemplate
	{
		get
		{
			return this.m_HierarchicalRowTemplate;
		}
		set
		{
			this.m_HierarchicalRowTemplate = value;
		}
	}

	/// <summary>
	/// ���������� ����� ��������� ������������ �������� (��������/��������).
	/// </summary>
	[Browsable(false)]
	public HierarchicalRowStateCollection HierarchicalRowStates
	{
		get
		{
			HierarchicalRowStateCollection states = (HierarchicalRowStateCollection)ViewState["HierarchicalRowStates"];
			if (states == null)
			{
				states = new HierarchicalRowStateCollection();
				ViewState["HierarchicalRowStates"] = states;
			}
			return states;
		}
	}

	/// <summary>
	/// URL ��� �������� � ������ � �������� ���������.
	/// </summary>
	[Browsable(true)]
	public string HierarchicalCollapsedImageUrl
	{
		get
		{
			object o = ViewState["HierarchicalCollapsedImageUrl"];
			return o != null ? (string)o : "~/images/collapsed.gif";
		}
		set
		{
			ViewState["HierarchicalCollapsedImageUrl"] = value;
		}
	}

	/// <summary>
	/// URL ��� �������� � ������ � �������� ���������.
	/// </summary>
	[Browsable(true)]
	public string HierarchicalExpandedImageUrl
	{
		get
		{
			object o = ViewState["HierarchicalExpandedImageUrl"];
			return o != null ? (string)o : "~/images/expanded.gif";
		}
		set
		{
			ViewState["HierarchicalExpandedImageUrl"] = value;
		}
	}

	#endregion

	#region ��������������� ��������
	/// <summary>
	/// ���������, ����� �� ������� ������������ ������ � ������ ��������������.
	/// </summary>
	protected bool AllowEditing
	{
		get
		{
			return !String.IsNullOrEmpty(AllowedRolesToEdit)
				? Core.Security.User.Current.IsInRoles(AllowedRolesToEdit)
				: true;
		}
	}

	/// <summary>
	/// ���������, ����� �� ������� ������������ ������ � ������ ����������.
	/// </summary>
	protected bool AllowAdding
	{
		get
		{
			return !String.IsNullOrEmpty(AllowedRolesToAdd)
				? Core.Security.User.Current.IsInRoles(AllowedRolesToAdd)
				: true;
		}
	}

	/// <summary>
	/// ���������, ����� �� ������� ������������ ������ � ������ ��������.
	/// </summary>
	protected bool AllowDeleting
	{
		get
		{
			return !String.IsNullOrEmpty(AllowedRolesToDelete)
				? Core.Security.User.Current.IsInRoles(AllowedRolesToDelete)
				: true;
		}
	}

	/// <summary>
	/// ��������� �� ���� � ������ ������.
	/// </summary>
	[Browsable(false)]
	public bool IsInSelectMode
	{
		get
		{
			return Request["SelectMode"] != null && Request["SelectGridID"] == ID;
		}
	}

	/// <summary>
	/// ������ �������� ���� ��������, ������� ���������� ����.
	/// ����� �������� ���� � �������� DataSource ������������ ������ �� ������������� �� BaseBindingColletion.
	/// </summary>
	[Browsable(true)]
	public string ItemTypeName
	{
		get
		{
			object o = ViewState["ItemTypeName"];
			return (string)o;
		}
		set
		{
			ViewState["ItemTypeName"] = value;
		}
	}

	#endregion

	#region ����������� ���������� �����
	/// <summary>
	/// 
	/// </summary>
	/// <param name="e"></param>
	protected override void OnInit(EventArgs e)
	{
		base.OnInit(e);
        //if (this.Page.IsPostBack)
        {
            AssureDataKeys();

            if (CreateColumns != null)
            {
                CreateColumns(this, EventArgs.Empty);
            }

            // ��������� �������, ������������ �������������, �� ���������� ����
            foreach (DataControlField field in m_columns)
            {
                innerGrid.Columns.Add(field);
            }

            // ������������� ��������� ���������
            innerGrid.ShowHeader = ShowHeader;

            // � ������ ������ ����������� ������� ��� �����
            if (Page.IsInPrintMode)
            {
                ShowPager = false;
                AllowSelection = false;
            }

            // ����������, �� ��������� �� ���� � ������ ������
            if (IsInSelectMode)
            {
                CommandField fld = new CommandField();
                fld.ShowSelectButton = true;
                fld.SelectText = (string)GetLocalResourceObject("SelectButtonCaption");
                fld.CausesValidation = false;
                fld.ItemStyle.Width = Unit.Pixel(60);
                innerGrid.Columns.Add(fld);

                // �������� ������ ���������� � ��������
                AutoGenerateAddButton = false;
                AutoGenerateDeleteButton = false;
                // �������� ������� ������
                AllowSelection = false;
            }
            else
            {
                if (AutoGenerateDeleteButton && AllowDeleting) // ��������� ������ ��������
                {
                    plhDeleteButton.Visible = true;

                    // ������������ ������ ������������� ��������
                    //Page.RegisterConfirm(btnDelete_AutoGenerated, Resources.Messages.DeleteConfirmation);

                    // ��������� ���������� � ������
                    gscDelete_AutoGenerated.GridControl = this;
                }

                if (AutoGenerateAddButton && AllowAdding) // ��������� ������ ����������
                {
                    plhAddButton.Visible = true;
                }

                if (AutoGenerateEditButton && AllowEditing) // ���������� ������ ��������������
                {
                    CommandField fld = new CommandField();
                    fld.ShowEditButton = true;
                    fld.EditText = (string)GetLocalResourceObject("EditButtonCaption");
                    fld.CausesValidation = false;
                    fld.ItemStyle.Width = Unit.Pixel(60);
                    innerGrid.Columns.Add(fld);

                    // ������������� �� ������� ��������������
                    innerGrid.RowEditing += new GridViewEditEventHandler(OnRowEditing);
                }

                if (AllowSelection)
                {
                    plhTotalCount.Visible = true;
                }
            }

            // ������������� �� ������� Select
            innerGrid.SelectedIndexChanging += new GridViewSelectEventHandler(OnSelectedIndexChanging);

            // TODO: ��� �� ������� ����� (���� ���-������ ����������)
            innerGrid.RowEditing += delegate(object s, GridViewEditEventArgs args)
            {
                if (RowEdit != null)
                    RowEdit(this, args);
            };

            // ������������ ��� ���� ������ ������� ������ (�� ������ ���� ��� �� PrintMode)
            if (m_ButtonContainerTemplate != null)
            {
                m_ButtonContainerTemplate.InstantiateIn(phButtonContainer);
            }

            if (ShowPager)
            {
                ScriptManager.GetCurrent(Page).RegisterAsyncPostBackControl(ddlPageSize);
                //TODO:��������� ����� ������� �� ���
                /*User.Current.PageSizeChanged += delegate()
                {
                    RefreshData(true, false);
                    updatePanel.Update();
                };*/
                this.PageSize = this.PageSize;
            }
        }
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="e"></param>
	protected override void OnLoad(EventArgs e)
	{
		base.OnLoad(e);

		if (IsInSelectMode)
		{
			Page.PrepareReturnUrl();
		}

		if (!IsPostBack)
		{
			ddlPageSize.Items.Insert(0, new ListItem(Resources.NewsTape.liAll, PagingArgs.MaxPageSize.ToString()));
		}
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="e"></param>
	protected override void OnPreRender(EventArgs e)
	{
		base.OnPreRender(e);
		if (!IsPostBack)
		{
			BindData();
		}

		if (ShowPager)
		{
			// ������������� � ������ ������ ��������
			ListItem li = ddlPageSize.Items.FindByValue(PageSize.ToString());
			if (li != null)
			{
				ddlPageSize.SelectedItem.Selected = false;
				li.Selected = true;
			}
		}
		// ����������� ��� ��������
		phPager.Visible = ShowPager;

		// ��������/���������� ������� ������ ������ � �����. ������
		innerGrid.Columns[0].Visible = AllowSelection;
		// ��������/���������� ������� ������ ��������� ������ � �����. ������
		innerGrid.Columns[1].Visible = AllowHierarchy;

		// ���� ��������� � ������ ������ ��� � ������ ������, �� �������� ������
		phButtonContainer.Visible = !Page.IsInPrintMode && !IsInSelectMode;

		// ������������ ����������� ���������� ������� ��� ������ � ����������
		if (AllowSelection)
		{
			RegisterHighLightOnCheckScriptBlock();

			// ����������� ���������� ����������� OnClick ��� ��������� �� ������ �������
			GridViewRow headerRow = this.HeaderRow;
			if (headerRow != null)
			{
				string script;
				CheckBox chkSelectAll = (CheckBox)headerRow.Cells[0].FindControl("chkSelectAll");
				script = String.Format("HighLightHeadCheckBoxClick('{0}', '{1}');", chkSelectAll.ClientID, this.ClientID + "_innerGrid");
				if (String.IsNullOrEmpty(chkSelectAll.Attributes["OnClick"]) || !chkSelectAll.Attributes["OnClick"].Contains(script))
				{
					chkSelectAll.Attributes["OnClick"] += script;
				}

			}

			// ��������� ���� � ����������� ��������� ��������
			totalSelected.Value = Selection.Count.ToString();
			lblTotalSelected.Text = Selection.Count.ToString();
		}

		// ������������ ��������������� ���������� �������
		RegisterSelectedRowsCountScriptBlock();
		RegisterSelectAllRowsScriptBlock();

		object selectedKey = new object();
		
		// ���������� ������������� ���������� ������������������ �������
		if (!Page.IsInPrintMode)
		{
			RegisterHighLightOnMouseScriptBlock();

			if (!String.IsNullOrEmpty(ItemTypeName) 
				&& LastEditedObjects.ContainsKey(ItemTypeName))
			{
				selectedKey = LastEditedObjects[ItemTypeName];
			}
		}

		foreach (GridViewRow row in this.Rows)
		{
			// ���������� �� ���������� ������ �����������

			CheckBox chkSelected = (CheckBox)row.FindControl("chkSelected");
			if (DataKeys.Count > 0)
			{
				if (selectedKey.ToString() == DataKeys[row.RowIndex].Value.ToString())
				{
					row.CssClass = "rowLastEdited";
				}
			}
			if (AllowSelection)
			{
				string script = String.Format("HighLightCheckBoxClick('{0}');", chkSelected.ClientID); ;
				// ����������� ���������� ����������� OnClick ��� ��������� �� ������ �������
				if (String.IsNullOrEmpty(chkSelected.Attributes["OnClick"]) || !chkSelected.Attributes["OnClick"].Contains(script))
				{
					chkSelected.Attributes["OnClick"] += script;
				}

				if(Selection[DataKeys[row.RowIndex]])
				{
					chkSelected.Checked = true;
					row.CssClass += " rowChecked";
				}
				else
				{
					chkSelected.Checked = false;
					row.CssClass = row.CssClass.Replace("rowChecked", String.Empty);

					
				}
			}

			// ����������� ���������� ����������� ��� ��������� ������ ��� ������
			if (!Page.IsInPrintMode)
			{
				row.Attributes["OnMouseOver"] = String.Format("HighLightTrMouseIn(this, '{0}');", chkSelected.ClientID);
				row.Attributes["OnMouseOut"] = String.Format("HighLightTrMouseOut(this, '{0}');", chkSelected.ClientID);
			}
		}

		//if (AllowSelection && HeaderRow != null)
		//{
		//    CheckBox chk = ((CheckBox)HeaderRow.FindControl("chkSelectAll"));
		//    chk.Checked = isAllSelected;
		//}
	}

	#endregion

	#region ������ �������� ������
	/// <summary>
	/// ��������� ������ � �������. ����� ���������� �������, ����� �� ����� ������ �������� ��� ���. 
	/// ����� ���������, ���� �������� ���������� ������ ���������� ����� �������� �������.
	/// </summary>
	/// <param name="resetPagingAndSelection">���������� �� ����� �������� (���������� �� ������) � ��������� ��� ���.</param>
	public void RefreshData(bool resetPagingAndSelection)
	{
		RefreshData(resetPagingAndSelection, resetPagingAndSelection);
	}

	/// <summary>
	/// ��������� ������ � �������. ����� ���������� �������, ����� �� ����� ������ �������� ��� ���. 
	/// ����� ���������, ���� �������� ���������� ������ ���������� ����� �������� �������.
	/// </summary>
	/// <param name="resetPaging">���������� �� ����� �������� (���������� �� ������) ��� ���.</param>
	/// <param name="resetSelection">���������� �� ��������� � ����������� ���������.</param>
	public void RefreshData(bool resetPaging, bool resetSelection)
	{
		if (resetPaging)
		{
			PageIndex = 0;
			// ��������� ��� ��������� ��������
			if (AllowHierarchy)
			{
				HierarchicalRowStates.Clear();
			}
		}
		// ������ ��������� ���������� ������(���������� �������� ��� ��������)
		if (resetSelection)
		{
			Selection.Clear();
		}
		BindData();
	}



	/// <summary>
	/// ������ ������ � �����.
	/// </summary>
	protected void BindData()
	{
		// ���� �������� �� �����, �� �������
		if (RequestDatasource == null) return;

		PagingResult result = RequestDatasource(
			this,
			new PagingArgs(
				PageIndex,
				PageSize,
				SortExpression,
				SortOrderAsc
				)
		);

		// � ������ ������, ���� ������ �� �������, ���������� ���������
		if (SearchMode && result.TotalCount == 0)
		{
			plhGrid.Visible = false;
			if (IsPostBack)
			{
				plhNothingFound.Visible = true;
			}
			else
			{
				plhNothingFound.Visible = false;
			}
		}
		else
		{
			plhGrid.Visible = true;
			plhNothingFound.Visible = false;

			// ��������� ����
			innerGrid.DataSource = result.Result;
			innerGrid.DataBind();

		/*	// ���������� ��� �������� ���������
			IItemTypeInfo itemTypeInfo = result.Result as IItemTypeInfo;
			if (String.IsNullOrEmpty(ItemTypeName))
			{
				if (itemTypeInfo != null)
				{
					ItemTypeName = itemTypeInfo.GetItemType().FullName;
				}
				else
				{
					ItemTypeName = null;
				}
			}*/
			// �������������� �������
			if (ShowPager)
			{
				BindPager(PageIndex, PageSize, 5, result.TotalCount);
			}

			// ��������� ���� � ����������� ��������� ��������
			lbTotalRecords.Text = result.TotalCount.ToString();

			// ��������� ������� DataBound
			OnDataBound(EventArgs.Empty);
		}
	}

	/// <summary>
	/// ��������� ������� DataBound.
	/// </summary>
	/// <param name="args"></param>
	protected virtual void OnDataBound(EventArgs args)
	{
		if (DataBound != null)
		{
			DataBound(this, args);
		}
	}

	#endregion

	#region ������ � ����������� ���������
	/// <summary>
	/// �������������� ������� ��� �����.
	/// </summary>
	/// <param name="pageIndex"></param>
	/// <param name="pageSize"></param>
	/// <param name="windowSize"></param>
	/// <param name="totalCount"></param>
	public void BindPager(
		int pageIndex,
		int pageSize,
		int windowSize,
		int totalCount)
	{
		int pageCount = totalCount % pageSize != 0
				? totalCount / pageSize + 1
				: totalCount / pageSize;

		int windowCount = pageCount % windowSize != 0
			? pageCount / windowSize + 1
			: pageCount / windowSize;
		int currentWindow = (pageIndex) / windowSize + 1;
		int firstPageInWindow = (currentWindow - 1) * windowSize;

		List<int> pages = new List<int>();
		for (int i = firstPageInWindow; (i < firstPageInWindow + windowSize) && (i < pageCount); i++)
		{
			pages.Add(i);
		}
		repPager.DataSource = pages;
		repPager.DataBind();

		lbPrev.Enabled = pageIndex > 0;
		lbPrev.CommandArgument = (pageIndex - 1).ToString();
		lbNext.Enabled = pageIndex < pageCount - 1;
		lbNext.CommandArgument = (pageIndex + 1).ToString();

		lbPrevWindow.Visible = currentWindow > 1;
		lbPrevWindow.CommandArgument = ((currentWindow - 2) * windowSize).ToString();

		lbNextWindow.Visible = currentWindow < windowCount;
		lbNextWindow.CommandArgument = ((currentWindow) * windowSize).ToString();

		lbFirst.Visible = currentWindow > 1;
		lbFirst.Text = "1";
		lbFirst.CommandArgument = (0).ToString();

		lbLast.Visible = currentWindow < windowCount;
		lbLast.Text = (pageCount).ToString();
		lbLast.CommandArgument = (pageCount - 1).ToString();
	}

	protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
	{
		PageSize = Convert.ToInt32(ddlPageSize.SelectedValue);
		RefreshData( true , false );
	}

	protected void repPager_ItemDataBound(object sender, RepeaterItemEventArgs e)
	{
		if (e.Item.ItemType == ListItemType.Item ||
			e.Item.ItemType == ListItemType.AlternatingItem)
		{
			int page = Convert.ToInt32(e.Item.DataItem);

			LinkButton lbPage = e.Item.FindControl("lbPage") as LinkButton;
			if (lbPage != null)
			{
				lbPage.Text = (page + 1).ToString();
				lbPage.Enabled = page != PageIndex;
				lbPage.CssClass = page != PageIndex ? "" : "current";
				lbPage.CommandArgument = page.ToString();
			}
		}
	}
	protected void repPager_ItemCommand(object source, RepeaterCommandEventArgs e)
	{
		PagerCommand(source, e);
	}

	protected void PagerCommand(object sender, System.Web.UI.WebControls.CommandEventArgs e)
	{
		if (e.CommandName == "pager")
		{
			PageIndex = Convert.ToInt32(e.CommandArgument);
			// ��������� ��� ��������� ��������
			if (AllowHierarchy)
			{
				HierarchicalRowStates.Clear();
			}
			// ������������� ������
			BindData();
		}
	}
	#endregion

	#region ���������� ����������

	protected void OnSorting(object sender, GridViewSortEventArgs args)
	{
		SortOrderAsc = (args.SortExpression == SortExpression)
			? !SortOrderAsc
			: true;
		SortExpression = args.SortExpression;
		// ��������� ��� ��������� ��������
		if (AllowHierarchy)
		{
			HierarchicalRowStates.Clear();
		}
		// ������������� ������
		BindData();
	}

	#endregion

	#region ����������� GridView
	/// <summary>
	/// ������������ �������� �������� ����� �����.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void OnRowCreated(object sender, GridViewRowEventArgs e)
	{
		// ������������ ��������� ��������
		if (e.Row.RowType == DataControlRowType.Header)
		{
			if (!String.IsNullOrEmpty(SortExpression))
			{
				// ��������� �� �������� � ���������, ��������� �� ������� ���� ��� ���������� � ����������
				for (int i = 0; i < innerGrid.Columns.Count; i++)
				{
					if (innerGrid.Columns[i].SortExpression == SortExpression)
					{
						// ���� ���������, ��������� � �����. ������ ������ ����������
						Image imSort = new Image();
						imSort.ID = "imSort";
						imSort.ImageUrl = SortOrderAsc
							? "~/images/arrowup.gif"
							: "~/images/arrowdown.gif";
						imSort.AlternateText = String.Empty;
						imSort.ToolTip = SortOrderAsc
							? (string)GetLocalResourceObject("Ascending")
							: (string)GetLocalResourceObject("Descending");
						e.Row.Cells[i].Controls.Add(imSort);
					}
				}
			}
		}

		// ������������ ������
		if (e.Row.RowType == DataControlRowType.DataRow)
		{
			// ��������� ������� ��������� �����
			if (RowCreated != null)
				RowCreated(this, e);

			// ���� ����� �������� �������� ��������
			if (AllowHierarchy)
			{
				// ��������� �������
				OnHierarchicalRowCreated(sender, e);
			}
		}
	}

	/// <summary>
	/// ������� ��� ����������� �������������� ��������, ���������� ��� �������,
	/// � ����� ��������� ������� �������� �����������.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void OnHierarchicalRowCreated(object sender, GridViewRowEventArgs e)
	{
		GridViewRow row = e.Row;
		// ��������� �������������� ������
		TableCell cell = new TableCell();
		row.Cells.Add(cell);
		cell.Style[HtmlTextWriterStyle.Display] = "none";
		HierarchicalRowState state = HierarchicalRowStates[e.Row.RowIndex];
		cell.Controls.Add(
			new LiteralControl(
				String.Format("</td></tr><tr style=\"display:{0};\"><td colspan=\"{1}\">",
				state == HierarchicalRowState.Collapsed ? "none" : "table-row",
				innerGrid.Columns.Count + 1
				)
				));

		// ��������� ������� ������ � ������, ���� �� �������
		if (state == HierarchicalRowState.Expanded)
		{
			// ��������� ������� ����� ��������� ��������
			if (HierarchicalRowPreCreated != null)
			{
				HierarchicalRowPreCreated(sender, e);
			}

			// ������������ ������ �����������
			if (m_HierarchicalRowTemplate != null)
			{
				m_HierarchicalRowTemplate.InstantiateIn(cell);
			}

			// ��������� ������� �������� ����������
			if (HierarchicalRowCreated != null)
			{
				HierarchicalRowCreated(sender, e);
			}
		}
	}

	/// <summary>
	/// ������������ ������������ ����� � ��������� ������� ����������.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
	{
		// ������������ ��������� ��������
		if (e.Row.RowType == DataControlRowType.Header)
		{
			// ������������� � CheckBox'� ������� SelectionGrid, ������� �������� ID ����� �����.
			// ���� ������� ��������� � SPAN'�, � ������� ����� ������� CheckBox.
			// ����� ��������� ���������� ������� ��� ��������� ���� CheckBox'�� � �����.
			CheckBox chkSelectAll = (CheckBox)e.Row.FindControl("chkSelectAll");
			if (chkSelectAll != null)
			{
				chkSelectAll.Attributes["SelectionGrid"] = this.ClientID;
				chkSelectAll.Attributes["MultiSelect"] = "yes";
				chkSelectAll.Attributes["OnClick"] = String.Format("SelectAllRows(this,'{0}');", this.ClientID);
			}
		}

		// ������������ ������
		if (e.Row.RowType == DataControlRowType.DataRow)
		{
			// ������������� � CheckBox'� ������� SelectionGrid, ������� �������� ID ����� �����.
			// ���� ������� ��������� � SPAN'�, � ������� ����� ������� CheckBox
			CheckBox chkSelected = (CheckBox)e.Row.FindControl("chkSelected");
			if (chkSelected != null)
			{
				chkSelected.Attributes["SelectionGrid"] = this.ClientID;
			}

			HierarchicalRowState toggleState = HierarchicalRowStates[e.Row.RowIndex];

			// ��������� ������ ��������� ��������
			ImageButton ibToggle = (ImageButton)e.Row.FindControl("ibToggle");
			ibToggle.CommandArgument = e.Row.RowIndex.ToString();
			ibToggle.ImageUrl = toggleState == HierarchicalRowState.Collapsed
				? HierarchicalCollapsedImageUrl
				: HierarchicalExpandedImageUrl;

			// ��������� ������� ��������� �����
			if (RowDataBound != null)
			{
				RowDataBound(this, e);
			}

			// ������ ���������� ������ � ��� ������, ���� �� �������
			if (AllowHierarchy
				&& toggleState == HierarchicalRowState.Expanded)
			{
				OnHierarchicalRowDataBound(sender, e);
			}

			// ������������� (���������) CssClass ������ �����
			if (e.Row.RowIndex % 2 != 0)
			{
				e.Row.CssClass += " row";
			}
			else
			{
				e.Row.CssClass += " rowAnother";
			}
		}
	}

	/// <summary>
	/// ��������� ������� ���������� �����������.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void OnHierarchicalRowDataBound(object sender, GridViewRowEventArgs e)
	{
		if (m_HierarchicalRowTemplate != null)
		{
			// ��������� ������� ��������� ����� ������������
			if (HierarchicalRowDataBound != null)
			{
				HierarchicalRowDataBound(this, e);
			}
		}
	}

	/// <summary>
	/// ��������� ������� �������.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void OnRowCommand(object sender, GridViewCommandEventArgs e)
	{
		if (e.CommandName == "toggle")
		{
			int rowIndex = Convert.ToInt32(e.CommandArgument);
			HierarchicalRowState newState = HierarchicalRowStates[rowIndex] == HierarchicalRowState.Collapsed
					? HierarchicalRowState.Expanded
					: HierarchicalRowState.Collapsed;
			// ���� ������������� ��������� ���������, �� ��������� ��� ������
			if (!AllowMultiExpanding)
			{
				HierarchicalRowStates.Clear();
			}
			HierarchicalRowStates[rowIndex] = newState;
			// ������������� ������
			BindData();
		}

		// ��������� ������� �������
		if (RowCommand != null)
		{
			RowCommand(this, e);
		}
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void OnRowEditing(object sender, GridViewEditEventArgs e)
	{
		string dataKeyValue = innerGrid.DataKeys[e.NewEditIndex].Value.ToString();

		if (EditMode == EditModes.Custom)
		{
			// � ������ Custom ��������� ������� �� ��������������
			if (AutoEditing != null)
			{
				AutoEditing(this, new GridEditEventArgs(dataKeyValue));
			}
		}
		else
		{
			AddEditRedirect(dataKeyValue);
		}
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void OnSelectedIndexChanging(object sender, GridViewSelectEventArgs e)
	{
		// ���� ��������� � ������ ������, ������� � ������ ��������� �������������
		if (IsInSelectMode)
		{
			Session[Page.ReturnUrl + "_" + Request["LinkButtonID"]] = innerGrid.DataKeys[e.NewSelectedIndex].Value;
			Page.RedirectToReferrer();
		}

		if (RowSelect != null)
		{
			RowSelect(this, e);
		}
	}

	#endregion

	#region IAccessible Members

	public bool CheckAccessibilityToUser(User user)
	{
		// ��������� ����������� �������
		foreach (DataControlField column in Columns)
		{
			IAccessible accessColumn = column as IAccessible;
			if (accessColumn != null)
			{
				if (!accessColumn.CheckAccessibilityToUser(user))
				{
					column.Visible = false;
				}
			}
		}
		// ��������� ����������� ������� ��������������
		bool isSelectionAccessible = !String.IsNullOrEmpty(AllowedRolesToSelection)
			? user.IsInRoles(AllowedRolesToSelection)
			: true;
		if (!isSelectionAccessible)
		{
			innerGrid.Columns[0].Visible = false;
		}

		// ����������� ������ ����������
		bool isAddButtonAccessible = !String.IsNullOrEmpty(AllowedRolesToAdd)
			? user.IsInRoles(AllowedRolesToAdd)
			: true;
		if (!isAddButtonAccessible)
			plhAddButton.Visible = false;

		// ����������� ������ ��������
		bool isDeleteButtonAccessible = !String.IsNullOrEmpty(AllowedRolesToDelete)
			? user.IsInRoles(AllowedRolesToDelete)
			: true;
		if (!isDeleteButtonAccessible)
			plhDeleteButton.Visible = false;

		return true;
	}

	#endregion

	#region ����������� ��������������� ���������� ��������

	/// <summary>
	/// ������������ ���������� �������, ������������ ���������� ��������� ������������� �����.
	/// </summary>
	protected void RegisterSelectedRowsCountScriptBlock()
	{
		StringBuilder scriptBody = new StringBuilder();
		scriptBody.Append("function SelectedRowsCount(grid)");
		scriptBody.Append("{");
		scriptBody.Append("  var selectedCount = 0;");
		scriptBody.Append("  var all = document.getElementsByTagName('*');");
		scriptBody.Append("  for (i = 0; i < all.length; ++i)");
		scriptBody.Append("  {");
		scriptBody.Append("		var element = all[i];");
		scriptBody.Append("    if (element.getAttribute('SelectionGrid') == grid");
		scriptBody.Append("      && element.getAttribute('MultiSelect') == undefined");
		scriptBody.Append("      && element.childNodes[0].tagName == 'INPUT'");
		scriptBody.Append("      && element.childNodes[0].type == 'checkbox'");
		scriptBody.Append("      && element.childNodes[0].checked)");
		scriptBody.Append("    {");
		scriptBody.Append("      ++selectedCount;");
		scriptBody.Append("    }");
		scriptBody.Append("  }");
		scriptBody.Append("  return selectedCount;");
		scriptBody.Append("}");

		ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "SelectedRowsCountScript", scriptBody.ToString(), true);
	}

	/// <summary>
	/// ������������ ���������� �������, ������� ��������/��������� ��� �������� � �����.
	/// </summary>
	protected void RegisterSelectAllRowsScriptBlock()
	{
		StringBuilder scriptBody = new StringBuilder();
		scriptBody.Append("function SelectAllRows(sender, grid)");
		scriptBody.Append("{");
		scriptBody.Append("  var all = document.getElementsByTagName('*');");
		scriptBody.Append("  for (i = 0; i < all.length; ++i)");
		scriptBody.Append("  {");
		scriptBody.Append("		var element = all[i];");
		scriptBody.Append("    if (element.getAttribute('SelectionGrid') == grid");
		scriptBody.Append("      && element.getAttribute('MultiSelect') == undefined");
		scriptBody.Append("      && element.childNodes[0].tagName == 'INPUT'");
		scriptBody.Append("      && element.childNodes[0].type == 'checkbox'");
		scriptBody.Append("      && !element.childNodes[0].disabled)");
		scriptBody.Append("    {");
		scriptBody.Append("      element.childNodes[0].checked = sender.checked;");
		scriptBody.Append("    }");
		scriptBody.Append("  }");
		scriptBody.Append("}");

		ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "SelectAllRows", scriptBody.ToString(), true);
	}

	/// <summary>
	/// ������������ ���������� �������, ������� ������������ ������ � ����� ��� ��������� ����.
	/// </summary>
	protected void RegisterHighLightOnMouseScriptBlock()
	{
		string scriptKey = "HighLightOnMouseScriptBlock";

		StringBuilder scriptBody = new StringBuilder();
		scriptBody.Append("function HighLightTrMouseIn(row, checkBoxId)");
		scriptBody.Append("{");
		scriptBody.Append("	row.className += ' ' + 'rowLight';");
		scriptBody.Append("}");
		scriptBody.Append("function HighLightTrMouseOut(row,checkBoxId)");
		scriptBody.Append("{");
		scriptBody.Append("	row.className = row.className.replace(' rowLight', '');");
		scriptBody.Append("}");

		ScriptManager.RegisterClientScriptBlock(this, this.GetType(), scriptKey, scriptBody.ToString(), true);
	}

	/// <summary>
	/// ������������ ���������� �������, ������� ������������ ������ � �����, ���������� ����������.
	/// </summary>
	protected void RegisterHighLightOnCheckScriptBlock()
	{
		string scriptKey = "HighLightOnCheckScriptBlock";

		StringBuilder scriptBody = new StringBuilder();

		scriptBody.Append("function HighLightCheckBoxClick(checkBoxId)");
		scriptBody.Append("{");
		scriptBody.Append(" var chk = document.getElementById(checkBoxId);");
		scriptBody.Append("if(chk.checked)");
		scriptBody.Append(" chk.parentNode.parentNode.parentNode.className += ' rowChecked';");
		scriptBody.Append("else");
		scriptBody.Append(" chk.parentNode.parentNode.parentNode.className = chk.parentNode.parentNode.parentNode.className.replace(' rowChecked', '');");
		scriptBody.Append("}");

		scriptBody.Append("function HighLightHeadCheckBoxClick(ckeckBoxId, gridId)");
		scriptBody.Append("{");
		scriptBody.Append(" var chk = document.getElementById(ckeckBoxId);");
		scriptBody.Append(" var grid = document.getElementById(gridId);");
		scriptBody.Append(" var trs = grid.rows;");
		scriptBody.Append("  for (i = 1; i < trs.length; ++i)");
		scriptBody.Append("  {");
		scriptBody.Append("    if((chk.checked)&&(!trs[i].getAttribute('disabled'))&&(trs[i].cells[0].colSpan*1<2))");
		scriptBody.Append("     {");
		scriptBody.Append("  	 trs[i].className = trs[i].className.replace(' rowChecked', '');");
		scriptBody.Append("  	 trs[i].className += ' rowChecked';");
		scriptBody.Append("     }");
		scriptBody.Append("    else");
		scriptBody.Append("		trs[i].className = trs[i].className.replace(' rowChecked', '');");
		scriptBody.Append("  }");
		scriptBody.Append("}");

		scriptBody.Append("function HighLightAllChecked(gridId)");
		scriptBody.Append("{");
		scriptBody.Append(" var grid = document.getElementById(gridId);");
		scriptBody.Append(" var trs = grid.getElementsByTagName('tr');");
		scriptBody.Append("  for (i = 1; i < trs.length; ++i)");
		scriptBody.Append("  {");
		scriptBody.Append("    var chk = trs[i].childNodes[0].childNodes[0].childNodes[0];");
		scriptBody.Append("    if((chk.checked)&&(!trs[i].getAttribute('disabled')))");
		scriptBody.Append("     {");
		scriptBody.Append("  	 trs[i].className = trs[i].className.replace(' rowChecked', '');");
		scriptBody.Append("  	 trs[i].className += ' rowChecked';");
		scriptBody.Append("     }");
		scriptBody.Append("    else");
		scriptBody.Append("		trs[i].className = trs[i].className.replace(' rowChecked', '');");
		scriptBody.Append("  }");
		scriptBody.Append("}");

		scriptBody.Append("function SelectRowByCheckBoxID(checkBoxID)");
		scriptBody.Append("{");
		scriptBody.Append(" var chk = document.getElementById(checkBoxId);");
		scriptBody.Append(" alert(chk);");
		scriptBody.Append(" chk.checked = true;");
		scriptBody.Append(" alert(chk.checked);");
		scriptBody.Append(" chk.parentNode.parentNode.parentNode.className += ' rowChecked';");
		scriptBody.Append("}");

		ScriptManager.RegisterClientScriptBlock(this, this.GetType(), scriptKey, scriptBody.ToString(), true);
	}

	#endregion

	#region ����������� ������������� ����������� ������

	protected void btnDelete_Click(object sender, EventArgs e)
	{
		if (AutoDeleting != null)
		{
			AutoDeleting(this, new GridDeleteEventArgs(this.SelectedKeys));
		}
	}

	protected void btnAdd_Click(object sender, EventArgs e)
	{
		if (EditMode == EditModes.Custom)
		{
			// � ������ Custom ��������� ������� �� ����������
			if (AutoAdding != null)
			{
				AutoAdding(this, e);
			}
		}
		else
		{
			AddEditRedirect("");
		}
	}

	protected void lbClearSelection_Click(object sender, EventArgs e)
	{
		Selection.Clear();
		totalSelected.Value = Selection.Count.ToString();
		lblTotalSelected.Text = Selection.Count.ToString();
		RefreshData(false, true);
	}

	protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
	{
		CheckBox chkSelectAll = (CheckBox)sender;
		if (chkSelectAll.Checked)
		{
			foreach (GridViewRow row in innerGrid.Rows)
			{
				CheckBox chkCurrent = (CheckBox)row.FindControl("chkSelected");
				if (chkCurrent.Checked)
				{
					Selection[DataKeys[row.RowIndex]] = true;
				}
				// ��������� ������� ��������� ������
				if(RowClientSelect != null)
				{
					RowClientSelect( this, new GridSelectEventArgs( row.RowIndex, true) );
				}
			}
		}
		else
		{
			foreach (GridViewRow row in innerGrid.Rows)
			{
				Selection[DataKeys[row.RowIndex]] = false;
				// ��������� ������� ��������� ������
				if (RowClientSelect != null)
				{
					RowClientSelect( this, new GridSelectEventArgs( row.RowIndex, false ) );
				}
			}
		}
		//updatePanel.Update();
	}

	protected void chkSelected_CheckedChanged(object sender, EventArgs e)
	{
		CheckBox chkSender = (CheckBox)sender;

		foreach (GridViewRow row in innerGrid.Rows)
		{
			CheckBox check = (CheckBox)row.Cells[0].FindControl("chkSelected");
			if (check.ClientID == chkSender.ClientID)// ���� �� ��� ���� ��� �����
			{
				Selection[DataKeys[row.RowIndex]] = chkSender.Checked;
				// ��������� ������� ��������� ������
				if(RowClientSelect != null)
				{
					RowClientSelect( this, new GridSelectEventArgs( row.RowIndex, chkSender.Checked ) );
				}
			}
		}
		//updatePanel.Update();
	}
	#endregion

	#region ��������������� ������
	/// <summary>
	/// 
	/// </summary>
	/// <param name="controlID"></param>
	/// <param name="base�ontrol"></param>
	/// <returns></returns>
	private Control FindControlRecursive(string controlID, Control base�ontrol)
	{
		if (base�ontrol.ID == controlID)
		{
			return base�ontrol;
		}

		foreach (Control control in base�ontrol.Controls)
		{
			Control ctl = FindControlRecursive(controlID, control);
			if (ctl != null)
				return ctl;
		}
		return null;
	}

	/// <summary>
	/// ���������� �� �������� �������������� �������.
	/// </summary>
	/// <param name="dataKey">����, ������� ������ ��� ������, ������� �����������.</param>
	private void AddEditRedirect(string dataKey)
	{
		if (EditURL == "")
			throw new NotSupportedException("EditUrl must be specified.");

		string targetUrl = EditURL;

		if (!string.IsNullOrEmpty(dataKey))
		{
			targetUrl = WebHelper.ReplaceUrlQueryParameter(targetUrl, "id", dataKey);
		}

		if (!string.IsNullOrEmpty(GridMarker))
		{
			targetUrl = WebHelper.ReplaceUrlQueryParameter(targetUrl, "grid_marker", GridMarker);
		}

		if (ResolveAddEditParameters != null)
		{
			Dictionary<string, string> parameters = ResolveAddEditParameters();
			foreach (string key in parameters.Keys)
			{
				targetUrl = WebHelper.ReplaceUrlQueryParameter(targetUrl, key, parameters[key]);
			}
		}

		switch (EditMode)
		{
			case EditModes.Simple:
				((BaseWebPage)Page).RedirectToUrlWithReturn(targetUrl);
				break;
			case EditModes.WithMarker:
				((IDialog)Page).FillObject();
				((BaseWebPage)Page).RedirectToUrlWithReturn(((IDialog)Page).GetUrlWithMarker(targetUrl));
				break;
			case EditModes.WithParentMarker:
				((IDialog)Page).FillObject();
				((BaseWebPage)Page).RedirectToUrlWithReturn(((IDialog)Page).GetUrlWithParentMarker(targetUrl));
				break;
		}
	}

	#endregion
}
