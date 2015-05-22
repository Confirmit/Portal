using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.ComponentModel;

using Core;
using Core.Security;
using EPAMSWeb.UI;

using CheckBox = EPAMSWeb.UI.CheckBox;
using Confirmit.Portal;

//#region GridSelection - класс, инкапсулирующий работу с выделением
///// <summary>
///// Инкапсулируюет работу с выделением в гриде.
///// </summary>
//public class GridSelection
//{
//    #region Конструкторы

//    public GridSelection( Grid owner )
//    {
//        m_Owner = owner;
//    }

//    #endregion

//    #region Поля

//    private Grid m_Owner;

//    #endregion

//    #region Свойства

//    /// <summary>
//    /// Грид, для которого создан данный контроллер выделения.
//    /// </summary>
//    public Grid Owner
//    {
//        get
//        {
//            return m_Owner;
//        }
//    }

//    /// <summary>
//    /// Устанавливает или возвращает флаг выделения строки с указанным ключом.
//    /// </summary>
//    /// <param name="key"></param>
//    /// <returns></returns>
//    public virtual bool this[DataKey key]
//    {
//        get
//        {
//            return AllSelectedKeys.ContainsKey( key.Value );
//        }
//        set
//        {
//            if(value)
//            {
//                if(!AllSelectedKeys.ContainsKey( key.Value ))
//                {
//                    AllSelectedKeys.Add( key.Value, key );
//                }
//            }
//            else
//            {
//                AllSelectedKeys.Remove( key.Value );
//            }
//        }
//    }

//    /// <summary>
//    /// Таблица с идентификаторами всех выбранных строк.
//    /// </summary>
//    private Dictionary<object, DataKey> AllSelectedKeys
//    {
//        get
//        {
//            string key = String.Format( "AllSelectedKeys_{0}", m_Owner.UniqueID );
//            Dictionary<object, DataKey> o = (Dictionary<object, DataKey>)HttpContext.Current.Session[key];
//            if(o == null)
//            {
//                o = new Dictionary<object, DataKey>();
//                HttpContext.Current.Session[key] = o;
//            }
//            return o;
//        }
//    }

//    /// <summary>
//    /// Коллекция ключей выбранных строк
//    /// </summary>
//    public virtual DataKeyArray SelectedKeys
//    {
//        get
//        {
//            ArrayList list = new ArrayList();
//            foreach(KeyValuePair<object, DataKey> de in AllSelectedKeys)
//            {
//                list.Add( de.Value );
//            }
//            return new DataKeyArray( list );
//        }
//    }

//    /// <summary>
//    /// Количество выделенных элементов.
//    /// </summary>
//    public virtual int Count
//    {
//        get
//        {
//            return AllSelectedKeys.Count;
//        }
//    }

//    #endregion

//    #region Методы
//    /// <summary>
//    /// Сбрасывает выделение.
//    /// </summary>
//    public virtual void Clear()
//    {
//        AllSelectedKeys.Clear();
//    }

//    /// <summary>
//    /// Возвращает строку, сформированную из значений выделенных ключей, перечисленных через запятую.
//    /// </summary>
//    /// <returns></returns>
//    public override string ToString()
//    {
//        StringBuilder keys = new StringBuilder();
//        foreach(DataKey key in SelectedKeys)
//        {
//            if(keys.Length > 0) keys.Append( "," );
//            keys.Append( key.Value.ToString() );
//        }
//        return keys.ToString();
//    }
//    #endregion
//}

//#endregion

/// <summary>
/// Контрол "Грид", позволяющий выводить данные в виде таблицы.
/// </summary>
public partial class Grid : BaseUserControl, IAccessible, IGridRowsContainer, IGridSelection
{
	#region Поля

	private ITemplate m_HierarchicalRowTemplate;
	private ITemplate m_ButtonContainerTemplate;
	private GridSelection m_Selection;

	#endregion

