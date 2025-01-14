using System;
using System.Collections;
using System.Collections.Specialized;
using System.Configuration;
using System.Web;
using System.Xml;

namespace UIProcess
{
	/// <summary>
	/// This class contains all of the UIP configuration from the configuration file.
	/// </summary>
	/// <remarks>
	/// The UIPConfigSettings hierarchy is as follows:
	///   UIPConfigSettings
	///     --- ObjectTypeSettings collection
	///     --- State keys collection
	///     --- ViewSettings collection
	///			--- SharedTransitionSettings collection
	///     --- NavigationGraphSettings
	///						--- SharedTransitionSettings collection
	///           --- NodeSettings collection
	///                 --- NavigateToSettings collection 
	/// </remarks>
	public class UIPConfigSettings
	{
		#region Declares variables

		private const string AttributeEnableStateCache = "enableStateCache";
		private const string AttributeExpirationMode = "cacheExpirationMode";
		private const string AttributeExpirationInterval = "cacheExpirationInterval";
		private const string AttributeAllowBackButton = "allowBackButton";
		private const string NodeObjectTypesXPath = "objectTypes";
		private const string NodeIViewManagerXPath = "iViewManager";
		private const string NodeILayoutManagerXPath = "layoutManager";
		private const string NodeStateXPath = "state";
		private const string NodeControllerXpath = "controllers/controller";
		private const string NodeCallInterceptorXpath = "interceptor";
		private const string NodeStateKeyXPath = "state-keys/state-key";
		private const string NodeViewXPath = "views/view";
		private const string NodeSharedTransitionsXPath = "sharedTransitions/sharedTransition";
		//private const string NodeWizardsXPath = "uipWizard";
		private const string NodePersistProviderXPath = "statePersistenceProvider";
		private const string NodeNavigationGraphXPath = "navigationGraph";
		private const string UserControlsXPath = "userControls";

		private bool _isStateCacheEnabled = true;
		private HybridDictionary _iLayoutManagerCollection;
		private HybridDictionary _iViewManagerCollection;
		private HybridDictionary _stateCollection;
		private HybridDictionary _interceptorCollection;
		private HybridDictionary _statePersistenceCollection;
		private Hashtable _stateKeyByNameCollection;
		private Hashtable _viewByNameCollection;
		private Hashtable _controllerByNameCollection;
		private HybridDictionary _navigatorCollection; 
		private HybridDictionary _globalSharedTransitions;
		private HybridDictionary _hostedControlsCollection;
		private ObjectTypeSettings _defaultViewManager;
		private ObjectTypeSettings _defaultState;
		private StatePersistenceProviderSettings _defaultStatePersistence;
		private CacheExpirationMode _defaultMode = CacheExpirationMode.None;
		private TimeSpan _defaultInterval = TimeSpan.MinValue;
		private bool _allowBackButton;

		#endregion

		public HybridDictionary InterceptorCollection
		{
			get { return _interceptorCollection; }
		}

		#region Constructors

		/// <summary>
		/// Constructor.
		/// </summary>
		public UIPConfigSettings( )
		{
			//Init the hashtables
			_iLayoutManagerCollection = new HybridDictionary();
			_iViewManagerCollection = new HybridDictionary();
			_stateCollection = new HybridDictionary();
			_interceptorCollection = new HybridDictionary();
			_statePersistenceCollection = new HybridDictionary();
			
			_viewByNameCollection = new Hashtable();
			_stateKeyByNameCollection = new Hashtable();
			_controllerByNameCollection = new Hashtable();

			_navigatorCollection = new HybridDictionary();
			_globalSharedTransitions=new HybridDictionary();
			_hostedControlsCollection = new HybridDictionary();
			_isStateCacheEnabled = true;
			_defaultViewManager = null;
			_defaultState = null;
			_defaultStatePersistence = null;
			_allowBackButton = true;
		}

