using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Xml;

namespace Core
{
	/// <summary>
	/// Class for multilingual text.
	/// </summary>
	[Serializable]
	public class MLText
	{
		#region Fields
		/// <summary>
		/// Validator of culture identifiers.
		/// </summary>
		//protected static Regex m_CultureIDValidator = new Regex(@"^(\w\w)(-(\w\w))?$");
		protected static Regex m_CultureIDValidator = new Regex(@"^([a-zA-Z][a-zA-Z])(-([a-zA-Z][a-zA-Z]))?$");
		/// <summary>
		/// Map from culture identifier to text for this culture.
		/// </summary>
		private Dictionary<string, string> m_TextsMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
		/// <summary>
		/// Identifier of default culture.
		/// </summary>
		private static string m_DefaultCultureID = "en";
		/// <summary>
		/// Identifier of current culture.
		/// </summary>
		private static string m_CurrentCultureID = "en";
		/// <summary>
		/// Procedure for request for current culture ID.
		/// </summary>
		public static RequestCurrentCultureIDCallback RequestCurrentCultureID;
		/// <summary>
		/// Procedure for storing of current culture ID.
		/// </summary>
		public static PersistCurrentCultureIDCallback PersistCurrentCultureID;
		#endregion

		#region Delegates
		/// <summary>
		/// Delegate for request for current culture ID.
		/// </summary>
		/// <returns>Current culture ID.</returns>
		public delegate string RequestCurrentCultureIDCallback();
		/// <summary>
		/// Delegate for storing of current culture ID.
		/// </summary>
		/// <param name="currentCultureID">Identifier of current culture.</param>
		public delegate void PersistCurrentCultureIDCallback(string currentCultureID);
		#endregion

		#region Properties
		/// <summary>
		/// Identifier of default culture.
		/// </summary>
		public static string DefaultCultureID
		{
			get { return m_DefaultCultureID; }
			set
			{
				if (!m_CultureIDValidator.IsMatch(value))
					throw new Exception(Core.Properties.Resources.WrongCultureId);
				m_DefaultCultureID = value;
			}
		}

		/// <summary>
		/// Identifier of current culture.
		/// </summary>
		public static string CurrentCultureID
		{
			get
			{
				if (RequestCurrentCultureID != null)
				{
					string currentCultureID = RequestCurrentCultureID();
					if (!m_CultureIDValidator.IsMatch(currentCultureID))
						throw new Exception(Core.Properties.Resources.WrongCultureId);
					return currentCultureID;
				}
				else
				{
					return m_CurrentCultureID;
				}
			}
			set
			{
				if (!m_CultureIDValidator.IsMatch(value))
					throw new Exception(Core.Properties.Resources.WrongCultureId);

				if (PersistCurrentCultureID != null)
				{
					PersistCurrentCultureID(value);
				}
				else
				{
					m_CurrentCultureID = value;
				}
			}
		}

		/// <summary>
		/// Indexer.
		/// </summary>
		/// <param name="cultureID">Identifier of culture.</param>
		/// <returns>Text for given culture.</returns>
		public string this[string cultureID]
		{
			get { return GetText(cultureID); }
			set { AddText(cultureID, value); }
		}

