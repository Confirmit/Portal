//===============================================================================
	// Microsoft User Interface Process Application Block for .NET
	// http://msdn.microsoft.com/library/en-us/dnbda/html/uip.asp
	// 
	//===============================================================================
	// Copyright (C) 2000-2001 Microsoft Corporation
	// All rights reserved.
	// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
	// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
	// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR
	// FITNESS FOR A PARTICULAR PURPOSE.
	//==============================================================================

using System;
using System.Configuration;

namespace UIProcess
{
	#region Enum Definitions

	/// <summary>
	/// Specifies the expiration modes supported by the state cache in the user process manager.
	/// </summary>
	public enum CacheExpirationMode 
	{
		/// <summary>
		/// Cache expiration cannot be changed.
		/// </summary>
		Absolute = 1,
		/// <summary>
		/// Cache expriation varies.
		/// </summary>
		Sliding = 2,
		/// <summary>
		/// Cache does not expire.
		/// </summary>
		None = 3
	}

	#endregion

	#region UIPConfiguration class

	/// <summary>
	/// Helper class to obtain UIP configuration from the configuration file.
	/// </summary>
	public class UIPConfiguration
	{
		#region Constant members

		private const string UipConfigXmlFile = "UIPSettings.xml";
		private const string UipConfigSection = "uipConfiguration";
		
		#endregion

		private static UIPConfigSettings _currentConfig = null;

		/// <summary>
		/// Gets the UIP configuration.
		/// </summary>
		public static UIPConfigSettings Config
		{
			get
			{
				if (_currentConfig == null)
				{
					try
					{
						_currentConfig = UIPConfigSettings.Create(UipConfigXmlFile, UipConfigSection);
						//UIPConfigSettings (UIPConfigSettings)); ConfigurationSettings.GetConfig(UipConfigSection);
					}
					catch (Exception e)
					{
						throw new Exception(string.Format("ExceptionLoadUIPConfig - {0}.", e.Message));
					}

					if (_currentConfig == null)
						throw new ConfigurationErrorsException(string.Format("ExceptionUIPConfigNotFound"));
				}

				return _currentConfig;
			}
			set { _currentConfig = value; }
		}
	}

	#endregion
}