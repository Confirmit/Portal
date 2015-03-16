using System.Configuration;
using System.ServiceProcess;
using Core.DB;

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

            //ConnectionManager.ConnectionTypeResolve += ConnectionTypeResolver;
            //ConnectionManager.DefaultConnectionString = ConfigurationManager.ConnectionStrings["DBConnStr"].ConnectionString;
            //TimerMethods.NotifyNonRegisteredUsers("fsdf");

			// More than one user Service may run within the same process. To add
			// another service to this process, change the following line to
			// create a second service object. For example,
			//
			//   ServicesToRun = new ServiceBase[] {new Service1(), new MySecondUserService()};
			//

            ServicesToRun = new ServiceBase[] { new PortalService() };
            new PortalService().Start();

			ServiceBase.Run(ServicesToRun);
		}
        public static ConnectionType ConnectionTypeResolver(ConnectionKind kind)
        {
            return ConnectionType.SQLServer;
        }
	}
}