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
	public class PersonAttributeTest : DBFixture
	{
		private PersonAttribute pAttrib;
		private PersonAttribute loaded;

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
			pAttrib.PersonID = 123;
			pAttrib.StringField = "Test";
			pAttrib.ValueType = typeof( string ).AssemblyQualifiedName;
			pAttrib.Save();

			loaded = new PersonAttribute();
			loaded.Load( pAttrib.ID.Value );
		}

		[TearDown]
		public void TearDown()
		{
			pAttrib.Delete();
		}

		[Test]
		public void BinaryField() 
		{ Assert.AreEqual( pAttrib.BinaryField, loaded.BinaryField ); }

		[Test]
		public void BooleanField() 
		{ Assert.AreEqual( pAttrib.BooleanField, loaded.BooleanField ); }

		[Test]
		public void DateTimeField() 
		{ Assert.AreEqual( pAttrib.DateTimeField, loaded.DateTimeField ); }

		[Test]
		public void DoubleField() 
		{ Assert.AreEqual( pAttrib.DoubleField, loaded.DoubleField ); }

		[Test]
		public void InsertionDate() 
		{ Assert.AreEqual( pAttrib.InsertionDate, loaded.InsertionDate ); }

		[Test]
		public void IntegerField() 
		{ Assert.AreEqual( pAttrib.IntegerField, loaded.IntegerField ); }

		[Test]
		public void PersonID() 
		{ Assert.AreEqual( pAttrib.PersonID, loaded.PersonID ); }

		[Test]
		public void StringField() 
		{ Assert.AreEqual( pAttrib.StringField, loaded.StringField ); }

		[Test]
		public void Value()
		{ Assert.AreEqual( pAttrib.Value, loaded.Value ); }

		[Test]
		public void ValueType() 
		{ Assert.AreEqual( pAttrib.ValueType, loaded.ValueType ); }

		[Test]
		public void Value_Null_NoValueType()
		{
			pAttrib.ValueType = null;
			Assert.IsNull( pAttrib.Value );
			pAttrib.ValueType = string.Empty;
			Assert.IsNull( pAttrib.Value );
		}

		[Test]
		public void Value_Null_IncorrectValueType()
		{
			pAttrib.ValueType = "This is incorrect type.";
			Assert.IsNull( pAttrib.Value );
		}

		[Test]
		public void Value_Null_NoData()
		{
			pAttrib.ValueType = typeof( string ).AssemblyQualifiedName;
			pAttrib.StringField = null;
			Assert.IsNull( pAttrib.Value );

			pAttrib.ValueType = typeof( int ).AssemblyQualifiedName;
			pAttrib.IntegerField = null;
			Assert.IsNull( pAttrib.Value );

			pAttrib.ValueType = typeof( double ).AssemblyQualifiedName;
			pAttrib.DoubleField = null;
			Assert.IsNull( pAttrib.Value );

			pAttrib.ValueType = typeof( bool ).AssemblyQualifiedName;
			pAttrib.BooleanField = null;
			Assert.IsNull( pAttrib.Value );

			pAttrib.ValueType = typeof( DateTime ).AssemblyQualifiedName;
			pAttrib.DateTimeField = null;
			Assert.IsNull( pAttrib.Value );

			pAttrib.ValueType = typeof( byte[] ).AssemblyQualifiedName;
			pAttrib.BinaryField = null;
			Assert.IsNull( pAttrib.Value );
		}

		[Test]
		public void Value_StringData()
		{
			pAttrib.ValueType = typeof( string ).AssemblyQualifiedName;
			Assert.AreEqual( pAttrib.StringField, pAttrib.Value );
		}

		[Test]
		public void Value_IntData()
		{
			pAttrib.ValueType = typeof( int ).AssemblyQualifiedName;
			Assert.IsInstanceOfType( typeof( Nullable<int> ), pAttrib.Value );
			Nullable<int> nv = (Nullable<int>) pAttrib.Value;
			Assert.AreEqual( pAttrib.IntegerField, nv.Value );
		}

		[Test]
		public void Value_DoubleData()
		{
			pAttrib.ValueType = typeof( double ).AssemblyQualifiedName;
			Assert.IsInstanceOfType( typeof( Nullable<double> ), pAttrib.Value );
			Nullable<double> nv = (Nullable<double>) pAttrib.Value;
			Assert.AreEqual( pAttrib.DoubleField, nv.Value );
		}

		[Test]
		public void Value_BoolData()
		{
			pAttrib.ValueType = typeof( bool ).AssemblyQualifiedName;
			Assert.IsInstanceOfType( typeof( Nullable<bool> ), pAttrib.Value );
			Nullable<bool> nv = (Nullable<bool>) pAttrib.Value;
			Assert.AreEqual( pAttrib.BooleanField, nv.Value );
		}

		[Test]
		public void Value_DateTimeData()
		{
			pAttrib.ValueType = typeof( DateTime ).AssemblyQualifiedName;
			Assert.IsInstanceOfType( typeof( Nullable<DateTime> ), pAttrib.Value );
			Nullable<DateTime> nv = (Nullable<DateTime>) pAttrib.Value;
			Assert.AreEqual( pAttrib.DateTimeField, nv.Value );
		}

		[Test]
		public void Value_BinaryData()
		{
			pAttrib.ValueType = typeof( byte[] ).AssemblyQualifiedName;
			Assert.IsNotNull( pAttrib.Value );
			Assert.IsInstanceOfType( typeof( byte[] ), pAttrib.Value );
			Assert.AreEqual( pAttrib.BinaryField, pAttrib.Value );
		}

		[Test]
		public void Value_Null_UnknownType()
		{
			pAttrib.ValueType = typeof( TimeSpan ).AssemblyQualifiedName;
			Assert.IsNull( pAttrib.Value );
		}
	}
}
