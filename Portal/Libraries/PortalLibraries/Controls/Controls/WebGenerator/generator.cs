using System;
using System.Xml;

namespace Controls.WebGenerator
{
	public class Generator
	{		
		#region Ctor and fields
		//private string header, caption, statics;
		private string image;
		private string item, root, lasttd, delimiter, subitem;
		private int i, j;			

		private readonly XmlNodeList source;
        private readonly String currentPage;
        private readonly String _jsControllerObjectName = String.Empty;
        private readonly String xmlResource;

		public Generator(String _xmlResource, XmlNodeList _source, String _currentPage
            , String jsControllerObjectName)
		{
            xmlResource = _xmlResource;
			source = _source;
            currentPage = _currentPage;
            _jsControllerObjectName = jsControllerObjectName;
        }

        #endregion
	
        #region BuildMenu
		
        public String BuildMenu (String picture, string AddParameter, String ID)
		{	
			string result = null, resultSub = null;
			
			XmlDocument xml = new XmlDocument();
            xml.LoadXml(xmlResource);
			
			XmlNodeList xel = xml.GetElementsByTagName("menuBody");
			string body = xel[0].InnerText;

			xel = xml.GetElementsByTagName("menuSub");
			string sub = xel[0].InnerText;
			
			xel = xml.GetElementsByTagName("item");
			item = xel[0].InnerText;

			xel = xml.GetElementsByTagName("root");
			root = xel[0].InnerText;	
		
			xel = xml.GetElementsByTagName("lasttd");
			lasttd = (xel != null && xel[0] != null) ? xel[0].InnerText : "";	

			xel = xml.GetElementsByTagName("delimiter");
            delimiter = (xel != null && xel[0] != null) ? xel[0].InnerText : "";
	
			xel = xml.GetElementsByTagName("subitem");
			subitem = xel[0].InnerText;

			
			image = picture;
			
			for (i = 0; i < source.Count; i++)
			{
                if (source[i].NodeType != XmlNodeType.Element)
                    continue;

				result += _buildSwitch (source[i], 1,AddParameter, ID);
				
				if (source[i].Attributes["type"].InnerText == "root")
				{
					resultSub += _buildItem(sub, i.ToString(), ID);					
					foreach (XmlNode c in source[i].ChildNodes)
					{
                        if (c.NodeType != XmlNodeType.Element)
                            continue;

						j++;
                        resultSub += _buildSwitch(c, 2, AddParameter, ID);
					}
					resultSub += @"</table>";					
				}								
			}

            result = body + "<tr>" + result + lasttd + "</tr>" + @"</table>";
            result = result + @"<DIV id=""HiddenPlace" + ID + "\">" + resultSub + "</DIV>";
            result = result.Replace("__IDENT__", ID);
            return result;	
		}
        
        #endregion

		private string _buildSwitch(XmlNode n, int index, string AddParameter, string ID)
		{
            if (n.NodeType != XmlNodeType.Element)
                return "";

			string _result = null;
			string tmp = i.ToString();
			int _index = 1;

			if (index > 1) _index = 2;			
			
			if (index == 2)
				tmp = i.ToString() + "_" + j.ToString();				
			
			switch (n.Attributes["type"].InnerText)
			{								  
				default:
                    _result = _buildItem(n.Attributes["type"].InnerText, tmp, n.InnerText, ID);
					break;					
				
                case "item":
				{
                    if (n.Attributes["addurl"] != null && 
                        System.Convert.ToString(n.Attributes["addurl"].InnerText)=="1")
					{        
						_result = _buildItem (item, tmp, n.InnerText,
                            n.Attributes["url"].InnerText + AddParameter, ID).Replace("imageplace",
                                System.Convert.ToString(n.Attributes["img"].InnerText ));
					}	  
					else {
                        if (n.Attributes["url"] != null)
                        {
                            _result = _buildItem(item, tmp, n.InnerText, n.Attributes["url"].InnerText, ID);
                        }
                        else _result = _buildItem(item, tmp, n.InnerText, "", ID);
                        _result = _result.Replace("imageplace", System.Convert.ToString(n.Attributes["img"].InnerText));
                    }

                    _result = processActionAttribute(n, _result);

                    if (n.Attributes["message"] == null)
                        _result = _result.Replace("title=\"on_over_message\"", "");
                    else
                        _result = _result.Replace("on_over_message", n.Attributes["message"].InnerText);
				    }

                    if (n.Attributes["usedelimetr"] == null || String.Compare(n.Attributes["usedelimetr"].ToString().ToLower(), "true") == 0)
                    {
                        _result = _result + delimiter;
                    }
					break;					
			
				case "root":	
					_result = _buildItem (root.Replace("1", _index.ToString()), tmp,
                        n.Attributes["name"].InnerText, ID).Replace("imageplace", image).Replace("arrow", System.Convert.ToString(n.Attributes["img"].InnerText)); ;
                    _result = SetMessageToNode(n, _result);

					break;

				case "subitem":
				{
					if (n.Attributes["addurl"] != null && 
                        System.Convert.ToString(n.Attributes["addurl"].InnerText)=="1")
					{        
						_result = _buildItem (subitem, tmp, n.InnerText,
                            n.Attributes["url"].InnerText + AddParameter, ID).Replace("imageplace", image);
					}	  
					else {
                        if (n.Attributes["url"] != null)
                        {
                            _result = _buildItem(subitem, tmp, n.InnerText, n.Attributes["url"].InnerText, ID);
                        }
                        else _result = _buildItem(subitem, tmp, n.InnerText, "", ID);
                        _result = _result.Replace ("imageplace", image);
                    }

                    _result = processActionAttribute(n, _result);

                    _result = SetMessageToNode(n, _result);
				} 
					break;
			}
			return _result;			
		}

		#region private methods	
		private string _buildItem (string _source, string index, string text, string ID)
		{
            return _buildItem(_source.Replace("Text", text), index, ID);		
		}

        private string _buildItem(string _source, string index, string text, string url, string ID)
		{
            String strUrl = url.Replace("current_page", currentPage);
            return _buildItem(_source.Replace("urlplace", strUrl), index, text, ID);
		}

		private string _buildItem (string _source, string index, string ID)
		{
            return _source.Replace("|", ID + index);
		}			
        		
		#endregion	
		
	    private static string SetMessageToNode(XmlNode n, string _result)
	    {
	        if (n.Attributes["message"] == null)
	            _result = _result.Replace("title=\"on_over_message\"", "");
	        else
	            _result = _result.Replace("on_over_message", n.Attributes["message"].InnerText);
	        return _result;
	    }

        private String processActionAttribute(XmlNode n, String _result)
        {
            if (n.Attributes["action"] == null)
            {
                _result = _result.Replace("menu_action", "");
            }
            else
            {
                if (n.Attributes["url"] == null)
                {
                    _result = _result.Replace("document.location.href='urlplace';", "");
                    _result = _result.Replace("document.location.href='';", "");
                }

                String strHandler = String.IsNullOrEmpty(_jsControllerObjectName)
                    ? n.Attributes["action"].InnerText
                    : (_jsControllerObjectName + "." + n.Attributes["action"].InnerText);

                _result = _result.Replace("menu_action", strHandler);
            }

            return _result;
        }
	}
}
