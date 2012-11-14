using System;
using System.Collections.Generic;
using System.Text;

namespace Core
{
	/// <summary>
	/// ��� ���������
	/// </summary>
	public enum MessageType
	{
		Text = 0,
		Error = 1
	}

	/// <summary>
	/// �����, �������������� ���������.
	/// </summary>
	public class Message
	{
		private string mText;
		private MessageType mType;

		/// <summary>
		/// ����� ���������
		/// </summary>
		public string Text
		{
			get { return mText; }
			set { mText = value; }
		}

		/// <summary>
		/// ��� ���������
		/// </summary>
		public MessageType MessageType
		{
			get { return mType; }
			set { mType = value; }
		}

		/// <summary>
		/// ������� ��������� 
		/// </summary>
		/// <param name="text">����� ���������</param>
		/// <param name="type">��� ���������</param>
		public Message( string text, MessageType type )
		{
			mText = text;
			mType = type;
		}

		/// <summary>
		/// ������� ��������� 
		/// </summary>
		/// <param name="text">����� ���������</param>
		public Message( string text )
			: this( text, MessageType.Text )
		{
		}

	}
}
