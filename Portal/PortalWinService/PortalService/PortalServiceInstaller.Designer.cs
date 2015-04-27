namespace UlterSystems.PortalService
{
	partial class PortalServiceInstaller
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.m_PortalServiceInstaller = new System.ServiceProcess.ServiceInstaller();
            this.m_PortalServiceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            // 
            // m_PortalServiceInstaller
            // 
            this.m_PortalServiceInstaller.Description = "Service of Portal";
            this.m_PortalServiceInstaller.DisplayName = "Portal Service";
            this.m_PortalServiceInstaller.ServiceName = "PortalService";
            this.m_PortalServiceInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // m_PortalServiceProcessInstaller
            // 
            this.m_PortalServiceProcessInstaller.Password = null;
            this.m_PortalServiceProcessInstaller.Username = null;
            // 
            // PortalServiceInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.m_PortalServiceInstaller,
            this.m_PortalServiceProcessInstaller});

		}

		#endregion

		private System.ServiceProcess.ServiceInstaller m_PortalServiceInstaller;
		private System.ServiceProcess.ServiceProcessInstaller m_PortalServiceProcessInstaller;

	}
}