	#region События
	/// <summary>
	/// Событие, возникающее, когда гриду необходимо получить данные.
	/// </summary>
	public event GridRequestDatasourceHandler RequestDatasource;
	/// <summary>
	/// Событие, возникающие, когда требуется получить контроллер выделения в гриде.
	/// Подписавшись на это событие, можно подменить стандартный контроллер выделения другим, 
	/// что позволит изменять логику выделения и подсчета выделенных строк.
	/// </summary>
	public event GridResolveSelectionEventHandler ResolveSelection;
	/// <summary>
	/// Событие, возникающее после того, как грид получил данные и заполнил себя (после обновления данных в гриде).
	/// </summary>
	public event EventHandler DataBound;
	/// <summary>
	/// Событие, возникающее на создание строки грида.
	/// </summary>
	public event GridViewRowEventHandler RowCreated;
	/// <summary>
	/// Событие, возникающее на формирование строки грида.
	/// </summary>
	public event GridViewRowEventHandler RowDataBound;
	/// <summary>
	/// Событие, возникающие перед созданием строки вложенного элемента.
	/// На данном этапе есть возможность изменить шалон вложенного элемента.
	/// Данное событие возникает только в том случае, когда вложенный элемент раскрыт.
	/// </summary>
	public event GridViewRowEventHandler HierarchicalRowPreCreated;
	/// <summary>
	/// Событие, возникающие сразу после создания строки вложенного элемента.
	/// На данном этапе есть возможность настроить контролы, находящиеся в шаблоне, или сформировать другие.
	/// Данное событие возникает только в том случае, когда вложенный элемент раскрыт.
	/// </summary>
	public event GridViewRowEventHandler HierarchicalRowCreated;
	/// <summary>
	/// Собыие, возникающие при биндинге данных у сложенного элемента.
	/// На данном этапе есть возможность заполнять контролы вложенного элемента.
	/// Данное событие возникает только в том случае, когда вложенный элемент раскрыт.
	/// </summary>
	public event GridViewRowEventHandler HierarchicalRowDataBound;
	/// <summary>
	/// Событие, возникающее на команду грида.
	/// </summary>
	public event GridViewCommandEventHandler RowCommand;
	/// <summary>
	/// ВНИМАНИЕ: Событие из старого грида, надо как-нибудь от него избавится.
	/// </summary>
	public event GridViewEditEventHandler RowEdit;
	/// <summary>
	/// Событие, возникающее на выбор элемента грида.
	/// </summary>
	public event GridViewSelectEventHandler RowSelect;
	/// <summary>
	/// Событие, возникающее при нажатии на кнопку Delete (в режиме AutoGenerateDeleteButton=true).
	/// </summary>
	public event GridDeleteEventHandler AutoDeleting;
	/// <summary>
	/// Событие, возникающее при нажатии на кнопку Add (в режиме AutoGenerateAddButton=true и EditMode=Custom ).
	/// </summary>
	public event EventHandler AutoAdding;
	/// <summary>
	/// Событие, возникающее при нажатии на кнопку Edit (в режиме AutoGenerateEditButton=true и EditMode=Custom).
	/// </summary>
	public event GridEditEventHandler AutoEditing;
	/// <summary>
	/// Событие, возникающее перед формированием столбцов грида.
	/// В нем можно программно добавить какие-нибудь столбцы.
	/// </summary>
	public event EventHandler CreateColumns;
	/// <summary>
	/// Событие, возникающее для получения дополнительных параметров, 
	/// передаваемых через Url странице редактирования объекта.
	/// </summary>
	public event GridAdditionalParametersHandler ResolveAddEditParameters;
	/// <summary>
	/// Событие, возникающее при выделении строки в гриде на клиенте.
	/// </summary>
	public event GridSelectEventHandler RowClientSelect;

	#endregion

	#region Свойства внутреннего грида, транслирующиеся на верхний уровень

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
	/// Показывать пейджер (использовать пейджинг).
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
	/// Показывать заголовки таблицы.
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
	/// Активен ли режим поиска.
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

	#region Свойства, отвечающие за поведение
	/// <summary>
	/// URL диалога для редактирования элемента (для режимов редактирования Simple, WithMarker и WithParentMarker).
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
	/// Режим редактирования.
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
	/// Добавлять колонку редактирования.
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
	/// Добавлять кнопку редактирования.
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
	/// Автоматически сгенерированная кнопка удаления
	/// </summary>
	public System.Web.UI.WebControls.Button DeleteButton
	{
		get
		{
			return AutoGenerateDeleteButton ? btnDelete_AutoGenerated : null;
		}
	}

