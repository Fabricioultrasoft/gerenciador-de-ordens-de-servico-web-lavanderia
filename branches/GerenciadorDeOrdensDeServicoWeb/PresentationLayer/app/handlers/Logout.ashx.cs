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
			FormsAuthentication.SignOut();
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