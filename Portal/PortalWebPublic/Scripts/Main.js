//---------------------------------//
//        toggleVisibility         //
//---------------------------------//
function toggleVisibility( obj_id )
{
	var page_obj = document.getElementById( obj_id );
	if( page_obj != null )
	{
		if( page_obj.style.display == "none" )
		{
			page_obj.style.display = "";
		}
		else
		{
			page_obj.style.display = "none";
		}
	}
}

//---------------------------------//
//          showControl            //
//---------------------------------//
function showControl( obj_id )
{
	var page_obj = document.getElementById( obj_id );
	if( page_obj != null )
	{
		page_obj.style.display = "";
	}
}

//---------------------------------//
//          hideControl            //
//---------------------------------//
function hideControl( obj_id )
{
	var page_obj = document.getElementById( obj_id );
	if( page_obj != null )
	{
		page_obj.style.display = "none";
	}
}

//---------------------------------//
//          hideControl            //
//---------------------------------//
function hideControl( obj_id )
{
	var page_obj = document.getElementById( obj_id );
	if( page_obj != null )
	{
		page_obj.style.display = "none";
	}
}

//---------------------------------//
//        hideAllControls          //
//---------------------------------//
function hideAllControls( obj_id, tagName )
{
	var controls = document.getElementsByTagName( tagName );
	if( controls != null )
	{
		for( i = 0; i < controls.length; i++ )
		{
			if( controls[i].id.match( obj_id ) )
			{
				controls[i].style.display = "none";
			}
		}
	}
}

//---------------------------------//
//        hideAllControls          //
//---------------------------------//
function showAllControls( obj_id, tagName )
{
	var controls = document.getElementsByTagName( tagName );
	if( controls != null )
	{
		for( i = 0; i < controls.length; i++ )
		{
			if( controls[i].id.match( obj_id ) )
			{
				controls[i].style.display = "";
			}
		}
	}
}
