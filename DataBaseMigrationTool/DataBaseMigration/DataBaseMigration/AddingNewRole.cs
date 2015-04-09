using Core;
using FluentMigrator;

namespace DataBaseMigration
{
    [Migration(3)]
    public class AddingNewRole : Migration
    {
        readonly MLText _name = new MLText("en", "HR Managers", "ru", "Менеджеры по персоналу");
        readonly MLText _description = new MLText("en", "HR Managers", "ru", "Менеджеры по персоналу");
        private const string RoleId = "HRManager";

        private object GetSettings()
        {
            return new
            {
                GroupID = RoleId,
                Name = _name.ToXMLString(),
                Description = _description.ToXMLString()
            };
        }

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
    }
}