		/// <summary>
		/// Creates an instance of the UIPConfigSettings class using the path fo setting file.
		/// </summary>
		/// <param name="configXmlFile">Setting xml file.</param>
		/// <param name="configSection">Name of section.</param>
		public static UIPConfigSettings Create(string configXmlFile, string configSection)
		{
			XmlNode root = null;
			if (String.IsNullOrEmpty(configXmlFile))
				return null;

			String xmlPath =
				string.Format("{0}\\{1}",
							  HttpContext.Current.Request.PhysicalApplicationPath
							  , configXmlFile);
			try
			{
				XmlDocument xmlMenuD = new XmlDocument();
				xmlMenuD.Load(xmlPath);
				XmlNodeList rootNodes = xmlMenuD.GetElementsByTagName(configSection);
				root = rootNodes[0];
			}
			catch
			{
				throw new Exception(string.Format("Cannot load UIPConfig file: {0}.", xmlPath));
			}

			return new UIPConfigSettings(root, System.Globalization.CultureInfo.CurrentCulture);
		}

		/// <summary>
		/// Creates an instance of the UIPConfigSettings class using the specified configNode.
		/// </summary>
		/// <param name="configNode">The XmlNode from the configuration file.</param>
		public UIPConfigSettings(XmlNode configNode)
			: this(configNode, System.Globalization.CultureInfo.InvariantCulture)
		{}

		/// <summary>
		/// Creates UIPConfigSettings from an XmlNode read of the app.config and an IFormatProvider.
		/// </summary>
		/// <param name="configNode">The XmlNode from the configuration file.</param>
		/// <param name="formatProvider">The provider.</param>
		public UIPConfigSettings(XmlNode configNode, IFormatProvider formatProvider) : this()
		{
			LoadAttributes(configNode, formatProvider);

			LoadViewManagementObjects(configNode);

			LoadStateKeys(configNode, formatProvider);

			LoadControllers(configNode, formatProvider);

			LoadViews(configNode, formatProvider);

			LoadSharedTransitions(configNode);
						
			LoadNavigationGraphs(configNode);

			LoadHostedControls(configNode);

			//LoadWizards(configNode, formatProvider);	
		}

		private void LoadHostedControls(XmlNode configNode) 
		{
			foreach( XmlNode hostedControlsNode in configNode.SelectNodes( UserControlsXPath ) ) 
			{
			  UserControlsSettings settings = new UserControlsSettings(hostedControlsNode);
				_navigatorCollection.Add(settings.Name, settings);
			}
		}

		private void LoadNavigationGraphs(XmlNode configNode)
		{
			//Get the configured navigation graphs
			NavigationGraphSettings navigationGraph;
			foreach( XmlNode navigationGraphNode in configNode.SelectNodes( NodeNavigationGraphXPath ) )
			{
				navigationGraph = new NavigationGraphSettings( navigationGraphNode );
				_navigatorCollection.Add( navigationGraph.Name, navigationGraph );
			}
		}


		private void LoadSharedTransitions(XmlNode configNode)
		{
			//Get the configured global shared transitions
			foreach (XmlNode sharedTransitionNode in configNode.SelectNodes( NodeSharedTransitionsXPath ) )
			{
				SharedTransitionSettings sharedTransition=new SharedTransitionSettings( sharedTransitionNode );
				if (!_globalSharedTransitions.Contains(sharedTransition.NavigateValue))
				{
					_globalSharedTransitions.Add(sharedTransition.NavigateValue, sharedTransition);
				}
				else
					throw new ConfigurationErrorsException(string.Format("ExceptionDuplicateGlobalSharedTransition {0}.",
																   sharedTransition.NavigateValue));

				
			}
		}
		private void LoadControllers(XmlNode configNode, IFormatProvider formatProvider)
		{
			ObjectTypeSettings typedObject;
			foreach (XmlNode viewNode in configNode.SelectNodes(NodeControllerXpath))
			{
				typedObject = new ControllerSettings(viewNode, formatProvider);
				if (!_controllerByNameCollection.ContainsKey(typedObject.Name))
				{
					_controllerByNameCollection.Add(typedObject.Name, typedObject);
				}
				else throw new Exception(string.Format("ExceptionViewSettingAlreadyConfigured {0}.", typedObject.Name));
			}
		}

