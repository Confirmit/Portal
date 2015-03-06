using System.Diagnostics;

namespace ConfirmIt.PortalLib.DAL
{
	/// <summary>
	/// Providers of site.
	/// </summary>
	public static class SiteProvider
	{
		/// <summary>
		/// Calendar provider.
		/// </summary>
		public static CalendarProvider Calendar
		{
			[DebuggerStepThrough]
			get { return CalendarProvider.Instance; }
		}

		/// <summary>
		/// Roles provider.
		/// </summary>
		public static RolesProvider Roles
		{
			[DebuggerStepThrough]
			get { return RolesProvider.Instance; }
		}

		/// <summary>
		/// Books provider.
		/// </summary>
        public static RequestObjectsProvider RequestObjects
		{
			[DebuggerStepThrough]
            get { return RequestObjectsProvider.Instance; }
		}

		/// <summary>
        /// Ability provider.
        /// </summary>
        public static AbilityProvider Abilities
        {
            [DebuggerStepThrough]
            get { return AbilityProvider.Instance; }
        }

        /// <summary>
        /// Project provider.
        /// </summary>
        public static ProjectProvider Projects
        {
            [DebuggerStepThrough]
            get { return ProjectProvider.Instance; }
        }

		/// <summary>
		/// Offices provider.
		/// </summary>
		public static OfficesProvider Offices
		{
			[DebuggerStepThrough]
			get { return OfficesProvider.Instance; }
		}

		/// <summary>
		/// Work events provider.
		/// </summary>
		public static WorkEventsProvider WorkEvents
		{
			[DebuggerStepThrough]
			get { return WorkEventsProvider.Instance; }
		}

        /// <summary>
        /// Events provider.
        /// </summary>
        public static EventsProvider Events
        {
            [DebuggerStepThrough]
            get { return EventsProvider.Instance; }
        }

        /// <summary>
        /// Users provider.
        /// </summary>
        public static UsersProvider Users
        {
            [DebuggerStepThrough]
            get { return UsersProvider.Instance; }
        }
	}
}
