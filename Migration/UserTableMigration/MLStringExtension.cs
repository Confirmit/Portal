using System.Text;
using Core;

namespace Migration.UserTableMigration
{
    public static class MLStringExtension
    {
        public static string ToXMLString(this MLString @string)
        {
			StringBuilder sb = new StringBuilder();
			sb.Append("<MLText>");

				sb.Append("<Text lang=\"ru\">");
                sb.Append(@string.RussianValue);
				sb.Append("</Text>");

                sb.Append("<Text lang=\"en\">");
                sb.Append(@string.EnglishValue);
                sb.Append("</Text>");

			sb.Append("</MLText>");
			return sb.ToString();
        }
    }
}