		private void LoadViews(XmlNode configNode, IFormatProvider formatProvider)
		{
			ObjectTypeSettings typedObject;
			//Get the configured views
			foreach( XmlNode viewNode in configNode.SelectNodes(NodeViewXPath))
			{
				typedObject = new ViewSettings(viewNode, formatProvider);
				if (!_viewByNameCollection.ContainsKey(typedObject.Name))
				{
					_viewByNameCollection.Add(typedObject.Name, typedObject);
				}
				else throw new Exception(string.Format("ExceptionViewSettingAlreadyConfigured {0}.", typedObject.Name));
			}
		}

		private void LoadStateKeys(XmlNode configNode, IFormatProvider formatProvider)
		{
			ObjectSettings typedObject;
			//Get the configured views
			foreach (XmlNode keyNode in configNode.SelectNodes(NodeStateKeyXPath))
			{
				typedObject = new StateKeySettings(keyNode, formatProvider);
				if (!_stateKeyByNameCollection.ContainsKey(typedObject.Name))
				{
					_stateKeyByNameCollection.Add(typedObject.Name, typedObject);
				}
				else throw new Exception(string.Format("ExceptionViewSettingAlreadyConfigured {0}.", typedObject.Name));
			}
		}

		/*private void LoadWizards(XmlNode configNode, IFormatProvider formatProvider)
		{
			foreach (XmlNode wizardNode in configNode.SelectNodes( NodeWizardsXPath ) )
			{
				WizardSettings wizard = new WizardSettings(_defaultStatePersistence.Name,_defaultState.Name,wizardNode);	
				NavigationGraphSettings navigationGraph = new NavigationGraphSettings(wizard.GetNavGraphXmlNode(formatProvider));
				_navigatorCollection.Add(navigationGraph.Name, navigationGraph);

			}
		}*/

		private void LoadViewManagementObjects(XmlNode configNode)
		{
			if (configNode.SelectSingleNode(NodeObjectTypesXPath) == null)
				return;
			
			//Get the configured IViewManager object types
			ObjectTypeSettings typedObject;
			foreach( XmlNode objectTypeNode in configNode.SelectSingleNode( NodeObjectTypesXPath ).ChildNodes )
			{
				switch( objectTypeNode.LocalName )
				{
					case NodeILayoutManagerXPath :					
						typedObject = new ObjectTypeSettings( objectTypeNode );
						_iLayoutManagerCollection.Add( typedObject.Name, typedObject );
						break;		
					case NodeIViewManagerXPath:
						typedObject = new ObjectTypeSettings( objectTypeNode );
						_iViewManagerCollection.Add( typedObject.Name, typedObject );
						if (IsDefault(objectTypeNode))
							_defaultViewManager = typedObject;
						break;
					case NodeStateXPath:
						typedObject = new ObjectTypeSettings( objectTypeNode );
						_stateCollection.Add( typedObject.Name, typedObject );
						if (IsDefault(objectTypeNode))
							_defaultState = typedObject;
						break;
					
					case NodeCallInterceptorXpath:
						typedObject = new ObjectTypeSettings(objectTypeNode);
						_interceptorCollection.Add(typedObject.Name, typedObject);
						break;

					case NodePersistProviderXPath:
						typedObject = new StatePersistenceProviderSettings( objectTypeNode );
						_statePersistenceCollection.Add( typedObject.Name, typedObject );
						if (IsDefault(objectTypeNode))
							_defaultStatePersistence = (StatePersistenceProviderSettings)typedObject;
						break;
				}
			}
		}


