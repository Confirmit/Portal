using System;
using System.Collections.Generic;
using System.Text;

using NUnit.Framework;
using Core;

namespace PortalTest.CoreLibTest.MLTextTest
{
	[TestFixture]
	[Category("MLText")]
	public class MLTextTest
	{
		private MLText m_MLText;

		private string m_CurrentCultureID = "ru";

		private string TextRu = "Это текст для ru";
		private string TextEn = "This text is for en";
		private string TextEnUK = "This text is for en-uk";

		[Test]
		public void Constructor()
		{
			m_MLText = new MLText();
			Assert.IsNotNull(m_MLText);

			m_MLText = new MLText("ru", TextRu);
			Assert.IsNotNull(m_MLText);

			m_MLText = new MLText("ru", TextRu, "en", TextEn);
			Assert.IsNotNull(m_MLText);
		}

		[Test]
		public void Constructor_Exception()
		{
			try
			{
				m_MLText = new MLText(null, TextRu);
				Assert.Fail();
			}
			catch { }

			try
			{
				m_MLText = new MLText(string.Empty, TextRu);
				Assert.Fail();
			}
			catch { }

			try
			{
				m_MLText = new MLText("rus", TextRu);
				Assert.Fail();
			}
			catch { }

			try
			{
				m_MLText = new MLText("ru-", TextRu);
				Assert.Fail();
			}
			catch { }

			try
			{
				m_MLText = new MLText("u-", TextRu);
				Assert.Fail();
			}
			catch { }

			try
			{
				m_MLText = new MLText("ru-s", TextRu);
				Assert.Fail();
			}
			catch { }

			try
			{
				m_MLText = new MLText("ru-ert", TextRu);
				Assert.Fail();
			}
			catch { }

			try
			{
				m_MLText = new MLText("u-rt", TextRu);
				Assert.Fail();
			}
			catch { }

			try
			{
				m_MLText = new MLText("ru", TextRu, "en");
				Assert.Fail();
			}
			catch { }

			try
			{
				m_MLText = new MLText("ru-", TextRu, "en", TextEn);
				Assert.Fail();
			}
			catch { }

			try
			{
				m_MLText = new MLText("11-23", TextRu, "en", TextEn);
				Assert.Fail();
			}
			catch { }
		}

		[Test]
		public void AddText()
		{
			m_MLText = new MLText();
			m_MLText.AddText("ru", TextRu);
		}

		[Test]
		public void AddText_Exception()
		{
			m_MLText = new MLText();
			try
			{
				m_MLText.AddText("ru_", TextRu);
				Assert.Fail();
			}
			catch { }
		}

		[Test]
		public void ContainsCulture_True()
		{
			m_MLText = new MLText();
			m_MLText.AddText("ru", TextRu);
			m_MLText.AddText("en", TextEn);

			Assert.IsTrue( m_MLText.ContainsCulture("ru") );
			Assert.IsTrue(m_MLText.ContainsCulture("en"));
		}

		[Test]
		public void ContainsCulture_False()
		{
			m_MLText = new MLText();
			m_MLText.AddText("ru", TextRu);
			m_MLText.AddText("en", TextEn);

			Assert.IsFalse(m_MLText.ContainsCulture("fr"));
			Assert.IsFalse(m_MLText.ContainsCulture("en-UK"));
		}

		[Test]
		public void ContainsCultureInvariant_True()
		{
			m_MLText = new MLText();
			m_MLText.AddText("en", TextEn);

			Assert.IsTrue(m_MLText.ContainsCultureInvariant("en"));
			Assert.IsTrue(m_MLText.ContainsCultureInvariant("en-UK"));
		}

		[Test]
		public void ContainsCultureInvariant_False()
		{
			m_MLText = new MLText();
			m_MLText.AddText("ru", TextRu);
			m_MLText.AddText("en", TextEn);

			Assert.IsFalse(m_MLText.ContainsCultureInvariant("fr"));
			Assert.IsFalse(m_MLText.ContainsCultureInvariant("gm"));
		}

		[Test]
		public void Remove()
		{
			m_MLText = new MLText();
			m_MLText.AddText("ru", TextRu);
			Assert.IsTrue(m_MLText.ContainsCulture("ru"));
			m_MLText.RemoveText("ru");
			Assert.IsFalse(m_MLText.ContainsCulture("ru"));
		}

