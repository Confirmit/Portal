using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web;
using System.Web.UI;

namespace EPAMSWeb.UI
{
	/// <summary>
	/// �������, ������� � ������� ���� ������� ��������� �� ���������� ��������.
	/// </summary>
	public class UpdateProgressStatus : BaseWebControl, IScriptControl
	{
		#region ����

		private ScriptManager m_ScriptManager = null;

		#endregion

		#region ��������

		private ScriptManager ScriptManager
		{
			get
			{
				if(m_ScriptManager == null)
				{
					m_ScriptManager = ScriptManager.GetCurrent(Page);

					if(m_ScriptManager == null)
						throw new HttpException( "A ScriptManager control must exist on the current page." );
				}

				return m_ScriptManager;
			}
		}

		/// <summary>
		/// �����, ������������ ��� ���������� ������.
		/// </summary>
		[Localizable( true )]
		public string Text
		{
			get
			{
				object o = ViewState["Text"];
				return o != null ? (string)ViewState["Text"] : String.Empty;
			}
			set
			{
				ViewState["Text"] = value;
			}
		}

		#endregion

		#region ��������� ����

		protected override void OnPreRender( EventArgs e )
		{
			if(!this.DesignMode)
			{
				ScriptManager.RegisterScriptControl( this );
			}

			base.OnPreRender( e );
		}

		protected override void Render( HtmlTextWriter writer )
		{
			if(!this.DesignMode)
			{
				ScriptManager.RegisterScriptDescriptors( this );
			}
			base.Render( writer );
		}

		#endregion

		#region ������ IScriptControl 

		protected virtual IEnumerable<ScriptReference> GetScriptReferences()
		{
			ScriptReference reference = new ScriptReference();
			//reference.Path = ResolveClientUrl("~/scripts/progress.js");
			reference.Path = "~/scripts/progress.js";
			return new ScriptReference[] { reference };
		}

		protected virtual IEnumerable<ScriptDescriptor> GetScriptDescriptors()
		{
			ScriptControlDescriptor descriptor = new ScriptControlDescriptor( "Ultersys.UI.UpdateProgressStatus", this.ClientID );
			descriptor.AddProperty( "message", Text );

			return new ScriptDescriptor[] { descriptor };
		}

		IEnumerable<ScriptReference> IScriptControl.GetScriptReferences()
		{
			return GetScriptReferences();
		}

		IEnumerable<ScriptDescriptor> IScriptControl.GetScriptDescriptors()
		{
			return GetScriptDescriptors();
		}

		#endregion
	}
}