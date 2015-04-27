using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using UlterSystems.PortalLib;
using System.Data.Common;

/// <summary>
/// �����, ����������� ������������.
/// </summary>
public class MoscowUser
{
	#region ����
	private string m_UserCode;
	private string m_FirstName;
	private string m_LastName;
	private float m_PartTimeFactor;
	#endregion

	#region ��������
	/// <summary>
	/// ��� ������������.
	/// </summary>
	public string USLName
	{
		get { return m_UserCode; }
		set { m_UserCode = value; }
	}

	/// <summary>
	/// ��� ������������.
	/// </summary>
	public string FirstName
	{
		get { return m_FirstName; }
		set { m_FirstName = value; }
	}

	/// <summary>
	/// ������� ������������.
	/// </summary>
	public string LastName
	{
		get { return m_LastName; }
		set { m_LastName = value; }
	}

	/// <summary>
	/// ���� �������, ������� ������������ ������ ����������.
	/// </summary>
	public float PartTimeFactor
	{
		get { return m_PartTimeFactor; }
		set { m_PartTimeFactor = value; }
	}

	/// <summary>
	/// ������ ��� ������������.
	/// </summary>
	public string FullName
	{
		get { return FirstName + " " + LastName; }
	}
	#endregion

	#region ������������
	/// <summary>
	/// �����������.
	/// </summary>
	public MoscowUser()
	{ }
	#endregion

	#region ������
	/// <summary>
	/// ���������� ������ ���������� �������������.
	/// </summary>
	/// <returns>������ ���������� �������������.</returns>
	public static MoscowUser[] GetLongServiceUsers()
	{
		List<MoscowUser> coll = new List<MoscowUser>();

		try
		{
			DbProviderFactory dbFactory = DbProviderFactories.GetFactory( ConfigurationManager.ConnectionStrings["DBConnStr"].ProviderName );
			using (DbConnection connection = dbFactory.CreateConnection())
			{
				connection.ConnectionString = ConfigurationManager.ConnectionStrings["DBConnStr"].ConnectionString;
				using (DbCommand command = dbFactory.CreateCommand())
				{
					command.Connection = connection;
					command.CommandText = "SELECT Name_ld, FirstName, LastName, PartTimeFactor FROM Collaborators WHERE (DateDischarged IS NULL)";

					connection.Open();

					using( DbDataReader reader = command.ExecuteReader() )
					{
						while( reader.Read() )
						{
							MoscowUser user = new MoscowUser();
							user.USLName = (string)reader["Name_ld"];
							user.FirstName = (string)reader["FirstName"];
							user.LastName = (string)reader["LastName"];
							user.PartTimeFactor = (float)reader["PartTimeFactor"];
							coll.Add(user);
						}
					}
				}
			}
		}
		catch (Exception ex)
		{
			Logger.Log.Error(Resources.Strings.GetUsersListError, ex);
		}

		return coll.ToArray();
	}

	public override string ToString()
	{
		return FullName;
	}
	#endregion
}