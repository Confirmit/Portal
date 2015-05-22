using System;
using System.Windows.Forms.VisualStyles;
using NUnit.Framework;
using Core;
using UlterSystems.PortalLib.BusinessObjects;
using System.Collections.Generic;

namespace PortalTest.PortalLibTest.PersonsTest
{
	[TestFixture]
	[Category( "Person" )]
	public class PersonTest : DBFixture
	{
		private string domainName = @"ultersysyar\test";

		private PersonAttribute domainNameAttr;
		private Person person;
		private Person loaded;
		//private Group group;

		[SetUp]
		public void SetUp()
		{
			person = new Person();
			person.Birthday = DateTime.Now;
			person.EmployeesUlterSYSMoscow = true;
			person.FirstName = new MLText( "en", "Tester", "ru", "Тест" );
			person.LastName = new MLText( "en", "Tester", "ru", "Тестов" );
			person.LongServiceEmployees = true;
			person.MiddleName = new MLText( "en", "T.", "ru", "Тестович" );
			person.PersonnelReserve = true;
			person.PrimaryEMail = "test@ultersys.ru";
			person.PrimaryIP = "127.0.0.1";
			person.Project = "Project";
			person.Room = "Room";
			person.Sex = Person.UserSex.Female;
			person.Save();

			domainNameAttr = new PersonAttribute();
			domainNameAttr.StringField = domainName;
			domainNameAttr.ValueType = typeof( string ).AssemblyQualifiedName;
			domainNameAttr.PersonID = person.ID.Value;
			domainNameAttr.Save();

			loaded = new Person();
			loaded.Load( person.ID.Value );

			/*group = new Group();
			group.GroupType = Group.GroupsEnum.Employee;
			group.Name = new MLText( "en", "Employees" );
			group.Description = new MLText( "en", "Employees" );
			group.Save();*/
		}

		[TearDown]
		public void TearDown()
		{
			domainNameAttr.Delete();
			person.Delete();
			//group.Delete();
		}

		[Test]
		public void Birthday()
		{ Assert.IsTrue( ( person.Birthday - loaded.Birthday ).Value.TotalSeconds < 1.0 ); }

		[Test]
		public void DomainNames()
		{
			Assert.IsNotNull( person.DomainNames );
			Assert.AreEqual( 1, person.DomainNames.Length );
			Assert.AreEqual( domainName, person.DomainNames[ 0 ] );
		}

		[Test]
		public void EmployeesUlterSYSMoscow()
		{ Assert.AreEqual( person.EmployeesUlterSYSMoscow, loaded.EmployeesUlterSYSMoscow ); }

		[Test]
		public void FirstName()
		{ Assert.AreEqual( person.FirstName, loaded.FirstName ); }

		[Test]
		public void FullName()
		{ Assert.AreEqual( person.FullName, loaded.FullName ); }

		[Test]
		public void LastName()
		{ Assert.AreEqual( person.LastName, loaded.LastName ); }

		[Test]
		public void LongServiceEmployees()
		{ Assert.AreEqual( person.LongServiceEmployees, loaded.LongServiceEmployees ); }

		[Test]
		public void MiddleName()
		{ Assert.AreEqual( person.MiddleName, loaded.MiddleName ); }

		[Test]
		public void PersonnelReserve()
		{ Assert.AreEqual( person.PersonnelReserve, loaded.PersonnelReserve ); }

		[Test]
		public void PrimaryEMail()
		{ Assert.AreEqual( person.PrimaryEMail, loaded.PrimaryEMail ); }

		[Test]
		public void PrimaryIP()
		{ Assert.AreEqual( person.PrimaryIP, loaded.PrimaryIP ); }

		[Test]
		public void Project()
		{ Assert.AreEqual( person.Project, loaded.Project ); }

		[Test]
		public void Room()
		{ Assert.AreEqual( person.Room, loaded.Room ); }

		[Test]
		public void Sex()
		{ Assert.AreEqual( person.Sex, loaded.Sex ); }

		[Test]
		public void SexID()
		{ Assert.AreEqual( person.SexID, loaded.SexID ); }

		[Test]
		[ExpectedException( typeof( ArgumentNullException ) )]
		public void LoadByDomainName_Exception()
		{
			Person p = new Person();
			p.LoadByDomainName( null );
		}

		[Test]
		public void LoadByDomainName_True()
		{
			Person p = new Person();
			Assert.IsTrue( p.LoadByDomainName( domainName.ToUpper() ) );
			Assert.AreEqual( person.ID.Value, p.ID.Value );
		}