		/// <summary>
		/// List of avaliable cultures.
		/// </summary>
		public string[] Cultures
		{
			get
			{
				Debug.Assert(m_TextsMap != null);
				string[] output = new string[m_TextsMap.Keys.Count];
				m_TextsMap.Keys.CopyTo(output, 0);
				return output;
			}
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Simple constructor.
		/// </summary>
		public MLText()
		{ }

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="cultureID">Identifier of culture.</param>
		/// <param name="text">Text for given culture.</param>
		public MLText(string cultureID, string text)
		{
			AddText(cultureID, text);
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="values">Pairs of culture IDs and texts.</param>
		public MLText(params string[] values)
		{
			if (values == null)
				return;

			if ((values.Length % 2) != 0)
				throw new Exception(Core.Properties.Resources.WrongMLTextsFormat);

			for (int i = 0; i < values.Length; i += 2)
			{
				AddText(values[i], values[i + 1]);
			}
		}

		/// <summary>
		/// Copying constructor.
		/// </summary>
		/// <param name="another">Object to be copied.</param>
		private MLText(MLText another)
		{
			if (another == null)
				throw new ArgumentNullException("another");

			foreach (string cultureID in another.Cultures)
			{
				AddText(cultureID, another[cultureID]);
			}
		}
		#endregion

		#region Methods
		/// <summary>
		/// Adds text for given culture into object.
		/// </summary>
		/// <param name="cultureID">Identifier of culture.</param>
		/// <param name="text">Text for given culture.</param>
		public void AddText(string cultureID, string text)
		{
			if (!m_CultureIDValidator.IsMatch(cultureID))
				throw new Exception(Core.Properties.Resources.WrongCultureId);

			Debug.Assert( m_TextsMap != null );
			if (string.IsNullOrEmpty(text))
				RemoveText(cultureID);
			else
				m_TextsMap[cultureID] = text;
		}

		/// <summary>
		/// Removes text with given culture identifier.
		/// </summary>
		/// <param name="cultureID">Identifier of culture.</param>
		public void RemoveText(string cultureID)
		{
			Debug.Assert(m_TextsMap != null);
			m_TextsMap.Remove(cultureID);
		}

		/// <summary>
		/// Is this object contains text for given culture.
		/// </summary>
		/// <param name="cultureID">Identifier of culture.</param>
		/// <returns>Is this object contains text for given culture.</returns>
		public bool ContainsCulture(string cultureID)
		{
			Debug.Assert(m_TextsMap != null);
			return m_TextsMap.ContainsKey(cultureID);
		}

		/// <summary>
		/// Is this object contains text for given invariant culture.
		/// </summary>
		/// <param name="cultureID">Identifier of culture.</param>
		/// <returns>Is this object contains text for given invariant culture.</returns>
		public bool ContainsCultureInvariant(string cultureID)
		{
			Debug.Assert(m_TextsMap != null);
			if (m_TextsMap.ContainsKey(cultureID))
				return true;

			MatchCollection mc = m_CultureIDValidator.Matches(cultureID);
			if ((mc == null) || (mc.Count == 0))
				return false;

			if (mc[0].Groups.Count == 0)
				return false;

			return m_TextsMap.ContainsKey(mc[0].Groups[1].Value);
		}

		/// <summary>
		/// Removes all texts.
		/// </summary>
		public void Clear()
		{
			Debug.Assert(m_TextsMap != null);
			m_TextsMap.Clear();
		}

		/// <summary>
		/// Returns text for given culture.
		/// </summary>
		/// <param name="cultureID">Identifier of culture.</param>
		/// <returns>Text for given culture.</returns>
		private string GetText(string cultureID)
		{
			if( !m_CultureIDValidator.IsMatch(cultureID) )
				throw new Exception(Core.Properties.Resources.WrongCultureId);

			Debug.Assert(m_TextsMap != null);

			if (m_TextsMap.Count == 0)
				return string.Empty;

			if (m_TextsMap.ContainsKey(cultureID))
				return m_TextsMap[cultureID];

			MatchCollection mc = m_CultureIDValidator.Matches(cultureID);
			if( (mc == null) || (mc.Count == 0) )
				throw new Exception(Core.Properties.Resources.WrongCultureId);
			if( mc[0].Groups.Count == 0 )
				throw new Exception(Core.Properties.Resources.WrongCultureId);

			if( m_TextsMap.ContainsKey(mc[0].Groups[1].Value) )
				return m_TextsMap[mc[0].Groups[1].Value];

			if (m_TextsMap.ContainsKey(CurrentCultureID))
				return m_TextsMap[CurrentCultureID];

			if (m_TextsMap.ContainsKey(DefaultCultureID))
				return m_TextsMap[DefaultCultureID];

			if( m_TextsMap.ContainsKey("en") )
				return m_TextsMap["en"];

			string[] values = new string[m_TextsMap.Values.Count];
			m_TextsMap.Values.CopyTo(values, 0);
			return values[0];
		}

		/// <summary>
		/// Returns XML presentation of object.
		/// </summary>
		/// <returns>XML presentation of object.</returns>
		public string ToXMLString()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("<MLText>");

			foreach (KeyValuePair<string, string> kvp in m_TextsMap)
			{
				sb.AppendFormat("<Text lang=\"{0}\">", kvp.Key);
				sb.Append(kvp.Value);
				sb.Append("</Text>");
			}

			sb.Append("</MLText>");
			return sb.ToString();
		}

		/// <summary>
		/// Loads data of object from XML string.
		/// </summary>
		/// <param name="xml">XML string with presentation of object.</param>
		public void LoadFromXML(string xml)
		{
			Clear();

			if (string.IsNullOrEmpty(xml))
				return;

			try
			{
				XmlDocument xmlDoc = new XmlDocument();
				xmlDoc.LoadXml(xml);

				foreach (XmlNode textNode in xmlDoc.DocumentElement.GetElementsByTagName("Text"))
				{
					AddText(textNode.Attributes["lang"].Value, textNode.InnerText);
				}
			}
			catch 
			{
				Clear();
				AddText(MLText.DefaultCultureID, xml);
			}
		}
		#endregion

		#region Overrides
		public override string ToString()
		{
			return GetText(CurrentCultureID);
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
				return false;

			if( !(obj is MLText) )
				return false;

			if (Object.ReferenceEquals(this, obj))
				return true;

			MLText another = obj as MLText;

			string[] thisCultures = this.Cultures;
			List<string> cultures = new List<string>(thisCultures.Length);
			for (int i = 0; i < thisCultures.Length; i++)
			{
				cultures.Add(thisCultures[i].ToLowerInvariant());
			}

			foreach (string cultureID in another.Cultures)
			{
				if (!cultures.Remove(cultureID.ToLowerInvariant()))
					return false;
			}

			if (cultures.Count != 0)
				return false;

			foreach (string cultureID in thisCultures)
			{
				if (this[cultureID] != another[cultureID])
					return false;
			}

			return true;
		}

		public override int GetHashCode()
		{
			StringBuilder sb = new StringBuilder();

			foreach (KeyValuePair<string, string> kvp in m_TextsMap)
			{
				sb.AppendFormat("{0} - {1}" + Environment.NewLine, kvp.Key, kvp.Value);
			}

			return sb.ToString().GetHashCode();
		}
		#endregion

		#region Operators
		public static bool operator ==(MLText ml1, MLText ml2)
		{
			if (Object.ReferenceEquals(ml1, null))
			{
				if (Object.ReferenceEquals(ml2, null))
					return true;
				else
					return false;
			}
			else
				return ml1.Equals(ml2);
		}

		public static bool operator !=(MLText ml1, MLText ml2)
		{
			return !(ml1 == ml2);
		}

		public static MLText operator +(MLText ml1, MLText ml2)
		{
			if( (ml1 == null) && (ml2 == null) )
				return null;
			if (ml1 == null)
				return new MLText(ml2);
			if (ml2 == null)
				return new MLText(ml1);

			MLText output = new MLText(ml1);

			foreach (string cultureID in ml2.Cultures)
			{
				if (output.ContainsCulture(cultureID))
				{
					output[cultureID] += ml2[cultureID];
				}
				else
				{
					output.AddText(cultureID, ml2[cultureID]);
				}
			}

			return output;
		}

		public static MLText operator +(MLText mlt, string str)
		{
			if ((mlt == null) && (str == null))
				return null;

			if (mlt == null)
				return new MLText(MLText.CurrentCultureID, str);

			if (str == null)
				return new MLText(mlt);

			MLText output = new MLText(mlt);
			output[MLText.CurrentCultureID] += str;
			return output;
		}

		public static MLText operator +(string str, MLText mlt)
		{
			MLText strMLT = new MLText(MLText.CurrentCultureID, str);
			return strMLT + mlt;
		}

        public static explicit operator MLString(MLText mlText)
        {
            return new MLString(mlText.GetText("ru"), mlText.GetText("en"));
        }
		#endregion
	}
}
