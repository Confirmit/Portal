using Core;
using FluentMigrator;
using Migration.UserTableMigration;
using Migration.Utilities;

namespace Migration
{
    [Migration(4)]
    public class UserTransliteration : FluentMigrator.Migration
    {
        public override void Up()
        {
            new ConnectionProvider().Connect(this.ConnectionString);
            var users = UserList.GetUserList<MLString>();

            foreach (var user in users)
            {
                var settings = GetEnglishSettings(user);
                Update.Table("Users").Set(settings).Where(new {ID = user.ID});
            }
        }

        public override void Down()
        {

        }

        private object GetEnglishSettings(Person<MLString> user)
        {
            string firstName = user.FirstName.EnglishValue;
            string middleName = user.MiddleName.EnglishValue;
            string lastName = user.LastName.EnglishValue;

            if (string.IsNullOrEmpty(user.FirstName.EnglishValue))
            {
                firstName = Transliteration.Front(user.FirstName.RussianValue, TransliterationType.Gost);
            }
            if (string.IsNullOrEmpty(user.MiddleName.EnglishValue))
            {
                middleName = Transliteration.Front(user.MiddleName.RussianValue, TransliterationType.Gost);
            }
            if (string.IsNullOrEmpty(user.LastName.EnglishValue))
            {
                lastName = Transliteration.Front(user.LastName.RussianValue, TransliterationType.Gost);
            }

            return new
            {
                FirstName_en = firstName,
                MiddleName_en = middleName,
                LastName_en = lastName
            };
        }
    }
}
