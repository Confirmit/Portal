using Core;
using FluentMigrator;

namespace Migration.Migrations
{
    [Migration(3)]
    public class HRManagerRoleMigration : FluentMigrator.Migration
    {
        readonly MLText _name = new MLText("en", "HR Managers", "ru", "Менеджеры по персоналу");
        private const string RoleId = "HRManager";

        public override void Up()
        {
            if (Schema.Table("Groups").Exists())
                Insert.IntoTable("Groups").Row(GetSettings());
        }

        public override void Down()
        {
            if (Schema.Table("Groups").Exists())
                Delete.FromTable("Groups").Row(new { GroupId = RoleId }); 
        }

        private object GetSettings()
        {
            return new
            {
                GroupID = RoleId,
                Name = _name.ToXMLString(),
            };
        }
    }
}
