using System;
using System.Data;
using System.Configuration;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.ComponentModel;

namespace EPAMSWeb.UI
{
	/// <summary>
	/// Контроллер отслеживает выбранные пользователем строки в контроллируемом гриде
	/// и соответствующим образом меняет состояние подчиненного элемента управления
	/// на стороне клиента.
	/// </summary>
	/// <remarks>
	/// В данное время контроллер используется для отключения (disable) подчиненных кнопок 
	/// (как правило, "Удалить выбранные"), если в контролируемом Grid'е не выбрано строк.
	/// </remarks>
	[ParseChildren( true )]
	[PersistChildren( false )]
	public class GridSelectionController : WebControl
	{
		#region Свойства контроллера

		/// <summary>
		/// Идентификатор контролируемого Grid'а.
		/// </summary>
		[Browsable( true )]
		[IDReferenceProperty(typeof(Button))]
		public string GridID
		{
			get
			{
				return (string)ViewState["GridID"];
			}
			set
			{
				ViewState["GridID"] = value;
			}
		}

		/// <summary>
		/// Идентификатор подчиненного элемента управления (например, кнопки).
		/// </summary>
		[Browsable( true )]
		public string TargetControlID
		{
			get
			{
				return (string)ViewState["TargetControlID"];
			}
			set
			{
				ViewState["TargetControlID"] = value;
			}
		}

		#endregion

		#region Котролируемые элементы управления

		Control m_GridControl = null;
		/// <summary>
		/// Контролируемый Grid.
		/// </summary>
		[Browsable( false )]
		public Control GridControl
		{
			get
			{
				if(m_GridControl == null)
				{
					Control control = ControlUtil.FindTargetControl( GridID, this, true );
					if(control == null)
						throw new InvalidOperationException( String.Format( "Referenced control '{0}' doesn't exist.", GridID ) );

					m_GridControl = control;
				}

				return m_GridControl;
			}
			set
			{
				m_GridControl = value;
			}
		}

		WebControl m_TargetControl = null;
		/// <summary>
		/// Подчиненный элемент управления.
		/// </summary>
		[Browsable( false )]
		public WebControl TargetControl
		{
			get
			{
				if(m_TargetControl == null)
				{
					Control control = ControlUtil.FindTargetControl( TargetControlID, this, true );
					if(control == null)
						throw new InvalidOperationException( String.Format( "Referenced control '{0}' doesn't exist.", TargetControlID ) );

					m_TargetControl = control as WebControl;
					if(m_TargetControl == null)
					{
						throw new InvalidOperationException( String.Format( "Referenced control '{0}' must be inherited from WebControl.", TargetControlID ) );
					}
				}

				return m_TargetControl;
			}
			set
			{
				m_TargetControl = value;
			}
		}

		#endregion

		#region Вспомогательные методы

		/// <summary>
		/// Рекурсивно ищет элемент управления по ID в иерархии заданного эелемента управления.
		/// </summary>
		/// <returns>Если элемент управления не найден, возвращает null.</returns>
		protected static Control FindControlRecursively( string controlID, Control searchIn )
		{
			// Ищем непосредственно в элементе управления
			Control control = searchIn.FindControl( controlID );
			if(control != null)
				return control;

			// Ищем в дочерних элементах
			foreach(Control child in searchIn.Controls)
			{
				control = FindControlRecursively( controlID, child );
				if(control != null)
					return control;
			}

			return null;
		}

		#endregion

		#region Регистрация клиентских скриптов

