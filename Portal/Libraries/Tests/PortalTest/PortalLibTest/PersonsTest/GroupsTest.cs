using NUnit.Framework;
using UlterSystems.PortalLib.BusinessObjects;

namespace PortalTest.PortalLibTest.PersonsTest
{
/*	[TestFixture]
	[Category( "Person" )]
	public class GroupsTest : DBFixture
	{
		private Group group;
		private Group loaded;

		[SetUp]
		public void SetUp()
		{
			group = new Group();
			group.GroupType = Group.GroupsEnum.Administrator;
			group.Name = new Core.MLText( "en", "Administrators" );
			group.Description = new Core.MLText( "en", "Group of administrators" );
			group.Save();

			loaded = new Group();
			loaded.Load( group.ID.Value );
		}

		[TearDown]
		public void TearDown()
		{
			group.Delete();
		}

		[Test]
		public void ID()
		{
			Assert.AreEqual( group.ID.Value, loaded.ID.Value );
		}

		[Test]
		public void GroupID()
		{
			Assert.AreEqual( group.GroupID, loaded.GroupID );
		}

		[Test]
		public void GroutType()
		{
			Assert.AreEqual( group.GroupType, loaded.GroupType );
		}

		[Test]
		public void Name()
		{
			Assert.AreEqual( group.Name, loaded.Name );
		}

		[Test]
		public void Description()
		{
			Assert.AreEqual( group.Description, loaded.Description );
		}

		[Test]
		public void Members()
		{
			Assert.IsNotNull( group.Members );
			Assert.AreEqual( 0, group.Members.Length );
		}

		[Test]
		public void GetGroup()
		{
			Group adminGroup = Group.GetGroup( Group.GroupsEnum.Administrator );
			Assert.IsNotNull( adminGroup );
			Assert.AreEqual( Group.GroupsEnum.Administrator, adminGroup.GroupType );
			Assert.AreEqual( Group.GroupsEnum.Administrator.ToString(), adminGroup.GroupID );
			Assert.AreEqual( group.Name, adminGroup.Name );
			Assert.AreEqual( group.Description, adminGroup.Description );
		}

		[Test]
		public void GetAllGroups()
		{
			Group[] arr = Group.GetAllGroups();
			Assert.IsNotNull( arr );
			Assert.IsTrue( arr.Length > 0 );
		}

		[Test]
		public void AddRemoveMember()
		{
			Person p = new Person();

			try
			{
				p.Save();

				try
				{
					group.AddMember( p );

					Assert.IsNotNull( group.Members );
					Assert.AreEqual( 1, group.Members.Length );

					Person member = group.Members[ 0 ];
					Assert.AreEqual( member.ID.Value, p.ID.Value );
				}
				finally
				{ group.RemoveMember( p ); }

			}
			finally
			{ p.Delete(); }
		}
	}*/
}
