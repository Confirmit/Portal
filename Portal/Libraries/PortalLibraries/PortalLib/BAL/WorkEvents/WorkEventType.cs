namespace ConfirmIt.PortalLib.BAL
{
	/// <summary>
	/// Types of work events.
	/// </summary>
	public enum WorkEventType
	{
        /// <summary>
        /// No data.
        /// </summary>
        NoData = 1,
		/// <summary>
		/// Main work in office.
		/// </summary>
		MainWork = 10,
		/// <summary>
		/// Break of work.
		/// </summary>
		TimeOff = 9,
		/// <summary>
		/// Lunch time.
		/// </summary>
		LanchTime = 3,
		/// <summary>
		/// Work out of office.
		/// </summary>
		OfficeOut = 7,
		/// <summary>
		/// Ill day.
		/// </summary>
		Ill = 11,
		/// <summary>
		/// Business trip day.
		/// </summary>
		BusinessTrip = 12,
		/// <summary>
		/// Vacation day.
		/// </summary>
		Vacation = 13,
		/// <summary>
		/// Trust ill day.
		/// </summary>
		TrustIll = 14,
		/// <summary>
		/// Studying english.
		/// </summary>
		StudyEnglish = 15,
	}
}
