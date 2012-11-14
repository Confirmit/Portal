using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Web.UI.WebControls;

using Core;
using UlterSystems.PortalLib.BusinessObjects;

/// <summary>
/// Control for editing of domain names of certain person.
/// </summary>
public partial class Controls_DomainNamesEditor : BaseUserControl
{
	#region Fields
	private static string absentDomainNameID = "-1";
	#endregion

	#region Properties
	/// <summary>
	/// ID of user which domain names are edited.
	/// </summary>
	public int? UserID
	{
		get
		{
			if( ViewState[ "UserID" ] == null )
				return null;
			else if( ViewState[ "UserID" ] is int )
				return (int) ViewState[ "UserID" ];
			else
				return null;
		}
		set
		{
			if( UserID != value )
			{
				ViewState[ "UserID" ] = value;
				RemovedDomainNames.Clear();
				ShowDomainNames();
			}
		}
	}

	/// <summary>
	/// List of removed domain names.
	/// </summary>
	protected List<int> RemovedDomainNames
	{
		get
		{
			if( ViewState[ "RemovedDomainNames" ] == null )
			{ ViewState[ "RemovedDomainNames" ] = new List<int>(); }
			return ViewState[ "RemovedDomainNames" ] as List<int>;
		}
	}
	#endregion

	#region Event handlers
	protected void Page_Load( object sender, EventArgs e )
	{

	}

	/// <summary>
	/// Handles adding of domain name.
	/// </summary>
	protected void OnAdd( object sender, EventArgs e )
	{
		try
		{
			if( string.IsNullOrEmpty( tbDomainName.Text ) )
				return;

			ListItem item = new ListItem( tbDomainName.Text, absentDomainNameID );
			lbDomainNames.Items.Add( item );

			tbDomainName.Text = string.Empty;
		}
		catch( Exception ex )
		{ Logger.Log.Error( ex.Message, ex ); }
	}

	/// <summary>
	/// Handles removing of domain name.
	/// </summary>
	protected void OnRemove( object sender, EventArgs e )
	{
		try
		{
			for( int itemIndex = lbDomainNames.Items.Count - 1; itemIndex >= 0; itemIndex-- )
			{
				ListItem item = lbDomainNames.Items[ itemIndex ];

				if( !item.Selected )
					continue;

				if( item.Value != absentDomainNameID )
					RemovedDomainNames.Add( int.Parse( item.Value ) );

				lbDomainNames.Items.RemoveAt( itemIndex );
			}
		}
		catch( Exception ex )
		{ Logger.Log.Error( ex.Message, ex ); }
	}
	#endregion

	#region Methods
	/// <summary>
	/// Shows domain names of user.
	/// </summary>
	protected void ShowDomainNames()
	{
        try
        {
            lbDomainNames.Items.Clear();

            if (UserID == null)
                return;

            Person person = Person.GetPersonByID(UserID.Value);
            if (person == null || person.ID == null)
                return;

            IList<PersonAttribute> dnAttribs =
                PersonAttributes.GetPersonAttributesByKeyword(person.ID.Value
                                                              , PersonAttributeTypes.DomainName.ToString());
            if (dnAttribs == null || dnAttribs.Count == 0)
                return;

            lbDomainNames.DataSource = dnAttribs;
            lbDomainNames.DataBind();
        }
        catch (Exception ex)
        {
            Logger.Log.Error(ex.Message, ex);
        }
	}

	/// <summary>
	/// Generates domain names for current user.
	/// </summary>
	/// <param name="personID">ID of person to which domain names belong to.</param>
    public void GenerateDomainNames(int personID)
	{
	    try
	    {
	        Person person = Person.GetPersonByID(personID);
	        if (person == null || person.ID == null)
	            return;

	        foreach (ListItem item in lbDomainNames.Items)
	        {
	            if (item.Value != absentDomainNameID)
	                continue;

	            PersonAttribute dnAttrib = person.AddStandardStringAttribute(PersonAttributeTypes.DomainName, item.Text);
	            Debug.Assert(dnAttrib.ID != null);
	            item.Value = dnAttrib.ID.Value.ToString();
	        }

	        // Remove deleted domain names from database.
            foreach (int attribID in RemovedDomainNames)
            {
                PersonAttribute attrib = new PersonAttribute();
                if (attrib.Load(attribID))
                    attrib.Delete();
            }
	    }
	    catch (Exception ex)
	    {
	        Logger.Log.Error(ex.Message, ex);
	    }
	}

    #endregion
}
