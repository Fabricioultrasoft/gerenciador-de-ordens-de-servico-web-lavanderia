﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using GerenciadorDeOrdensDeServicoWeb.DataTransferObjects.usuarios;
using GerenciadorDeOrdensDeServicoWeb.DataTransferObjects;
using GerenciadorDeOrdensDeServicoWeb.BusinessLogicLayer.usuarios;
using System.Web.Script.Serialization;
using System.Web.SessionState;

namespace GerenciadorDeOrdensDeServicoWeb.PresentationLayer.app.handlers.usuarios {
	/// <summary>
	/// Summary description for UsuariosHandler
	/// </summary>
	public class UsuariosHandler : IHttpHandler, IRequiresSessionState {

		public void ProcessRequest( HttpContext context ) {
			String action = String.Empty;// metodos CRUD
			String response = String.Empty;

			action = context.Request.QueryString["action"];

			switch( action ) {
				case "create":
					response = createUsuarios( context.Request.Form["records"] );
					break;
				case "read":
					UInt32 start = 0;
					UInt32 limit = 0;

					UInt32.TryParse( context.Request.QueryString["start"], out start );
					UInt32.TryParse( context.Request.QueryString["limit"], out limit );

					context.Session["readUsuarios_start"] = start;
					context.Session["readUsuarios_limit"] = limit;
					
					response = readUsuarios( start, limit );
					break;
				case "update":
					response = updateUsuarios( context.Request.Form["records"] );
					break;
				case "destroy":
					response = destroyUsuarios( context.Request.Form["records"], context );
					break;
			}

			context.Response.ContentType = "application/json";
			context.Response.Write( response );
		}

		private String createUsuarios( String records ) {
			List<Usuario> usuarios = jsonToUsuarios( records );
			StringBuilder jsonResposta = new StringBuilder();
			List<Erro> erros = GerenciadorDeUsuarios.cadastrarListaDeUsuarios( ref usuarios );

			#region CONSTROI O JSON
			formatarSaida( ref usuarios );
			jsonResposta.AppendLine( "{" );
			jsonResposta.AppendLine( "    \"total\": " + usuarios.Count + "," );

			if( erros.Count == 0 ) {
				jsonResposta.AppendLine( "    \"success\": true," );
				jsonResposta.AppendLine( "    \"message\": [\"Dados cadastrados com sucesso\"]," );
			} else {
				jsonResposta.AppendLine( "    \"success\": false," );
				Compartilhado.construirParteDoJsonMensagensDeErros( ref jsonResposta, erros );
			}

			jsonResposta.AppendLine( "    \"data\": [" );
			foreach( Usuario usu in usuarios ) {
				jsonResposta.Append( "{" );
				jsonResposta.AppendFormat( " \"codigo\": {0}, ", usu.codigo );
				jsonResposta.AppendFormat( " \"nome\": \"{0}\", ", usu.nome );
				jsonResposta.AppendFormat( " \"senha\": \"{0}\", ", usu.senha );
				jsonResposta.Append( "}," );
			}
			if( usuarios.Count > 0 ) jsonResposta.Remove( jsonResposta.Length - 1, 1 );// remove a ultima virgula
			jsonResposta.Append( "]" );

			// fim do json
			jsonResposta.AppendLine( "}" );
			#endregion

			return jsonResposta.ToString();
		}

		private String readUsuarios( UInt32 start, UInt32 limit ) {

			List<Usuario> usuarios;
			List<Erro> erros = GerenciadorDeUsuarios.preencherListaDeUsuarios( out usuarios, start, limit );
			long qtdRegistros = GerenciadorDeUsuarios.countUsuarios();
			StringBuilder jsonResposta = new StringBuilder();

			#region CONSTROI O JSON
			formatarSaida( ref usuarios );
			jsonResposta.AppendLine( "{" );
			jsonResposta.AppendLine( "    \"total\": " + qtdRegistros + "," );

			if( erros.Count == 0 ) {
				jsonResposta.AppendLine( "    \"success\": true," );
				jsonResposta.AppendLine( "    \"message\": [\"Foram encontrados " + qtdRegistros + " registros\"]," );
			} else {
				jsonResposta.AppendLine( "    \"success\": false," );
				Compartilhado.construirParteDoJsonMensagensDeErros( ref jsonResposta, erros );
			}

			jsonResposta.AppendLine( "    \"data\": [" );
			foreach( Usuario usu in usuarios ) {
				jsonResposta.Append( "{" );
				jsonResposta.AppendFormat( " \"codigo\": {0}, ", usu.codigo );
				jsonResposta.AppendFormat( " \"nome\": \"{0}\", ", usu.nome );
				jsonResposta.AppendFormat( " \"senha\": \"{0}\", ", usu.senha );
				jsonResposta.Append( "}," );
			}
			if( usuarios.Count > 0 ) jsonResposta.Remove( jsonResposta.Length - 1, 1 );// remove a ultima virgula
			jsonResposta.Append( "]" );

			// fim do json
			jsonResposta.AppendLine( "}" );
			#endregion

			return jsonResposta.ToString();
		}

