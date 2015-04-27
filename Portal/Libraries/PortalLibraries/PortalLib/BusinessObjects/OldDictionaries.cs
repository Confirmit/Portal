using System.Data;
using System.Data.SqlClient;
using Core.DB;

namespace UlterSystems.PortalLib.BusinessObjects
{
	///<summary>
	/// ����� ����� ������������.
	///</summary>
	public class OldDictionaries
	{
		#region ����
		private DataSet _dsDictionaries = new DataSet(); //DataSet
		#endregion

		#region ��������
		/// <summary>
		/// ���������� ������ DataSet.
		/// </summary>
		/// <param name="index">�������� �����������.</param>
		/// <returns>������� �����������.</returns>
		public DataTable this[string index]
		{
			get { return _dsDictionaries.Tables[index]; }
		}
		#endregion

		#region �����������
		/// <summary>
		/// �����������.
		/// </summary>
		public OldDictionaries()
		{
			LoadDicFromBase("Projects");
			LoadDicFromBase("UptimeEventTypes");
			LoadDicFromBase("WorkCategories");
		}
		#endregion

		#region ������
		/// <summary>
		/// ������� ���� �������� ����������� �� ID.
		/// </summary>
		/// <param name="dicname">�������� �����������.</param>
		/// <param name="field">��� ����.</param>
		/// <param name="id">ID �������� �����������.</param>
		/// <returns>�������� ���� �������� ����������� � �������� ID.</returns>
		public object GetFieldByID(string dicname, string field, int id)
		{
			foreach (DataRow row in _dsDictionaries.Tables[dicname].Rows)
			{
				if ((int)row["ID"] == id)
					return (object)row[field];
			}
			return null;
		}
		/// <summary>
		/// �������� ����������� �� ����.
		/// </summary>
		/// <param name="dicname">�������� �����������.</param>
		/// <returns>����� ����������� �������.</returns>
		public int LoadDicFromBase(string dicname)
		{
			using (SqlConnection conn = (SqlConnection)ConnectionManager.GetConnection(ConnectionKind.Default))
			{
				//����������� ����������� �������� ������� � �������� ���������
				SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM " + dicname, conn);
				conn.Open();
				da.Fill(_dsDictionaries, dicname);
			}
			return _dsDictionaries.Tables[dicname].Rows.Count;
		}
		#endregion
	}
}
