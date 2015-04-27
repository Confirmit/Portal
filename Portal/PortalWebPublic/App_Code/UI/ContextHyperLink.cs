using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.ComponentModel;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Text.RegularExpressions;

using Core;
using Core.Security;

namespace EPAMSWeb.UI
{
	/// <summary>
	/// ������� �������, �������������� � ���� �����������.
	/// </summary>
	[ParseChildren( true )]
	[PersistChildren(false)]
	[DefaultProperty( "Text" )]
	public class ContextHyperLink : ContextListItem, INamingContainer
	{
		#region ��������
		/// <summary>
		/// ����� �������.
		/// </summary>
		[Browsable( true )]
		[Localizable(true)]
		[DefaultValue("")]
		public override string Text
		{
			get
			{
				EnsureChildControls();
				return m_HyperLink.Text;
			}
			set
			{
				EnsureChildControls();
				m_HyperLink.Text = value;
			}
		}

		/// <summary>
		/// Url ������.
		/// </summary>
		[Browsable( true )]
		[UrlProperty]
		[DefaultValue( "" )]
		public virtual string NavigateUrl
		{
			get
			{
				EnsureChildControls();
				return m_HyperLink.NavigateUrl;
			}
			set
			{
				EnsureChildControls();
				m_HyperLink.NavigateUrl = value;
			}
		}

		/// <summary>
		/// Url ��������.
		/// </summary>
		[Browsable( true )]
		[UrlProperty]
		[DefaultValue( "" )]
		public virtual string ImageUrl
		{
			get
			{
				object o = ViewState["ImageUrl"];
				return o != null ? (string)o : String.Empty;
			}
			set
			{
				ViewState["ImageUrl"] = value;
			}
		}

		/// <summary>
		/// ����������, �������� �� ������� ��� �������� ������������.
		/// � ���������� � ������� ���������� ��������� ����������� �������, �� ������� ��������� NavigateUrl.
		/// </summary>
		public override bool IsAccessible
		{
			get
			{
				// ������� ��������� �� ����, �.�. ����������� ���� �� �������� ��� ����� ����������.
				string paramlessUrl = Regex.Replace( NavigateUrl, @"(\?|#).*", "" );

				return base.IsAccessible && UrlAuthorizationModule.CheckUrlAccessForPrincipal(
					paramlessUrl,
					HttpContext.Current.User,
					"*" );
			}
		}

		/// <summary>
		/// CSS ����� ��� ������.
		/// </summary>
		[Browsable(true)]
		public string CssClass
		{
			get
			{
				EnsureChildControls();
				return m_HyperLink.CssClass;
			}
			set
			{
				EnsureChildControls();
				m_HyperLink.CssClass = value;
			}
		}

		#endregion

		#region �������� ��������

		private HyperLink m_HyperLink;

		#endregion

		protected override void CreateChildControls()
		{
			m_HyperLink = new HyperLink();
			m_HyperLink.ID = "InnerHyperLink";

			Controls.Add( m_HyperLink );

			ChildControlsCreated = true;
		}

		protected override void RenderChildren( HtmlTextWriter writer )
		{
			if(!String.IsNullOrEmpty( ImageUrl ))
			{
				// ���� ������ ������ �� �����������, �� ��������� ������������ � ���������
				writer.AddAttribute( HtmlTextWriterAttribute.Cellpadding, "0" );
				writer.AddAttribute( HtmlTextWriterAttribute.Cellspacing, "0" );
				writer.RenderBeginTag( HtmlTextWriterTag.Table );
				writer.RenderBeginTag( HtmlTextWriterTag.Tr );
				writer.AddStyleAttribute( HtmlTextWriterStyle.PaddingRight, "3px" );
				writer.RenderBeginTag( HtmlTextWriterTag.Td );

				Image image = new Image();
				image.ID = "InnerImage";
				image.ImageUrl = ImageUrl;
				image.RenderControl( writer );

				writer.RenderEndTag();

				writer.RenderBeginTag( HtmlTextWriterTag.Td );
				base.RenderChildren( writer );
				writer.RenderEndTag(); // </td>
				writer.RenderEndTag(); // </tr>
				writer.RenderEndTag(); // </table>
			}
			else
			{
				base.RenderChildren( writer );
			}
		}
	}

}