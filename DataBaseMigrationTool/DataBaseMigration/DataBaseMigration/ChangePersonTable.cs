using FluentMigrator;
using Core;
using Core.DB;
using DataBaseMigration.UserTableMigration;
using DataBaseMigration.Utilities;
using UserList = DataBaseMigration.UserTableMigration.UserList;

namespace DataBaseMigration
{
    [Migration(2)]
    public class CreateUserTable : Migration
    {
       
       

        public void CreateColumn(string tableName, string columnName, int size)
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

        private void AddUsersToSeparatedColumns(string tableName)
        {
           new ConnectionProvider().Connect();

            var users = UserList.GetUserList<MLText>();

            foreach (var user in users)
            {
                AddUserToSeparatedColumns(user, tableName);
            }
        }

        private void AddUserToSeparatedColumns(Person<MLText> user, string tableName)
        {
            MLText.CurrentCultureID = "en";
            var settings = GetEngSettings(user);
            Update.Table(tableName).Set(settings).Where(new { ID = user.ID });

            MLText.CurrentCultureID = "ru";
            settings = GetRusSettings(user);
            Update.Table(tableName).Set(settings).Where(new { ID = user.ID });
        }

        public override void Down()
        {
            CreateColumn("Users", "FirstName", 0);
            CreateColumn("Users", "MiddleName", 0);
            CreateColumn("Users", "LastName", 0);

            AddUsersToUniversalColumns("Users");

            DeleteColumns("Users", "FirstName_ru");
            DeleteColumns("Users", "MiddleName_ru");
            DeleteColumns("Users", "LastName_ru");
            DeleteColumns("Users", "FirstName_en");
            DeleteColumns("Users", "MiddleName_en");
            DeleteColumns("Users", "LastName_en");
        }

        private void AddUsersToUniversalColumns(string tableName)
        {
            new ConnectionProvider().Connect();
            var users = UserList.GetUserList<MLString>();

            foreach (var user in users)
            {
                AddUserToUniversalColumns(user, tableName);
            }
        }

        private void AddUserToUniversalColumns(Person<MLString> user, string tableName)
        {
            var settings = GetUnivrsalSettings(user);
            Update.Table(tableName).Set(settings).Where(new { ID = user.ID });
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

        private object GetEngSettings(Person<MLText> user)
        {
            return new
            {
                FirstName_en = user.FirstName.ContainsCulture("en") ? user.FirstName.ToString() : "",
                MiddleName_en = user.MiddleName.ContainsCulture("en") ? user.MiddleName.ToString() : "",
                LastName_en = user.LastName.ContainsCulture("en") ? user.LastName.ToString() : ""
            };
        }

        private object GetRusSettings(Person<MLText> user)
        {
            return new
            {
                FirstName_ru = user.FirstName.ContainsCulture("ru") ? user.FirstName.ToString() : "",
                MiddleName_ru = user.MiddleName.ContainsCulture("ru") ? user.MiddleName.ToString() : "",
                LastName_ru = user.LastName.ContainsCulture("ru") ? user.LastName.ToString() : ""
            };
        }
    }
}