		[Test]
		public void LoadByDomainName_False()
		{
			Person p = new Person();
			Assert.IsFalse( p.LoadByDomainName( "unknown" ) );
		}

		[Test]
		public void Groups()
		{
			try
			{
				Assert.IsNotNull( person.Roles );
				Assert.AreEqual( 0, person.Roles.Count );

				//group.AddMember( person );

				Assert.IsNotNull( person.Roles );
				Assert.AreEqual( 1, person.Roles.Count );

				//Group emplGroup = person.Roles[ 0 ];
				//Assert.AreEqual( group.ID.Value, emplGroup.ID.Value );
			}
			finally
			{
				//group.RemoveMember( person );
			}
		}

		[Test]
		public void GroupNames()
		{
			try
			{
				Assert.IsNotNull( person.GroupNames );
				Assert.AreEqual( 0, person.GroupNames.Length );

			//	group.AddMember( person );

				Assert.IsNotNull( person.GroupNames );
				Assert.AreEqual( 1, person.GroupNames.Length );

				string emplGroupName = person.GroupNames[ 0 ];
				//Assert.AreEqual( group.GroupID, emplGroupName );
			}
			finally
			{
				//group.RemoveMember( person );
			}
		}

		[Test]
		public void IsInRole_False()
		{
			Assert.IsFalse( person.IsInRole( "Employee" ) );
			/*Assert.IsFalse( person.IsInRole( Group.GroupsEnum.Employee ) );
			Assert.IsFalse( person.IsInRole( group ) );*/
		}

		[Test]
		public void IsInRole_True()
		{
			/*try
			{
				group.AddMember( person );

				Assert.IsTrue( person.IsInRole( "Employee" ) );
				Assert.IsTrue( person.IsInRole( Group.GroupsEnum.Employee ) );
				Assert.IsTrue( person.IsInRole( group ) );
			}
			finally
			{ group.RemoveMember( person ); }*/
		}

		[Test]
		public void GetPersonByDomainName_Null()
		{
			Assert.IsNull( Person.GetPersonByDomainName( null ) );
			Assert.IsNull( Person.GetPersonByDomainName( "" ) );
			Assert.IsNull( Person.GetPersonByDomainName( "unknown" ) );
		}

		[Test]
		public void GetPersonByDomainName_NotNull()
		{
			Person p = Person.GetPersonByDomainName( domainName.ToUpper() );
			Assert.IsNotNull( p );
			Assert.AreEqual( person.ID.Value, p.ID.Value );

			Person p2 = Person.GetPersonByDomainName( domainName );
			Assert.AreSame( p, p2 );
		}

		[Test]
		public void GetPersonByID_Null()
		{
			Assert.IsNull( Person.GetPersonByID( -1 ) );
		}

		[Test]
		public void GetPersonByID_NotNull()
		{
			Person p = Person.GetPersonByID( person.ID.Value );
			Assert.IsNotNull( p );
			Assert.AreEqual( person.ID.Value, p.ID.Value );

			Person p2 = Person.GetPersonByID( person.ID.Value );
			Assert.AreSame( p, p2 );
		}

		[Test]
		public void AddStandartStringAttribute_Null()
		{
			Assert.IsNull( person.AddStandardStringAttribute( PersonAttributeTypes.DomainName, null ) );
			Assert.IsNull( person.AddStandardStringAttribute( PersonAttributeTypes.DomainName, string.Empty ) );

			Person p = new Person();
			Assert.IsNull( p.AddStandardStringAttribute( PersonAttributeTypes.DomainName, "aaa" ) );
		}

		[Test]
		public void AddStandartStringAttribute_NotNull()
		{
			try
			{
				PersonAttribute pa = person.AddStandardStringAttribute( PersonAttributeTypes.PublicPassword, "test@ultersys.ru" );
				Assert.IsNotNull( pa );

				IList<PersonAttribute> attrs = PersonAttributes.GetPersonAttributesByKeyword( person.ID.Value, PersonAttributeTypes.PublicPassword.ToString() );
				Assert.IsNotNull( attrs );
				Assert.AreEqual( 1, attrs.Count );
				Assert.AreEqual( "test@ultersys.ru", attrs[ 0 ].Value );
				Assert.AreEqual( pa.Value, attrs[ 0 ].Value );
			}
			finally
			{ person.RemoveStandardAttributes( new PersonAttributeType() ); }
		}
	}
}
