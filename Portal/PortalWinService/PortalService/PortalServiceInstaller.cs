using System.Collections;
using System.ComponentModel;
using System.Configuration.Install;
using System.Configuration;
using UlterSystems.PortalService.Properties;

namespace UlterSystems.PortalService
{
	[RunInstaller(true)]
	public partial class PortalServiceInstaller : Installer
	{
		public PortalServiceInstaller()
		{
			InitializeComponent();
		}

		public override void Install(IDictionary stateSaver)
		{
			base.Install(stateSaver);

			var assemblyPath = Context.Parameters["assemblypath"];

			if (Context.Parameters.ContainsKey("splitlogger") && !string.IsNullOrEmpty("assemblyPath"))
			{
				var splitLogFile = Context.IsParameterTrue("splitlogger");
				var config = ConfigurationManager.OpenExeConfiguration(assemblyPath);

				config.AppSettings.Settings["SplitLogFile"].Value = splitLogFile ? bool.TrueString : bool.FalseString;
				config.Save();
			}
		}
	}
}