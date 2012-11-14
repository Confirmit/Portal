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
	public class PersonAttributeTest
	{
		private PersonAttribute m_PA;

		[TestFixtureSetUp]
		public void SetUp()
		{
			Utils.InitDBConnection();
		}

		[Test]
		public void PropertiesTest()
		{
			m_PA = new PersonAttribute();
			m_PA.PersonID = 12;
			Assert.AreEqual(12, m_PA.PersonID);
			m_PA.InsertionDate = new DateTime(2007, 03, 12);
			Assert.AreEqual(new DateTime(2007, 03, 12), m_PA.InsertionDate);
			m_PA.KeyWord = "Phone";
			Assert.AreEqual("Phone", m_PA.KeyWord);
			m_PA.ValueType = typeof(string).AssemblyQualifiedName;
			Assert.AreEqual(typeof(string).AssemblyQualifiedName, m_PA.ValueType);
			m_PA.StringField = "Hello";
			Assert.AreEqual("Hello", m_PA.StringField);
			m_PA.IntegerField = 34;
			Assert.AreEqual(34, m_PA.IntegerField);
			m_PA.DoubleField = 23.4;
			Assert.AreEqual(23.4, m_PA.DoubleField);
			m_PA.BooleanField = true;
			Assert.IsTrue(m_PA.BooleanField.Value);
			m_PA.DateTimeField = new DateTime(2005, 11, 07);
			Assert.AreEqual(new DateTime(2005, 11, 07), m_PA.DateTimeField);
			m_PA.BinaryField = new byte[1];
			Assert.IsNotNull(m_PA.BinaryField);
			Assert.AreEqual(1, m_PA.BinaryField.Length);
		}

		[Test]
		public void Value_Null_NoValueType()
		{
			m_PA = new PersonAttribute();
			Assert.IsNull(m_PA.Value);
			m_PA.ValueType = string.Empty;
			Assert.IsNull(m_PA.Value);
		}

		[Test]
		public void Value_Null_IncorrectValueType()
		{
			m_PA = new PersonAttribute();
			m_PA.ValueType = "This is incorrect type.";
			Assert.IsNull(m_PA.Value);
		}

		[Test]
		public void Value_Null_NoData()
		{
			m_PA = new PersonAttribute();
			m_PA.ValueType = typeof(string).AssemblyQualifiedName;
			Assert.IsNull(m_PA.Value);
			m_PA.ValueType = typeof(int).AssemblyQualifiedName;
			Assert.IsNull(m_PA.Value);
			m_PA.ValueType = typeof(float).AssemblyQualifiedName;
			Assert.IsNull(m_PA.Value);
			m_PA.ValueType = typeof(bool).AssemblyQualifiedName;
			Assert.IsNull(m_PA.Value);
			m_PA.ValueType = typeof(DateTime).AssemblyQualifiedName;
			Assert.IsNull(m_PA.Value);
			m_PA.ValueType = typeof(byte[]).AssemblyQualifiedName;
			Assert.IsNull(m_PA.Value);
		}

		[Test]
		public void Value_StringData()
		{
			m_PA = new PersonAttribute();
			m_PA.ValueType = typeof(string).AssemblyQualifiedName;
			m_PA.StringField = "Hello";
			Assert.AreEqual("Hello", m_PA.Value);
		}

		[Test]
		public void Value_IntData()
		{
			m_PA = new PersonAttribute();
			m_PA.ValueType = typeof(int).AssemblyQualifiedName;
			m_PA.IntegerField = 12;
			Assert.IsInstanceOfType(typeof(Nullable<int>), m_PA.Value);
			Nullable<int> nv = (Nullable<int>)m_PA.Value;
			Assert.AreEqual(12, nv.Value);
		}

		[Test]
		public void Value_DoubleData()
		{
			m_PA = new PersonAttribute();
			m_PA.ValueType = typeof(double).AssemblyQualifiedName;
			m_PA.DoubleField = 23.4;
			Assert.IsInstanceOfType(typeof(Nullable<double>), m_PA.Value);
			Nullable<double> nv = (Nullable<double>)m_PA.Value;
			Assert.AreEqual(23.4, nv.Value);
		}

		[Test]
		public void Value_BoolData()
		{
			m_PA = new PersonAttribute();
			m_PA.ValueType = typeof(bool).AssemblyQualifiedName;
			m_PA.BooleanField = true;
			Assert.IsInstanceOfType(typeof(Nullable<bool>), m_PA.Value);
			Nullable<bool> nv = (Nullable<bool>)m_PA.Value;
			Assert.AreEqual(true, nv.Value);
		}

		[Test]
		public void Value_DateTimeData()
		{
			m_PA = new PersonAttribute();
			m_PA.ValueType = typeof(DateTime).AssemblyQualifiedName;
			m_PA.DateTimeField = new DateTime(1999, 10, 1);
			Assert.IsInstanceOfType(typeof(Nullable<DateTime>), m_PA.Value);
			Nullable<DateTime> nv = (Nullable<DateTime>)m_PA.Value;
			Assert.AreEqual(new DateTime(1999, 10, 1), nv.Value);
		}

		[Test]
		public void Value_BinaryData()
		{
			byte[] arr = new byte[1];
			arr[0] = 23;
			m_PA = new PersonAttribute();
			m_PA.ValueType = typeof(byte[]).AssemblyQualifiedName;
			m_PA.BinaryField = arr;
			Assert.IsNotNull(m_PA.Value);
			Assert.IsInstanceOfType(typeof(byte[]), m_PA.Value);
			byte[] retArr = (byte[])m_PA.Value;
			Assert.AreEqual(1, retArr.Length);
			Assert.AreEqual(23, retArr[0]);
		}

		[Test]
		public void Value_Null_UnknownType()
		{
			m_PA = new PersonAttribute();
			m_PA.ValueType = typeof(TimeSpan).AssemblyQualifiedName;
			m_PA.StringField = "Hello";
			m_PA.DateTimeField = DateTime.Now;
			Assert.IsNull(m_PA.Value);
		}

		[Test]
		public void Save()
		{
			DateTime now = DateTime.Now;
			byte[] arr = new byte[1];
			arr[0] = 2;

			m_PA = new PersonAttribute();
			m_PA.PersonID = 12;
			m_PA.InsertionDate = now;
			m_PA.KeyWord = "Phone";
			m_PA.ValueType = typeof(string).AssemblyQualifiedName;
			m_PA.StringField = "123456";
			m_PA.IntegerField = 1;
			m_PA.DoubleField = 1.4;
			m_PA.BooleanField = true;
			m_PA.DateTimeField = new DateTime(1934, 12, 1);
			m_PA.BinaryField = arr;
			m_PA.Save();
			Assert.IsNotNull(m_PA.ID);

			PersonAttribute pa = new PersonAttribute();
			Assert.IsTrue(pa.Load(m_PA.ID.Value));

			Assert.AreEqual(m_PA.PersonID, pa.PersonID);
			TimeSpan diff = m_PA.InsertionDate - pa.InsertionDate;
			Assert.IsTrue(Math.Abs(diff.TotalSeconds) < 1.0);
			Assert.AreEqual(m_PA.KeyWord, pa.KeyWord);
			Assert.AreEqual(m_PA.ValueType, pa.ValueType);
			Assert.AreEqual(m_PA.StringField, pa.StringField);
			Assert.AreEqual(m_PA.IntegerField, pa.IntegerField);
			Assert.AreEqual(m_PA.DoubleField, pa.DoubleField);
			Assert.AreEqual(m_PA.BooleanField, pa.BooleanField);
			Assert.AreEqual(m_PA.DateTimeField, pa.DateTimeField);
			Assert.AreEqual(m_PA.BinaryField.Length, pa.BinaryField.Length);
			Assert.AreEqual(m_PA.BinaryField[0], pa.BinaryField[0]);
			Assert.AreEqual(m_PA.Value, pa.Value);

			m_PA.Delete();
		}
	}
}
