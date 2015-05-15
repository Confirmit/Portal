using System;
using System.Collections.Generic;
using System.Text;

using NUnit.Framework;
using TypeMock;
using UlterSystems.PortalLib.BusinessObjects;

namespace PortalTest.PortalLibTest.PersonsTest
{
	[TestFixture]
	[Category("Person")]
	public class PersonAttributesTest
	{
		[TestFixtureSetUp]
		public void SetUp()
		{
			Utils.InitDBConnection();
		}

		[Test]
		public void GetAllPersonAttributes_Empty()
		{
			PersonAttribute[] coll = PersonAttributes.GetAllPersonAttributes(-10);
			Assert.IsNotNull(coll);
			Assert.AreEqual(0, coll.Length);
		}

		[Test]
		public void GetAllPersonAttributes_Data()
		{
			PersonAttribute pa = new PersonAttribute();
			pa.PersonID = -10;
			pa.InsertionDate = DateTime.Now;
			pa.KeyWord = "Phone";
			pa.ValueType = typeof(string).AssemblyQualifiedName;
			pa.StringField = "123456";
			pa.Save();
			Assert.IsNotNull(pa.ID);

			try
			{

				PersonAttribute[] coll = PersonAttributes.GetAllPersonAttributes(pa.PersonID);
				Assert.IsNotNull(coll);
				Assert.AreEqual(1, coll.Length);

				Assert.AreEqual(pa.PersonID, coll[0].PersonID);
				Assert.AreEqual(pa.InsertionDate.Date, coll[0].InsertionDate.Date);
				Assert.AreEqual(pa.KeyWord, coll[0].KeyWord);
				Assert.AreEqual(pa.ValueType, coll[0].ValueType);
				Assert.AreEqual(pa.StringField, coll[0].StringField);
				Assert.AreEqual(pa.Value, coll[0].Value);
			}
			finally
			{ pa.Delete(); }
		}

		[Test]
		public void GetPersonAttributesByKeyword_Empty()
		{
			PersonAttribute[] coll = PersonAttributes.GetPersonAttributesByKeyword(-10, null);
			Assert.IsNotNull(coll);
			Assert.AreEqual(0, coll.Length);

			coll = PersonAttributes.GetPersonAttributesByKeyword(-10, string.Empty);
			Assert.IsNotNull(coll);
			Assert.AreEqual(0, coll.Length);

			coll = PersonAttributes.GetPersonAttributesByKeyword(-10, "Phone");
			Assert.IsNotNull(coll);
			Assert.AreEqual(0, coll.Length);

			PersonAttribute pa = new PersonAttribute();
			pa.PersonID = -10;
			pa.InsertionDate = DateTime.Now;
			pa.KeyWord = "Phone";
			pa.ValueType = typeof(string).AssemblyQualifiedName;
			pa.StringField = "123456";
			pa.Save();
			Assert.IsNotNull(pa.ID);

			try
			{
				coll = PersonAttributes.GetPersonAttributesByKeyword(-10, "TelePhone");
				Assert.IsNotNull(coll);
				Assert.AreEqual(0, coll.Length);
			}
			finally
			{ pa.Delete(); }
		}

		[Test]
		public void GetPersonAttributesByKeyword_Data()
		{
			PersonAttribute pa = new PersonAttribute();
			pa.PersonID = -10;
			pa.InsertionDate = DateTime.Now;
			pa.KeyWord = "Phone";
			pa.ValueType = typeof(string).AssemblyQualifiedName;
			pa.StringField = "123456";
			pa.Save();
			Assert.IsNotNull(pa.ID);

			try
			{
				PersonAttribute[] coll = PersonAttributes.GetPersonAttributesByKeyword(pa.PersonID, "Phone");
				Assert.IsNotNull(coll);
				Assert.AreEqual(1, coll.Length);

				Assert.AreEqual(pa.PersonID, coll[0].PersonID);
				Assert.AreEqual(pa.InsertionDate.Date, coll[0].InsertionDate.Date);
				Assert.AreEqual(pa.KeyWord, coll[0].KeyWord);
				Assert.AreEqual(pa.ValueType, coll[0].ValueType);
				Assert.AreEqual(pa.StringField, coll[0].StringField);
				Assert.AreEqual(pa.Value, coll[0].Value);
			}
			finally
			{ pa.Delete(); }
		}

		[Test]
		public void GetPersonAttributesByKeyword()
		{
			Guid g = Guid.NewGuid();

			PersonAttribute pa = new PersonAttribute();
			pa.PersonID = -10;
			pa.InsertionDate = DateTime.Now;
			pa.KeyWord = g.ToString();
			pa.ValueType = typeof(string).AssemblyQualifiedName;
			pa.StringField = "123456";
			pa.Save();
			Assert.IsNotNull(pa.ID);

			try
			{
				PersonAttribute[] coll = PersonAttributes.GetPersonAttributesByKeyword(g.ToString());
				Assert.IsNotNull(coll);
				Assert.AreEqual(1, coll.Length);

				Assert.AreEqual(pa.PersonID, coll[0].PersonID);
				Assert.AreEqual(pa.InsertionDate.Date, coll[0].InsertionDate.Date);
				Assert.AreEqual(pa.KeyWord, coll[0].KeyWord);
				Assert.AreEqual(pa.ValueType, coll[0].ValueType);
				Assert.AreEqual(pa.StringField, coll[0].StringField);
				Assert.AreEqual(pa.Value, coll[0].Value);
			}
			finally
			{ pa.Delete(); }
		}
	}
}
