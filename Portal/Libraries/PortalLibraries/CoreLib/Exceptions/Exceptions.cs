using System;
using System.Configuration;
using System.Security;

using Core.Resources;

namespace Core.Exceptions
{
    /// <summary>
    /// ����������, ������� ������ ���������, ���� ��������� ������ �� ������ (� ��, ����� ��� ������ ����������)
    /// </summary>
    [Serializable]
    public class CoreObjectNotFoundException : CoreException
	{
		#region Constructors

		public CoreObjectNotFoundException() { }

		/// <summary>
		/// �����������, ������� ������������� ������� ��������� � ���, ��� ������ �� ������.
		/// </summary>
		/// <param name="objectName">��� �������.</param>
        public CoreObjectNotFoundException(string objectName)
            : base( ResourceManager.GetString("ObjectNotFoundException",objectName).ToString() ) { }

		/// <summary>
		/// ������ ���������� "������ �� ������" �� ����� �������.
		/// </summary>
		/// <param name="objectName">��� �������.</param>
		public CoreObjectNotFoundException( MLString objectName )
			: base( ResourceManager.GetString( "ObjectNotFoundException", objectName ).ToString() ) { }

		public CoreObjectNotFoundException(string message, Exception inner) : base(message, inner) { }

		#endregion

		#region Properties

		public override string Message
		{
			get
			{
				return base.MLMessage.ToString();
			}
		}
		#endregion
	}

    /// <summary>
    /// ����������, ������� ������ ���������, ���� � ���������� ������� ������� �������� ��������.
    /// </summary>
    [Serializable]
    public class CoreInvalidRequestParamException : CoreException
	{
		#region Constructors
		
		public CoreInvalidRequestParamException() { }
        
		/// <summary>
		/// �����������, ������� ������������� ������� ��������� � ���, ��� ������� �������� ��������.
		/// </summary>
		/// <param name="paramName">��� ���������.</param>
		public CoreInvalidRequestParamException(string paramName)
			: base(ResourceManager.GetString("InvalidRequestParamException", paramName).ToString()) { }
		public CoreInvalidRequestParamException( MLString message )
			: base( message ) { }
		public CoreInvalidRequestParamException(string message, Exception inner) : base(message, inner) { }

		#endregion

		#region Properties

		public override string Message
		{
			get
			{
				return base.MLMessage.ToString();
			}
		}
		#endregion
    }

	/// <summary>
	/// ����������, ������� ������ ���������, ���� � ������� �� ������� �������� ��������.
	/// </summary>
	[Serializable]
	public class DictionaryKeyNotFoundException : CoreException
	{
		#region Constructors

		public DictionaryKeyNotFoundException() { }
		public DictionaryKeyNotFoundException( string keyName, string dictName )
			: base( ResourceManager.GetString( "KeyNotFoundException", keyName, dictName ) ) { }
		public DictionaryKeyNotFoundException( MLString message )
			: base( message ) { }
		public DictionaryKeyNotFoundException( string message, Exception inner ) : base( message, inner ) { }

		#endregion

		#region Properties

		public override string Message
		{
			get
			{
				return base.MLMessage.ToString();
			}
		}
		#endregion
	}

	/// <summary>
	/// ���������� ���������, ����� ������������ ������� ������������� ����������, �� ���������� ��� 
	/// �������������.
	/// </summary>
	[Serializable]
	public class DictionaryCantBeImportedException : CoreException
	{
		public DictionaryCantBeImportedException( MLString dictName )
			: base( ResourceManager.GetString( "DictionaryCantBeImportedException", dictName ) )
		{
		}
	}

	/// <summary>
	/// ����������, ������� ���������, ���� � ������� ��� ���������� ������� � ����� ������.
	/// </summary>
	[Serializable]
	public class DictionaryKeyAlreadyExistsException : CoreException
	{
		public DictionaryKeyAlreadyExistsException() : base() { }

		/// <summary>
		/// �����������, ������ ���������� "���� ��� ����������" �� ����� � ����� �������.
		/// </summary>
		/// <param name="keyName">����.</param>
		/// <param name="dictName">�������� �������.</param>
		public DictionaryKeyAlreadyExistsException(MLString keyName, MLString dictName)
			: base( ResourceManager.GetString( "KeyAlreadyExistsException", keyName, dictName ) ) { }