		private void LoadAttributes(XmlNode configNode, IFormatProvider formatProvider)
		{
			//Get the enableStateCache attribute
			XmlNode currentAttribute = configNode.Attributes.RemoveNamedItem(AttributeEnableStateCache);
			
			if(currentAttribute != null)
				_isStateCacheEnabled = Convert.ToBoolean(currentAttribute.Value,formatProvider);

			currentAttribute = configNode.Attributes.RemoveNamedItem(AttributeExpirationMode);

			if(currentAttribute != null)
			{
				_defaultMode = (CacheExpirationMode)Enum.Parse(typeof(CacheExpirationMode), currentAttribute.Value, true);                
				currentAttribute = configNode.Attributes.RemoveNamedItem(AttributeExpirationInterval);

				try
				{
					switch(_defaultMode)
					{
						case CacheExpirationMode.Sliding:
							_defaultInterval = new TimeSpan(0, 0, 0, 0, int.Parse(currentAttribute.Value, System.Globalization.CultureInfo.InvariantCulture));
							break;
						case CacheExpirationMode.Absolute:
							_defaultInterval = TimeSpan.Parse(currentAttribute.Value);
							if (_defaultInterval.Days > 0)
								throw new ConfigurationErrorsException(string.Format("_ExceptionInvalidAbsoluteInterval"));
							break;
					}
				}
				catch(Exception e)
				{
					throw new ConfigurationErrorsException(string.Format("ExceptionInvalidCacheExpirationInterval - {0}.", e.Message));
				}
			}

			currentAttribute = configNode.Attributes.RemoveNamedItem(AttributeAllowBackButton);
			
			if(currentAttribute != null)
				_allowBackButton = Convert.ToBoolean(currentAttribute.Value, formatProvider);
			 
		}

		private bool IsDefault(XmlNode objectTypeNode) 
		{
			XmlAttribute defaultAttribute = objectTypeNode.Attributes["default"];
			if(defaultAttribute != null && defaultAttribute.Value == "true")
				return true;

			return false;
		}
		#endregion

		#region Get Methods

		/// <summary>
		/// Returns the settings for the shared transition.
		/// </summary>
		/// <param name="navigateValue">The name of the navigation graph element.</param>
		/// <returns>The settings.</returns>
		private SharedTransitionSettings GetSharedTransitionSettings(string navigateValue)
		{
			return (SharedTransitionSettings)_globalSharedTransitions[navigateValue];
		}

		/// <summary>
		/// Checks if a navigation graph by that name exists.
		/// </summary>
		/// <param name="navigationGraphName">The name of the navigation graph element.</param>
		/// <returns></returns>
		public bool ContainsNavigationGraphSettings(string navigationGraphName)
		{
			NavigationGraphSettings navigationGraph = (NavigationGraphSettings)_navigatorCollection[ navigationGraphName ];
			return navigationGraph != null;
		}

		/// <summary>
		/// Returns the settings for the navigation graph.
		/// </summary>
		/// <param name="navigationGraphName">The name of the navigation graph element.</param>
		/// <returns>The settings.</returns>
		public NavigationGraphSettings GetNavigationGraphSettings( string navigationGraphName )
		{
			NavigationGraphSettings navigationGraph = (NavigationGraphSettings)_navigatorCollection[ navigationGraphName ];
			if (navigationGraph == null)
				throw new Exception(string.Format("ExceptionNavigationGraphNotFound {0}.", navigationGraphName));

			return navigationGraph;
		}

		/// <summary>
		/// Checks if a hosted controls element exists.
		/// </summary>
		/// <param name="hostedControlsName">The name of the element to check for.</param>
		/// <returns></returns>
		public bool ContainsHostedControlsSettings(string hostedControlsName)
		{
		  UserControlsSettings settings = (UserControlsSettings) _navigatorCollection[ hostedControlsName ];
			
			return settings != null;
		}

		/// <summary>
		/// Returns the settings for hosted controls.
		/// </summary>
		/// <param name="hostedControlsName">The name of the hosted controls element.</param>
		/// <returns>The settings.</returns>
		public UserControlsSettings GetHostedControlsSettings( string hostedControlsName ) 
		{
		  UserControlsSettings settings = (UserControlsSettings) _navigatorCollection[ hostedControlsName ];
		  if (settings == null)
			  throw new Exception(string.Format("ExceptionHostedControlsNotFound - {0}.", hostedControlsName));
			return settings;

		}

