using System;
using System.Collections.Generic;
using System.Text;

using NUnit.Framework;
using TypeMock;
using UlterSystems.PortalLib.BusinessObjects;

namespace PortalTest.PortalLibTest.PersonsTest
{
	[TestFixture]
	[Category( "Person" )]
	public class PersonAttributesTest : DBFixture
	{
		private int personID = -123;
		private string keyword = "Keyword";
		private PersonAttribute pAttrib;

		[SetUp]
		public void SetUp()
		{
			pAttrib = new PersonAttribute();
			pAttrib.BinaryField = new byte[] { 1, 2, 3 };
			pAttrib.BooleanField = true;
			pAttrib.DateTimeField = DateTime.Today;
			pAttrib.DoubleField = 1.233;
			pAttrib.InsertionDate = DateTime.Today;
			pAttrib.IntegerField = 44;
			pAttrib.PersonID = personID;
			pAttrib.StringField = "Test";
			pAttrib.ValueType = typeof( string ).AssemblyQualifiedName;
			pAttrib.Save();
		}

		[TearDown]
		public void TearDown()
		{
			pAttrib.Delete();
		}

		[Test]
		public void GetAllPersonAttributes_Empty()
		{
			IList<PersonAttribute> coll = PersonAttributes.GetAllPersonAttributes( -10 );
			Assert.IsNotNull( coll );
			Assert.AreEqual( 0, coll.Count );
		}

		[Test]
		public void GetAllPersonAttributes_Data()
		{
			IList<PersonAttribute> coll = PersonAttributes.GetAllPersonAttributes( personID );
			Assert.IsNotNull( coll );
			Assert.AreEqual( 1, coll.Count );

			Assert.AreEqual( pAttrib.PersonID, coll[ 0 ].PersonID );
			Assert.AreEqual( pAttrib.InsertionDate.Date, coll[ 0 ].InsertionDate.Date );
			Assert.AreEqual( pAttrib.ValueType, coll[ 0 ].ValueType );
			Assert.AreEqual( pAttrib.StringField, coll[ 0 ].StringField );
			Assert.AreEqual( pAttrib.Value, coll[ 0 ].Value );
		}

		[Test]
		public void GetPersonAttributesByKeyword_Empty()
		{
			IList<PersonAttribute> coll = PersonAttributes.GetPersonAttributesByKeyword( -10, (string)null );
			Assert.IsNotNull( coll );
			Assert.AreEqual( 0, coll.Count );

			coll = PersonAttributes.GetPersonAttributesByKeyword( -10, string.Empty );
			Assert.IsNotNull( coll );
			Assert.AreEqual( 0, coll.Count );

			coll = PersonAttributes.GetPersonAttributesByKeyword( -10, "Phone" );
			Assert.IsNotNull( coll );
			Assert.AreEqual( 0, coll.Count );
		}

		[Test]
		public void GetPersonAttributesByKeyword_Data()
		{
			IList<PersonAttribute> coll = PersonAttributes.GetPersonAttributesByKeyword( personID, keyword );
			Assert.IsNotNull( coll );
			Assert.AreEqual( 1, coll.Count );

			Assert.AreEqual( pAttrib.PersonID, coll[ 0 ].PersonID );
			Assert.AreEqual( pAttrib.InsertionDate.Date, coll[ 0 ].InsertionDate.Date );
			Assert.AreEqual( pAttrib.ValueType, coll[ 0 ].ValueType );
			Assert.AreEqual( pAttrib.StringField, coll[ 0 ].StringField );
			Assert.AreEqual( pAttrib.Value, coll[ 0 ].Value );
		}

		[Test]
		public void GetAllPersonAttributesByKeyword_Null()
		{
			IList<PersonAttribute> coll = PersonAttributes.GetPersonAttributesByKeyword( "Unknown" );
			Assert.IsNotNull( coll );
			Assert.AreEqual( 0, coll.Count );
		}

		[Test]
		public void GetAllPersonAttributesByKeyword_Data()
		{
			IList<PersonAttribute> coll = PersonAttributes.GetPersonAttributesByKeyword( keyword );
			Assert.IsNotNull( coll );
			Assert.AreEqual( 1, coll.Count );

			Assert.AreEqual( pAttrib.PersonID, coll[ 0 ].PersonID );
			Assert.AreEqual( pAttrib.InsertionDate.Date, coll[ 0 ].InsertionDate.Date );
			Assert.AreEqual( pAttrib.ValueType, coll[ 0 ].ValueType );
			Assert.AreEqual( pAttrib.StringField, coll[ 0 ].StringField );
			Assert.AreEqual( pAttrib.Value, coll[ 0 ].Value );
		}
	}
}