	/// <summary>
	/// Добавлять кнопку создания нового объекта.
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
	/// Автоматически сгенерированная кнопка добавления
	/// </summary>
	public System.Web.UI.WebControls.Button AddButton
	{
		get
		{
			return AutoGenerateAddButton ? btnAdd_AutoGenerated : null;
		}
	}

	/// <summary>
	/// Определяет список ролей, которым доступно добавление
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
	/// Определяет список ролей, которым доступно редактирование
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
	/// Определяет список ролей, которым доступно удаление
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
	/// Дополнительный параметр, передающийся в поддиалоги в результате нажатия кнопок "Добавить" и "Редактировать"
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
	/// Шаблон для дополнительных контролов (кнопок), 
	/// которые будут располагаться под гридом справа от автоматически сгенерированных кнопок.
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

	#region Свойства внутренней UpdatePanel
	/// <summary>
	/// Триггеры на обновление грида.
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
	/// Обновляет внутреннюю панель.
	/// Нужно использовать этот метод для обновления грида 
	/// в случае регистрации контролов не через свойство Triggers, а через методы ScriptManager,
	/// например, RegisterAsyncPostBackControl().
	/// </summary>
	public void Update()
	{
		updatePanel.Update();
	}

	#endregion

	#region Свойства, связанные с сортировкой и пейджингом

	/// <summary>
	/// Формирует и возвращает уникальное значение ключа для хранения в сессии параметров грида.
	/// </summary>
	/// <param name="key">Значение ключа (например, SortExpression)</param>
	/// <returns></returns>
	public string GetUniqueSessionKey(string key)
	{
		return String.Format("{0}_{1}_{2}", Request.RawUrl, UniqueID, key);
	}

