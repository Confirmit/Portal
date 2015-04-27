using System;
using System.Collections.Generic;
using System.Text;

namespace Core
{
	/// <summary>
	/// Тип сообщения
	/// </summary>
	public enum MessageType
	{
		Text = 0,
		Error = 1
	}

	/// <summary>
	/// Класс, представляющий сообшение.
	/// </summary>
	public class Message
	{
		private string mText;
		private MessageType mType;

		/// <summary>
		/// Текст сообщения
		/// </summary>
		public string Text
		{
			get { return mText; }
			set { mText = value; }
		}

		/// <summary>
		/// Тип сообщения
		/// </summary>
		public MessageType MessageType
		{
			get { return mType; }
			set { mType = value; }
		}

		/// <summary>
		/// Создает сообщение 
		/// </summary>
		/// <param name="text">текст сообщения</param>
		/// <param name="type">тип сообщения</param>
		public Message( string text, MessageType type )
		{
			mText = text;
			mType = type;
		}

		/// <summary>
		/// Создает сообщение 
		/// </summary>
		/// <param name="text">текст сообщения</param>
		public Message( string text )
			: this( text, MessageType.Text )
		{
		}

	}
}
