using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GerenciadorDeOrdensDeServicoWeb.PresentationLayer.app.handlers.servicos {
	/// <summary>
	/// Summary description for ServicosHandler
	/// </summary>
	public class ServicosHandler : IHttpHandler {

		public void ProcessRequest( HttpContext context ) {
			String action = String.Empty;// metodos CRUD
			String response = String.Empty;

			action = context.Request.QueryString["action"];

			switch( action ) {
				case "create":
					//response = createClientes( context.Request.Form["records"] );
					break;
				case "read":
					//UInt32 start = 0;
					//UInt32 limit = 0;
					//String filters = String.Empty;
					//String sorters = String.Empty;

					//UInt32.TryParse( context.Request.QueryString["start"], out start );
					//UInt32.TryParse( context.Request.QueryString["limit"], out limit );
					//if( String.IsNullOrEmpty( context.Request.QueryString["filter"] ) == false ) {
					//    filters = context.Request.QueryString["filter"];
					//}
					//if( String.IsNullOrEmpty( context.Request.QueryString["sort"] ) == false ) {
					//    sorters = context.Request.QueryString["sort"];
					//}
					//context.Session["readClientes_start"] = start;
					//context.Session["readClientes_limit"] = limit;
					//context.Session["readClientes_filters"] = filters;
					//context.Session["readClientes_sorters"] = sorters;

					//response = readClientes( start, limit, filters, sorters );
					break;
				case "update":
					//response = updateClientes( context.Request.Form["records"] );
					break;
				case "destroy":
					//response = destroyClientes( context.Request.Form["records"], context );
					break;
			}

			context.Response.ContentType = "application/json";
			context.Response.Write( response );
		}

		public bool IsReusable {
			get {
				return false;
			}
		}
	}
}