		[Test]
		public void Clear()
		{
			m_MLText = new MLText();
			m_MLText.AddText("ru", TextRu);
			m_MLText.AddText("en", TextEn);
			Assert.IsTrue(m_MLText.ContainsCulture("ru"));
			Assert.IsTrue(m_MLText.ContainsCulture("en"));
			m_MLText.Clear();
			Assert.IsFalse(m_MLText.ContainsCulture("ru"));
			Assert.IsFalse(m_MLText.ContainsCulture("en"));
		}

		[Test]
		public void IndexerGet()
		{
			m_MLText = new MLText();
			m_MLText.AddText("ru", TextRu);
			m_MLText.AddText("en", TextEn);
			Assert.AreEqual(TextRu, m_MLText["ru"]);
			Assert.AreEqual(TextEn, m_MLText["en"]);
		}

		[Test]
		public void IndexerGet_Exception()
		{
			m_MLText = new MLText();
			try
			{
				string text = m_MLText["ert"];
				Assert.Fail();
			}
			catch { }
		}

		[Test]
		public void IndexerSet()
		{
			m_MLText = new MLText();
			m_MLText["ru"] = TextRu;
			m_MLText["en"] = TextEn;
			Assert.IsTrue(m_MLText.ContainsCulture("ru"));
			Assert.IsTrue(m_MLText.ContainsCulture("en"));
		}

		[Test]
		public void IndexerSet_Exception()
		{
			m_MLText = new MLText();
			try
			{
				m_MLText["ert"] = TextEn;
				Assert.Fail();
			}
			catch { }
		}

		[Test]
		public void CurrentCultureID()
		{
			string ccID = MLText.CurrentCultureID;

			try
			{
				MLText.CurrentCultureID = "ru";
				Assert.AreEqual( "ru", MLText.CurrentCultureID);
			}
			finally
			{
				MLText.CurrentCultureID = ccID;
			}
		}


		[Test]
		public void CurrentCultureID_Exception()
		{
			string ccID = MLText.CurrentCultureID;

			try
			{
				MLText.CurrentCultureID = "rudf";
				Assert.Fail();
			}
			catch { }
			finally
			{
				MLText.CurrentCultureID = ccID;
			}
		}

		[Test]
		public void DefaultCultureID()
		{
			string ccID = MLText.DefaultCultureID;

			try
			{
				MLText.DefaultCultureID = "ru";
				Assert.AreEqual("ru", MLText.DefaultCultureID);
			}
			finally
			{
				MLText.DefaultCultureID = ccID;
			}
		}


		[Test]
		public void DefaultCultureID_Exception()
		{
			string ccID = MLText.DefaultCultureID;

			try
			{
				MLText.DefaultCultureID = "rudf";
				Assert.Fail();
			}
			catch { }
			finally
			{
				MLText.DefaultCultureID = ccID;
			}
		}

		[Test]
		public void GetText()
		{
			m_MLText = new MLText();
			Assert.AreEqual( string.Empty, m_MLText["ru"] );
			Assert.AreEqual(string.Empty, m_MLText["en"]);

			m_MLText["ru"] = TextRu;
			Assert.AreEqual(TextRu, m_MLText["ru"]);
			Assert.AreEqual(TextRu, m_MLText["en"]);

			m_MLText["en"] = TextEn;
			Assert.AreEqual(TextRu, m_MLText["ru"]);
			Assert.AreEqual(TextEn, m_MLText["en"]);
			Assert.AreEqual(TextEn, m_MLText["en-UK"]);

			m_MLText["en-UK"] = TextEnUK;
			Assert.AreEqual(TextRu, m_MLText["ru"]);
			Assert.AreEqual(TextEn, m_MLText["en"]);
			Assert.AreEqual(TextEnUK, m_MLText["en-UK"]);

			string dcID = MLText.DefaultCultureID;
			string ccID = MLText.CurrentCultureID;

			try
			{
				Assert.AreEqual(TextEn, m_MLText["fr"]);

				MLText.CurrentCultureID = "ru";
				Assert.AreEqual(TextRu, m_MLText["fr"]);

				m_MLText.RemoveText("ru");
				Assert.AreEqual(TextEn, m_MLText["fr"]);

				MLText.DefaultCultureID = "en-UK";
				Assert.AreEqual(TextEnUK, m_MLText["fr"]);
			}
			finally
			{
				MLText.CurrentCultureID = ccID;
				MLText.DefaultCultureID = dcID;
			}
		}

