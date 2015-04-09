using System.Text;
using Core;

namespace DataBaseMigration.UserTableMigration
{
    public static class ExtendForMLString
    {
        public static string ToXMLString(this MLString mlstring)
        {
			StringBuilder sb = new StringBuilder();
			sb.Append("<MLText>");

				sb.Append("<Text lang=\"ru\">");
                sb.Append(mlstring.RussianValue);
				sb.Append("</Text>");

                sb.Append("<Text lang=\"en\">");
                sb.Append(mlstring.EnglishValue);
                sb.Append("</Text>");

			sb.Append("</MLText>");
			return sb.ToString();
        }
    }
}
