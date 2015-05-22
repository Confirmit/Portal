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
	/// Команда, представленная в виде ссылки с постбэком (LinkButton).
	/// </summary>
	[ParseChildren( true )]
	[PersistChildren(false)]
	[DefaultEvent("Click")]
	[DefaultProperty("Text")]
	public class ContextLinkButton : ContextListItem, INamingContainer, IPostBackEventHandler
	{
		#region События
		/// <summary>
		/// Событие, возникающие при нажатии на ссылку команды.
		/// </summary>
		public event EventHandler Click;

		#endregion

		#region Свойства
		/// <summary>
		/// Текст команды.
		/// </summary>
		[Browsable( true )]
		[Localizable(true)]
		[DefaultValue("")]
		public override string Text
		{
			get
			{
				object o = ViewState["Text"];
				return o != null ? (string)o : String.Empty;
			}
			set
			{
				ViewState["Text"] = value;
			}
		}

		/// <summary>
		/// Url картинки.
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

		#endregion

		protected virtual PostBackOptions GetPostBackOptions()
		{
			PostBackOptions options = new PostBackOptions( this, String.Empty );
			return options;
		}
		
		/// <summary>
		/// Рендерит внутренний LinkButton.
		/// </summary>
		/// <param name="writer"></param>
		protected void RenderLinkButton( HtmlTextWriter writer )
		{
			if(this.Page != null)
			{
				this.Page.VerifyRenderingInServerForm( this );
			}

			/*
			bool isEnabled = base.IsEnabled;
			if(this.Enabled && !isEnabled)
			{
				writer.AddAttribute( HtmlTextWriterAttribute.Disabled, "disabled" );
			}
			*/
			if(/*isEnabled && */(this.Page != null))
			{
				PostBackOptions postBackOptions = GetPostBackOptions();
				string uniqueID = UniqueID;
				if((uniqueID != null) 
					&& ((postBackOptions == null) || (postBackOptions.TargetControl == this)))
				{
					writer.AddAttribute( HtmlTextWriterAttribute.Name, uniqueID );
					writer.AddAttribute( HtmlTextWriterAttribute.Id, ClientID );
				}
				string postBackEventReference = null;
				if(postBackOptions != null)
				{
					postBackEventReference = String.Format("javascript:{0}",Page.ClientScript.GetPostBackEventReference( postBackOptions, true ));
				}
				if(string.IsNullOrEmpty( postBackEventReference ))
				{
					postBackEventReference = "javascript:void(0)";
				}
				writer.AddAttribute( HtmlTextWriterAttribute.Href, postBackEventReference );
			}

			writer.RenderBeginTag( HtmlTextWriterTag.A );
			writer.WriteEncodedText( Text );
			writer.RenderEndTag();
		}

		protected override void RenderChildren( HtmlTextWriter writer )
		{
			if(!String.IsNullOrEmpty( ImageUrl ))
			{
				// если задана ссылка на изображение, то формируем минитабличку с картинкой
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

				RenderLinkButton( writer );

				writer.RenderEndTag(); // </td>
				writer.RenderEndTag(); // </tr>
				writer.RenderEndTag(); // </table>
			}
			else
			{
				RenderLinkButton( writer );
			}
		}

		protected virtual void OnClick( EventArgs e )
		{
			// поднимаем событие
			if(Click != null)
			{
				Click( this, e );
			}
		}

		#region IPostBackEventHandler Members

		void IPostBackEventHandler.RaisePostBackEvent( string eventArgument )
		{
			this.RaisePostBackEvent( eventArgument );
		}

		protected virtual void RaisePostBackEvent( string eventArgument )
		{
			OnClick( EventArgs.Empty );
		}

		#endregion
	}

}