		/// <summary>
		/// �����������, ������ ���������� "���� ��� ����������" �� ����� � ����� �������.
		/// </summary>
		/// <param name="keyName">����.</param>
		/// <param name="dictName">�������� �������.</param>
		public DictionaryKeyAlreadyExistsException( string keyName, string dictName )
			: base( ResourceManager.GetString( "KeyAlreadyExistsException", keyName, dictName ) ) { }
	}

	/// <summary>
	/// ����������, ������� ���������, ���� �� ������� ����� ������� ������ �����������
	/// � ��������� �������.
	/// </summary>
	[Serializable]
	public class DictionaryLinkNotFoundException : CoreException
	{
		public DictionaryLinkNotFoundException() : base() { }

		public DictionaryLinkNotFoundException( MLString linkDictName, string keys, MLString referenceDictName, string referenceKeys )
			: base( ResourceManager.GetString( "LinkNotFoundException", linkDictName, new MLString( keys ),
			referenceDictName, new MLString( referenceKeys ) ) )
		{
		}
	}

    /// <summary>
    /// ����������, ������� ������ ���������, ���� ...
    /// </summary>
    [Serializable]
    public class CoreArgumentException : ArgumentException, ICoreException
    {
        #region Private Members

        private MLString m_MLMessage = MLString.Empty;

        #endregion

        #region Constructors

        public CoreArgumentException(string message)
            : base(message)
        {
            m_MLMessage = new MLString(message, message);
        }

        public CoreArgumentException(string message, Exception innerException)
            : base(message, innerException)
        {
            m_MLMessage = new MLString(message, message);
        }

        public CoreArgumentException(MLString mlMessage)
			: base( mlMessage.ToString() )
        {
            m_MLMessage = mlMessage;
        }

        public CoreArgumentException(MLString mlMessage, Exception innerException)
			: base( mlMessage.ToString(), innerException )
        {
            m_MLMessage = mlMessage;
        }

        #endregion

        #region Interface

        /// <summary>
        /// �������������� ���������.
        /// </summary>
        public MLString MLMessage
        {
            get
            {
                return m_MLMessage;
            }
            set
            {
                m_MLMessage = value;
            }
        }

        #endregion
    }

    /// <summary>
    /// ����������, ������� ������ ���������, ���� ...
    /// </summary>
    [Serializable]
    public class CoreInvalidDataException : SystemException, ICoreException
    {
        #region Private Members

        private MLString m_MLMessage = MLString.Empty;

        #endregion

        #region Constructors

        public CoreInvalidDataException(string message)
            : base(message)
        {
            m_MLMessage = new MLString(message, message);
        }

        public CoreInvalidDataException(string message, Exception innerException)
            : base(message, innerException)
        {
            m_MLMessage = new MLString(message, message);
        }

        public CoreInvalidDataException(MLString mlMessage)
			: base( mlMessage.ToString() )
        {
            m_MLMessage = mlMessage;
        }

        public CoreInvalidDataException(MLString mlMessage, Exception innerException)
			: base( mlMessage.ToString(), innerException )
        {
            m_MLMessage = mlMessage;
        }

        #endregion

        #region Interface

        /// <summary>
        /// �������������� ���������.
        /// </summary>
        public MLString MLMessage
        {
            get
            {
                return m_MLMessage;
            }
            set
            {
                m_MLMessage = value;
            }
        }

        #endregion
    }

    /// <summary>
    /// ����������, ������� ������ ���������, ���� ...
    /// </summary>
    [Serializable]
    public class CoreInvalidOperationException : InvalidOperationException, ICoreException
    {
        #region Private Members

        private MLString m_MLMessage = MLString.Empty;

        #endregion

        #region Constructors

        public CoreInvalidOperationException(string message)
            : base(message)
        {
            m_MLMessage = new MLString(message, message);
        }

        public CoreInvalidOperationException(string message, Exception innerException)
            : base(message, innerException)
        {
            m_MLMessage = new MLString(message, message);
        }

