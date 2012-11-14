using System;

using System.Collections.Generic;
using Core;
using Core.ORM.Attributes;

namespace UlterSystems.PortalLib.BusinessObjects
{
	/// <summary>
	/// Types of person attributes.
	/// </summary>
	public enum PersonAttributeTypes
	{
		DomainName,
		PublicPassword,
		USL_name,
		Photo
	}

	/// <summary>
	/// Class for work with attributes types.
	/// </summary> 
	[DBTable("PersonAttributeTypes")]
	public class PersonAttributeType : BasePlainObject
	{
		#region Fields

		private bool m_ShowUsesrs;
		private string m_AttributeName;

		#endregion

		#region Properties

		/// <summary>
		/// Name of attribute.
		/// </summary>
		[DBRead("AttributeName")]
		public string AttributeName
		{
			get { return m_AttributeName; }
			set { m_AttributeName = value; }
		}

		/// <summary>
		/// Show to users.
		/// </summary>
		[DBRead("IsShownToUsers")]
		public bool ShowToUsers
		{
			get { return m_ShowUsesrs; }
			set { m_ShowUsesrs = value; }
		}

		#endregion

		#region Methods

		/// <summary>
		/// Get PersonAttributeType of specific name.
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public static PersonAttributeType GetAttributeType(PersonAttributeTypes type)
		{
			return GetAttributeType(type.ToString());
		}

		/// <summary>
		/// Get PersonAttributeType of specific name.
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public static PersonAttributeType GetAttributeType(string type)
		{
			try
			{
				BaseObjectCollection<PersonAttributeType> coll =
					(BaseObjectCollection<PersonAttributeType>) GetObjects(typeof (PersonAttributeType)
																		   , "AttributeName", type);
				if (coll == null || coll.Count == 0)
					return null;

				return coll[0];
			}
			catch (Exception ex)
			{
				Logger.Log.Error(ex.Message, ex);
				return null;
			}
		}

		/// <summary>
		/// Returns all types of attributes.
		/// </summary>
		/// <returns>All attributes of person.</returns>
		public static PersonAttributeType[] GetAllTypesAttributes()
		{
			try
			{
				BaseObjectCollection<PersonAttributeType> coll =
					(BaseObjectCollection<PersonAttributeType>) GetObjects(typeof (PersonAttributeType));
				if (coll == null)
					return new PersonAttributeType[0];

				return coll.ToArray();
			}
			catch (Exception ex)
			{
				Logger.Log.Error(ex.Message, ex);
				return new PersonAttributeType[0];
			}
		}

		#endregion
	}

	/// <summary>
	/// Class for work with attributes of person.
	/// </summary>
	public class PersonAttributes
	{
		#region Methods

		/// <summary>
		/// Returns attributes of person.
		/// </summary>
		/// <param name="param">Params to select.</param>
		/// <returns>Attributes of person.</returns>
		private static IList<PersonAttribute> getAttributesByParams(params object[] param)
		{
			try
			{
				BaseObjectCollection<PersonAttribute> coll = (BaseObjectCollection<PersonAttribute>)BasePlainObject.GetObjects(typeof(PersonAttribute), param);
				if (coll == null)
					return new List<PersonAttribute>();

				return coll;
			}
			catch (Exception ex)
			{
				Logger.Log.Error(ex.Message, ex);
				return new List<PersonAttribute>();
			}
		}

		/// <summary>
		/// Returns all attributes of person.
		/// </summary>
		/// <param name="personID">ID of person.</param>
		/// <returns>All attributes of person.</returns>
		public static IList<PersonAttribute> GetAllPersonAttributes(int personID)
		{
			return getAttributesByParams("PersonID", personID);
		}

		/// <summary>
		/// Returns all person attributes with given keyword.
		/// </summary>
		/// <param name="personID">ID of person.</param>
		/// <param name="keyword">Keyword.</param>
		/// <returns>All person attributes with given keyword.</returns>
		public static IList<PersonAttribute> GetPersonAttributesByKeyword(int personID, string keyword)
		{
			if (string.IsNullOrEmpty(keyword))
				return new List<PersonAttribute>();
			return GetPersonAttributesByKeyword(personID, keyword, null);
		}

		/// <summary>
		/// Returns all person attributes with given keyword.
		/// </summary>
		/// <param name="personID">ID of person.</param>
		/// <param name="attr">Attribute.</param>
		/// <returns>All person attributes with given keyword.</returns>
		public static IList<PersonAttribute> GetPersonAttributesByKeyword(int personID, PersonAttributeType attr)
		{
			if (string.IsNullOrEmpty(attr.AttributeName))
				return new List<PersonAttribute>();
			return GetPersonAttributesByKeyword(personID, attr, null);
		}

		/// <summary>
		/// Returns all person attributes with given keyword.
		/// </summary>
		/// <param name="personID">ID of person.</param>
		/// <param name="keyword">Keyword.</param>
		/// <param name="param">Additiona params.</param>
		/// <returns>Filtered person attributes with given keyword.</returns>
		public static IList<PersonAttribute> GetPersonAttributesByKeyword(int personID, string keyword, params string[] param)
		{
			if (string.IsNullOrEmpty(keyword))
				return new List<PersonAttribute>();

			PersonAttributeType attr = new PersonAttributeType();
			attr.LoadByReference("AttributeName", keyword);
			return GetPersonAttributesByKeyword(personID, attr, param);
		}