	/// <summary>
	/// Является ли сортировка по возрастанию.
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
	/// Выражение для сортировки.
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
	/// Размер страницы (количество записей на странице).
	/// </summary>
	protected int PageSize
	{
		get
		{
			if (!ShowPager) return PagingArgs.MaxPageSize; // если отключен пейджинг, всегда возвращаем размер "бесконечный" страницы
			
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
	/// Индекс страницы (страницы нумеруются с 0).
	/// </summary>
	protected int PageIndex
	{
		get
		{
			if (!ShowPager) return 0; // если отключен пейджинг, всегда возвращаем первую страницу
			
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

	#region Свойства, связанные с функционалом селектирования

	/// <summary>
	/// Определяет, надо ли показывать колонку для выбора элементов (checkboxes)
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
	/// Определяет список ролей, которым доступно селектирование
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
	/// Имя первичного ключа в таблице
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
	/// Коллекция всех ключей.
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
	/// Коллекция ключей выбранных строк.
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
	/// Объект, ответственный за выделение в гриде. Позволяет выделять и сбрасывать отдельные строки грида.
	/// </summary>
	[Browsable( false )]
	public GridSelection Selection
	{
		get
		{
			if(m_Selection == null)
			{
				// поднимаем событие получения контроллера выделения
				if(ResolveSelection != null)
				{
					m_Selection = ResolveSelection( this );
				}
				else
				{
					// если на событие никто не подписан, то создаем экземпляр контроллера по-умолчанию
					m_Selection = new GridSelection( this );
				}
			}
			return m_Selection;
		}
	}

	/// <summary>
	/// Колличество выбранных элесментов.
	/// </summary>
	public int SelectionCount
	{
		get
		{
			return Selection.Count;
		}
	}

	/// <summary>
	/// Таблица с идентификаторами только что отредактированных объектов.
	/// В качастве ключа - url страницы со списком объектов.
	/// В качестве значения - идентификатор объекта.
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

	#region Свойства, связанные со иерархией
	/// <summary>
	/// Позволяет включить поддержку вложенных элементов строк.
	/// В этом режиме появляется дополнительная колонка с кнопками для каждой строки, 
	/// по нажатию которых под строкой разворачаивается дополнительная строка, 
	/// содержимое которой можно установить с помощью шшаблона HierarchicalRowTemplate.
	/// По умолчанию иерархия отключена.
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
	/// Разрешает раскрытие нескольких строк одновременно.
	/// По умолчанию разрешено.
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
	/// Шаблон подэлемента строки, который появялется после нажатия на кнопку раскрытия.
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
	/// Возвращает набор состояний подэлементов иерархии (раскрыты/свернуты).
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
	/// URL для картинки у кнопки в закрытом состоянии.
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
	/// URL для картинки у кнопки в открытом состоянии.
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

	#region Вспомогательные свойства
	/// <summary>
	/// Проверяет, имеет ли текущий пользователь доступ к кнопке редактирования.
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
	/// Проверяет, имеет ли текущий пользователь доступ к кнопке добавления.
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
	/// Проверяет, имеет ли текущий пользователь доступ к кнопке удаления.
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
	/// Находится ли грид в режиме выбора.
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
	/// Полное название типа объектов, которые отображает грид.
	/// Имеет значение если в качестве DataSource используется объект не произведенный от BaseBindingColletion.
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

	#region Обработчики жизненного цикла
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

			// добавляем колонки, определенные пользователем, во внутренний грид
			foreach (DataControlField field in m_columns)
			{
				innerGrid.Columns.Add(field);
			}

			// устанавливаем видимость заголовка
			innerGrid.ShowHeader = ShowHeader;

			// в режима печати настраиваем внешний вид грида
			if (Page.IsInPrintMode)
			{
				ShowPager = false;
				AllowSelection = false;
			}

			// определяем, не находится ли грид в режиме выбора
			if (IsInSelectMode)
			{
				CommandField fld = new CommandField();
				fld.ShowSelectButton = true;
				fld.SelectText = (string)GetLocalResourceObject("SelectButtonCaption");
				fld.CausesValidation = false;
				fld.ItemStyle.Width = Unit.Pixel(60);
				innerGrid.Columns.Add(fld);

				// скрываем кнопки добавления и удаления
				AutoGenerateAddButton = false;
				AutoGenerateDeleteButton = false;
				// скрываем колонку выбора
				AllowSelection = false;
			}
			else
			{
				if (AutoGenerateDeleteButton && AllowDeleting) // открываем кнопку удаления
				{
					plhDeleteButton.Visible = true;

					// регистрируем скрипт подтверждения удаления
					//Page.RegisterConfirm(btnDelete_AutoGenerated, Resources.Messages.DeleteConfirmation);

					// связываем контроллер с гридом
					gscDelete_AutoGenerated.GridControl = this;
				}

				if (AutoGenerateAddButton && AllowAdding) // открываем кнопку добавления
				{
					plhAddButton.Visible = true;
				}

				if (AutoGenerateEditButton && AllowEditing) // генерируем кнопку редактирования
				{
					CommandField fld = new CommandField();
					fld.ShowEditButton = true;
					fld.EditText = (string)GetLocalResourceObject("EditButtonCaption");
					fld.CausesValidation = false;
					fld.ItemStyle.Width = Unit.Pixel(60);
					innerGrid.Columns.Add(fld);

					// подписываемся на событие редактирования
					innerGrid.RowEditing += new GridViewEditEventHandler(OnRowEditing);
				}

				if (AllowSelection)
				{
					plhTotalCount.Visible = true;
				}
			}

			// подписываемся на событие Select
			innerGrid.SelectedIndexChanging += new GridViewSelectEventHandler(OnSelectedIndexChanging);

			// TODO: код из старого грида (надо как-нибудь избавиться)
			innerGrid.RowEditing += delegate(object s, GridViewEditEventArgs args)
			{
				if (RowEdit != null)
					RowEdit(this, args);
			};

			// инстанцируем под грид шаблон внешних кнопок (но только если это не PrintMode)
			if (m_ButtonContainerTemplate != null)
			{
				m_ButtonContainerTemplate.InstantiateIn(phButtonContainer);
			}

			if (ShowPager)
			{
				ScriptManager.GetCurrent(Page).RegisterAsyncPostBackControl(ddlPageSize);
				//TODO:изменение числа записей на стр
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
			// устанавливаем в списке размер страницы
			ListItem li = ddlPageSize.Items.FindByValue(PageSize.ToString());
			if (li != null)
			{
				ddlPageSize.SelectedItem.Selected = false;
				li.Selected = true;
			}
		}
		// настраиваем вид пейджера
		phPager.Visible = ShowPager;

		// скрываем/показываем столбец выбора только в соотв. режиме
		innerGrid.Columns[0].Visible = AllowSelection;
		// скрываем/показываем столбец кнопок раскрытия только в соотв. режиме
		innerGrid.Columns[1].Visible = AllowHierarchy;

		// если находимся в режиме печати или в режиме выбора, то скрываем кнопки
		phButtonContainer.Visible = !Page.IsInPrintMode && !IsInSelectMode;

		// регистрируем специцльные клиентские функции для работы с чекбоксами
		if (AllowSelection)
		{
			RegisterHighLightOnCheckScriptBlock();

			// проставляем клиентские обработчики OnClick для чекбоксов из левого столбца
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

			// заполняем поле с количеством выбранных объектов
			totalSelected.Value = Selection.Count.ToString();
			lblTotalSelected.Text = Selection.Count.ToString();
		}

		// Регистрируем вспомогательные клиентские функции
		RegisterSelectedRowsCountScriptBlock();
		RegisterSelectAllRowsScriptBlock();

		//bool isAllSelected = true;
		object selectedKey = new object();
		
		// запоминаем идентификатор последнего отредактированного объекта
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
			// необходимо ли подсветить строку обособленно

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
				// проставляем клиентские обработчики OnClick для чекбоксов из левого столбца
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

					/*if (row.Enabled)
					{
						isAllSelected = false;
					}*/
				}
			}

			// проставляем клиентские обработчики для выделения строки под мышкой
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

	#region Методы привязки данных
	/// <summary>
	/// Обновляет данные в таблице. Также необходимо указать, нужен ли сброс номера страницы или нет. 
	/// Сброс необходим, если например обновление данных происходит после удаления записей.
	/// </summary>
	/// <param name="resetPagingAndSelection">Сбрасывать ли номер страницы (переходить на первую) и выделение или нет.</param>
	public void RefreshData(bool resetPagingAndSelection)
	{
		RefreshData(resetPagingAndSelection, resetPagingAndSelection);
	}

	/// <summary>
	/// Обновляет данные в таблице. Также необходимо указать, нужен ли сброс номера страницы или нет. 
	/// Сброс необходим, если например обновление данных происходит после удаления записей.
	/// </summary>
	/// <param name="resetPaging">Сбрасывать ли номер страницы (переходить на первую) или нет.</param>
	/// <param name="resetSelection">Сбрасывать ли коллекцию с выделенными объектами.</param>
	public void RefreshData(bool resetPaging, bool resetSelection)
	{
		if (resetPaging)
		{
			PageIndex = 0;
			// закрываем все вложенные элементы
			if (AllowHierarchy)
			{
				HierarchicalRowStates.Clear();
			}
		}
		// чистим коллекцию отмеченных ключей(необходимо например при удалении)
		if (resetSelection)
		{
			Selection.Clear();
		}
		BindData();
	}



	/// <summary>
	/// Биндит данные к гриду.
	/// </summary>
	protected void BindData()
	{
		// если источник не задан, то выходим
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

		// в режиме поиска, если ничего не найдено, показываем сообщение
		if (SearchMode && (result == null || result.TotalCount == 0))
		{
			plhGrid.Visible = false;
			plhNothingFound.Visible = IsPostBack ? true : false;
		}
		else
		{
			plhGrid.Visible = true;
			plhNothingFound.Visible = false;

			// заполняем грид
			innerGrid.DataSource = result.Result;
			innerGrid.DataBind();

		/*	// определяем тип элемента коллекции
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
			// инициализируем пейджер
			if (ShowPager)
			{
				BindPager(PageIndex, PageSize, 5, result.TotalCount);
			}

			// заполняем поле с количеством выбранных объектов
			lbTotalRecords.Text = result.TotalCount.ToString();

			// поднимаем событие DataBound
			OnDataBound(EventArgs.Empty);
		}
	}

	/// <summary>
	/// Поднимает событие DataBound.
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

	#region Методы и обработчики пейджинга
	/// <summary>
	/// Инициализирует пейджер для грида.
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
			// закрываем все вложенные элементы
			if (AllowHierarchy)
			{
				HierarchicalRowStates.Clear();
			}
			// перебиндиваем данные
			BindData();
		}
	}
	#endregion

	#region Обработчик сортировки

	protected void OnSorting(object sender, GridViewSortEventArgs args)
	{
		SortOrderAsc = (args.SortExpression == SortExpression)
			? !SortOrderAsc
			: true;
		SortExpression = args.SortExpression;
		// закрываем все вложенные элементы
		if (AllowHierarchy)
		{
			HierarchicalRowStates.Clear();
		}
		// перебиндиваем данные
		BindData();
	}

	#endregion

	#region Обработчики GridView
	/// <summary>
	/// Обрабатывает создание иерархии строк грида.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void OnRowCreated(object sender, GridViewRowEventArgs e)
	{
		// обрабатываем заголовки столбцов
		if (e.Row.RowType == DataControlRowType.Header)
		{
			if (!String.IsNullOrEmpty(SortExpression))
			{
				// пробегаем по колонкам и проверяем, совпадает ли текущее поле для сортировки с колоночным
				for (int i = 0; i < innerGrid.Columns.Count; i++)
				{
					if (innerGrid.Columns[i].SortExpression == SortExpression)
					{
						// если совпадает, добавляем в соотв. ячейку значок сортировки
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

		// обрабатываем строки
		if (e.Row.RowType == DataControlRowType.DataRow)
		{
			// поднимаем событие созданния строк
			if (RowCreated != null)
				RowCreated(this, e);

			// если задан включена поддерка иерархии
			if (AllowHierarchy)
			{
				// формируем контрол
				OnHierarchicalRowCreated(sender, e);
			}
		}
	}

	/// <summary>
	/// Создает все необходимые дополнительные контролы, образующие сам элемент,
	/// а также поднимает событие создания подэлемента.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void OnHierarchicalRowCreated(object sender, GridViewRowEventArgs e)
	{
		GridViewRow row = e.Row;
		// добавляем дополнительную ячейку
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

		// формируем элемент только в случае, если он раскрыт
		if (state == HierarchicalRowState.Expanded)
		{
			// поднимаем событие перед созданием элемента
			if (HierarchicalRowPreCreated != null)
			{
				HierarchicalRowPreCreated(sender, e);
			}

			// инстанцируем шаблон подэлемента
			if (m_HierarchicalRowTemplate != null)
			{
				m_HierarchicalRowTemplate.InstantiateIn(cell);
			}

			// поднимаем событие создания поэлемента
			if (HierarchicalRowCreated != null)
			{
				HierarchicalRowCreated(sender, e);
			}
		}
	}

	/// <summary>
	/// Обрабатывает формирование строк и поднимает событие заполнения.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
	{
		// обрабатываем заголовки столбцов
		if (e.Row.RowType == DataControlRowType.Header)
		{
			// Устанавливаем у CheckBox'а атрибут SelectionGrid, который содержит ID этого грида.
			// Этот атрибут появиться у SPAN'а, в который будет помещен CheckBox.
			// Также добавляем обработчик нажатия для выделения всех CheckBox'ов в гриде.
			CheckBox chkSelectAll = (CheckBox)e.Row.FindControl("chkSelectAll");
			if (chkSelectAll != null)
			{
				chkSelectAll.Attributes["SelectionGrid"] = this.ClientID;
				chkSelectAll.Attributes["MultiSelect"] = "yes";
				chkSelectAll.Attributes["OnClick"] = String.Format("SelectAllRows(this,'{0}');", this.ClientID);
			}
		}

		// обрабатываем строки
		if (e.Row.RowType == DataControlRowType.DataRow)
		{
			// устанавливаем у CheckBox'а атрибут SelectionGrid, который содержит ID этого грида.
			// Этот атрибут появиться у SPAN'а, в который будет помещен CheckBox
			CheckBox chkSelected = (CheckBox)e.Row.FindControl("chkSelected");
			if (chkSelected != null)
			{
				chkSelected.Attributes["SelectionGrid"] = this.ClientID;
			}

			HierarchicalRowState toggleState = HierarchicalRowStates[e.Row.RowIndex];

			// формируем кнопку раскрытия сокрытия
			ImageButton ibToggle = (ImageButton)e.Row.FindControl("ibToggle");
			ibToggle.CommandArgument = e.Row.RowIndex.ToString();
			ibToggle.ImageUrl = toggleState == HierarchicalRowState.Collapsed
				? HierarchicalCollapsedImageUrl
				: HierarchicalExpandedImageUrl;

			// поднимаем событие обработки строк
			if (RowDataBound != null)
			{
				RowDataBound(this, e);
			}

			// биндим подэлемент только в том случае, если он раскрыт
			if (AllowHierarchy
				&& toggleState == HierarchicalRowState.Expanded)
			{
				OnHierarchicalRowDataBound(sender, e);
			}

			// устанавливаем (дополняем) CssClass строки грида
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
	/// Поднимает событие заполнения подэлемента.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void OnHierarchicalRowDataBound(object sender, GridViewRowEventArgs e)
	{
		if (m_HierarchicalRowTemplate != null)
		{
			// поднимаем событие обработки строк подэлементов
			if (HierarchicalRowDataBound != null)
			{
				HierarchicalRowDataBound(this, e);
			}
		}
	}

	/// <summary>
	/// Поднимает событие команды.
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
			// если множественное раскрытие запрещено, то закрываем все строки
			if (!AllowMultiExpanding)
			{
				HierarchicalRowStates.Clear();
			}
			HierarchicalRowStates[rowIndex] = newState;
			// перебиндиваем данные
			BindData();
		}

		// поднимаем событие команды
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
			// в режиме Custom поднимаем событие на редактирование
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
		// если находимся в режиме выбора, заносим в сессию выбранный идентификатор
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
		// проверяем доступность колонок
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
		// проверяем доступность столбца селектирования
		bool isSelectionAccessible = !String.IsNullOrEmpty(AllowedRolesToSelection)
			? user.IsInRoles(AllowedRolesToSelection)
			: true;
		if (!isSelectionAccessible)
		{
			innerGrid.Columns[0].Visible = false;
		}

		// доступность кнопки добавления
		bool isAddButtonAccessible = !String.IsNullOrEmpty(AllowedRolesToAdd)
			? user.IsInRoles(AllowedRolesToAdd)
			: true;
		if (!isAddButtonAccessible)
			plhAddButton.Visible = false;

		// доступность кнопки удаления
		bool isDeleteButtonAccessible = !String.IsNullOrEmpty(AllowedRolesToDelete)
			? user.IsInRoles(AllowedRolesToDelete)
			: true;
		if (!isDeleteButtonAccessible)
			plhDeleteButton.Visible = false;

		return true;
	}

	#endregion

	#region Регистрация вспомогательных клиентских скриптов

	/// <summary>
	/// Регистрирует клиентскую функцию, возвращающую количество выбранных пользователем строк.
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
	/// Регистрирует клиентскую функцию, которая выбирает/отключает все чекбоксы в гриде.
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
	/// Регистрирует клиентскую функцию, которая подсвечивает строки в гриде при наведении мыши.
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
	/// Регистрирует клиентскую функцию, которая подсвечивает строки в гриде, отмеченные чекбоксами.
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

	#region Обработчики автоматически создаваемых кнопок

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
			// в режиме Custom поднимаем событие на добавление
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
				// поднимаем событие выделения строки
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
				// поднимаем событие выделения строки
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
			if (check.ClientID == chkSender.ClientID)// этот ли чек бокс был нажат
			{
				Selection[DataKeys[row.RowIndex]] = chkSender.Checked;
				// поднимаем событие выделения строки
				if(RowClientSelect != null)
				{
					RowClientSelect( this, new GridSelectEventArgs( row.RowIndex, chkSender.Checked ) );
				}
			}
		}
		//updatePanel.Update();
	}
	#endregion

	#region Вспомогательные методы
	/// <summary>
	/// 
	/// </summary>
	/// <param name="controlID"></param>
	/// <param name="baseСontrol"></param>
	/// <returns></returns>
	private Control FindControlRecursive(string controlID, Control baseСontrol)
	{
		if (baseСontrol.ID == controlID)
		{
			return baseСontrol;
		}

		foreach (Control control in baseСontrol.Controls)
		{
			Control ctl = FindControlRecursive(controlID, control);
			if (ctl != null)
				return ctl;
		}
		return null;
	}

	/// <summary>
	/// Редиректит на страницу редактирования объекта.
	/// </summary>
	/// <param name="dataKey">Ключ, который указан для строки, которую редактируют.</param>
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