        public CoreInvalidOperationException(MLString mlMessage)
			: base( mlMessage.ToString() )
        {
            m_MLMessage = mlMessage;
        }

        public CoreInvalidOperationException(MLString mlMessage, Exception innerException)
			: base( mlMessage.ToString(), innerException )
        {
            m_MLMessage = mlMessage;
        }

        #endregion

        #region Interface

        /// <summary>
        /// �������������� ���������.
        /// </summary>
        public MLString MLMessage
        {
            get
            {
                return m_MLMessage;
            }
            set
            {
                m_MLMessage = value;
            }
        }

        #endregion
    }

    /// <summary>
    /// ����������, ������� ������ ���������, ���� ...
    /// </summary>
    [Serializable]
    public class CoreApplicationException : ApplicationException, ICoreException
    {
        #region Private Members

        private MLString m_MLMessage = MLString.Empty;

        #endregion

        #region Constructors

        public CoreApplicationException(string message)
            : base(message)
        {
            m_MLMessage = new MLString(message, message);
        }

        public CoreApplicationException(string message, Exception innerException)
            : base(message, innerException)
        {
            m_MLMessage = new MLString(message, message);
        }

        public CoreApplicationException(MLString mlMessage)
			: base( mlMessage.ToString() )
        {
            m_MLMessage = mlMessage;
        }

        public CoreApplicationException(MLString mlMessage, Exception innerException)
			: base( mlMessage.ToString(), innerException )
        {
            m_MLMessage = mlMessage;
        }

        #endregion

        #region Interface

        /// <summary>
        /// �������������� ���������.
        /// </summary>
        public MLString MLMessage
        {
            get
            {
                return m_MLMessage;
            }
            set
            {
                m_MLMessage = value;
            }
        }

        #endregion
    }

    /// <summary>
    /// ����������, ������� ������ ���������, ���� ...
    /// </summary>
    [Serializable]
    public class CoreNullReferenceException : NullReferenceException, ICoreException
    {
        #region Private Members

        private MLString m_MLMessage = MLString.Empty;

        #endregion

        #region Constructors

        public CoreNullReferenceException(string message)
            : base(message)
        {
            m_MLMessage = new MLString(message, message);
        }

        public CoreNullReferenceException(string message, Exception innerException)
            : base(message, innerException)
        {
            m_MLMessage = new MLString(message, message);
        }

        public CoreNullReferenceException(MLString mlMessage)
			: base( mlMessage.ToString() )
        {
            m_MLMessage = mlMessage;
        }

        public CoreNullReferenceException(MLString mlMessage, Exception innerException)
			: base( mlMessage.ToString(), innerException )
        {
            m_MLMessage = mlMessage;
        }

        #endregion

        #region Interface

        /// <summary>
        /// �������������� ���������.
        /// </summary>
        public MLString MLMessage
        {
            get
            {
                return m_MLMessage;
            }
            set
            {
                m_MLMessage = value;
            }
        }

        #endregion
    }

    /// <summary>
    /// ����������, ������� ������ ���������, ���� ...
    /// </summary>
    [Serializable]
    public class CoreMissingMethodException : MissingMethodException, ICoreException
    {
        #region Private Members

        private MLString m_MLMessage = MLString.Empty;

        #endregion

        #region Constructors

        public CoreMissingMethodException(string message)
            : base(message)
        {
            m_MLMessage = new MLString(message, message);
        }

        public CoreMissingMethodException(string message, Exception innerException)
            : base(message, innerException)
        {
            m_MLMessage = new MLString(message, message);
        }

        public CoreMissingMethodException(MLString mlMessage)
			: base( mlMessage.ToString() )
        {
            m_MLMessage = mlMessage;
        }

        public CoreMissingMethodException(MLString mlMessage, Exception innerException)
			: base( mlMessage.ToString(), innerException )
        {
            m_MLMessage = mlMessage;
        }

        #endregion

        #region Interface

        /// <summary>
        /// �������������� ���������.
        /// </summary>
        public MLString MLMessage
        {
            get
            {
                return m_MLMessage;
            }
            set
            {
                m_MLMessage = value;
            }
        }

        #endregion
    }