		/// <summary>
		/// Returns all person attributes with given keyword.
		/// </summary>
		/// <param name="personID">ID of person.</param>
		/// <param name="attr">Attribute.</param>
		/// <param name="param">Additiona params.</param>>
		/// <returns>Filtered person attributes with given keyword.</returns>
		public static IList<PersonAttribute> GetPersonAttributesByKeyword(int personID, PersonAttributeType attr, params string[] param)
		{
			if (string.IsNullOrEmpty(attr.AttributeName))
				return new List<PersonAttribute>();
			if (param != null)
				return getAttributesByParams("PersonID", personID, "AttributeID", attr.ID, param);

			return getAttributesByParams("PersonID", personID, "AttributeID", attr.ID);
		}

		/// <summary>
		/// Returns all attributes with given keyword.
		/// </summary>
		/// <param name="keyword">Keyword.</param>
		/// <returns>All attributes with given keyword.</returns>
		public static IList<PersonAttribute> GetPersonAttributesByKeyword(string keyword)
		{
			if (string.IsNullOrEmpty(keyword))
				return new List<PersonAttribute>();

			PersonAttributeType attr = new PersonAttributeType();
			attr.LoadByReference("AttributeName", keyword);
			return GetPersonAttributesByKeyword(attr);
		}

		/// <summary>
		/// Returns all attributes with given keyword.
		/// </summary>
		/// <param name="attr">Attribute.</param>
		/// <returns>All attributes with given keyword.</returns>
		public static IList<PersonAttribute> GetPersonAttributesByKeyword(PersonAttributeType attr)
		{
			if (string.IsNullOrEmpty(attr.AttributeName))
				return new List<PersonAttribute>();
			return getAttributesByParams("AttributeID", attr.ID);
		}

		#endregion
	}

	/// <summary>
	/// Class of single attribute of person.
	/// </summary>
	[DBTable("PersonAttributes")]
	public class PersonAttribute : BasePlainObject
	{
		#region Fields

		private int m_PersonID;
		private DateTime m_InsertionDate = DateTime.Now;
		private int m_AttributeID;
		private string m_ValueType;
		private string m_StringValue;
		private int? m_IntValue;
		private double? m_DoubleValue;
		private DateTime? m_DateTimeValue;
		private bool? m_BoolValue;
		private byte[] m_BinaryValue;

		#endregion

		#region Properties

		/// <summary>
		/// ID of person.
		/// </summary>
		[DBRead("PersonID")]
		public int PersonID
		{
			get { return m_PersonID; }
			set { m_PersonID = value; }
		}

		/// <summary>
		/// Date of attribute creation.
		/// </summary>
		[DBRead("InsertionDate")]
		public DateTime InsertionDate
		{
			get { return m_InsertionDate; }
			set { m_InsertionDate = value; }
		}

		/// <summary>
		/// Keyword of attribute.
		/// </summary>
		[DBRead("AttributeID")]
		public int AttributeID
		{
			get { return m_AttributeID; }
			set { m_AttributeID = value; }
		}

		/// <summary>
		/// Type of attribute.
		/// </summary>
		[DBRead("ValueType")]
		public string ValueType
		{
			get { return m_ValueType; }
			set { m_ValueType = value; }
		}

		public Type Type
		{
			get
			{
				if (string.IsNullOrEmpty(ValueType))
					return null;

				return Type.GetType(ValueType);
			}
		}

		/// <summary>
		/// String value of attribute.
		/// </summary>
		[DBRead("StringField")]
		[DBNullable]
		public string StringField
		{
			get { return m_StringValue; }
			set { m_StringValue = value; }
		}

		/// <summary>
		/// Integer value of attribute.
		/// </summary>
		[DBRead("IntegerField")]
		[DBNullable]
		public int? IntegerField
		{
			get { return m_IntValue; }
			set { m_IntValue = value; }
		}

		/// <summary>
		/// Float value of attribute.
		/// </summary>
		[DBRead("DoubleField")]
		[DBNullable]
		public double? DoubleField
		{
			get { return m_DoubleValue; }
			set { m_DoubleValue = value; }
		}

		/// <summary>
		/// Boolean value of attribute.
		/// </summary>
		[DBRead("BooleanField")]
		[DBNullable]
		public bool? BooleanField
		{
			get { return m_BoolValue; }
			set { m_BoolValue = value; }
		}

		/// <summary>
		/// Date'n'time value of attribute.
		/// </summary>
		[DBRead("DateTimeField")]
		[DBNullable]
		public DateTime? DateTimeField
		{
			get { return m_DateTimeValue; }
			set { m_DateTimeValue = value; }
		}

		/// <summary>
		/// Binary value of attribute.
		/// </summary>
		[DBRead("BinaryField")]
		[DBNullable]
		public byte[] BinaryField
		{
			get { return m_BinaryValue; }
			set { m_BinaryValue = value; }
		}

		/// <summary>
		/// Value of attribute.
		/// </summary>
		public object Value
		{
			get
			{
				if (string.IsNullOrEmpty(ValueType))
					return null;

				try
				{
					Type valueType = Type.GetType(ValueType, false, true);
					if (valueType == null)
						return null;

					if (valueType.Equals(typeof(string)))
						return m_StringValue;

					if (valueType.Equals(typeof(int)))
						return m_IntValue;

					if (valueType.Equals(typeof(double)))
						return m_DoubleValue;

					if (valueType.Equals(typeof(bool)))
						return m_BoolValue;

					if (valueType.Equals(typeof(DateTime)))
						return m_DateTimeValue;

					if (valueType.Equals(typeof(byte[])))
						return m_BinaryValue;

					return null;
				}
				catch (Exception ex)
				{
					Logger.Log.Error(ex.Message, ex);
					return null;
				}
			}
		}

		#endregion
	}
}
