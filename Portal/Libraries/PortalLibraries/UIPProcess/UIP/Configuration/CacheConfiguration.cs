//===============================================================================
// IT Co. User Interface Process Application Block for .NET
//
// ControllerFactory.cs
//
// This file contains the implementations of the ControllerFactory class.
//
// For more information see the User Interface Process Application Block Implementation Overview. 
// 
//===============================================================================
// Copyright (C) 2007-2008 IT Co. Limited
// All rights reserved.
//==============================================================================

using System;

namespace UIProcess
{
	/// <summary>
	/// Stores information needed to configure the state cache.
	/// </summary>
	public class CacheConfiguration
	{
		private CacheExpirationMode _mode;
		private TimeSpan _interval;

		/// <summary>
		/// Create a CacheConfiguration object.
		/// </summary>
		/// <param name="mode">The expiration mode for the cache. <see cref="CacheExpirationMode"/></param>
		/// <param name="interval">How often to check for expiration.</param>
		public CacheConfiguration(CacheExpirationMode mode, TimeSpan interval)
		{
			_mode = mode;
			_interval = interval;
		}

		/// <summary>
		/// The expiration mode of the cache. <see cref="CacheExpirationMode"/>
		/// </summary>
		public CacheExpirationMode Mode
		{
			get { return _mode; }
		}

		/// <summary>
		/// The expiration interval for the cache.
		/// </summary>
		public TimeSpan Interval
		{
			get { return _interval; }
		}
	}
}
