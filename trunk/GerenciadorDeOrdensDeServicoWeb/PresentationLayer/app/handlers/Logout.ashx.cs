using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace GerenciadorDeOrdensDeServicoWeb.PresentationLayer.app.handlers {
	/// <summary>
	/// Summary description for Logout
	/// </summary>
	public class Logout : IHttpHandler {

		public void ProcessRequest( HttpContext context ) {
			// finaliza a sessao
			FormsAuthentication.SignOut();

			// limpa os cookies
			HttpCookie aCookie;
			string cookieName;
			int limit = context.Request.Cookies.Count;
			for( int i=0; i<limit; i++ ) {
				cookieName = context.Request.Cookies[i].Name;
				aCookie = new HttpCookie( cookieName );
				aCookie.Expires = DateTime.Now.AddDays( -1 );
				context.Response.Cookies.Add( aCookie );
			}

			context.Response.ContentType = "application/json";
			context.Response.Write( "{ success:true, redirect:'/PresentationLayer/login.html' }" );
		}

		public bool IsReusable {
			get {
				return false;
			}
		}
	}
}