using Core;
using FluentMigrator;
using Migration.Utility;
using Migration.Utility.PersonNameType;

namespace Migration.Migrations
{
    [Migration(2)]
    public class PersonNameTypeMigration : FluentMigrator.Migration
    {
        public void CreateColumn(string tableName, string columnName, int size)
        {
            if (!(Schema.Table(tableName).Column(columnName).Exists()))
            {
                Create.Column(columnName).OnTable(tableName).AsAnsiString(size).Nullable();
            }
        }

        public void DeleteColumns(string tableName, string columnName)
        {
            if (Schema.Table(tableName).Column(columnName).Exists())
                Delete.Column(columnName).FromTable(tableName);
        }

        public override void Up()
        {
            CreateColumn("Users", "FirstName_ru", 50);
            CreateColumn("Users", "MiddleName_ru", 50);
            CreateColumn("Users", "LastName_ru", 100);
            CreateColumn("Users", "FirstName_en", 50);
            CreateColumn("Users", "MiddleName_en", 50);
            CreateColumn("Users", "LastName_en", 100);

            AddUsersToSeparatedColumns("Users");

            DeleteColumns("Users", "FirstName");
            DeleteColumns("Users", "MiddleName");
            DeleteColumns("Users", "LastName");
        }

        public override void Down()
        {
            CreateColumn("Users", "FirstName", 255);
            CreateColumn("Users", "MiddleName", 255);
            CreateColumn("Users", "LastName", 255);

            AddUsersToUniversalColumns("Users");

            DeleteColumns("Users", "FirstName_ru");
            DeleteColumns("Users", "MiddleName_ru");
            DeleteColumns("Users", "LastName_ru");
            DeleteColumns("Users", "FirstName_en");
            DeleteColumns("Users", "MiddleName_en");
            DeleteColumns("Users", "LastName_en");
        }

        private void AddUsersToSeparatedColumns(string tableName)
        {
            new ConnectionProvider().Connect(this.ConnectionString);
            var users = PersonConverter.ConvertPerson(UserList.GetUserList<MLText>());

            foreach (var user in users)
            {
                var settings = GetSeparatedSettings(user);
                Update.Table(tableName).Set(settings).Where(new { ID = user.ID });
            }
        }

        private void AddUsersToUniversalColumns(string tableName)
        {
            new ConnectionProvider().Connect(this.ConnectionString);
            var users = PersonConverter.ConvertPerson(UserList.GetUserList<MLString>());

            foreach (var user in users)
            {
                var settings = GetUnivrsalSettings(user);
                Update.Table(tableName).Set(settings).Where(new { ID = user.ID });
            }
        }

        private object GetUnivrsalSettings(Person<MLText> user)
        {
            return new
            {
                FirstName = user.FirstName.ToXMLString(),
                MiddleName = user.MiddleName.ToXMLString(),
                LastName = user.LastName.ToXMLString()
            };
        }

        private object GetSeparatedSettings(Person<MLString> user)
        {
            return new
            {
                FirstName_en = user.FirstName.EnglishValue,
                MiddleName_en = user.MiddleName.EnglishValue,
                LastName_en = user.LastName.EnglishValue,

                FirstName_ru = user.FirstName.RussianValue,
                MiddleName_ru = user.MiddleName.RussianValue,
                LastName_ru = user.LastName.RussianValue
            };
        }
    }
}
