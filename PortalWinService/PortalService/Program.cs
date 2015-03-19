using System;
using System.Configuration;
using System.ServiceProcess;
using ConfirmIt.PortalLib.BAL;
using ConfirmIt.PortalLib.Notification;
using Core.DB;
using UlterSystems.PortalLib.Notification;

namespace UlterSystems.PortalService
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		static void Main()
		{
			ServiceBase[] ServicesToRun;

            ConnectionManager.ConnectionTypeResolve += ConnectionTypeResolver;
            ConnectionManager.DefaultConnectionString = ConfigurationManager.ConnectionStrings["DBConnStr"].ConnectionString;
            
			// More than one user Service may run within the same process. To add
			// another service to this process, change the following line to
			// create a second service object. For example,
			//
			//   ServicesToRun = new ServiceBase[] {new Service1(), new MySecondUserService()};
			//
            //WorkEvent.CreateEvent(DateTime.Now.AddDays(-1), DateTime.Now.AddDays(-1).AddHours(1), 198, 10);
            //var wevent = WorkEvent.GetMainWorkEvent(198, DateTime.Today.AddDays(-1));
            ServicesToRun = new ServiceBase[] { new PortalService() };
            //new PortalService().Start();

			ServiceBase.Run(ServicesToRun);
		}
        public static ConnectionType ConnectionTypeResolver(ConnectionKind kind)
        {
            return ConnectionType.SQLServer;
        }
	}
}