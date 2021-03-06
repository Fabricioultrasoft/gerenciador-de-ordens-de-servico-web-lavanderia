﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using GerenciadorDeOrdensDeServicoWeb.BusinessLogicLayer.usuarios;
using GerenciadorDeOrdensDeServicoWeb.DataTransferObjects.usuarios;
using System.Web.SessionState;

namespace GerenciadorDeOrdensDeServicoWeb.PresentationLayer.app.handlers {
	/// <summary>
	/// Summary description for Login
	/// </summary>
	public class Login : IHttpHandler, IRequiresSessionState {

		public void ProcessRequest( HttpContext context ) {
			Usuario usuario = new Usuario();
			String response = String.Empty;

			usuario.nome = context.Request.Form["nome"];
			usuario.senha = context.Request.Form["senha"];

			try {
				if( GerenciadorDeUsuarios.autenticar( ref usuario ) ) {
					FormsAuthentication.SetAuthCookie( usuario.nome, false );

					// adiciona o usuariologado na sessao do servidor
					context.Session["usuario"] = usuario;

					HttpCookie nomeUsuarioCookie = new HttpCookie( "nomeUsuario", usuario.nome );
					nomeUsuarioCookie.Expires = DateTime.MinValue;
					context.Response.Cookies.Add( nomeUsuarioCookie );

					response = "{ success:true, redirect:'/PresentationLayer/index.html' }";
				} else {
					response = "{ success:false, message:'nome e senha incorretos' }";
				}
			} catch {
				response = "{ success:false, message:'N&atilde;o foi poss&iacute;vel estabelecer a conex&atilde;o com o servidor de banco de dados,"
					+" verifique se o servidor de banco de dados encontra-se em execu&ccedil;&atilde;o!' }";
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