		[Test]
		public void Cultures()
		{
			m_MLText = new MLText();
			Assert.IsNotNull(m_MLText.Cultures);
			Assert.AreEqual(0, m_MLText.Cultures.Length);

			m_MLText["ru"] = TextRu;
			Assert.IsNotNull(m_MLText.Cultures);
			Assert.AreEqual(1, m_MLText.Cultures.Length);
			Assert.AreEqual("ru", m_MLText.Cultures[0]);

			m_MLText["en"] = TextEn;
			Assert.IsNotNull(m_MLText.Cultures);
			Assert.AreEqual(2, m_MLText.Cultures.Length);

			m_MLText["ru"] = "1" + TextRu;
			Assert.IsNotNull(m_MLText.Cultures);
			Assert.AreEqual(2, m_MLText.Cultures.Length);
		}

		private string GetCurrentCulture()
		{
			return m_CurrentCultureID;
		}

		private string GetWrongCurrentCulture()
		{
			return "dkjfhf";
		}

		private void SetCurrentCulture(string cId)
		{
			m_CurrentCultureID = cId;
		}

		[Test]
		public void CurrentCultureGet_FromCallback()
		{
			MLText.RequestCurrentCultureID = new MLText.RequestCurrentCultureIDCallback(GetCurrentCulture);
			try
			{
				Assert.AreEqual(m_CurrentCultureID, MLText.CurrentCultureID);
			}
			finally
			{
				MLText.RequestCurrentCultureID = null;
			}
		}

		[Test]
		public void CurrentCultureGet_FromCallback_Exception()
		{
			MLText.RequestCurrentCultureID = new MLText.RequestCurrentCultureIDCallback(GetWrongCurrentCulture);
			try
			{
				string ccID = MLText.CurrentCultureID;
				Assert.Fail();
			}
			catch { }
			finally
			{
				MLText.RequestCurrentCultureID = null;
			}
		}

		[Test]
		public void CurrentCultureSet_FromCallback()
		{
			MLText.PersistCurrentCultureID = new MLText.PersistCurrentCultureIDCallback(SetCurrentCulture);

			try
			{
				MLText.CurrentCultureID = "fr";
				Assert.AreEqual("fr", m_CurrentCultureID);
			}
			finally
			{
				MLText.PersistCurrentCultureID = null;
			}
		}

		[Test]
		public void ToStringTest()
		{
			m_MLText = new MLText();
			Assert.AreEqual(string.Empty, m_MLText.ToString());

			m_MLText["ru"] = TextRu;
			Assert.AreEqual(TextRu, m_MLText.ToString());

			m_MLText["en"] = TextEn;
			Assert.AreEqual(TextEn, m_MLText.ToString());

			string ccID = MLText.CurrentCultureID;

			try
			{
				MLText.CurrentCultureID = "ru";
				Assert.AreEqual(TextRu, m_MLText.ToString());
			}
			finally
			{
				MLText.CurrentCultureID = ccID;
			}
		}

		[Test]
		public void Equals_True()
		{
			m_MLText = new MLText();

			Assert.IsTrue(m_MLText.Equals(m_MLText));

			MLText mlt = new MLText();
			Assert.IsTrue(m_MLText.Equals(mlt));

			m_MLText["ru"] = TextRu;
			mlt["ru"] = TextRu;
			Assert.IsTrue(m_MLText.Equals(mlt));
		}

		[Test]
		public void Equals_False()
		{
			m_MLText = new MLText();
			Assert.IsFalse(m_MLText.Equals(null));

			Assert.IsFalse(m_MLText.Equals(1));

			MLText mlt = new MLText();
			m_MLText["ru"] = TextRu;
			Assert.IsFalse(m_MLText.Equals(mlt));

			mlt["ru"] = TextEn;
			Assert.IsFalse(m_MLText.Equals(mlt));

			mlt["ru"] = TextRu;
			mlt["en"] = TextEn;
			Assert.IsFalse(m_MLText.Equals(mlt));
		}

