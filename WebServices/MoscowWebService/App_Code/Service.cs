using System;
using System.Collections.Generic;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Configuration;

using UlterSystems.PortalLib.BusinessObjects;
using UlterSystems.PortalLib;
using ConfirmIt.PortalLib.BAL;

[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class Service : System.Web.Services.WebService
{
	/*#region Fields
	private AuthHeader m_AuthHeader;
	#endregion

	#region Properties
	/// <summary>
	/// Header for authentication.
	/// </summary>
	public AuthHeader AuthenticationHeader
	{
		get { return m_AuthHeader; }
		set { m_AuthHeader = value; }
	}
	#endregion*/

	#region ������������
	/// <summary>
	/// �����������.
	/// </summary>
	public Service () 
	{
		// ���������������� ������.
		log4net.Config.XmlConfigurator.Configure();
		Logger.Log.Info(String.Format(Resources.Strings.ServiceStarted, GetOfficeName()));

		//WindowsIdentity currentUser = WindowsIdentity.GetCurrent();
		//if (currentUser != null)
		//   Logger.Log.Info("������� ������������: " + currentUser.Name);
		//else
		//   Logger.Log.Info("�� ������� ���������� �������� ������������.");

		//Uncomment the following line if using designed components 
		//InitializeComponent(); 
	}
	#endregion

	#region ������
	/// <summary>
	/// ���������� ������� ������������� ����������� �����.
	/// </summary>
	/// <returns>������� ������������� ����������� �����.</returns>
	private XMLSerializableUserStatusInfo[] GetMoscowUsersStatuses()
	{
		MoscowUser[] users = MoscowUser.GetLongServiceUsers();
		if ((users == null) || (users.Length == 0))
			return new XMLSerializableUserStatusInfo[0];

		List<XMLSerializableUserStatusInfo> coll = new List<XMLSerializableUserStatusInfo>();
		foreach (MoscowUser user in users)
		{
			MoscowUserEvents[] userEvents = MoscowUserEvents.GetUserEvents(user, DateTime.Today);

			XMLSerializableUserStatusInfo usInfo = GetUserStatus(user, userEvents);
			if (usInfo != null)
				coll.Add(usInfo);
		}
		return coll.ToArray();
	}

	/// <summary>
	/// ���������� ������ ������������.
	/// </summary>
	/// <param name="user">������������.</param>
	/// <param name="userEvents">������� ������������.</param>
	/// <returns>������ ������������.</returns>
	private XMLSerializableUserStatusInfo GetUserStatus(MoscowUser user, MoscowUserEvents[] userEvents)
	{
		if (user == null)
			return null;
		if ((userEvents == null) || (userEvents.Length == 0))
		{
			XMLSerializableUserStatusInfo absentInfo = new XMLSerializableUserStatusInfo();
			absentInfo.UserID = -1; // �� ��������� ����� ������������� ������������.
			absentInfo.USLName = user.USLName;
			absentInfo.UserName = user.FullName;
			absentInfo.EventType = WorkEventType.TimeOff;
			absentInfo.Status = GetEventName(absentInfo.EventType);
			return absentInfo;
		}

		XMLSerializableUserStatusInfo usInfo = new XMLSerializableUserStatusInfo();
		usInfo.UserID = -1; // �� ��������� ����� ������������� ������������.
		usInfo.USLName = user.USLName;
		usInfo.UserName = user.FullName;

		WorkEventType? eventType = null;
		DateTime begin = DateTime.MaxValue;
		DateTime now = DateTime.Now;
		TimeSpan workDuration = TimeSpan.Zero;
		bool userWorked = false;
		bool workEventClosed = true;

		foreach (MoscowUserEvents curEvent in userEvents)
		{
			switch (curEvent.EventType)
			{
				case MoscowEventType.BusinessTrip:
					eventType = WorkEventType.BusinessTrip;
					break;
				case MoscowEventType.Ill:
					eventType = WorkEventType.Ill;
					break;
				case MoscowEventType.TrustIll:
					eventType = WorkEventType.TrustIll;
					break;
				case MoscowEventType.Vacation:
					eventType = WorkEventType.Vacation;
					break;
				case MoscowEventType.OffDay:
					eventType = WorkEventType.Vacation;
					break;
				case MoscowEventType.TakenDay:
					eventType = WorkEventType.Vacation;
					break;
				case MoscowEventType.WorkDay:
					if (curEvent.BeginTime != null)
					{
						userWorked = true;
						if (begin > curEvent.BeginTime.Value)
							begin = curEvent.BeginTime.Value;
						if (curEvent.EndTime != null)
						{
							workDuration += (curEvent.EndTime.Value - curEvent.BeginTime.Value); 
						}
						else
						{
							DateTime bt = new DateTime(now.Year, now.Month, now.Day, curEvent.BeginTime.Value.Hour, curEvent.BeginTime.Value.Minute, curEvent.BeginTime.Value.Second); 
							workDuration += (now - bt);
							workEventClosed = false;
						}
					}
					break;
			}
		}

		if (eventType != null)
		{
			usInfo.EventType = eventType.Value;
			usInfo.Status = GetEventName(usInfo.EventType);
		}
		else
		{
			// ������������ �� �������.
			if (!userWorked)
			{
				usInfo.EventType = WorkEventType.TimeOff;
				usInfo.Status = GetEventName( WorkEventType.TimeOff );
			}
			else
			{
				// ������������ �������
				usInfo.BeginWork = begin;
				if (workEventClosed)
				{
					// ���� ������� �������� ������.
					usInfo.EventType = WorkEventType.TimeOff;
					usInfo.Status = GetEventName( WorkEventType.TimeOff );
				}
				else
				{
					// ���� ������� �������� �� ������.
					usInfo.EventType = WorkEventType.MainWork;
					usInfo.Status = GetEventName( WorkEventType.MainWork );
				}
				// ������ ������� ��������� ������.
				TimeSpan restTime = GetDayRate(user) - workDuration;
				if (restTime < TimeSpan.Zero)
					restTime = TimeSpan.Zero;
				usInfo.EndWork = now + restTime;
			}
		}

		return usInfo;
	}

	/// <summary>
	/// ���������� ��� ���� �������.
	/// </summary>
	/// <param name="eventType">��� �������.</param>
	/// <returns>��� ���� �������.</returns>
	private string GetEventName(WorkEventType eventType)
	{
		return Enum.GetName( typeof( WorkEventType ), eventType );
	}

	/// <summary>
	/// ���������� ������� ����� ������������ �� �������.
	/// </summary>
	/// <param name="user">������������.</param>
	/// <returns>������� ����� ������������ �� �������.</returns>
	private TimeSpan GetDayRate(MoscowUser user)
	{
		if (user == null)
			return TimeSpan.Zero;

		CalendarItem cItem = CalendarItem.GetCalendarItem(DateTime.Today);

		TimeSpan output;
		if (cItem == null)
		{ output = new TimeSpan(8, 30, 0); }
		else
		{ output = cItem.WorkTime;	}

		double seconds = output.TotalSeconds;
		seconds = seconds * user.PartTimeFactor;
		output = TimeSpan.FromSeconds(seconds);
		return output;
	}
	#endregion

	#region Web-������
	/// <summary>
	/// �����, ������������ ��� �����.
	/// </summary>
	/// <returns>��� �����.</returns>
	[WebMethod(Description = "Returns name of office.")]
	public string GetOfficeName()
	{
		string officeName = string.Empty;
		try
		{
		    officeName = ConfigurationManager.AppSettings["OfficeName"];
		}
		catch (Exception ex)
		{
		    Logger.Log.Error(Resources.Strings.GetOfficeNameError, ex);
		}

		return officeName;
	}

	/// <summary>
	/// �����, ������������ ������� �������������.
	/// </summary>
	/// <returns>������� �������������.</returns>
	[WebMethod(Description = "Returns statuses of users.")]
	[SoapHeader("AuthenticationHeader")]
	public XMLSerializableUserStatusInfo[] GetUserStatuses()
	{
		/*if ((AuthenticationHeader.UserName != ConfigurationManager.AppSettings["UserName"]) 
			|| (AuthenticationHeader.Password != ConfigurationManager.AppSettings["Password"]))
		{ throw new HttpException(401, Resources.Strings.AuthenticationFail); }
        */
		try
		{ return GetMoscowUsersStatuses(); }
		catch (Exception ex)
		{
			Logger.Log.Error(Resources.Strings.GetUsersStatusesError, ex);
			return null;
		}
	}
	#endregion
}
