using System;

namespace Core.Exceptions
{
    /// <summary>
    /// Базовый класс для всех исключений библиотеки. Поддерживает многоязычные описания исключений
    /// </summary>
    [Serializable]
    public class CoreException : Exception, ICoreException
    {
        private MLString m_MLMessage = MLString.Empty;

        public CoreException() { }

        public CoreException( string message )
            : base( message )
        {
            m_MLMessage = new MLString( message, message );
        }

        public CoreException( string message, Exception innerException )
            : base( message, innerException )
        {
            m_MLMessage = new MLString( message, message );
        }

        public CoreException( string messageRU, string messageEN )
            : base( messageEN )
        {
            m_MLMessage = new MLString( messageRU, messageEN );
        }

        public CoreException( string messageRU, string messageEN, Exception innerException )
            : base( messageEN, innerException )
        {
            m_MLMessage = new MLString( messageRU, messageEN );
        }

        public CoreException( MLString message )
            : base( message.EnglishValue )
        {
            m_MLMessage = message;
        }

        public CoreException( MLString message, Exception innerException )
			: base( message.EnglishValue, innerException )
        {
            m_MLMessage = message;
        }

        public MLString MLMessage
        {
            get { return m_MLMessage; }
            set { m_MLMessage = value; }
        }

		public override string Message
		{
			get
			{
				return MLMessage.ToString();
			}
		}
    }
}