		[Test]
		public void OperatorEqual_True()
		{
			m_MLText = new MLText();
			Assert.IsTrue(m_MLText == m_MLText);

			MLText mlt = new MLText();
			Assert.IsTrue(m_MLText == mlt);

			m_MLText["ru"] = TextRu;
			mlt["ru"] = TextRu;
			Assert.IsTrue(m_MLText == mlt);
		}

		[Test]
		public void OperatorEqual_False()
		{
			m_MLText = new MLText();
			Assert.IsFalse(m_MLText == null);
			Assert.IsFalse(null == m_MLText);

			MLText mlt = new MLText();
			mlt["ru"] = TextRu;
			Assert.IsFalse(m_MLText == mlt);

			m_MLText["ru"] = TextEn;
			Assert.IsFalse(m_MLText == mlt);
		}

		[Test]
		public void OperatorNotEqual_False()
		{
			m_MLText = new MLText();
			Assert.IsFalse(m_MLText != m_MLText);

			MLText mlt = new MLText();
			Assert.IsFalse(m_MLText != mlt);

			m_MLText["ru"] = TextRu;
			mlt["ru"] = TextRu;
			Assert.IsFalse(m_MLText != mlt);
		}

		[Test]
		public void OperatorNotEqual_True()
		{
			m_MLText = new MLText();
			Assert.IsTrue(m_MLText != null);
			Assert.IsTrue(null != m_MLText);

			MLText mlt = new MLText();
			mlt["ru"] = TextRu;
			Assert.IsTrue(m_MLText != mlt);

			m_MLText["ru"] = TextEn;
			Assert.IsTrue(m_MLText != mlt);
		}

		[Test]
		public void OperatorPlusMLText()
		{
			m_MLText = new MLText();
			MLText mlt = m_MLText + (MLText) null;
			Assert.IsNotNull(mlt);
			Assert.AreEqual(mlt, m_MLText);

			mlt = (MLText)null + m_MLText;
			Assert.IsNotNull(mlt);
			Assert.AreEqual(mlt, m_MLText);

			m_MLText["ru"] = TextRu;
			mlt["en"] = TextEn;

			mlt = mlt + m_MLText;
			Assert.AreEqual(TextRu, mlt["ru"]);
			Assert.AreEqual(TextEn, mlt["en"]);

			mlt = new MLText("ru", TextEn);
			mlt = mlt + m_MLText;
			Assert.AreEqual(TextEn + TextRu, mlt["ru"]);
		}

		[Test]
		public void OperatorPlusString()
		{
			m_MLText = new MLText();
			MLText mlt = m_MLText + (string)null;
			Assert.IsNotNull(mlt);
			Assert.AreEqual(mlt, m_MLText);

			mlt = (string)null + m_MLText;
			Assert.IsNotNull(mlt);
			Assert.AreEqual(mlt, m_MLText);

			mlt = m_MLText + TextRu;
			Assert.AreEqual(TextRu, mlt[MLText.CurrentCultureID]);
		}

		[Test]
		public void ToXMLString()
		{
			m_MLText = new MLText();
			Assert.AreEqual("<MLText></MLText>", m_MLText.ToXMLString());

			m_MLText["ru"] = TextRu;
			Assert.AreEqual("<MLText><Text lang=\"ru\">" + TextRu + "</Text></MLText>", m_MLText.ToXMLString());
		}

		[Test]
		public void LoadFromXML()
		{
			m_MLText = new MLText();
			m_MLText["ru"] = TextRu;
			m_MLText["en"] = TextEn;

			MLText mlt = new MLText();
			mlt.LoadFromXML(m_MLText.ToXMLString());

			Assert.AreEqual(TextRu, mlt["ru"]);
			Assert.AreEqual(TextEn, mlt["en"]);

			mlt.LoadFromXML(new MLText().ToXMLString());
			Assert.AreEqual(0, mlt.Cultures.Length);

			mlt.LoadFromXML(TextEnUK);
			Assert.AreEqual(TextEnUK, mlt[MLText.CurrentCultureID]);
		}
	}
}