		/// <summary>
		/// Gets the navigator settings.
		/// </summary>
		/// <param name="navigatorName">The navigator name.</param>
		/// <returns>The settings.</returns>
		public NavigatorSettings GetNavigatorSettings( string navigatorName ) 
		{
			NavigatorSettings settings = (NavigatorSettings) _navigatorCollection[ navigatorName ];
			return settings;
		}

		/// <summary>
		/// Returns an ObjectTypeSettings wrapper around type information for the IViewManager found in the configuration file.
		/// </summary>
		/// <param name="navigatorName">The name of the navigation graph.</param>
		/// <returns>The IViewManager settings configured for the specified navigation graph.</returns>
		public virtual ObjectTypeSettings GetIViewManagerSettingsFromNavigatorName( string navigatorName )
		{
			NavigatorSettings context = GetNavigatorSettings( navigatorName );
			return (ObjectTypeSettings)_iViewManagerCollection [ context.ViewManager ];
		}

		/// <summary>
		/// Returns the settings for a view manager.
		/// </summary>
		/// <param name="viewName">The name of the view manager.</param>
		/// <returns>The settings.</returns>
		public ObjectTypeSettings GetIViewManagerSettings( string viewName ) 
		{
			return (ObjectTypeSettings)_iViewManagerCollection [ viewName ];
		}

		/// <summary>
		/// Specifies whether or not the state cache is enabled.
		/// </summary>
		public virtual bool IsStateCacheEnabled
		{
			get{ return _isStateCacheEnabled; }
		}

		///<summary>
		///Looks up a view name based on view name.
		///</summary>  
		///<param name="viewName">The name of the view to retrieve the settings for.</param>
		public virtual ViewSettings GetViewSettingsFromName( string viewName )
		{
			Int32 nQuerySign = viewName.LastIndexOf('?');
			String view = (nQuerySign == -1) ? viewName : viewName.Substring(0, nQuerySign);

			return (ViewSettings)_viewByNameCollection[view];
		}

		///<summary>
		///Looks up a state key based on key name.
		///</summary>  
		///<param name="keyName">The name of the state key to retrieve the settings for.</param>
		public virtual StateKeySettings GetStateKeySettingsFromName(String keyName)
		{
			return (StateKeySettings)_stateKeyByNameCollection[keyName];
		}

		///<summary>
		///Looks up a state key based on key name.
		///</summary>  
		///<param name="controllerName">The name of the controller to retrieve the settings for.</param>
		public virtual ControllerSettings GetControllersSettingsFromName(String controllerName)
		{
			return (ControllerSettings)_controllerByNameCollection[controllerName];
		}
		
		/// <summary>
		/// Finds the view setting for a specified view type.
		/// </summary>
		/// <param name="viewType">The view type defined in the app.config.</param>
		/// <returns>The settings.</returns>
		public virtual ViewSettings GetViewSettingsFromType( string viewType)
		{
			ViewSettings viewForType = null;

			foreach (ViewSettings view in _viewByNameCollection.Values)
			{
				if (view.Type == viewType)
				{
					viewForType = view;
					break;
				}
			}

			return viewForType;
		}

		/// <summary>
		/// Looks up the next view based on the incoming graph, view, and navigation values.
		/// </summary>
		/// <param name="navigationGraphName">Name of the navigation graph that is being worked in.</param>
		/// <param name="currentViewName">Name of the current view.</param>
		/// <param name="navigateValue">Navigate value used to determine the next view to be navigated to from the current view.</param>
		public virtual ViewSettings GetNextViewSettings(string navigationGraphName, string currentViewName, string navigateValue)
		{
			//Retrieve a navgraph class based on nav name
			NavigationGraphSettings navigationGraph = GetNavigationGraphSettings( navigationGraphName );
			ViewSettings nextView=null;
			
			//  Get the current view node settings
			NodeSettings node = navigationGraph[currentViewName];
			if (null == node)
				throw new ConfigurationErrorsException(string.Format("ExceptionCouldNotGetNextViewType {0} - {1} - {2}.",
															   navigationGraphName, currentViewName, navigateValue));

			//  Get the next view name from the navigateTo node
			NavigateToSettings navigateTo = node[navigateValue];

			if ( null == navigateTo ) 			
				nextView	= GetSharedTransitionView( navigationGraph , navigateValue ); 			
			else			
				nextView = GetViewSettingsFromName( navigateTo.View );

			if (nextView == null)
				throw new ConfigurationErrorsException(string.Format("ExceptionCouldNotGetNextViewType {0} - {1} - {2}.",
															   navigationGraphName, currentViewName, navigateValue));

			return nextView;
		}

