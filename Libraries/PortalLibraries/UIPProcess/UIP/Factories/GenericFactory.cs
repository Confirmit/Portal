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
using System.Reflection;

namespace UIProcess
{
	/// <summary>
	/// Acts as the basic implementation of the multiple factory classes used in UIProcess.
	/// UIP needs to create instances based on configuration information for state, state persistence providers, ViewManagers, and so on.
	/// UIP has factories for these, and because there is much common code for doing reflection-based activation,
	/// it keeps that code in one central place.
	/// </summary>
	public sealed class GenericFactory
	{
		#region Constructors

		/// <summary>
		/// Static constructor.
		/// </summary>
        static GenericFactory(){}


		private GenericFactory(){}


		#endregion

		#region Create Overloads
        /// <summary>
        /// Creates an object using the full name type contained in typeSettings.
        /// </summary>
        /// <param name="typeSettings">A typeSetting object with the needed type information to create a class instance.</param>
        /// <returns>An instance of the specified type.</returns>
        
        public static object Create(ObjectTypeSettings typeSettings)
        {
            return Create(typeSettings, null);
        }

        /// <summary>
		/// Creates an object using the full name type contained in typeSettings.
		/// </summary>
		/// <param name="typeSettings">A typeSetting object with the needed type information to create a class instance.</param>
		/// <param name="args">Constructor arguments.</param>
		/// <returns>An instance of the specified type.</returns>
		public static object Create(ObjectTypeSettings typeSettings, object[] args)
		{
            Type typeInstance = CreateTypeInstance(typeSettings);
            return Create(typeInstance, args);
		}

        /// <summary>
        /// Creates an object using the type, created from typeSettings.
        /// </summary>
        /// <param name="typeInstance">A typeInstance object to create a class instance.</param>
        /// <returns>An instance of the specified type.</returns>
        public static object Create(Type typeInstance)
        {
            return Create(typeInstance, null);
        }

        /// <summary>
        /// Creates an object using the type, created from typeSettings.
        /// </summary>
        /// <param name="typeInstance">A typeInstance object to create a class instance.</param>
        /// <param name="args">Constructor arguments.</param>
        /// <returns>An instance of the specified type.</returns>
        public static object Create(Type typeInstance, object[] args)
        {
            try
            {
                if (args != null)
                    return Activator.CreateInstance(typeInstance, args);
                else
                    return Activator.CreateInstance(typeInstance);
            }
            catch (Exception e)
            {
                throw new Exception(
                    string.Format("ExceptionCantCreateInstanceUsingActivate: {0} - {1}.", typeInstance, e.Message));
            }
        }

        public static Type CreateTypeInstance(ObjectTypeSettings typeSettings)
        {
            Assembly assemblyInstance = null;
            Type typeInstance = null;

            try
            {
                //  Use full assembly name to get assembly instance
                assemblyInstance = Assembly.Load(typeSettings.Assembly);
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("ExceptionCantLoadAssembly: {0} - {1}.", typeSettings.Assembly,
                                                  e.Message));
            }

            //  use type name to get type from assembly
            try
            {
                typeInstance = assemblyInstance.GetType(typeSettings.Type, true, false);
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("ExceptionCantGetTypeFromAssembly: {0} - {1} - {2}.",
                                                  typeSettings.Type, typeSettings.Assembly, e.Message));
            }

            return typeInstance;
        }

		#endregion
	}
}