    /// <summary>
    /// ����������, ������� ������ ���������, ���� ...
    /// </summary>
    [Serializable]
    public class CoreSettingsPropertyNotFoundException : SettingsPropertyNotFoundException, ICoreException
    {
        #region Private Members

        private MLString m_MLMessage = MLString.Empty;

        #endregion

        #region Constructors

        public CoreSettingsPropertyNotFoundException(string message)
            : base(message)
        {
            m_MLMessage = new MLString(message, message);
        }

        public CoreSettingsPropertyNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
            m_MLMessage = new MLString(message, message);
        }

        public CoreSettingsPropertyNotFoundException(MLString mlMessage)
			: base( mlMessage.ToString() )
        {
            m_MLMessage = mlMessage;
        }

        public CoreSettingsPropertyNotFoundException(MLString mlMessage, Exception innerException)
			: base( mlMessage.ToString(), innerException )
        {
            m_MLMessage = mlMessage;
        }

        #endregion

        #region Interface

        /// <summary>
        /// �������������� ���������.
        /// </summary>
        public MLString MLMessage
        {
            get
            {
                return m_MLMessage;
            }
            set
            {
                m_MLMessage = value;
            }
        }

        #endregion
    }

    /// <summary>
    /// ����������, ������� ������ ���������, ���� ...
    /// </summary>
    [Serializable]
    public class CoreArgumentOutOfRangeException : ArgumentOutOfRangeException, ICoreException
    {
        #region Private Members

        private MLString m_MLMessage = MLString.Empty;

        #endregion

        #region Constructors

        public CoreArgumentOutOfRangeException(string message)
            : base(message)
        {
            m_MLMessage = new MLString(message, message);
        }

        public CoreArgumentOutOfRangeException(string message, Exception innerException)
            : base(message, innerException)
        {
            m_MLMessage = new MLString(message, message);
        }

        public CoreArgumentOutOfRangeException(MLString mlMessage)
			: base( mlMessage.ToString() )
        {
            m_MLMessage = mlMessage;
        }

        public CoreArgumentOutOfRangeException(MLString mlMessage, Exception innerException)
			: base( mlMessage.ToString(), innerException )
        {
            m_MLMessage = mlMessage;
        }

        #endregion

        #region Interface

        /// <summary>
        /// �������������� ���������.
        /// </summary>
        public MLString MLMessage
        {
            get
            {
                return m_MLMessage;
            }
            set
            {
                m_MLMessage = value;
            }
        }

        #endregion
    }

    /// <summary>
    /// ����������, ������� ������ ���������, ���� ...
    /// </summary>
    [Serializable]
    public class CoreNotSupportedException : NotSupportedException, ICoreException
    {
        #region Private Members

        private MLString m_MLMessage = MLString.Empty;

        #endregion

        #region Constructors

        public CoreNotSupportedException()
            : base()
        {
            m_MLMessage = Resources.ResourceManager.GetString("NotSupportedException");
        }

        public CoreNotSupportedException(string message)
            : base(message)
        {
            m_MLMessage = new MLString(message, message);
        }

        public CoreNotSupportedException(string message, Exception innerException)
            : base(message, innerException)
        {
            m_MLMessage = new MLString(message, message);
        }

        public CoreNotSupportedException(MLString mlMessage)
			: base( mlMessage.ToString() )
        {
            m_MLMessage = mlMessage;
        }

        public CoreNotSupportedException(MLString mlMessage, Exception innerException)
			: base( mlMessage.ToString(), innerException )
        {
            m_MLMessage = mlMessage;
        }

        #endregion

        #region Interface

        /// <summary>
        /// �������������� ���������.
        /// </summary>
        public MLString MLMessage
        {
            get
            {
                return m_MLMessage;
            }
            set
            {
                m_MLMessage = value;
            }
        }

        #endregion
    }

    /// <summary>
    /// ����������, ������� ������ ���������, ���� ...
    /// </summary>
    [Serializable]
    public class CoreArgumentNullException : ArgumentNullException, ICoreException
    {
        #region Private Members

        private MLString m_MLMessage = MLString.Empty;

        #endregion

        #region Constructors

        public CoreArgumentNullException()
            : base()
        {
            m_MLMessage = Resources.ResourceManager.GetString("ArgumentNullException");
        }

        public CoreArgumentNullException(string paramName)
            : base(paramName)
        {
            m_MLMessage = Resources.ResourceManager.GetString("ArgumentNullWithParamException", paramName);
        }

        public CoreArgumentNullException(string message, Exception innerException)
            : base(message, innerException)
        {
            m_MLMessage = new MLString(message,message);
        }

        public CoreArgumentNullException(MLString mlMessage, Exception innerException)
			: base( mlMessage.ToString(), innerException )
        {
            m_MLMessage = mlMessage;
        }

        public CoreArgumentNullException(string paramName, string message)
            : base(paramName, message)
        {
            m_MLMessage = new MLString(message, message);
        }

        public CoreArgumentNullException(string paramName, MLString mlMessage)
			: base( paramName, mlMessage.ToString() )
        {
            m_MLMessage = mlMessage;
        }

        #endregion

        #region Interface

        /// <summary>
        /// �������������� ���������.
        /// </summary>
        public MLString MLMessage
        {
            get
            {
                return m_MLMessage;
            }
            set
            {
                m_MLMessage = value;
            }
        }

        #endregion
    }

    /// <summary>
    /// ����������, ������� ������ ���������, ���� ...
    /// </summary>
    [Serializable]
    public class CoreSecurityException : SecurityException, ICoreException
    {
        #region Private Members

        private MLString m_MLMessage = MLString.Empty;

        #endregion

        #region Constructors

        public CoreSecurityException(string message)
            : base(message)
        {
            m_MLMessage = new MLString(message, message);
        }

        public CoreSecurityException(string message, Exception innerException)
            : base(message, innerException)
        {
            m_MLMessage = new MLString(message, message);
        }

        public CoreSecurityException(MLString mlMessage)
			: base( mlMessage.ToString() )
        {
            m_MLMessage = mlMessage;
        }

        public CoreSecurityException(MLString mlMessage, Exception innerException)
			: base( mlMessage.ToString(), innerException )
        {
            m_MLMessage = mlMessage;
        }

        #endregion

        #region Interface

        /// <summary>
        /// �������������� ���������.
        /// </summary>
        public MLString MLMessage
        {
            get
            {
                return m_MLMessage;
            }
            set
            {
                m_MLMessage = value;
            }
        }

        #endregion
    }

	/// <summary>
	/// ����������, ������� ������ ���������, ���� �������� ������� ������ �������,
	/// �� ������� ���������� ������ �� ������ ��������.
	/// </summary>
	[Serializable]
	public class DictionaryElementCouldNotBeDeletedException : CoreException
	{
		#region Constructors

		public DictionaryElementCouldNotBeDeletedException( string message )
			: base( message )
		{
		}

		public DictionaryElementCouldNotBeDeletedException( MLString message )
			: base( message )
		{
		}

		public DictionaryElementCouldNotBeDeletedException( MLString dictName, string kyes, 
			MLString refDictName, string refKeys )
			: base ( ResourceManager.GetString( "DictionaryElementCouldNotBeDeletedException", 
							dictName, 
							new MLString( kyes ), 
							refDictName, 
							new MLString( refKeys ) ) )
		{
		}

		#endregion
	}

	/// <summary>
	/// ����������, ������� ������ ���������, ���� �������� ������� ������ �������,
	/// �� ������� ���������� ������ �� ������ ��������.
	/// </summary>
	[Serializable]
	public class DictionaryElementCouldNotBeClosedException : CoreException
	{
		#region Constructors

		public DictionaryElementCouldNotBeClosedException( string message )
			: base( message )
		{
		}

		public DictionaryElementCouldNotBeClosedException( MLString message )
			: base( message )
		{
		}

		public DictionaryElementCouldNotBeClosedException( MLString dictName, string kyes,
			MLString refDictName, string refKeys )
			: base( ResourceManager.GetString( "DictionaryElementCouldNotBeClosedException",
							dictName,
							new MLString( kyes ),
							refDictName,
							new MLString( refKeys ) ) )
		{
		}

		#endregion
	}
}