		private ViewSettings GetSharedTransitionView(NavigationGraphSettings navigationGraph,string navigateValue)
		{
			SharedTransitionSettings sharedTransition = null;
			ViewSettings sharedView = null;
	
			sharedTransition = navigationGraph.GetSharedTransitionSettings ( navigateValue );
				
			if ( null == sharedTransition )
				sharedTransition = this.GetSharedTransitionSettings ( navigateValue );

			if ( sharedTransition != null)
				sharedView = GetViewSettingsFromName( sharedTransition.View );

			return sharedView;			
		}

		/// <summary>
		/// Returns the first view name in given navigation graph. 
		/// </summary>
		/// <param name="navigationGraphName">The name of the navigation graph to retrieve the first view for.</param>
		public virtual ViewSettings GetFirstViewSettings(string navigationGraphName) 
		{			
			//Retrieve a navgraph class based on nav name
			NavigationGraphSettings navigationGraph = GetNavigationGraphSettings( navigationGraphName );

			//  return first view settings
			if( navigationGraph.FirstView == null )
				return null;
			else
				return GetViewSettingsFromName(navigationGraph.FirstView.View);
		}

		/// <summary>
		/// Looks up the state persistence provider by using the navigator name.
		/// </summary>
		/// <param name="navigatorName">The name of the navigator to retrieve the persistence provider settings for.</param>
		public virtual StatePersistenceProviderSettings GetPersistenceProviderSettings(string navigatorName)
		{
			//Retrieve a navigator from the collection
			NavigatorSettings navigator = GetNavigatorSettings( navigatorName );

			return (StatePersistenceProviderSettings)_statePersistenceCollection[ navigator.StatePersist ];
		}

		/// <summary>
		/// Gets the settings of the state object used by the specified navigator.
		/// </summary>
		/// <param name="navigatorName">The name of the navigator to retrieve the state settings for.</param>
		public virtual ObjectTypeSettings GetStateSettings( string navigatorName )
		{			
			//Retrieve a navigator from the collection
			NavigatorSettings navigator = GetNavigatorSettings( navigatorName );

			return GetStateSettingsByName(navigator.State);
		}

		/// <summary>
		/// Finds the state setting for a specified state.
		/// </summary>
		/// <param name="stateName">The state name.</param>
		/// <returns>The settings.</returns>
		public virtual ObjectTypeSettings GetStateSettingsByName(string stateName) 
		{
			return (ObjectTypeSettings) _stateCollection[stateName];
		}

		/// <summary>
		/// Gets the settings of the controller object used by the specified view.
		/// </summary>
		/// <param name="viewName">The name of the view to retrieve the controller settings for.</param>
		public virtual ControllerSettings GetControllerSettings(String viewName)
		{
			ViewSettings viewSettings = GetViewSettingsFromName(viewName);
			if (viewSettings == null)
				throw new Exception(string.Format("ExceptionViewConfigNotFound {0}.", viewName));

			if (String.IsNullOrEmpty(viewSettings.Controller))
				throw new Exception(string.Format("ExceptionViewControllerAttributesIsMissed {0}.", viewName));

			return (ControllerSettings)_controllerByNameCollection[viewSettings.Controller]; 
		}

