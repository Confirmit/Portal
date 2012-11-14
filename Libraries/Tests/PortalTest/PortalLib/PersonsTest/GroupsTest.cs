using System;
using System.Collections.Generic;
using System.Text;

using NUnit.Framework;
using UlterSystems.PortalLib.BusinessObjects;

namespace PortalTest.PortalLibTest.PersonsTest
{
	[TestFixture]
	[Category("Person")]
	public class GroupsTest
	{
		private Group m_Group;

		[TestFixtureSetUp]
		public void SetUp()
		{
			Utils.InitDBConnection();
		}

		[Test]
		public void GetGroup()
		{
			m_Group = Group.GetGroup( Group.GroupsEnum.Administrator );
			Assert.IsNotNull(m_Group);
			Assert.AreEqual(Group.GroupsEnum.Administrator, m_Group.GroupType);
			Assert.AreEqual(Group.GroupsEnum.Administrator.ToString(), m_Group.GroupID);
		}

		[Test]
		public void GetAllGroups()
		{
			Group[] arr = Group.GetAllGroups();
			Assert.IsNotNull(arr);
			Assert.IsTrue(arr.Length > 0);
		}

		[Test]
		public void AddRemoveMember()
		{
			Person person = new Person();
			person.LastName = new Core.MLText("en", "Yakimov");
			person.PersonGender = Person.Gender.Male;
			person.Save();

			Assert.IsFalse(person.IsInGroup(Group.GroupsEnum.Administrator));
			Assert.AreEqual(0, person.Groups.Length);

			Group admins = Group.GetGroup(Group.GroupsEnum.Administrator);
			admins.AddMember(person);

			Assert.IsTrue(person.IsInGroup(Group.GroupsEnum.Administrator));
			Assert.AreEqual(1, person.Groups.Length);

			admins.RemoveMember(person);

			Assert.IsFalse(person.IsInGroup(Group.GroupsEnum.Administrator));
			Assert.AreEqual(0, person.Groups.Length);

			person.Delete();
		}

		[Test]
		public void Members()
		{
			Person person = new Person();
			person.LastName = new Core.MLText("en", "Yakimov");
			person.PersonGender = Person.Gender.Male;
			person.Save();

			Group admins = Group.GetGroup(Group.GroupsEnum.Administrator);
			admins.AddMember(person);

			Person[] arr = admins.Members;
			Assert.IsNotNull(arr);
			Assert.IsTrue(arr.Length > 0);

			admins.RemoveMember(person);

			person.Delete();
		}
	}
}
