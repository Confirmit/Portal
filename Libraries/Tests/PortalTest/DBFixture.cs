using System;
using System.Collections.Generic;
using System.Text;

using NUnit.Framework;

namespace PortalTest
{
	public abstract class DBFixture
	{
		[TestFixtureSetUp]
		public void FixtureSetUp()
		{
			Utils.InitDBConnection();
		}
	}
}