		/// <summary>
		/// Gets the settings of the layout manager used by the specified view.
		/// </summary>
		/// <param name="viewName">The name of the view to retrieve layout manager settings for.</param>
		public virtual ObjectTypeSettings GetLayoutManagerSettings( string viewName )
		{
			ObjectTypeSettings layoutManagerSettings=null;
			
			ViewSettings viewSettings = GetViewSettingsFromName( viewName );
			if (viewSettings == null)
				throw new Exception(string.Format("ExceptionViewConfigNotFound - {0}.", viewName));
		
			if (ViewExpectsLayoutManager(viewSettings))
			{
				layoutManagerSettings = (ObjectTypeSettings)_iLayoutManagerCollection[viewSettings.LayoutManager];

				if ( layoutManagerSettings == null )
				{
					throw new Exception(string.Format("ExceptionLayoutManagerNotFound - {0}.", viewSettings.LayoutManager));
				}
			}
   
			return layoutManagerSettings; 
		}

		/// <summary>
		/// Method used to determine if a view exists within a navigation graph.
		/// </summary>
		/// <param name="navigationGraphName">Name of the navigation graph to look in.</param>
		/// <param name="viewName">Name of the view to search for in the navigation graph.</param>
		/// <returns></returns>
		internal bool ViewExistsInNavigationGraph(string navigationGraphName, string viewName)
		{			
			NavigationGraphSettings settings = GetNavigationGraphSettings(navigationGraphName);
			
			foreach (NodeSettings node in settings.Views())
			{
				if (node.View == viewName)				
					return true;
			}

			foreach (SharedTransitionSettings sharedTransition in settings.SharedTransitions())
			{
				if (sharedTransition.View  == viewName)				
					return true;
			}

			foreach (SharedTransitionSettings sharedTransition in UIPConfiguration.Config.SharedTransitions)
			{
				if (sharedTransition.View == viewName)
					return true;
			}

			return false;
		}

		/// <summary>
		/// Returns true if a layout manager is expected. Returns false if no layout manager was specified for a particular view.
		/// </summary>
		/// <param name="viewSettings">The configuration of a specific view.</param>
		/// <returns></returns>
		private bool ViewExpectsLayoutManager(ViewSettings viewSettings)
		{
			return (viewSettings.LayoutManager != null && viewSettings.LayoutManager.Length > 0);
		}

		/// <summary>
		/// Gets the cache settings used by the specified navigation graph.
		/// </summary>
		/// <param name="navigatorName">The name of the navigation graph to retrieve cache configuration information for.</param>
		public virtual CacheConfiguration GetCacheConfiguration( string navigatorName)
		{
			//Retrieve a navgraph class based on nav name
			NavigatorSettings navigator = GetNavigatorSettings(navigatorName);
			return new CacheConfiguration(navigator.CacheExpirationMode, navigator.CacheExpirationInterval);
		}

		/// <summary>
		/// Gets the <see cref="UIProcess.CacheConfiguration"/> for the default mode and interval.
		/// </summary>
		/// <returns></returns>
		public virtual CacheConfiguration GetCacheConfiguration() 
		{
			return new CacheConfiguration(_defaultMode, _defaultInterval);
		}

		/// <summary>
		/// Gets the default state as specified in app.config.
		/// </summary>
		public ObjectTypeSettings DefaultState 
		{
			get { return _defaultState; }
		}

		/// <summary>
		/// Gets the default state persistence as specified in app.config.
		/// </summary>
		public StatePersistenceProviderSettings DefaultStatePersistence 
		{
			get { return _defaultStatePersistence; }
		}

		/// <summary>
		/// Gets the default view manager as specified in app.config.
		/// </summary>
		public ObjectTypeSettings DefaultViewManager 
		{
			get { return _defaultViewManager; }
		}

		/// <summary>
		/// Returns true if Back button support is enabled. Returns false to prevent the Back button from functioning.
		/// </summary>
		public bool AllowBackButton
		{
			get { return _allowBackButton; }
		}

		/// <summary>
		/// Array of global shared transition settings.
		/// </summary>
		internal SharedTransitionSettings[] SharedTransitions
		{
			get
			{
				SharedTransitionSettings[] results = new SharedTransitionSettings[_globalSharedTransitions.Count];
				_globalSharedTransitions.Values.CopyTo(results,0);
				return results;
			}
		}
		#endregion
	}
}
