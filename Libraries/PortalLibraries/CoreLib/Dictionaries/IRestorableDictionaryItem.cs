using System;

namespace Core.Dictionaries
{
	/// <summary>
	/// ��������� ��� ������� ��������(BaseObject), ������� ������� � ��������������� ���� � ������
	/// </summary>
	public interface IRestorableDictionaryItem
	{
		/// <summary>
		/// ������ �� ������
		/// </summary>
		bool IsRemoved { get; set;  }

		/// <summary>
		/// ��������������� ������: ������������� ���� IsRemoved = false.
		/// </summary>
		/// <returns></returns>
		void Restore();

		/// <summary>
		/// ��������� ������: ������������� ���� IsRemoved = true;
		/// </summary>
		/// <returns></returns>
		void Close();
	}
}
