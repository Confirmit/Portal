using System;
using System.Collections.Generic;
using System.Text;

using NUnit.Framework;
using Core;
using Core.ORM;

namespace PortalTest.CoreLibTest.MLTextTest
{
	//[TestFixture]
	[Category("MLText")]
	public class MLTextDatabaseTest
	{
		private string TextRu = "Это текст для ru";
		private string TextEn = "This text is for en";

		[TestFixtureSetUp]
		public void FixtureSetUp()
		{
			Utils.InitDBConnection();
		}

		[Test]
		public void Save()
		{
			MLTextTestClass mlttc = new MLTextTestClass();
			mlttc.StringMLText["ru"] = TextRu;
			mlttc.XmlMLText["en"] = TextEn;

			Assert.IsFalse(mlttc.ID.HasValue);
			mlttc.Save();
			Assert.IsTrue(mlttc.ID.HasValue);

			mlttc.Delete();
		}

		[Test]
		public void Load()
		{
			MLTextTestClass mlttc = new MLTextTestClass();
			mlttc.StringMLText["ru"] = TextRu;
			mlttc.XmlMLText["en"] = TextEn;

			Assert.IsFalse(mlttc.ID.HasValue);
			mlttc.Save();
			Assert.IsTrue(mlttc.ID.HasValue);

			MLTextTestClass loaded = new MLTextTestClass();
			Assert.IsFalse(loaded.ID.HasValue);
			loaded.Load(mlttc.ID.Value);
			Assert.IsTrue(loaded.ID.HasValue);

			Assert.AreEqual(mlttc.ID.Value, loaded.ID.Value);

			Assert.AreEqual(mlttc.StringMLText, loaded.StringMLText);
			Assert.AreEqual(mlttc.XmlMLText, loaded.XmlMLText);

			mlttc.Delete();
		}
	}

	[DBTable("MLTestTable")]
	internal class MLTextTestClass : BasePlainObject
	{
		#region Fields
		private MLText m_StringText = new MLText();
		private MLText m_XMLText = new MLText();
		#endregion

		#region Properties
		[DBRead("StringMLText")]
		[DBNullable]
		public MLText StringMLText
		{
			get { return m_StringText; }
			set { m_StringText = value; }
		}

		[DBRead("XmlMLText")]
		[DBNullable]
		public MLText XmlMLText
		{
			get { return m_XMLText; }
			set { m_XMLText = value; }
		}
		#endregion
	}
}
