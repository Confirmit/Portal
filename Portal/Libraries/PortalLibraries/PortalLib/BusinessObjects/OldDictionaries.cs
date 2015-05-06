using System.Data;
using System.Data.SqlClient;
using Core.DB;

namespace UlterSystems.PortalLib.BusinessObjects
{
	///<summary>
	/// Класс общих справочников.
	///</summary>
	public class OldDictionaries
	{
		#region Поля
		private DataSet _dsDictionaries = new DataSet(); //DataSet
		#endregion

		#region Свойства
		/// <summary>
		/// Индексатор таблиц DataSet.
		/// </summary>
		/// <param name="index">Название справочника.</param>
		/// <returns>Таблица справочника.</returns>
		public DataTable this[string index]
		{
			get { return _dsDictionaries.Tables[index]; }
		}
		#endregion

		#region Конструктор
		/// <summary>
		/// Конструктор.
		/// </summary>
		public OldDictionaries()
		{
			LoadDicFromBase("Projects");
			LoadDicFromBase("UptimeEventTypes");
			LoadDicFromBase("WorkCategories");
		}
		#endregion

		#region Методы
		/// <summary>
		/// Выборка поля элемента справочника по ID.
		/// </summary>
		/// <param name="dicname">Название справочника.</param>
		/// <param name="field">Имя поля.</param>
		/// <param name="id">ID элемента справочника.</param>
		/// <returns>Заданное поле элемента справочника с заданным ID.</returns>
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
		/// Загрузка справочника из базы.
		/// </summary>
		/// <param name="dicname">Название справочника.</param>
		/// <returns>Число загруженных записей.</returns>
		public int LoadDicFromBase(string dicname)
		{
			using (SqlConnection conn = (SqlConnection)ConnectionManager.GetConnection(ConnectionKind.Default))
			{
				//рассмотреть возможность переноса запроса в хранимую процедуру
				SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM " + dicname, conn);
				conn.Open();
				da.Fill(_dsDictionaries, dicname);
			}
			return _dsDictionaries.Tables[dicname].Rows.Count;
		}
		#endregion
	}
}