		private String updateUsuarios( String records ) {
			List<Usuario> usuarios = jsonToUsuarios( records );
			StringBuilder jsonResposta = new StringBuilder();
			List<Erro> erros = GerenciadorDeUsuarios.atualizarListaDeUsuarios( usuarios );

			#region CONSTROI O JSON
			formatarSaida( ref usuarios );
			jsonResposta.AppendLine( "{" );
			jsonResposta.AppendLine( "    \"total\": " + usuarios.Count + "," );

			if( erros.Count == 0 ) {
				jsonResposta.AppendLine( "    \"success\": true," );
				jsonResposta.AppendLine( "    \"message\": [\"Dados atualizados com sucesso\"]," );
			} else {
				jsonResposta.AppendLine( "    \"success\": false," );
				Compartilhado.construirParteDoJsonMensagensDeErros( ref jsonResposta, erros );
			}

			jsonResposta.AppendLine( "    \"data\": [" );
			foreach( Usuario usu in usuarios ) {
				jsonResposta.Append( "{" );
				jsonResposta.AppendFormat( " \"codigo\": {0}, ", usu.codigo );
				jsonResposta.AppendFormat( " \"nome\": \"{0}\", ", usu.nome );
				jsonResposta.AppendFormat( " \"senha\": \"{0}\", ", usu.senha );
				jsonResposta.Append( "}," );
			}
			if( usuarios.Count > 0 ) jsonResposta.Remove( jsonResposta.Length - 1, 1 );// remove a ultima virgula
			jsonResposta.Append( "]" );

			// fim do json
			jsonResposta.AppendLine( "}" );
			#endregion

			return jsonResposta.ToString();
		}

		private String destroyUsuarios( String records, HttpContext context ) {
			List<Usuario> usuarios = jsonToUsuarios( records );
			List<Erro> erros = GerenciadorDeUsuarios.excluirListaDeUsuarios( usuarios );

			if( erros.Count == 0 ) {
				UInt32 start = 0;
				UInt32 limit = 25;
				String filters = String.Empty;
				String sorters = String.Empty;

				UInt32.TryParse( context.Session["readUsuarios_start"].ToString(), out start );
				UInt32.TryParse( context.Session["readUsuarios_limit"].ToString(), out limit );

				return readUsuarios( start, limit );
			} else {
				StringBuilder jsonResposta = new StringBuilder();

				formatarSaida( ref usuarios );
				jsonResposta.AppendLine( "{" );
				jsonResposta.AppendLine( "    \"total\": " + usuarios.Count + "," );
				jsonResposta.AppendLine( "    \"success\": false," );
				Compartilhado.construirParteDoJsonMensagensDeErros( ref jsonResposta, erros );
				jsonResposta.AppendLine( "    \"data\": []" );
				jsonResposta.AppendLine( "}" );

				return jsonResposta.ToString();
			}
		}

		public static List<Usuario> jsonToUsuarios( String json ) {
			JavaScriptSerializer js = new JavaScriptSerializer();
			List<Usuario> usuarios = js.Deserialize<List<Usuario>>( json );
			return usuarios;
		}

		public static void formatarSaida( ref List<Usuario> usuarios ) {
			for( int i = 0; i < usuarios.Count; i++ ) {
				Compartilhado.tratarCaracteresEspeciais<Usuario>( usuarios[i] );
			}
		}


		public bool IsReusable {
			get {
				return false;
			}
		}
	}
}