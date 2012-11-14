using System;
using System.Configuration;

namespace UlterSystems.PortalService.Configuration
{
	public class MailExpireConfigSection : ConfigurationSection
	{
		[ConfigurationProperty("", IsRequired = false, IsKey = false, IsDefaultCollection = true)]
		public ItemCollection Items
		{
			get { return ((ItemCollection)(base[""])); }
			set { base[""] = value; }
		}
	}

	[ConfigurationCollection(typeof(MailExpireItem), CollectionType = ConfigurationElementCollectionType.BasicMapAlternate)]
	public class ItemCollection : ConfigurationElementCollection
	{
		internal const string ItemPropertyName = "MailExpire";

		public override ConfigurationElementCollectionType CollectionType
		{
			get { return ConfigurationElementCollectionType.BasicMapAlternate; }
		}

		protected override string ElementName
		{
			get { return ItemPropertyName; }
		}

		protected override bool IsElementName(string elementName)
		{
			return (elementName == ItemPropertyName);
		}

		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((MailExpireItem)element).MailType;
		}

		protected override ConfigurationElement CreateNewElement()
		{
			return new MailExpireItem();
		}

		public override bool IsReadOnly()
		{
			return false;
		}

		public MailExpireItem this[int index]
		{
			get
			{
				return (MailExpireItem)BaseGet(index);
			}
			set
			{
				if (BaseGet(index) != null)
					BaseRemoveAt(index);

				BaseAdd(index, value);
			}
		}

		public int IndexOf(MailExpireItem mailExpireItem)
		{
			return BaseIndexOf(mailExpireItem);
		}
	}

	public class MailExpireItem : ConfigurationElement
	{
		[ConfigurationProperty("MailType")]
		public int MailType
		{
			get
			{
				return (int)base["MailType"];
			}
			set
			{
				base["MailType"] = value;
			}
		}

		[ConfigurationProperty("Name", IsRequired = false)]
		public string Name
		{
			get
			{
				return (string)base["Name"];
			}
			set
			{
				base["Name"] = value;
			}
		}

		[ConfigurationProperty("ExpirationTimeSpan")]
		public TimeSpan ExpirationTimeSpan
		{
			get
			{
				return (TimeSpan)base["ExpirationTimeSpan"];
			}
			set
			{
				base["ExpirationTimeSpan"] = value;
			}
		}
	}
}