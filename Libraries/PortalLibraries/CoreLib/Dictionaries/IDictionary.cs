using System;
using System.Data;
using System.Collections.Generic;

using Core;
using Core.Import;

namespace Core.Dictionaries
{
	/// <summary>
	/// ����������.
	/// </summary>
	public interface IDictionary
	{
		/// <summary>
		/// ��� �����������.
		/// </summary>
		MLString DictionaryName { get; }

		/// <summary>
		/// �������������� �������� �����������.
		/// </summary>
		MLString DictionaryTitle { get; }

		/// <summary>
		/// ������ �������� ����� �������.
		/// </summary>
		string[] Keys { get; }

		/// <summary>
		/// ������������ ���������� �� ����������� � �������.
		/// </summary>
		/// <param name="dictManager">�������� ������������.</param>
		/// <returns></returns>
		DataTable Export( IDictionaryManager dictManager );

		/// <summary>
		/// ����������� ���������� �� ������� � ����������.
		/// </summary>
		/// <param name="table">������� � �������.</param>
		/// <param name="context">�������� ������� �����������.</param>
		/// <returns>��������� �������.</returns>
		ImportResult Import( DataTable table, DictionaryImportContext context );

		/// <summary>
		/// ������ ������ ������� �����������.
		/// </summary>
		/// <returns>������� �����������.</returns>
		object CreateDictionaryItem();
	}

	public class DictionaryCollection : BaseBindingCollection<IDictionary>
	{
	}
}
