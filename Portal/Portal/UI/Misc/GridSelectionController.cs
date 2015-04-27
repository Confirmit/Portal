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
	/// ���������� ����������� ��������� ������������� ������ � ��������������� �����
	/// � ��������������� ������� ������ ��������� ������������ �������� ����������
	/// �� ������� �������.
	/// </summary>
	/// <remarks>
	/// � ������ ����� ���������� ������������ ��� ���������� (disable) ����������� ������ 
	/// (��� �������, "������� ���������"), ���� � �������������� Grid'� �� ������� �����.
	/// </remarks>
	[ParseChildren( true )]
	[PersistChildren( false )]
	public class GridSelectionController : WebControl
	{
		#region �������� �����������

		/// <summary>
		/// ������������� ��������������� Grid'�.
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
		/// ������������� ������������ �������� ���������� (��������, ������).
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

		#region ������������� �������� ����������

		Control m_GridControl = null;
		/// <summary>
		/// �������������� Grid.
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
		/// ����������� ������� ����������.
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

		#region ��������������� ������

		/// <summary>
		/// ���������� ���� ������� ���������� �� ID � �������� ��������� ��������� ����������.
		/// </summary>
		/// <returns>���� ������� ���������� �� ������, ���������� null.</returns>
		protected static Control FindControlRecursively( string controlID, Control searchIn )
		{
			// ���� ��������������� � �������� ����������
			Control control = searchIn.FindControl( controlID );
			if(control != null)
				return control;

			// ���� � �������� ���������
			foreach(Control child in searchIn.Controls)
			{
				control = FindControlRecursively( controlID, child );
				if(control != null)
					return control;
			}

			return null;
		}

		#endregion

		#region ����������� ���������� ��������

		/// <summary>
		/// ������������ ���������� ������� ��� �������� �� ����������
		/// ������������ �������� ����������.
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

			// ������������ ���������� �������, �������������� ��������� (enabled/disabled) �������������
			// �������� ���������� (target control). ������ ������� ���������� ��� �������� ��������
			// � ��� ��������� ��������� ��������� � �������������� Grid'�.
			ScriptManager.RegisterClientScriptBlock( this, GetType(), scriptKey + GridControl.ClientID, scriptBody.ToString(), true );
		}

		#endregion

		#region ��������� ����

		protected override void OnLoad(EventArgs e)
		{
			TargetControl.Attributes["itemsSelected"] = "0";
		}

		protected override void OnPreRender( EventArgs e )
		{
			base.OnPreRender( e );

			// ���� ���������� ��������, �� ������ �� ������
			if(!IsEnabled) return;

			// ������������ ������
			RegisterEnableDisableScriptBlock();
			string script = String.Format("EnableDisableTargetControl('{0}', '{1}');", GridControl.ClientID, TargetControl.ClientID);
			

			IGridRowsContainer grid = GridControl as IGridRowsContainer;		
			if (grid==null)
			{
				throw new InvalidOperationException( String.Format( "'{0}' doesn't implement '{1}' interface.", GridID, typeof( IGridRowsContainer ).FullName ) );
			}
			// ����������� ���������� ����������� OnClick ��� ��������� ������ �����
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
			
			// ������������� ��������� ������ � ����������� �� ����������� ��������� ���������
			IGridSelection gridSelection = GridControl as IGridSelection;
			int selectionCount = gridSelection.SelectionCount;

			TargetControl.Enabled = selectionCount > 0;
			
			// �������� ����������� ��������� ���������, � ������������� � ���������� �����.
			int currentCount = Convert.ToInt32(TargetControl.Attributes["itemsSelected"]);
			TargetControl.Attributes["itemsSelected"] = (currentCount + selectionCount).ToString();
			// ������������ ������, ������� ���������� ��������� ������ � ����������� �� ����������� ��������� ���������
			string disableScript = String.Format("SetDisabledTargetControl({0},'{1}');", ((currentCount + selectionCount) == 0).ToString().ToLower(), TargetControl.ClientID);
			
			ScriptManager.RegisterStartupScript(Page, typeof(Page), "StartUpEnableDisableTargetControl" + GridControl.ClientID + TargetControl.ClientID, disableScript, true);
		}

		#endregion

	}

}