		/// <summary>
		/// Регистрирует клиентскую функцию для контроля за состоянием
		/// подчиненного элемента управления.
		/// </summary>
		protected void RegisterEnableDisableScriptBlock()
		{
			string scriptKey = "EnableDisableTargetControlScriptBlock";

			StringBuilder scriptBody = new StringBuilder();
			scriptBody.Append("function EnableDisableTargetControl(gridID, targetID)" );
			scriptBody.Append("{" );
			scriptBody.Append(" var target = document.getElementById(targetID);");
			scriptBody.Append(" if (target == null) return;");
			//scriptBody.Append(" alert(target.getAttribute('itemsSelected'));");
			scriptBody.Append(" target.disabled = (target.getAttribute('itemsSelected')==0);");
			//scriptBody.Append(" var totalSelected = document.getElementById(gridID + '_totalSelected');");
			//scriptBody.Append("  if (document.getElementById(targetID) == null) return;" );
			//scriptBody.Append("  if (0 < totalSelected.value)" );
			//scriptBody.Append("    document.getElementById(targetID).disabled = false;" );
			//scriptBody.Append("  else" );
			//scriptBody.Append("    document.getElementById(targetID).disabled = true;" );
			scriptBody.Append("}" );

			scriptBody.Append("function SetDisabledTargetControl(state, targetID)");
			scriptBody.Append("{");
			scriptBody.Append(" var target = document.getElementById(targetID);");
			scriptBody.Append(" if (target == null) return;");
			//scriptBody.Append(" alert(target.disabled +' '  + state);");
			scriptBody.Append(" target.disabled = state;");
			//scriptBody.Append(" alert(target.disabled);");
			scriptBody.Append("}");

			// Регистрируем клиентскую функцию, контролирующую состояние (enabled/disabled) подчиненнтого
			// элемента управления (target control). Данная функция вызывается при загрузке страницы
			// и при изменении состояния чекбоксов в контролируемом Grid'е.
			ScriptManager.RegisterClientScriptBlock( this, GetType(), scriptKey + GridControl.ClientID, scriptBody.ToString(), true );
		}

		#endregion

		#region Жизненный цикл

		protected override void OnLoad(EventArgs e)
		{
			TargetControl.Attributes["itemsSelected"] = "0";
		}

		protected override void OnPreRender( EventArgs e )
		{
			base.OnPreRender( e );

			// если контроллер отключен, то ничего не делаем
			if(!IsEnabled) return;

			// регистрируем скрипт
			RegisterEnableDisableScriptBlock();
			string script = String.Format("EnableDisableTargetControl('{0}', '{1}');", GridControl.ClientID, TargetControl.ClientID);
			

			IGridRowsContainer grid = GridControl as IGridRowsContainer;		
			if (grid==null)
			{
				throw new InvalidOperationException( String.Format( "'{0}' doesn't implement '{1}' interface.", GridID, typeof( IGridRowsContainer ).FullName ) );
			}
			// Проставляем клиентские обработчики OnClick для чекбоксов выбора строк
			/*GridViewRow headerRow = grid.HeaderRow;
			if (headerRow != null)
			{
				CheckBox chkSelectAll = (CheckBox)headerRow.Cells[0].FindControl("chkSelectAll");
				chkSelectAll.Attributes["OnClick"] +=
					String.Format("EnableDisableTargetControl('{0}', '{1}');", GridControl.ClientID, TargetControl.ClientID); ;
			}

			foreach (GridViewRow row in grid.Rows)
			{
				CheckBox chkSelected = (CheckBox)row.Cells[0].FindControl("chkSelected");
				chkSelected.Attributes["OnClick"] +=
					String.Format("EnableDisableTargetControl('{0}', '{1}');", GridControl.ClientID, TargetControl.ClientID); ;
			}
			*/
			
			// устанавливаем состояние кнопки в зависимости от колличества выбранных элементов
			IGridSelection gridSelection = GridControl as IGridSelection;
			int selectionCount = gridSelection.SelectionCount;

			TargetControl.Enabled = selectionCount > 0;
			
			// изменяем колличество выбранных элементов, в соостветствии с состоянием грида.
			int currentCount = Convert.ToInt32(TargetControl.Attributes["itemsSelected"]);
			TargetControl.Attributes["itemsSelected"] = (currentCount + selectionCount).ToString();
			// регистрируем скрипт, который выставляет состояние кнопки в зависимости от колличества выбранных элементов
			string disableScript = String.Format("SetDisabledTargetControl({0},'{1}');", ((currentCount + selectionCount) == 0).ToString().ToLower(), TargetControl.ClientID);
			
			ScriptManager.RegisterStartupScript(Page, typeof(Page), "StartUpEnableDisableTargetControl" + GridControl.ClientID + TargetControl.ClientID, disableScript, true);
		}

		#endregion

	}

}