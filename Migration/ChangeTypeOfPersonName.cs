using Core;
using FluentMigrator;
using Migration.UserTableMigration;
using Migration.Utilities;

namespace Migration
{
    [Migration(2)]
    public class ChangeTypeOfPersonName : FluentMigrator.Migration
    {
        public void CreateColumn(string tableName, string columnName, int size = 0)
        {
            if (!(Schema.Table(tableName).Column(columnName).Exists()))
            {
                if (size == 0)
                    Create.Column(columnName).OnTable(tableName).AsAnsiString().Nullable();
                else
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
            CreateColumn("Users", "FirstName");
            CreateColumn("Users", "MiddleName");
            CreateColumn("Users", "LastName");

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
            var users = UserList.GetUserList<MLText>();

            foreach (var user in users)
            {
                var settings = GetSeparatedSettings(user);
                Update.Table(tableName).Set(settings).Where(new {ID = user.ID});
            }
        }

        private void AddUsersToUniversalColumns(string tableName)
        {
            new ConnectionProvider().Connect(this.ConnectionString);
            var users = UserList.GetUserList<MLString>();

            foreach (var user in users)
            {
                var settings = GetUnivrsalSettings(user);
                Update.Table(tableName).Set(settings).Where(new {ID = user.ID});
            }
        }

        private object GetUnivrsalSettings(Person<MLString> user)
        {
            return new
            {
                FirstName = user.FirstName.ToXMLString(),
                MiddleName = user.MiddleName.ToXMLString(),
                LastName = user.LastName.ToXMLString()
            };
        }

        private object GetSeparatedSettings(Person<MLText> user)
        {
            const string en = "en";
            const string ru = "ru";
            return new
            {
                FirstName_en = user.FirstName.ContainsCulture(en) ? user.FirstName[en] : "",
                FirstName_ru = user.FirstName.ContainsCulture(ru) ? user.FirstName[ru] : "",
                MiddleName_en = user.MiddleName.ContainsCulture(en) ? user.MiddleName[en] : "",
                MiddleName_ru = user.MiddleName.ContainsCulture(ru) ? user.MiddleName[ru] : "",
                LastName_en = user.LastName.ContainsCulture(en) ? user.LastName[en] : "",
                LastName_ru = user.LastName.ContainsCulture(ru) ? user.LastName[ru] : ""
            };
        }
    }
}
