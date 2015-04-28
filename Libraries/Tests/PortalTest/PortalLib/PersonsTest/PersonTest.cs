using System;
using System.Collections.Generic;
using System.Text;

using NUnit.Framework;
using Core;
using UlterSystems.PortalLib.BusinessObjects;

namespace PortalTest.PortalLibTest.PersonsTest
{
	[TestFixture]
	[Category("Person")]
	public class PersonTest
	{
		private Person m_Person;

		[TestFixtureSetUp]
		public void SetUp()
		{
			Utils.InitDBConnection();
		}

		[Test]
		public void Save()
		{
			m_Person = new Person();
			m_Person.LastName = new MLText("en", "Yakimov");
			m_Person.PersonGender = Person.Gender.Male;
			Assert.IsFalse(m_Person.ID.HasValue);
			m_Person.Save();
			Assert.IsTrue(m_Person.ID.HasValue);

			try
			{
				Person loaded = new Person();
				Assert.IsTrue(loaded.Load(m_Person.ID.Value));
				Assert.AreEqual(m_Person.ID.Value, loaded.ID.Value);
				Assert.AreEqual("Yakimov", loaded.LastName["en"]);
				Assert.AreEqual(Person.Gender.Male, loaded.PersonGender);
			}
			finally
			{ m_Person.Delete(); }
		}

		[Test]
		public void FullName()
		{
			m_Person = new Person();
			Assert.AreEqual(string.Empty, m_Person.FullName.ToString());
			m_Person.LastName = new MLText("en", "Yakimov");
			Assert.AreEqual("Yakimov", m_Person.FullName.ToString());
			m_Person.FirstName = new MLText("en", "Ivan");
			Assert.AreEqual("Ivan Yakimov", m_Person.FullName.ToString());
			m_Person.MiddleName = new MLText("en", "Mihailovich");
			Assert.AreEqual("Ivan Mihailovich Yakimov", m_Person.FullName.ToString());
			m_Person.MiddleName = new MLText("ru", "Михайлович");
			Assert.AreEqual("Ivan Михайлович Yakimov", m_Person.FullName.ToString());
		}

		[Test]
		public void LoadByDomainName_False()
		{
			Person p = new Person();
			Assert.IsFalse(p.LoadByDomainName(@"ultersysyar\yim"));

			m_Person = new Person();
			m_Person.LastName = new MLText("en", "Yakimov");
			m_Person.PersonGender = Person.Gender.Male;
			Assert.IsFalse(m_Person.ID.HasValue);
			m_Person.Save();
			Assert.IsTrue(m_Person.ID.HasValue);

			try
			{
				Assert.IsFalse(p.LoadByDomainName(@"ultersysyar\yim"));

				PersonAttribute pa = new PersonAttribute();
				pa.PersonID = m_Person.ID.Value;
				pa.InsertionDate = DateTime.Now;
				pa.KeyWord = PersonAttributeTypes.DomainName.ToString();
				pa.ValueType = typeof(string).AssemblyQualifiedName;
				pa.StringField = @"ultersysyar\yim1";
				pa.Save();
				Assert.IsNotNull(pa.ID);

				try
				{
					Assert.IsFalse(p.LoadByDomainName(@"ultersysyar\yim"));
				}
				finally
				{ pa.Delete(); }
			}
			finally
			{ m_Person.Delete(); }
		}

		[Test]
		public void LoadByDomainName_True()
		{
			Person p = new Person();

			m_Person = new Person();
			m_Person.LastName = new MLText("en", "Yakimov");
			m_Person.PersonGender = Person.Gender.Male;
			Assert.IsFalse(m_Person.ID.HasValue);
			m_Person.Save();
			Assert.IsTrue(m_Person.ID.HasValue);

			try
			{
				PersonAttribute pa = new PersonAttribute();
				pa.PersonID = m_Person.ID.Value;
				pa.InsertionDate = DateTime.Now;
				pa.KeyWord = PersonAttributeTypes.DomainName.ToString();
				pa.ValueType = typeof(string).AssemblyQualifiedName;
				pa.StringField = @"ultersysyar\yim";
				pa.Save();
				Assert.IsNotNull(pa.ID);

				try
				{
					Assert.IsTrue(p.LoadByDomainName(@"ultersysyar\yim"));

					Assert.AreEqual(m_Person.ID.Value, p.ID.Value);
					Assert.AreEqual("Yakimov", p.LastName["en"]);
					Assert.AreEqual(Person.Gender.Male, p.PersonGender);
				}
				finally
				{ pa.Delete(); }
			}
			finally
			{ m_Person.Delete(); }
		}

		[Test]
		public void AddRemoveStandartStringAttributes()
		{
			m_Person = new Person();
			m_Person.LastName = new MLText("en", "Yakimov");
			m_Person.PersonGender = Person.Gender.Male;
			Assert.IsFalse(m_Person.ID.HasValue);
			m_Person.Save();
			Assert.IsTrue(m_Person.ID.HasValue);

			try
			{
				PersonAttribute[] coll;

				m_Person.AddStandardStringAttribute(PersonAttributeTypes.EMail, "yim@ultersys.ru");
				coll = PersonAttributes.GetPersonAttributesByKeyword(m_Person.ID.Value, PersonAttributeTypes.EMail.ToString());
				Assert.AreEqual(1, coll.Length);

				m_Person.AddStandardStringAttribute(PersonAttributeTypes.EMail, "edlin@uniyar.ac.ru");
				coll = PersonAttributes.GetPersonAttributesByKeyword(m_Person.ID.Value, PersonAttributeTypes.EMail.ToString());
				Assert.AreEqual(2, coll.Length);

				m_Person.RemoveStandardAttributes(PersonAttributeTypes.EMail);
				coll = PersonAttributes.GetPersonAttributesByKeyword(m_Person.ID.Value, PersonAttributeTypes.EMail.ToString());
				Assert.AreEqual(0, coll.Length);
			}
			finally
			{ m_Person.Delete(); }
		}
	}
}
