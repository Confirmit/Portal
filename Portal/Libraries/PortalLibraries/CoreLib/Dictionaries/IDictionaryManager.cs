using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Dictionaries
{
	/// <summary>
	/// ��������� ���������� �������������.
	/// </summary>
	public interface IDictionaryManager
	{
		/// <summary>
		/// ���������� �������� ��������� �����������.
		/// </summary>
		/// <param name="dict">����������.</param>
		/// <param name="args">��������� ���������.</param>
		/// <returns>PagingResult</returns>
		PagingResult GetItemsPage(IDictionary dict, PagingArgs args);

		/// <summary>
		/// ���������� ������ �������� ����������� �� ������ �� ������������ �����.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		MLString GetFullName(string name);

		/// <summary>
		/// ������ ���������� �� ��������.
		/// </summary>
		/// <param name="name">�������� �����������.</param>
		/// <returns>����������, ���� ������� ��� �������, ����� null.</returns>
		IDictionary CreateDictionary(string name);

		/// <summary>
		/// ������ ���������� �� ��������.
		/// </summary>
		/// <param name="name">�������� �����������.</param>
		/// <returns>����������, ���� ������� ��� �������, ����� null.</returns>
		IDictionary CreateDictionary(MLString name);

		/// <summary>
		/// �������� ���������� ������������� ��� ���.
		/// </summary>
		/// <param name="dictionary">����������.</param>
		/// <returns>True, ���� ���������� �����������, ����� False.</returns>
		bool IsImportable(IDictionary dictionary);

		/// <summary>
		/// �������� ���������� �������������� ��� ���.
		/// </summary>
		/// <param name="dictionary">����������.</param>
		/// <returns>True, ���� ���������� ������������, ����� False.</returns>
		bool IsExportable(IDictionary dictionary);

		/// <summary>
		/// ���������� ��������� ��������, ����������� �� ������.
		/// </summary>
		/// <param name="type">�������.</param>
		/// <returns>���������.</returns>
		DictionaryCollection GetReferenceDictionaries(IDictionary dictionary);
	}
}
