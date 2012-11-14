using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Reflection;

namespace EPAMSWeb.UI
{
	/// <summary>
	/// ������� ��� ������, ����������� ������ �������� ����� �������� ��������� ��� ������ ����� ��������.
	/// </summary>
	public class AsyncPostBackTrigger : System.Web.UI.AsyncPostBackTrigger
	{
		#region ����

		private Control m_AssociatedControl;
		private Control m_AssociatedControlContainer;
		private string m_ControlContainerID;
		private static MethodInfo m_EventHandler;
		private ScriptManager m_ScriptManager;

		#endregion

		#region ��������

		/// <summary>
		/// ������������� ��������, ������� �������� ����������� ��� �������� ControlID.
		/// ���� �� �� ������, �� ������� �������� ��� ������������ AsyncPostBackTrigger.
		/// </summary>
		public string ControlContainerID
		{
			get
			{
				return m_ControlContainerID;
			}
			set
			{
				m_ControlContainerID = value;
			}
		}

		/// <summary>
		/// ����������, ������� ������������� �� ��������� ������� ��������.
		/// </summary>
		private static MethodInfo EventHandler
		{
			get
			{
				if(m_EventHandler == null)
				{
					m_EventHandler = typeof( AsyncPostBackTrigger ).GetMethod( "OnEvent" );
				}
				return m_EventHandler;
			}
		}

		/// <summary>
		/// ��������� ScriptManager ��� ����������� ��������.
		/// </summary>
		internal ScriptManager ScriptManager
		{
			get
			{
				if(m_ScriptManager == null)
				{
					Page page = Owner.Page;
					if(page == null)
					{
						throw new InvalidOperationException( "Page cannot be null." );
					}
					m_ScriptManager = ScriptManager.GetCurrent( page );
					if(m_ScriptManager == null)
					{
						throw new InvalidOperationException( String.Format( "ScriptManager is required for using UpdatePanel '{0}'.", Owner.ID ) );
					}
				}
				return m_ScriptManager;
			}
		}

		#endregion

		protected override void Initialize()
		{
			if(!String.IsNullOrEmpty( ControlContainerID ))
			{
				// ���� ����� ��������� ��� ��������, �� ������� ���� ��� ���������
				m_AssociatedControlContainer = ControlUtil.FindTargetControl( ControlContainerID, Owner, true );
				if(m_AssociatedControlContainer == null)
				{
					throw new InvalidOperationException( String.Format( "A control with ID '{0}' could not be found for the trigger in UpdatePanel '{1}'.", ControlContainerID, Owner.ID ) );
				}
				// ����� ���� ������� � ���� ����������
				m_AssociatedControl = m_AssociatedControlContainer.FindControl( ControlID );
				if(m_AssociatedControl == null)
				{
					throw new InvalidOperationException( String.Format( "A control with ID '{0}' could not be found in control container '{1}' for the trigger in UpdatePanel '{2}'.", ControlID, ControlContainerID, Owner.ID ) );
				}
				// ������������ ������� ��� �����������
				ScriptManager.RegisterAsyncPostBackControl( m_AssociatedControl );
				// ������������� �� ��������� ������� ��������
				string name = EventName;
				if(name.Length != 0)
				{
					EventInfo info = m_AssociatedControl.GetType().GetEvent( name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase );
					if(info == null)
					{
						throw new InvalidOperationException( String.Format( "An event '{0}' could not be found in control '{1}' for the trigger in UpdatePanel '{2}'.", name, ControlID, Owner.ID ) );
					}

					MethodInfo method = info.EventHandlerType.GetMethod( "Invoke" );
					ParameterInfo[] parameters = method.GetParameters();
					if((!method.ReturnType.Equals( typeof( void ) ) || (parameters.Length != 2)) || !typeof( EventArgs ).IsAssignableFrom( parameters[1].ParameterType ))
					{
						throw new InvalidOperationException( String.Format( "Invalid event '{0}' in control '{1}' for the trigger in UpdatePanel '{2}'. This event must have (object, EventArgs).", name, ControlID, Owner.ID ) );
					}

					Delegate handler = Delegate.CreateDelegate( info.EventHandlerType, this, EventHandler );
					info.AddEventHandler( m_AssociatedControl, handler );
				}

			}
			else
			{
				// ���� ���������
				base.Initialize();
			}
		}
	}

}